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
        m_tDefaultGameData.bArrUnlockAbility[1] = true;
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
            //if (m_tGameData.bArrUnlockAbility[0] == false)
            //    m_tGameData.bArrUnlockAbility[0] = true;
            //if (m_tGameData.bArrUnlockAbility[1] == false)
            //    m_tGameData.bArrUnlockAbility[1] = true;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            /*
             * �� �̵� �� �̹� �ν��Ͻ��� �Ǿ� �ִٸ� �ı��Ѵ�.
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
