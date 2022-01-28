using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    #region Value
    private static Game_Manager m_cInstance = null;

    // public static UI_Manager CUI_Mgr = UI_Manager.Instance;
    // public static Object_Manager CObj_Mgr = Object_Manager.Instance;
    // public static Controller_Manager CCtrl_Mgr = Controller_Manager.Instance;

    // public float fDeltaTime = Time.deltaTime; 
    public int m_iStageSeed = 0;

    public DataStruct.tagGameData m_tGameData;
    public DataStruct.tagStageInfo m_tStageInfo;
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
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        m_tGameData.bArrUnlockAbility = new bool[21];
        m_tGameData.bArrUnlockAbility[0] = true;
        m_tGameData.bArrUnlockAbility[3] = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AppQuit()
    {
        Resource_Manager.Instance.SaveData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
                Application.OpenURL("some url such as your homepage");
#else
                Application.Quit();
#endif

    }
}
