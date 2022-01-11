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
    #region Value

    [SerializeField] private int m_iRandomCallCount = 0;
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
        m_iSeedNumber = Time.time.GetHashCode();
        if (m_iSeedNumber == 0)
        {
            m_iSeedNumber = System.DateTime.Now.GetHashCode();
        }
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
        int iSeed = GameObject.FindWithTag("TotalController").GetComponent<DataController>().Get_Seed + m_iRandomCallCount;
        ++m_iRandomCallCount;
        System.Random rd = new System.Random(iSeed);
        return rd.Next(_iMin, _iMax);
    }
    public float ExtractRandomNumberFromSeed()
    {
        int iSeed = GameObject.FindWithTag("TotalController").GetComponent<DataController>().Get_Seed + m_iRandomCallCount;
        ++m_iRandomCallCount;
        System.Random rd = new System.Random(iSeed);
        return (float)rd.NextDouble();
    }
}
