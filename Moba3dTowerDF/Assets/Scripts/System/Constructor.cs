using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructor : MonoBehaviour
{
    [SerializeField]
    private ConstructionController m_construction;
    [SerializeField]
    private StageController m_stageController;

    // Start is called before the first frame update
    void Start()
    {
        m_construction = GameObject.FindWithTag("TotalController").GetComponent<ConstructionController>();
        m_stageController = GameObject.FindWithTag("TotalController").GetComponent<StageController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Construct_Normal()
    {
        if (!m_stageController.Get_WaveOnOff)
            m_construction.CallTower(DataEnum.eRankID.Normal);
    }
}
