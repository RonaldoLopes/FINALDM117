using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicControllerComp : MonoBehaviour {


    public AudioClip[] audioClip;
    
    AudioSource audioSource;
  

    public static MusicControllerComp musicControllerComp = null;


    private void Awake()
    {
        int indexScene = SceneManager.GetActiveScene().buildIndex;
        print("*************************************** " + indexScene.ToString());
        playSoundByScene(indexScene);

        if (musicControllerComp != null)
        {
            Destroy(gameObject);
        }
        else
        {
            musicControllerComp = this;
            DontDestroyOnLoad(gameObject);
        }
    }

   
    private void Start()
    {       
       
    }
    public void playSoundByScene(int indexScene)
    {
        audioSource = new AudioSource();
        audioSource = gameObject.AddComponent<AudioSource>();
        

        if (indexScene == 0) {
            audioSource.clip = audioClip[0];
            audioSource.Play();
        }else if(indexScene == 1) {
            audioSource.Stop();
            audioSource.clip = audioClip[1];
            audioSource.Play();
        } else if (indexScene == 2) {
            audioSource.Stop();
            audioSource.clip = audioClip[2];
            audioSource.Play();
        } else if (indexScene == 3) {
            audioSource.Stop();
            audioSource.clip = audioClip[3];
            audioSource.Play();
        } else if (indexScene == 4) {
            audioSource.Stop();
            audioSource.clip = audioClip[4];
            audioSource.Play();
        } else if (indexScene == 5) {
            audioSource.Stop();
            audioSource.clip = audioClip[5];
            audioSource.Play();
        } else if (indexScene == 6) {
            audioSource.Stop();
            audioSource.clip = audioClip[7];
            audioSource.Play();
        }


    }
    // Update is called once per frame
    void Update()
    {

    }
}
