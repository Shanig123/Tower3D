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
        m_iNextWave = 0;
        m_iCreateCount = 0;
        m_iMaxCreateCount = 0;
        m_iCurMobCount = 0;

        m_fStageTimer = 0;
        m_fWaitTimer = 0;

      //  m_bUpdateInit = false;
        m_bCreateModeOnOff = false;
        m_bWaveOnOff = false;

        m_bDayNight = false;
    }
    #region Value

    [SerializeField] public bool m_bWaveStart = false;

    [SerializeField] private DataEnum.eDifficulty m_eLevel ;

    [SerializeField] private int m_iCurWave;
    [SerializeField] private int m_iNextWave;

    [SerializeField] private string m_strWaveName;

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

    [SerializeField] private bool m_bDayNight;

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

    //true = day / false = night
    public bool Get_DayNight
    {
        get
        {
            return m_bDayNight;
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

        int iSeed = GameObject.FindGameObjectWithTag("TotalController").GetComponent<DataController>().Get_Seed;
        System.Random rd = new System.Random(iSeed);
        m_bDayNight = rd.Next(0, 2) > 0 ? true : false;

    }
    // Update is called once per frame

    void Update()
    {
        m_fStageTimer += Time.deltaTime;

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
            if(m_fWaitTimeMax < m_fWaitTimer)
            {
                m_bWaveStart = false;
                m_bWaveOnOff = true;
                m_bCreateModeOnOff = true;
                ++m_iCurWave;
                m_strWaveName = "Wave_" + m_iCurWave;
                List<GameObject> gameObjects = new List<GameObject>(m_iMaxCreateCount);
                m_Object_Manager.m_dictClone_Object.Add(m_strWaveName,gameObjects);
            }
        }
        else
            m_fWaitTimer = 0;
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
          
            GameObject TempGameObject = m_Object_Manager.InstanceObject(vPos, m_strWaveName, "Object", "TestMob00", m_iCreateCount);
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
        if (m_iMaxCreateCount <= m_iCreateCount)
        {
            m_iCreateCount = 0;
            m_bCreateModeOnOff = false;
        }
        //yield return null;
    } 
}
