using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    Game_Manager()
        : base()
    {
        m_tStageInfo.iStartAbility = 0;
        m_tGameData = GFunc.Function.InitGameData();

        m_tDefaultGameData = GFunc.Function.InitGameData();
        m_tDefaultGameData.bArrUnlockAbility[0] = true;
    }
    #region Value
    private static Game_Manager m_cInstance = null;

    // public static UI_Manager CUI_Mgr = UI_Manager.Instance;
    // public static Object_Manager CObj_Mgr = Object_Manager.Instance;
    // public static Controller_Manager CCtrl_Mgr = Controller_Manager.Instance;

    // public float fDeltaTime = Time.deltaTime; 
    public int m_iStageSeed = 0;

    public DataStruct.tagGameData m_tGameData;
    [SerializeField]
    private DataStruct.tagGameData m_tDefaultGameData;
    public DataStruct.tagStageInfo m_tStageInfo;

    private bool m_bAppPause = false;
    #endregion
    #region Property
    public static Game_Manager Instance
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

    public DataStruct.tagGameData Get_DefaultGameData { get { return m_tDefaultGameData; } }

    #endregion

    private void OnDestroy()
    {

    }

    #region Awake
    void Awake()
    {
        if (null == m_cInstance)
        {
            m_cInstance = this;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            if (m_tGameData.bArrUnlockAbility[0] == false)
                m_tGameData.bArrUnlockAbility[0] = true;

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

    // Start is called before the first frame update
    void Start()
    {
        //  Screen.SetResolution(1920, 1080, true);
      

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void CheckScene()
    {
        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            Resource_Manager.Instance.SaveData();
            //메인메뉴 씬 진입 시 초기화 해야 할 값들을 지정해야함.

        }
        else
        {
        }
        
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
        {
            m_bAppPause = true;
            Resource_Manager.Instance.SaveData();
            Option_Manager.Instance.SaveFileData();

            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                Game_Pause(true);
                GameObject.Find("Detect_SettingBoard").GetComponent<Detect_SettingBoard>().m_mainSceneSetting.OnClick_SettingBoard();
            }
        }
        else
        {
            if(m_bAppPause)
            {
                m_bAppPause = false;

            
            }
        }
      
    }

    public void AppQuit()
    {
        Resource_Manager.Instance.SaveData();
        Option_Manager.Instance.SaveFileData();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL("some url such as your homepage");
#else
            Application.Quit();
#endif

    }
    
    


    public void Game_Pause(bool _bPause)
    {
        if (_bPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
