using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    #region Value

    [SerializeField] private int m_iIncomeKillCount = 0;
    [SerializeField] private int m_iTotalKillCount = 0;

    [SerializeField] private DataStruct.tagPlayerData m_tPlayerData;

    [SerializeField] private string m_strLayerMaskName = "";

    [SerializeField] private StageController m_stageController;

    [SerializeField] private DataEnum.eControl_Mode m_eNextControlState = DataEnum.eControl_Mode.End;
    [SerializeField] private DataEnum.eControl_Mode m_eCurControlState = DataEnum.eControl_Mode.End;

    [SerializeField] private bool m_bFirstInit = false;

    #endregion

    #region Property

    public int Get_TotalKillCount
    {
        get
        {
            return m_iTotalKillCount;
        }
    }
    public int Get_Gold
    {
        get
        {
            return m_tPlayerData.iGold;
        }
    }
    public int Set_Gold
    {
        set
        {
            m_tPlayerData.iGold = value;
        }
    }


    #endregion
    #region PropertyFunc
    public void Add_KillCount() { ++m_iIncomeKillCount; ++m_iTotalKillCount; }
    public int Get_SpecialCost(DataEnum.eRankID _eRankID)
    {
        if(_eRankID == DataEnum.eRankID.Normal)
        {
            print("Input Err. Player Controller.Get_SpecialCost. Err: -2");
            return -2;
        }
        else if(_eRankID == DataEnum.eRankID.Magic)
        {
            if(m_tPlayerData.iMagicCost< 0)
            {
                print("Input Err. Player Controller.Get_SpecialCost. Err: -3");
                return -3;
            }
            return m_tPlayerData.iMagicCost;
        }
        else if (_eRankID == DataEnum.eRankID.Rare)
        {
            if (m_tPlayerData.iRareCost < 0)
            {
                print("Input Err. Player Controller.Get_SpecialCost. Err: -3");
                return -3;
            }
            return m_tPlayerData.iRareCost;
        }
        else if (_eRankID == DataEnum.eRankID.Epic)
        {
            if (m_tPlayerData.iEpicCost < 0)
            {
                print("Input Err. Player Controller.Get_SpecialCost. Err: -3");
                return -3;
            }
            return m_tPlayerData.iEpicCost;
        }
        else if (_eRankID == DataEnum.eRankID.Unique)
        {
            if (m_tPlayerData.iUniqueCost < 0)
            {
                print("Input Err. Player Controller.Get_SpecialCost. Err: -3");
                return -3;
            }
            return m_tPlayerData.iUniqueCost;
        }
        else
        {
            print("Input Err. Player Controller.Get_SpecialCost. Err: -1");
            return -1;
        }
    }
    public void Add_Gold(int _iAddGoldVal)
    {
        m_tPlayerData.iGold += _iAddGoldVal;
        if(m_tPlayerData.iGold<0)
        {
            m_tPlayerData.iGold = 0;
        }
    }
    public int Add_SpecialCost(DataEnum.eRankID _eRankID, int _iAddCost = 1)
    {
        if (_eRankID == DataEnum.eRankID.Normal)
        {
            print("Input Err. Player Controller.Add_SpecialCost. Err: -2");
            return -2;
        }
        else if (_eRankID == DataEnum.eRankID.Magic)
        {
            m_tPlayerData.iMagicCost += _iAddCost;
            if (m_tPlayerData.iMagicCost < 0)
            {
                m_tPlayerData.iMagicCost = 0;
            }
            return 1;
        }
        else if (_eRankID == DataEnum.eRankID.Rare)
        {
            m_tPlayerData.iRareCost += _iAddCost;
            if (m_tPlayerData.iRareCost < 0)
            {
                m_tPlayerData.iRareCost = 0;
            }
            return 1;
        }
        else if (_eRankID == DataEnum.eRankID.Epic)
        {
            m_tPlayerData.iEpicCost += _iAddCost;
            if (m_tPlayerData.iEpicCost < 0)
            {
                m_tPlayerData.iEpicCost = 0;
            }
            return 1;
        }
        else if (_eRankID == DataEnum.eRankID.Unique)
        {
            m_tPlayerData.iUniqueCost += _iAddCost;
            if (m_tPlayerData.iUniqueCost < 0)
            {
                m_tPlayerData.iUniqueCost = 0;
            }
            return 1;
        }
        else
        {
            print("Input Err. Player Controller.Add_SpecialCost. Err: -1");
            return -1;
        }
    }
    #endregion

    void Start()
    {
        m_eNextControlState = DataEnum.eControl_Mode.NoControl;
        m_tPlayerData.iGold = 10000;
        //m_tPlayerData.iGold = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if(false == m_bFirstInit)
            FirstInit();

        ChangeController();
        DoController();


    }

    void  FirstInit()
    {
        m_bFirstInit = true;

    }

    void ChangeController()
    {
        if(m_eCurControlState != m_eNextControlState)
        {
            switch(m_eNextControlState)
            {
                case DataEnum.eControl_Mode.Construction:
                    {
                        m_strLayerMaskName = "Tower";
                    }
                    break;

                case DataEnum.eControl_Mode.NoControl:
                    {
                        m_strLayerMaskName = "Tile";
                    }
                    break;
                default:
                    break;
            }
            m_eCurControlState = m_eNextControlState;
        }
    }

    void DoController()
    {
        switch (m_eCurControlState)
        {
            case DataEnum.eControl_Mode.Construction:
                {

                }
                break;
            case DataEnum.eControl_Mode.NoControl:
                {

                }
                break;
            default:
                break;
        }
    }
    void RayPicking()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
 
        int iLayerMask = (1 << LayerMask.NameToLayer(m_strLayerMaskName)) | (1 << LayerMask.NameToLayer("UI"));

        if (Physics.Raycast(ray, out hit, 20.0f, iLayerMask))
        {

            print("Picked object name: " + hit.transform.name + ", position: " + hit.transform.position + "   " + iLayerMask);

        }
    }
}
