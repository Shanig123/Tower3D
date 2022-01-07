using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_MainScene : MonoBehaviour
{

   public void GoToGameScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void AppQuit()
    {
        Game_Manager.Instance.AppQuit();

    }
}
