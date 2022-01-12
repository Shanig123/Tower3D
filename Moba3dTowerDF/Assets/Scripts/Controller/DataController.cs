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
        print("Free DataController.");
        Game_Manager.Instance.m_iStageSeed = 0;     
    }

    #region Value

    [SerializeField] private int m_iRandomIntCallCount = 0;
    [SerializeField] private int m_iRandomFloatCallCount = 0;
    [SerializeField] private int m_iSeedNumber;

    #endregion
    #region Property

    public int Get_Seed
    {
        get
        {
            return m_iSeedNumber;
        }
    } 

    #endregion

    private void Awake()
    {
        if (0 == Game_Manager.Instance.m_iStageSeed)
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
        m_iSeedNumber = 958020666;
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

    public int ExtractRandomNumberFromSeed(int _iMin, int _iMax)
    {
        int iSeed = GameObject.FindWithTag("TotalController").GetComponent<DataController>().Get_Seed + m_iRandomIntCallCount;
        ++m_iRandomIntCallCount;
        System.Random rd = new System.Random(iSeed);
        return rd.Next(_iMin, _iMax);
    }
    public float ExtractRandomNumberFromSeed()
    {
        int iSeed = GameObject.FindWithTag("TotalController").GetComponent<DataController>().Get_Seed + m_iRandomFloatCallCount;
        ++m_iRandomFloatCallCount;
        System.Random rd = new System.Random(iSeed);
        return (float)rd.NextDouble();
    }
}
