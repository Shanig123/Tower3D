using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Manager : MonoBehaviour
{
    // Start is called before the first frame update


    #region Value
    private static Controller_Manager m_cInstance = null;
#if UNITY_EDITOR
    public bool m_bDebugmode = false;
#endif

    #endregion
    #region Property
    public static Controller_Manager Instance
    {
        get
        {
            if (null == m_cInstance)
            {
                return null;
            }
            return m_cInstance;
        }
    }
    #endregion
    #region Awake
    void Awake()
    {
        if (null == m_cInstance)
        {
            m_cInstance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            /*
             * 씬 이동 후 이미 인스턴스가 되어 있다면 파괴한다.
             */
            Destroy(this.gameObject);
        }
    }
    #endregion
    

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        m_bDebugmode = true;
        DebugConsoleMode();
#endif
        if(Application.platform == RuntimePlatform.Android)
            AndroidKeyInput();
    }

    private void DebugConsoleMode()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
        }
    }

    private void AndroidKeyInput()
    {
        if(Input.GetKey(KeyCode.Home))
        {
            //Android HomeKey
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            //Android Escape
            Game_Manager.Instance.AppQuit();
        }
        else if (Input.GetKey(KeyCode.Menu))
        {
            //Android Menu
        }
    }

}
