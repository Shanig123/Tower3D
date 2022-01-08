using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    [SerializeField] private int m_iRandomCallCount = 0;

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
        int iSeed = GameObject.FindWithTag("TotalController").GetComponent<StageController>().Get_Seed + m_iRandomCallCount;
        ++m_iRandomCallCount;
        System.Random rd = new System.Random(iSeed);
        return rd.Next(_iMin, _iMax);
    }
    public float ExtractRandomNumberFromSeed()
    {
        int iSeed = GameObject.FindWithTag("TotalController").GetComponent<StageController>().Get_Seed + m_iRandomCallCount;
        ++m_iRandomCallCount;
        System.Random rd = new System.Random(iSeed);
        return (float)rd.NextDouble();
    }
}
