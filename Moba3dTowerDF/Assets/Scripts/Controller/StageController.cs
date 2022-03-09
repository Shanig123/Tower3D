using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    StageController()
        :base()
    {
        m_eLevel = DataEnum.eDifficulty.End;

        m_iCurWave = 0;
     //   m_iNextWave = 0;
        m_iCreateCount = 0;
        m_iMaxCreateCount = 0;
        m_iCurMobCount = 0;

        m_fStageTimer = 0;
        m_fWaitTimer = 0;

      //  m_bUpdateInit = false;
        m_bCreateModeOnOff = false;
        m_bWaveOnOff = false;

    }
    #region Value

    [SerializeField] public bool m_bWaveStart = false;

    [SerializeField] private DataEnum.eDifficulty m_eLevel;
    [SerializeField] private int m_iMaxWave;

    [SerializeField] private int m_iCurWave;
   // [SerializeField] private int m_iNextWave;

    [SerializeField] private string m_strWaveName;

    private int m_iCurTimerCount;
    private int m_iPreTimerCount;
    [SerializeField] private int m_iCreateCount;
    [SerializeField] private int m_iMaxCreateCount;
    [SerializeField] private int m_iCurMobCount;

    [SerializeField] private float m_fStageTimer;
    [SerializeField] private float m_fWaitTimer;
    [SerializeField] private float m_fWaitTimeMax;
    [SerializeField] private float m_fCreateCoolTime;
    [SerializeField] private float m_fCreateCoolTimeMax;

    [SerializeField] private Object_Manager m_Object_Manager;
    
    [SerializeField] private bool m_bCreateModeOnOff;
    [SerializeField] private bool m_bWaveOnOff;

    #endregion
    #region Property
    public DataEnum.eDifficulty Get_Difficulty
    {
        get
        {
            return m_eLevel;
        }
    }
    public DataEnum.eDifficulty Set_Difficulty
    {
        set
        {
            m_eLevel = value;
        }
    }
    public int Get_Wave
    {
        get
        {
            return m_iCurWave;
        }
    }
    public int Get_MobCount
    {
        get
        {
            return m_iCurMobCount;
        }
    }
    public int Get_WaveMaxCreateCount
    {
        get
        {
            return m_iMaxCreateCount;
        }
    }
    public bool Get_WaveOnOff
    {
        get
        {
            return m_bWaveOnOff;
        }
    }


    public void Add_MobCount()
    {
        ++m_iCurMobCount;
    }
    public void Min_MobCount()
    {
        --m_iCurMobCount;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_Object_Manager = GameObject.FindGameObjectWithTag("Management").GetComponent<Object_Manager>();
        StageSettingInfo();
    }

    void StageSettingInfo()
    {
        m_eLevel = Game_Manager.Instance.m_tStageInfo.eDifficulty;

        if (Game_Manager.Instance.m_tStageInfo.eDifficulty == DataEnum.eDifficulty.Infinite)
        {
           m_iMaxWave = -1;
        }
        else if (Game_Manager.Instance.m_tStageInfo.eDifficulty == DataEnum.eDifficulty.Hard)
        {
            m_iMaxWave = 40;
        }
        else 
        {
            m_iMaxWave = 30;
        }       
    }
    // Update is called once per frame

    void Update()
    {
        m_fStageTimer += Time.deltaTime;
        WayPointRenderOff();
        CheckWait();
        CheckCreateWave();
    }

    private void CheckCreateWave()
    {
        
        if(m_bCreateModeOnOff)
        {
            //웨이브 시작 후 생성될 때
            //StartCoroutine(WaveCtrl());

            WaveCtrl();
        }
        else
        {
            //웨이브 몹 생성 완료일 때

            //if(m_iCurWave >0 )
            //{
                if (1 > m_iCurMobCount&& m_bWaveOnOff)
                {
                    if(m_Object_Manager.m_dictClone_Object.ContainsKey(m_strWaveName))
                    {
                        if (m_Object_Manager.m_dictClone_Object[m_strWaveName].Count > 0)
                        {
                            Debug.Log(m_Object_Manager.m_dictClone_Object[m_strWaveName].Count);
                        }
                        m_Object_Manager.m_dictClone_Object.Remove(m_strWaveName);
                        if (!m_Object_Manager.m_dictClone_Object.ContainsKey(m_strWaveName))
                        {
                            Debug.Log(m_strWaveName+" Remove!");
                        }
                    }
                    m_bWaveOnOff = false;
                   // ++m_iCurWave;
                }
           // }
        }

    }

    private void CheckWait()
    {
        if (!m_bWaveOnOff && m_bWaveStart)
        {
            m_fWaitTimer += Time.deltaTime;
            if (m_fWaitTimer >1)
            {
                ++m_iCurTimerCount;
                m_fWaitTimer -= 1f;
            }
            if ((m_iPreTimerCount != m_iCurTimerCount) &&
                (m_fWaitTimeMax> m_iCurTimerCount))
            {
                m_iPreTimerCount = m_iCurTimerCount;
                Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.UI, 1, new Vector3(100, 100, 100));
            }
            if (m_fWaitTimeMax < (m_fWaitTimer+(float)m_iCurTimerCount))
            {
                m_bWaveStart = false;
                m_bWaveOnOff = true;
                m_bCreateModeOnOff = true;
                ++m_iCurWave;
                m_strWaveName = "Wave_" + m_iCurWave;
                List<GameObject> gameObjects = new List<GameObject>(m_iMaxCreateCount);
                m_Object_Manager.m_dictClone_Object.Add(m_strWaveName, gameObjects);
                Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.UI, 2, new Vector3(100,100,100));
            }
        }
        else
        { m_fWaitTimer = 0; m_iCurTimerCount = 0; m_iPreTimerCount = 0; }
    }
    
    private /*IEnumerator*/void WaveCtrl()
    {
      //  Debug.Log("WaveCreateInstancing");
        m_fCreateCoolTime += Time.deltaTime;
        if (m_fCreateCoolTimeMax < m_fCreateCoolTime)
        {
            m_fCreateCoolTime = 0;

            //몬스터 생성
    
            Vector3 vPos = m_Object_Manager.m_dictClone_Object["CreateZone"][0].GetComponent<Transform>().position;

            string strWaveName = "Wave_";
            if (m_iCurWave < 10)
                strWaveName += ("0" + m_iCurWave);
            else
                strWaveName += m_iCurWave;
            GameObject TempGameObject = 
                m_Object_Manager.InstanceObject(vPos, m_strWaveName, "Wave_Monster", strWaveName, m_iCreateCount);
            MobAI Ai = TempGameObject.GetComponent<MobAI>();

            Ai.SetState = DataEnum.eState.Ready;

            //Ai.m_bCheckDead = false;
            //Ai.m_bObjActiveOnOff = true;

            TempGameObject.GetComponent<Transform>().position = vPos;
            string objName = TempGameObject.name +"_"+ m_iCreateCount;
            TempGameObject.name = objName;

            ++m_iCreateCount;
            ++m_iCurMobCount;
          //  yield return null;
        }
        if((m_iCurWave%10) == 0)
        {
            if(m_iCreateCount >= 1)
            {
                m_iCreateCount = 0;
                m_bCreateModeOnOff = false;
            }
           
        }
        else if (m_iMaxCreateCount <= m_iCreateCount)
        {
            m_iCreateCount = 0;
            m_bCreateModeOnOff = false;
        }
        //yield return null;
    } 

    private void WayPointRenderOff()
    {
        if (m_fStageTimer > 6.0f)
        {
            m_Object_Manager.WayPointsRenderOnOff(false);
            return;
        }

        int iFlickerCount = 3;
        float fStageTimer = m_fStageTimer * iFlickerCount;
        float fAlpha = (-(Mathf.Cos(fStageTimer)) + 1) * 0.5f;
        m_Object_Manager.WayPointsColor(new Vector4(0, 1, 0, fAlpha*0.3f));
    }
}
