using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainControllerComp : MonoBehaviour {


    MusicControllerComp musicControllerComp;

    public void LoadSceneByNameWithAds(string nameScene)
    {
       
        
        SceneManager.LoadScene(nameScene);

        if (UnityAdControler.showAds)
        {
            UnityAdControler.ShowAd();
        }
    }

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}
   

}
