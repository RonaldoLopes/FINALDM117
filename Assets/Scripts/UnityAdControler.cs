using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdControler : MonoBehaviour
{

    //Tipo que pode ser null
    public static DateTime? nextTimeReward = null;

    /// <summary>
    /// Variavel de controle se devemos ou nao mostrar ads
    /// </summary>
    public static bool showAds = true;

    /// <summary>
    /// Metodo para invocar anuncios
    /// </summary>
    public static void ShowAd()
    {

#if UNITY_ADS

        //Opcoes para o ad
        ShowOptions options = new ShowOptions();
        options.resultCallback = Unpause;

        if (Advertisement.IsReady())
        {
            //Mostra o anuncio
            Advertisement.Show(options);
        }
        //Pausar o jogo enquanto
        //o ad esta sendo mostrad
        LevelControllerComp.PauseGame(true);
#endif
    }
    /// <summary>
    /// Metodo para mostrar ad com recompensa
    /// </summary>
    public static void ShowRewardAd()
    {

#if UNITY_ADS

        nextTimeReward = DateTime.Now.AddSeconds(15);
        if (Advertisement.IsReady())
        {
            // Pausar o jogo
            LevelControllerComp.PauseGame(true);
            //Outra forma de criar a 
            //instancia do ShowOptions e setar o callback
            var opcoes = new ShowOptions
            {
                resultCallback = TratarMostrarResultado
            };

            Advertisement.Show(opcoes);
        }
#endif
    }

    private static void Unpause(ShowResult obj)
    {
        //Quando o anuncio acabar 
        //sai do modo paused
        LevelControllerComp.PauseGame(false);
    }

#if UNITY_ADS
    public static void TratarMostrarResultado(ShowResult result)
    {

        switch (result)
        {
            case ShowResult.Finished:
                // Anuncio mostrado. Continue o jogo
                GameObject.FindObjectOfType<LevelControllerComp>().Continue();
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad pulado. Faz nada");
                break;
            case ShowResult.Failed:
                Debug.LogError("Erro no ad. Faz nada");
                break;
        }

        // Saia do modo paused
        LevelControllerComp.PauseGame(false);
    }
#endif

}
