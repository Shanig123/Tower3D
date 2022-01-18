using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  
    /*
     * ó�� ��ŷ�� Ÿ���� ���� ���̶�� -> ��������� ��ġ�ϴ� ����̿�����.
     * �� ���� ���̶�� Ÿ���� ��������� ������ üũ�ؾ��� �� ����.
     * �� ª�� ������ �ÿ��� �۵�.
     * �� �ѹ��̶� ������Ʈ�� Ÿ�� ������ ���Դٸ� �ݵ�� ��ġ�ؾ���.
     * �� ������ ��ġ�Ǿ��ٸ� �Ǽ���Ʈ�ѷ����� �ش� ������Ʈ�� ����Ʈ ������ ���� �ʿ���.
     * 
     * 
     * ó�� ��ŷ�� Ÿ���� ���� ���̶�� -> ���� �ȿ��� �̵��ϰų� Ÿ���� ���� ��ġ�� ����̿�����.
     * �� �׷��ٸ� �̵��߿� ���� �� �ÿ� Ÿ���� ��ŷ�� �ȴٸ� ��ġ�� ������� �ϴ� ���� �մ���.
     * �� ��ĥ �� ����ó���� �ʿ���. ù ��ŷ�� ��ġ�� ������ �ʿ䰡 ����.
     * �� Ÿ�� ���� ���ý� ��� ������ ����� �ʿ���.
     * 
     * ui ���� �� �ܺ� �������� ��ġ���� �� �����ϴ� ��� �ʿ���.
     * 
     * */

    #region Value
    private const int INBOARD = 1;
    private const int OUTBOARD = 0;
    private const int OTHER = -1;

    [SerializeField] private int m_iIncomeKillCount = 0;
    [SerializeField] private int m_iTotalKillCount = 0;
    [SerializeField] private int m_iCheckPickingTower = OTHER;
    [SerializeField] private int m_iPick_AwaitBoxNumber = -1;

    [SerializeField] private DataStruct.tagPlayerData m_tPlayerData;

    [SerializeField] private string m_strLayerMaskName = "";

    [SerializeField] private DataEnum.ePickingMode m_eNextControlState = DataEnum.ePickingMode.End;
    [SerializeField] private DataEnum.ePickingMode m_eCurControlState = DataEnum.ePickingMode.End;

    //[SerializeField] private bool m_bFirstInit = false;

    [SerializeField] private GameObject m_objPickTower;
    [SerializeField] private Vector3 m_vPickTowerPos;
    [SerializeField] private GameObject m_objPicking;

    [SerializeField] private UpgradeController m_upgradeController;

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

    // Start is called before the first frame update
    void Start()
    {
        m_upgradeController = new UpgradeController();
        m_eNextControlState = DataEnum.ePickingMode.Obj_Tower;
        m_tPlayerData.iGold = 10000;
        //m_tPlayerData.iGold = 20;
    }

    // Update is called once per frame
    void Update()
    {
        //if(false == m_bFirstInit)
        //    UpdateInit();
        CheckInputMouse();
        ChangeController();
      
        DoController(false);

        DebugText();

    }

    private void DebugText()
    {
        UnityEngine.UI.Text tLog = GameObject.Find("GoldText").GetComponent<UnityEngine.UI.Text>();
        tLog.text = "Gold : " + m_tPlayerData.iGold +"\n"+"Life : " + m_tPlayerData.iLife + "\n";
    }

    //void  UpdateInit()
    //{
    //    m_bFirstInit = true;

    //}

    void ChangeController()
    {
        if(m_eCurControlState != m_eNextControlState)
        {
            switch(m_eNextControlState)
            {
                case DataEnum.ePickingMode.Obj_Tower:
                    {
                        m_objPicking = null;
                        m_objPickTower = null;
                        m_vPickTowerPos = Vector3.zero;
                        m_iCheckPickingTower = OTHER;
                        m_iPick_AwaitBoxNumber = -1;
                        m_strLayerMaskName = "Tower";
                    }
                    break;

                case DataEnum.ePickingMode.Tile:
                    {
                        m_objPicking = null;
                        m_strLayerMaskName = "Tile";
                    }
                    break;
                default:
                    break;
            }
            m_eCurControlState = m_eNextControlState;
        }
    }

    void DoController(bool _bCheckPicking)
    {
        switch (m_eCurControlState)
        {
            case DataEnum.ePickingMode.Obj_Tower:
                {
                    if (_bCheckPicking)
                        Picking_Tower();
                    else
                        Picked_Tower();
                }
                break;
            case DataEnum.ePickingMode.Tile:
                {
                    if (_bCheckPicking)
                    {
                        Picking_Tile();
                    }
                    else
                        Picked_Tile();
                }
                break;
            default:
                break;
        }
    }

    void CheckInputMouse()
    {
        
        float fClickTime =    Controller_Manager.Instance.Get_ClickTime;
        if(fClickTime > 1.0f)
        {

        }
        else
        {
           if( Controller_Manager.Instance.LButtonUp())
           {

           }
           else if(Controller_Manager.Instance.LButtonDown())
           {
                RayPicking();
           }
        }
    }

    public void Reset_PickingInfo()
    {
        m_objPicking = null;
        m_objPickTower = null;
        m_eNextControlState = DataEnum.ePickingMode.Obj_Tower;
        m_vPickTowerPos = Vector3.zero;
        m_iCheckPickingTower = OTHER;
        m_iPick_AwaitBoxNumber = -1;
        m_strLayerMaskName = "Tower";

        ChangeController();
    }

    void RayPicking()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
 
        int iLayerMask = (1 << LayerMask.NameToLayer(m_strLayerMaskName)) | (1 << LayerMask.NameToLayer("UI"));

        if (Physics.Raycast(ray, out hit, 20.0f, iLayerMask))
        {
            print("Picked object name: " + hit.transform.name + ", position: " + hit.transform.position + "   " + iLayerMask);

            if (hit.collider.gameObject)
            {
                m_objPicking = hit.collider.gameObject;
                DoController(true);
            }
            else
                m_objPicking = null;
        }
        else
        {
            Reset_PickingInfo();
        }
    }

    public bool RayPicking(string _strLayerMask)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int iLayerMask = (1 << LayerMask.NameToLayer(_strLayerMask)) | (1 << LayerMask.NameToLayer("UI"));

        if (Physics.Raycast(ray, out hit, 20.0f, iLayerMask))
        {
            print("Picked object name: " + hit.transform.name + ", position: " + hit.transform.position + "   " + iLayerMask);

            if (hit.collider.gameObject)
                m_objPicking = hit.collider.gameObject;

            return true;
        }
        return false;
    }

    public GameObject RayPicking(string _strLayerMask, RaycastHit _RayHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int iLayerMask = (1 << LayerMask.NameToLayer(_strLayerMask)) | (1 << LayerMask.NameToLayer("UI"));

        if (Physics.Raycast(ray, out _RayHit, 20.0f, iLayerMask))
        {
            print("Picked object name: " + _RayHit.transform.name + ", position: " + _RayHit.transform.position + "   " + iLayerMask);

            return _RayHit.collider.gameObject;
        }
        return null;
    }

    private void Picking_Tower()
    {
        print("picking Tower");
        m_objPickTower = m_objPicking;
        m_eNextControlState = DataEnum.ePickingMode.Tile;
        
        Check_TowerInBoard();
        Check_Exception_TowerInBoard();

        m_objPicking = null;
    }

    private void Picked_Tower()
    {

    }

    private void Check_TowerInBoard()
    {
       if((m_objPickTower.GetComponent<TowerAI>().Get_TowerInfo.iStatus & GConst.BaseValue.iStatFlag_CheckInStage)
                == GConst.BaseValue.iStatFlag_CheckInStage)
        {
            m_iCheckPickingTower = INBOARD;
            m_vPickTowerPos = m_objPickTower.transform.position;
        }
       else if((m_objPickTower.GetComponent<TowerAI>().Get_TowerInfo.iStatus & GConst.BaseValue.iStatFlag_CheckInStage)
                != GConst.BaseValue.iStatFlag_CheckInStage)
        {
            m_iCheckPickingTower = OUTBOARD;
            Check_AwaitBoxNumber();
            m_vPickTowerPos = m_objPickTower.transform.position;
        }
    }

    private void Check_AwaitBoxNumber()
    {
        Vector3 vPickPos = m_objPickTower.transform.position;

        vPickPos.y += 1.0f;
  
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int iLayerMask = 1 << LayerMask.NameToLayer("Box");
      
        if (Physics.Raycast(m_objPickTower.transform.position, -m_objPickTower.transform.up , out hit, 5.0f, iLayerMask))
        {
            if (hit.collider.gameObject)
            {
                string strBoxName = hit.collider.gameObject.name;
                string strBoxNumber = null;

                int i = 0;
                int iNumber = -1;
                while (true)
                {
                    string strTemp = null;
                    strTemp = strBoxName[strBoxName.Length - (i+1)].ToString();

                    if(strTemp == "_")
                    {
                        break;
                    }
                    strBoxNumber = strTemp + strBoxNumber;
                    ++i;
                }
                int.TryParse(strBoxNumber, out iNumber);

                if (iNumber < 0)
                    return;
               
                //
                print(strBoxName + "\n" + iNumber);
                m_iPick_AwaitBoxNumber = iNumber;
            }

        }
    }

    private void Check_Exception_TowerInBoard()
    {
        if (OTHER == m_iCheckPickingTower)
        {
            m_eNextControlState = DataEnum.ePickingMode.Obj_Tower;
            m_vPickTowerPos = Vector3.zero;
            m_objPickTower = null;
        }
    }

    private void Picking_Tile()
    {
        print("picking Tile");
        if (INBOARD == m_iCheckPickingTower)
        {
            if (Tile_Check_InTower())
                return;

            Vector3 vPickPos = m_objPicking.transform.position;
            vPickPos.y = 0;
            m_objPickTower.transform.position = vPickPos;

            m_eNextControlState = DataEnum.ePickingMode.Obj_Tower;
        }
        else if(OUTBOARD == m_iCheckPickingTower) //���� �ۿ� �ִ� ���� ������ �ű�� ���
        {
            bool bCheck = Tile_Check_InTower();
            if (bCheck) // �ѹ��� ������ŷ�� �Ͽ� �ش� Ÿ�Ͽ� ������Ʈ�� �ִ� �� Ȯ��.
            {
               
                return; // ������Ʈ�� �ִٸ� �ٷ� ������.
            }
            

            Vector3 vPickPos = m_objPicking.transform.position;
            vPickPos.y = 0;
            m_objPickTower.transform.position = vPickPos;

            GameObject.FindWithTag("TotalController").GetComponent<ConstructionController>().Sort_AwaitList(m_iPick_AwaitBoxNumber);
            m_eNextControlState = DataEnum.ePickingMode.Obj_Tower;
            
        }
    }
    private void Picked_Tile()
    {
        if (INBOARD == m_iCheckPickingTower)
        {
            if(RayPicking("Tower",new RaycastHit()))
            {

            }
        }
        else if (OUTBOARD == m_iCheckPickingTower)
        {
            //GameObject pickedObj = RayPicking("Tower", new RaycastHit());
            //if (pickedObj)
            //{

            //}
        }
      

    }

    private bool Tile_Check_InTower()
    {
        if (RayPicking("Tower")) // �ѹ��� ������ŷ�� �Ͽ� �ش� Ÿ�Ͽ� ������Ʈ�� �ִ� �� Ȯ��.
        {
            //Ÿ�� ���� -> ���� ���
            if (m_objPicking.GetComponent<TowerAI>().m_strPrefabName ==
                m_objPickTower.GetComponent<TowerAI>().m_strPrefabName)
            {
                //Ŭ�и� üũ�ؾ���.
                if(m_objPicking.name == m_objPickTower.name)
                {
                    print("sameObject");
                    return false;
                }

                TowerStatUp();
                //Ÿ���� ���� Ÿ����� Ÿ�� ����
            }
            else //�ٸ��ٸ� Ƽ�� üũ �� ���� Ƽ���� Ƽ� ����
            {
                CheckTowerRank();
            }

            m_eNextControlState = DataEnum.ePickingMode.Obj_Tower;
            return true; // ������Ʈ�� �ִٸ� �ٷ� ������.
        }
        return false;
    }

    private void CheckTowerRank()
    {
        //
        if (m_objPicking.GetComponent<TowerAI>().Get_TowerInfo.iTowerId < 0)
            return;
        int iPickingTowerRank = (int)((m_objPicking.GetComponent<TowerAI>().Get_TowerInfo.iTowerId + 10) *0.1f);
        int iPickedTowerRank = (int)((m_objPickTower.GetComponent<TowerAI>().Get_TowerInfo.iTowerId + 10) * 0.1f);

        if (iPickedTowerRank != iPickingTowerRank)  //Ÿ�� ��ũ�� �ٸ��ٸ� ����
            return;

        if(iPickingTowerRank >0 && iPickingTowerRank<5)
        {
            CreateRankUpTower();
        }
        else
        {
            return;
        }
    }
    private void CreateRankUpTower()
    {
        GameObject objEvent = GameObject.FindGameObjectWithTag("EventActor");
        GameObject.FindWithTag("TotalController").GetComponent<ConstructionController>().Sort_AwaitList(m_iPick_AwaitBoxNumber);

        if (objEvent.GetComponent<Constructor>().Construction_Tower(DataEnum.eRankID.Normal))
        {
            Destroy(m_objPicking);
            Destroy(m_objPickTower);
        }
    }
    
    private void TowerStatUp()
    {
        DataStruct.tagTowerStatus towerinfo =    m_objPicking.GetComponent<TowerAI>().Get_TowerInfo;
        ++towerinfo.iLvl;
        m_objPicking.GetComponent<TowerAI>().Set_TowerInfo = towerinfo;
        Destroy(m_objPickTower);
        GameObject.FindWithTag("TotalController").GetComponent<ConstructionController>().Sort_AwaitList(m_iPick_AwaitBoxNumber);
    }

    private void Sell_Tower()
    {

    }
}
