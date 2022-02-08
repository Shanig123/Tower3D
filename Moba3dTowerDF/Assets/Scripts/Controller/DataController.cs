using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    DataController()
       : base()
    {
        m_iSeedNumber = 0;     
    }
    ~DataController()
    {
        GFunc.Function.Print_Log("Free DataController.");
        Game_Manager.Instance.m_iStageSeed = 0;     
    }

    #region Value

    [SerializeField] private int m_iRandomIntCallCount = 0;
    [SerializeField] private int m_iRandomFloatCallCount = 0;
    [SerializeField] private int m_iRandomTotalCallCount = 0;
    [SerializeField] private int m_iSeedNumber;

    [SerializeField] private GameObject totalManager;
    #endregion
    #region Property

    public int Get_Seed { get { return m_iSeedNumber; } }
    public int Get_TotalCallCount { get { return m_iRandomTotalCallCount; } }

    #endregion

    private void Awake()
    {
        if (!GameObject.Find("Management"))
        {
            Instantiate(totalManager);
        }
        Game_Manager game_Manager;
        if (Game_Manager.Instance)
            game_Manager = Game_Manager.Instance;
        else
            game_Manager = GameObject.FindGameObjectWithTag("Management").GetComponent<Game_Manager>();

        if (0 == game_Manager.m_iStageSeed)
        {
            m_iSeedNumber = Time.time.GetHashCode();
            if (m_iSeedNumber == 0)
            {
                m_iSeedNumber = System.DateTime.Now.GetHashCode();
            }
        }
        else
        {
            m_iSeedNumber = Game_Manager.Instance.m_iStageSeed;
        }

        //¹ã SeedTest : 958020666
        //³· : 1815607085
        //m_iSeedNumber = 1815607085;

        UnityEngine.Random.InitState(m_iSeedNumber);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int TierRandomNumberFromSeed()
    {
        int iSeed = GameObject.FindWithTag("TotalController").GetComponent<DataController>().Get_Seed + m_iRandomIntCallCount;
        ++m_iRandomIntCallCount;
        ++m_iRandomTotalCallCount;

        return 0;
    }


    public int ExtractRandomNumberFromSeed(int _iMin, int _iMax)
    {
        int iSeed = GameObject.FindWithTag("TotalController").GetComponent<DataController>().Get_Seed + m_iRandomIntCallCount;
        ++m_iRandomIntCallCount;
        ++m_iRandomTotalCallCount;
        System.Random rd = new System.Random(iSeed);
        return rd.Next(_iMin, _iMax);
    }
    public float ExtractRandomNumberFromSeed()
    {
        int iSeed = GameObject.FindWithTag("TotalController").GetComponent<DataController>().Get_Seed + m_iRandomFloatCallCount;
        ++m_iRandomFloatCallCount;
        ++m_iRandomTotalCallCount;
        System.Random rd = new System.Random(iSeed);
        return (float)rd.NextDouble();
    }
}
