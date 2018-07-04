using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipComp : MonoBehaviour {

    [SerializeField]
    Vector3 goposition;

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private ParticleSystem explosion;

    [SerializeField]
    [Tooltip("Environment Laser Shot")]
    public GameObject[] laserShots;

    [SerializeField]
    [Tooltip("Inteval time between Laser Shot")]
    [Range(0, 1)]
    public float intevalTimeSeconds = 0.1f;

    private AudioSource audioSource;

    private LevelControllerComp levelControllerComp;

    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    private int numHits;

    private long timeReference;

    private bool isKeyBoard = false;

    // Use this for initialization
    void Start()
    {
        levelControllerComp = FindObjectOfType<LevelControllerComp>();
        LevelControllerComp.PrintDebug("SpaceShipComp.Start ");

        numHits = 0;
        timeReference = DateTime.Now.ToFileTime();

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) {
            isKeyBoard = true;
           
        }else if (Input.GetButtonDown("Fire1")) {
            isKeyBoard = false;
        }

        if (isKeyBoard) {
            KeyboardMovement();
        }else
            MouseMovement();
        //SendLaserShot();

        //TouchMovement();

    }

    public void SendLaserShot()
    {
        if (levelControllerComp.GameStarted && !levelControllerComp.GamePaused)
        {
           
            if (Input.GetButtonDown("Fire1") || (Input.GetKeyDown(KeyCode.Space))) {
                goposition = this.gameObject.transform.position;
                //goposition.y = 3;
                GameObject newLaserShot = Instantiate(laserShots[levelControllerComp.numBonus], goposition, Quaternion.identity);
                LaserShotComp laserShotComp = newLaserShot.GetComponent<LaserShotComp>();
                laserShotComp.StarShot();
            }
        }
    }


    private void MouseMovement()
    {
        //LevelControllerComp.PrintDebug("SpaceShipComp.MouseMovement ");
        float mousePosWorldUnitX = ((Input.mousePosition.x) / Screen.width * 16);
        Vector2 spaceShipPos = new Vector2(0, transform.position.y);
        spaceShipPos.x = Mathf.Clamp(mousePosWorldUnitX, 1f, 15f);
        transform.position = spaceShipPos;
    }

    private void TouchMovement()
    {
        if (Input.touchCount > 0)
        {
            LevelControllerComp.PrintDebug("SpaceShipComp.TouchMovement ");
            Touch touch = Input.touches[0];
            float touchPosWorldUnitX = ((touch.position.x) / Screen.width * 16);
            Vector2 spaceShipPos = new Vector2(Mathf.Clamp(touchPosWorldUnitX, 0f, 15f), transform.position.y);
            transform.position = spaceShipPos;
        }
    }

    private void KeyboardMovement()
    {
        /* LevelControllerComp.PrintDebug("SpaceShipComp.KeyboardMovement ");
         var horizontalStimulus = Input.GetAxis("Horizontal");
         float resultLateralSpeed = horizontalStimulus * levelControllerComp.lateralSpeed;
         rb.AddForce(new Vector2(resultLateralSpeed, 0));*/

        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");

        Vector2 direcao = new Vector2(eixoX, eixoZ);

        transform.Translate(direcao * 10 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<ShieldComp>())
        {
            LevelControllerComp.PrintDebug("SpaceShipComp.OnCollisionEnter2D - Shield = " + gameObject.name + " - " + collision.gameObject.name);
            numHits = levelControllerComp.numShield + 1;
            LevelControllerComp.PrintDebug("SpaceShipComp.OnCollisionEnter2D - Shield : numHits " + numHits);
            LoadSprite();
            return;
        }
        else if (collision.gameObject.GetComponent<AsteroidComp>())
        {
            levelControllerComp.numBonus = 0;
            LevelControllerComp.PrintDebug("SpaceShipComp.OnCollisionEnter2D - Asteroid = " + gameObject.name + " - " + collision.gameObject.name);
            if (!levelControllerComp.IsShieldActivated())
            {
                numHits = sprites.Length + 1;
            }

            numHits++;
            int maxHits = sprites.Length + 1;
            if (numHits >= maxHits)
            {
                //Destroy(gameObject);
                ExplosionEffect();
                numHits = 0;
                LoadSprite();
                levelControllerComp.ResetGame();
            }
            else
            {
                LoadSprite();
            }
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            return;
        }
        else
        {
            LevelControllerComp.PrintDebug("SpaceShipComp.OnCollisionEnter2D ## " + gameObject.name + " - " + collision.gameObject.name);
        }
        
    }

    private void ExplosionEffect()
    {
        LevelControllerComp.PrintDebug("SpaceShipComp.ExplosionEffect ");
        if (explosion)
        {
            ParticleSystem ps = Instantiate<ParticleSystem>(explosion, transform.position, Quaternion.identity);
            ParticleSystem.MainModule main = ps.main;
            main.startColor = spriteRenderer.color;
        }
    }

    private void LoadSprite()
    {
        LevelControllerComp.PrintDebug("SpaceShipComp.LoadSprite ");
        int spriteIndex = numHits - 1;
        if (spriteIndex > sprites.Length || spriteIndex < 0)
            spriteIndex = 0;
        if (sprites[spriteIndex])
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

}
