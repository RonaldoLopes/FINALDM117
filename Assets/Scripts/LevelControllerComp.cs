using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelControllerComp : MonoBehaviour {

    [Header("Game Environment Config")]

    [SerializeField]
    [Tooltip("Environment Reference")]
    private Transform environment;

    [SerializeField]
    [Tooltip("Asteroid Type1 Reference")]
    private Transform[] asteroidsType1;

    [SerializeField]
    [Tooltip("Asteroid Type2 Reference")]
    private Transform[] asteroidsType2;

    [SerializeField]
    [Tooltip("Asteroid Type3 Reference")]
    private Transform[] asteroidsType3;

    [SerializeField]
    [Tooltip("Shield Reference")]
    private Transform shield;


    [SerializeField]
    [Tooltip("Inteval time between asteroids")]
    [Range(1, 100)]
    public long intevalTimeSeconds = 10;

    [SerializeField]
    [Tooltip("Number of asteroids type1 per Level")]
    [Range(0, 50)]
    public int numAsteroidsType1 = 1;

    [SerializeField]
    [Tooltip("Number of asteroids type2 per Level")]
    [Range(0, 50)]
    public int numAsteroidsType2 = 2;

    [SerializeField]
    [Tooltip("Number of asteroids type3 per Level")]
    [Range(0, 50)]
    public int numAsteroidsType3 = 3;

    [SerializeField]
    [Tooltip("Number of shields per Level")]
    [Range(0, 50)]
    public int numShields = 3;


    [SerializeField]
    [Tooltip("Number of asteroids to win the bonus")]
    [Range(10, 1000)]
    public int numAsteroidsPerBonus = 10;


    [Header("keyboard Config")]

    [SerializeField]
    [Tooltip("The speed that the spaceship will moviment laterally")]
    [Range(1, 10)]
    public float lateralSpeed = 5.0f;

    [Header("References")]

    [Header("PauseMenuPainel")]
    [SerializeField]
    [Tooltip("PauseMenuPainel Reference")]
    public GameObject pauseMenuPainel;

    [Header("GameOverMenuPainel")]
    [SerializeField]
    [Tooltip("GameOverMenuPainel Reference")]
    public GameObject gameOverMenuPainel;

    [Header("InfoPanel")]
    [SerializeField]
    [Tooltip("InfoPanel Reference")]
    public GameObject infoPanel;

    private Text goText;
    public Text GoText
    {
        get
        {
            if (!goText)
                goText = GameObject.FindGameObjectWithTag("GoText").GetComponent<Text>();

            return goText;
        }
    }

    private Text infoAsteroidsValue;
    public Text InfoAsteroidsValue
    {
        get
        {
            if (!infoAsteroidsValue)
                infoAsteroidsValue = GameObject.FindGameObjectWithTag("InfoBonusValue").GetComponent<Text>();

            return infoAsteroidsValue;
        }
    }

    private Text infoShieldValue;
    public Text InfoShieldValue
    {
        get
        {
            if (!infoShieldValue)
                infoShieldValue = GameObject.FindGameObjectWithTag("InfoBonusValue").GetComponent<Text>();

            return infoShieldValue;
        }
    }

    private Text infoBonusValue;
    public Text InfoBonusValue
    {
        get
        {
            if (!infoBonusValue)
                infoBonusValue = GameObject.FindGameObjectWithTag("InfoBonusValue").GetComponent<Text>();

            return infoBonusValue;
        }
    }

    [Header("Current game info:")]

    public static bool shieldActivated = false;

    public static int countAsteroids = 0;

    public int numAsteroids = 0;

    public int numShield = 0;

    public int numBonus = 0;

    public int numAsteroidsType1Added = 0;

    public int numAsteroidsType2Added = 0;

    public int numAsteroidsType3Added = 0;

    public int numShieldsAdded = 0;

    public int numAsteroidsDestroyedPerBonus = 0;

    private static bool gamePaused;
    public bool GamePaused
    {
        get
        {
            return gamePaused;
        }
    }

    private long timeReference;

    private List<GameObject> spots;

    public static bool activedDebug = true;

    private SpaceShipComp spaceShipComp;
    public SpaceShipComp SpaceShip
    {
        get
        {
            if (!spaceShipComp)
                spaceShipComp = FindObjectOfType<SpaceShipComp>();
            
            return spaceShipComp;
        }
    }

    private bool gameStarted = false;
    public bool GameStarted
    {
        get
        {
            return gameStarted;
        }
    }

    // Use this for initialization
    void Start () {
        

        PrintDebug("LevelControllerComp.Start");

        spaceShipComp = FindObjectOfType<SpaceShipComp>();

        goText = GameObject.FindGameObjectWithTag("GoText").GetComponent<Text>();
        infoAsteroidsValue = GameObject.FindGameObjectWithTag("InfoAsteroidsValue").GetComponent<Text>();
        infoShieldValue = GameObject.FindGameObjectWithTag("InfoShieldValue").GetComponent<Text>();
        infoBonusValue = GameObject.FindGameObjectWithTag("InfoBonusValue").GetComponent<Text>();

        Initialize();

        UpdateValueAsteroids();
        UpdateValueShield();
        UpdateValueBonus();

        ListSpots();
    }

    // Update is called once per frame
    void Update () {


        //audioSource.clip = musicControllerComp.audioClip[0];
        if (Input.GetMouseButton(0))
        {
            //SpaceShip.SendLaserShot();
            if (!gameStarted)
            {
                timeReference = DateTime.Now.ToFileTime();
                GoText.gameObject.SetActive(false);
                gameStarted = true;
                PrintDebug("LevelControllerComp.Update GameStarted " + GameStarted);
            }
        }

        if (GameStarted && !gamePaused)
        {
            UpdateValueAsteroids();
            UpdateValueShield();
            UpdateValueBonus();
            SpaceShip.SendLaserShot();
            InserComponets();
        }
    }

    private void InserComponets()
    {
        DateTime dataTime = DateTime.Now;
        long currentTimeNow = dataTime.ToFileTime();
        long timeDifference = (currentTimeNow - timeReference);
        long interval = (intevalTimeSeconds * 10000000);
        if (timeDifference >= interval)
        {
            timeReference = currentTimeNow;
            int comp = UnityEngine.Random.Range(1, 10);
            if (comp > 8 && numShieldsAdded < numShields)
            {
                AddShield();
            }
            else
            {
                AddAsteroids();
            }

        }
    }

    void Initialize()
    {
        PrintDebug("LevelControllerComp.Initialize");
        GoText.gameObject.SetActive(true);
        gameStarted = false;
        countAsteroids = 0;
        shieldActivated = false;
        numAsteroids = 0;
        numBonus = 0;
        numAsteroidsType1Added = 0;
        numAsteroidsType2Added = 0;
        numAsteroidsType3Added = 0;
        numShieldsAdded = 0;
        numAsteroidsDestroyedPerBonus = 0;
        gamePaused = false;
        timeReference = DateTime.Now.ToFileTime();
    }

    private void ListSpots()
    {
        spots = new List<GameObject>();
        foreach (Transform coponent in environment)
        {
            if (coponent.CompareTag("Spot"))
            {
                spots.Add(coponent.gameObject);
            }
        }
    }

    public void LoadLevel(string sceneName)
    {
        PrintDebug("LevelControllerComp.LoadLevel " + sceneName);
        SceneManager.LoadScene(sceneName);
        Initialize();
    }

    public void LoadNextLevel()
    {
        PrintDebug("LevelControllerComp.LoadNextLevel "+ (SceneManager.GetActiveScene().buildIndex + 1));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Initialize();
    }

    private void AddShield()
    {
        if (numShieldsAdded <= numShields)
        {
            if (spots.Count > 0)
            {
                var spot = spots[UnityEngine.Random.Range(0, spots.Count)];
                var pointSpawnPos = spot.transform.position;
                var newShield = Instantiate(shield, pointSpawnPos, Quaternion.identity);
                newShield.SetParent(spot.transform);
                ++numShieldsAdded;
            }
        }
    }

    private void AddAsteroids()
    {
        if (numAsteroidsType1Added <= numAsteroidsType1 ||
            numAsteroidsType2Added <= numAsteroidsType2 ||
            numAsteroidsType3Added <= numAsteroidsType3)
        {
            int asteroidType = UnityEngine.Random.Range(1, 3);
            if (asteroidType == 1 && numAsteroidsType1Added <= numAsteroidsType1)
            {
                numAsteroidsType1Added++;
                AddAsterois(asteroidsType1);
            }
            else if (asteroidType == 2 && numAsteroidsType2Added <= numAsteroidsType2)
            {
                numAsteroidsType2Added++;
                AddAsterois(asteroidsType2);
            }
            else if (asteroidType == 3 && numAsteroidsType3Added <= numAsteroidsType3)
            {
                numAsteroidsType3Added++;
                AddAsterois(asteroidsType3);
            }
        }
    }

    private void AddAsterois(Transform[] asteroids)
    {
        var asteroid = asteroids[UnityEngine.Random.Range(0, asteroids.Length)];
        if (spots.Count > 0)
        {
            var spot = spots[UnityEngine.Random.Range(0, spots.Count)];
            var pointSpawnPos = spot.transform.position;
            var newAsteroid = Instantiate(asteroid, pointSpawnPos, Quaternion.identity);
            newAsteroid.SetParent(spot.transform);
        }
    }

    public void UpdateValueAsteroids()
    {
        InfoAsteroidsValue.text = numAsteroids.ToString();
    }

    public void UpdateValueShield()
    {
        InfoShieldValue.text = numShield.ToString();
    }

    public void UpdateValueBonus()
    {
        InfoBonusValue.text = numBonus.ToString();
    }

    public void IncreaseAsteroidCounter()
    {
        countAsteroids++;
    }
    public void DecreaseAsteroidCounter()
    {
        countAsteroids--;
        if (countAsteroids <= 0)
        {
            LoadNextLevel();
        }
    }

    public bool IsShieldActivated()
    {
        return shieldActivated;
    }
    public void ActivateShield()
    {
        shieldActivated = true;
    }
    public void DeactivateSield()
    {
        shieldActivated = false;
    }


    /// <summary>
    /// Method to pause the game
    /// </summary>
    /// <param name="isPaused"></param>
    public void SetPauseMenu(bool isPaused)
    {
        PrintDebug("LevelControllerComp.SetPauseMenu "+ isPaused);
        gamePaused = isPaused;
        PauseGame(gamePaused);
        pauseMenuPainel.SetActive(gamePaused);
    }


    public static void PauseGame(bool isPaused)
    {
        LevelControllerComp.PrintDebug("LevelControllerComp.PauseGame " + isPaused);
        gamePaused = isPaused;
        //Se o jogo estiver paused, timescale recebe 0
        Time.timeScale = (isPaused) ? 0 : 1;
    }




    public void Restart()
    {
       // PrintDebug("LevelControllerComp.Restart " );
        PauseGame(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSceneByName(string nameScene)
    {
        //PrintDebug("LevelControllerComp.LoadSceneByName ");
        PauseGame(false);
        SceneManager.LoadScene(nameScene);
    }



    //////////////////////////////////////////////////////////////////////

    public void DestroyObj(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public void ResetGame()
    {
        //PrintDebug("LevelControllerComp.ResetGame ");
        gameOverMenuPainel.SetActive(true);

        var buttons = gameOverMenuPainel.transform.GetComponentsInChildren<Button>();

        Button continueButton = null;
        foreach (var button in buttons)
        {
            if (button.name.Equals("ContinueButton"))
            {
                continueButton = button;
                break;
            }
        }

        if (continueButton != null)
        {
#if UNITY_ADS
            //Se o button continue for clicado, iremos tocar o anúncio
            StartCoroutine(ShowContinue(continueButton));
            //buttonContinue.onClick.AddListener(UnityAdControler.ShowRewardAd);
#else
            //Se nao existe add, nao precisa mostrar o botao Continue
            continueButton.gameObject.SetActive(false);
#endif
        }

    }

    /// <summary>
    /// Metodo para reiniciar o jogo
    /// </summary>
    private void Reset()
    {
        //PrintDebug("LevelControllerComp.Reset ");
        gamePaused = false;
        Initialize();
        //Reinicia o level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    /// <summary>
    /// Faz o reset do jogo
    /// </summary>
    public void Continue()
    {
        //PrintDebug("LevelControllerComp.Continue ");
        gameOverMenuPainel.SetActive(false);

        PauseGame(false);
        //ActiveDeactiveGameObjects(true);

    }

    public void ActiveDeactiveGameObjects(bool active)
    {
        PrintDebug("LevelControllerComp.ActiveDeactiveGameObjects "+ active);
        AsteroidComp[] asteroids = GameObject.FindObjectsOfType<AsteroidComp>(); ;
        foreach (AsteroidComp asteroid in asteroids)
        {
            asteroid.gameObject.SetActive(active);
        }
        ShieldComp[] shields = GameObject.FindObjectsOfType<ShieldComp>(); ;
        foreach (ShieldComp shield in shields)
        {
            shield.gameObject.SetActive(active);
        }
        SpaceShipComp spaceShip = GameObject.FindObjectOfType<SpaceShipComp>(); ;
        spaceShip.gameObject.SetActive(active);
    }

    public IEnumerator ShowContinue(Button continueButton)
    {
        PrintDebug("LevelControllerComp.ShowContinue ");
        var btnText = continueButton.GetComponentInChildren<Text>();
        while (true)
        {
            if (UnityAdControler.nextTimeReward.HasValue && (DateTime.Now < UnityAdControler.nextTimeReward.Value))
            {
                continueButton.interactable = false;

                TimeSpan restante = UnityAdControler.nextTimeReward.Value - DateTime.Now;

                var contagemRegressiva = string.Format("{0:D2}:{1:D2}", restante.Minutes, restante.Seconds);
                btnText.text = contagemRegressiva;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                continueButton.interactable = true;
                continueButton.onClick.AddListener(UnityAdControler.ShowRewardAd);
                btnText.text = "Continue (Ver Ad)";
                break;
            }
        }
    }

    public void IncreaseAsteroidsDestroyed()
    {
        numAsteroids++;
        numAsteroidsDestroyedPerBonus++;
        if(numAsteroidsDestroyedPerBonus >= numAsteroidsPerBonus)
        {
            numAsteroidsDestroyedPerBonus = 0;
            numBonus++;
            if (numBonus > spaceShipComp.laserShots.Length)
                numBonus = spaceShipComp.laserShots.Length;
        }
    }

    public static void PrintDebug(string msg)
    {
        if(activedDebug)
            print(msg);
    }
}
