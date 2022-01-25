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
    public bool Construct_Normal()
    {
        if (!m_stageController.Get_WaveOnOff)
            return m_construction.CallTower(DataEnum.eRankID.Normal);
        return false;
    }
    public void Construct_Normal2()
    {
        //if (!m_stageController.Get_WaveOnOff)
             m_construction.CallTower(DataEnum.eRankID.Normal);
    }

    public bool Construction_Tower(DataEnum.eRankID _eRankId)
    {
        if (!m_stageController.Get_WaveOnOff)
        {
            return m_construction.CallTower(_eRankId);
        }
        return false;
    }

    public bool Construction_Tower(DataEnum.eRankID _eRankId, int _iTowernum)
    {
        if (!m_stageController.Get_WaveOnOff)
        {
            return m_construction.CallTower(_eRankId, _iTowernum);
        }
        return false;
    }

    public bool Construction_Tower_NoAwait(DataEnum.eRankID _eRankId, Vector3 _vCreatePos)
    {
        if (!m_stageController.Get_WaveOnOff)
        {
            return m_construction.CallTowerNotInAwait(_eRankId,_vCreatePos);
        }
        return false;
    }

    public void AotoInBoard()
    {
        m_construction.AutoInBoard();
    }

    //public bool Construction_Tower_NoAwait(DataEnum.eRankID _eRankId, Vector3 _vCreatePos, int _iTowernumMin, int _iTowernumMax)
    //{
    //    if (!m_stageController.Get_WaveOnOff)
    //    {
    //        return m_construction.CallTowerNotInAwait(_eRankId, _vCreatePos, _iTowernumMin, _iTowernumMax);
    //    }
    //    return false;
    //}
}
