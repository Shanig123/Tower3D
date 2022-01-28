using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  
    /*
     * 처음 피킹한 타워가 보드 밖이라면 -> 보드안으로 배치하는 기능이여야함.
     * ㄴ 보드 밖이라면 타워가 보드밖인지 안인지 체크해야할 수 있음.
     * ㄴ 짧게 눌렀을 시에만 작동.
     * ㄴ 한번이라도 오브젝트가 타워 안으로 들어왔다면 반드시 배치해야함.
     * ㄴ 안으로 배치되었다면 건설컨트롤러에서 해당 오브젝트를 리스트 내에서 제거 필요함.
     * 
     * 
     * 처음 피킹항 타워가 보드 안이라면 -> 보드 안에서 이동하거나 타워를 서로 합치는 기능이여야함.
     * ㄴ 그렇다면 이동중에 손을 뗄 시에 타워가 피킹이 된다면 합치는 기능으로 하는 것이 합당함.
     * ㄴ 합칠 때 예외처리가 필요함. 첫 피킹시 위치를 가져올 필요가 있음.
     * ㄴ 타워 안을 선택시 길게 누르는 기능이 필요함.
     * 
     * ui 관련 및 외부 지형들을 터치했을 때 리셋하는 기능 필요함.
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
            GFunc.Function.Print_Log("Input Err. Player Controller.Get_SpecialCost. Err: -2");
            return -2;
        }
        else if(_eRankID == DataEnum.eRankID.Magic)
        {
            if(m_tPlayerData.iMagicCost< 0)
            {
                GFunc.Function.Print_Log("Input Err. Player Controller.Get_SpecialCost. Err: -3");
                return -3;
            }
            return m_tPlayerData.iMagicCost;
        }
        else if (_eRankID == DataEnum.eRankID.Rare)
        {
            if (m_tPlayerData.iRareCost < 0)
            {
                GFunc.Function.Print_Log("Input Err. Player Controller.Get_SpecialCost. Err: -3");
                return -3;
            }
            return m_tPlayerData.iRareCost;
        }
        else if (_eRankID == DataEnum.eRankID.Epic)
        {
            if (m_tPlayerData.iEpicCost < 0)
            {
                GFunc.Function.Print_Log("Input Err. Player Controller.Get_SpecialCost. Err: -3");
                return -3;
            }
            return m_tPlayerData.iEpicCost;
        }
        else if (_eRankID == DataEnum.eRankID.Unique)
        {
            if (m_tPlayerData.iUniqueCost < 0)
            {
                GFunc.Function.Print_Log("Input Err. Player Controller.Get_SpecialCost. Err: -3");
                return -3;
            }
            return m_tPlayerData.iUniqueCost;
        }
        else
        {
            GFunc.Function.Print_Log("Input Err. Player Controller.Get_SpecialCost.  Err: -1");
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
            GFunc.Function.Print_Log("Input Err. Player Controller.Add_SpecialCost. Err: -2");
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
            GFunc.Function.Print_Log("Input Err. Player Controller.Add_SpecialCost. Err: -1");
            return -1;
        }
    }
    #endregion
  public  bool m_bFirstInit =  false;
    // Start is called before the first frame update
    void Start()
    {
        if (Caching.ClearCache())
            print("CLEAR CACHE");
        m_upgradeController = new UpgradeController();
        m_eNextControlState = DataEnum.ePickingMode.Obj_Tower;
        StageSettingInfo();
        m_tPlayerData.iGold = 10000;
        //m_tPlayerData.iGold = 20;
    }
    void StageSettingInfo()
    {
        if(Game_Manager.Instance.m_tStageInfo.eDifficulty == DataEnum.eDifficulty.Easy)
        {
            m_tPlayerData.iLife = 30;
            m_tPlayerData.iGold = 30;
        }
        else if (Game_Manager.Instance.m_tStageInfo.eDifficulty == DataEnum.eDifficulty.Normal)
        {
            m_tPlayerData.iLife = 20;
            m_tPlayerData.iGold = 20;
        }
        else if (Game_Manager.Instance.m_tStageInfo.eDifficulty == DataEnum.eDifficulty.Hard)
        {

            m_tPlayerData.iLife = 20;
            m_tPlayerData.iGold = 20;
        }
        else if (Game_Manager.Instance.m_tStageInfo.eDifficulty == DataEnum.eDifficulty.Infinite)
        {
            m_tPlayerData.iLife = 15;
            m_tPlayerData.iGold = 20;
        }
        else
        {
            m_tPlayerData.iLife = 30;
            m_tPlayerData.iGold = 30;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        //if (false == m_bFirstInit)
        //    UpdateInit();
        CheckInputMouse();
        ChangeController();
      
        DoController(false);

        DebugText();

    }

    //void  UpdateInit()
    //{
    //    m_bFirstInit = true;

    //}

    private void UpdateInit()
    {
        int k = 0;
        for (int i =0; i<5; ++i)
        {
            for(int j=0; j<8;++j)
            {
                if (i == 4 && j > 1)
                    break;
                if (Object_Manager.Instance.m_dictClone_Object.ContainsKey("AlphaBlock"))
                {
                    while (k < Object_Manager.Instance.m_dictClone_Object["AlphaBlock"].Count)
                    {
                        int iIndex = k;

                        if (Object_Manager.Instance.m_dictClone_Object["AlphaBlock"][iIndex].layer
                            == LayerMask.NameToLayer("Tile"))
                        {
                            //타일위에 오브젝트가 있는 지 판단이 필요함.
                            RaycastHit hit;

                            Vector3 vector3Orin = Object_Manager.Instance.
                               m_dictClone_Object["AlphaBlock"][iIndex].transform.position;

                            vector3Orin.y -= 0.5f;
                            Ray ray = new Ray(vector3Orin,
                               new Vector3(0, 1, 0));
                            print(k);
                            if (!(Physics.Raycast(ray, out hit, 3f, (1 << LayerMask.NameToLayer("Tower")))))
                            {
                                vector3Orin.y += 1f;

                                if (GetComponent<ConstructionController>().CallTower((DataEnum.eRankID)(1 << (i)),
                                    j,
                                    vector3Orin))
                                {
                                    print("succese");
                                }
                                else
                                {
                                    print("Create Fail");
                                }
                                ++k;
                                break;
                            }
                        }
                        ++k;
                    }

                }
                else
                { print("No Cotains"); }
            }
          
        }
        m_bFirstInit = true;
    }

    private void DebugText()
    {
        UnityEngine.UI.Text tLog = GameObject.Find("DataText").GetComponent<UnityEngine.UI.Text>();
        tLog.text =
            "<DebugText> \n"+
            "Life : " + m_tPlayerData.iLife + "\n" +
            "Synergy : " + m_tPlayerData.iTowerSynergy + "\n" +
            "Gold : " + m_tPlayerData.iGold +"\n" +
            "MagicGold : " + m_tPlayerData.iMagicCost + "\n" +
            "RareGold : " + m_tPlayerData.iRareCost + "\n" +
            "EpicGold : " + m_tPlayerData.iEpicCost + "\n" +
            "UniqueGold : " + m_tPlayerData.iUniqueCost + "\n";

    }

    void ChangeController()
    {
        if(m_eCurControlState != m_eNextControlState)
        {
            switch(m_eNextControlState)
            {
                case DataEnum.ePickingMode.Obj_Tower:
                    {
                        if(m_objPickTower)
                            m_objPickTower.GetComponent<BaseObj>().m_bSelect = false;
                        Picking_Tower_ChangeRenderer(false);
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
                if (!GetComponent<StageController>().Get_WaveOnOff)
                    RayPicking();
                else
                {
                    Reset_PickingInfo();
                }
            }
           else if(Controller_Manager.Instance.LButtonDown())
           {
           
           }
        }
    }

    public void Reset_PickingInfo()
    {
        if (m_objPickTower)
            m_objPickTower.GetComponent<BaseObj>().m_bSelect = false;
        Picking_Tower_ChangeRenderer(false);
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
            Resource_Manager.Instance.InstanceObj("Effect", "DonutTrail_Bust", hit.collider.gameObject.transform.position);
       
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
            if (hit.collider.gameObject)
                m_objPicking = hit.collider.gameObject;

            return true;
        }
        return false;
    }

    public bool TileUpto_RayPicking(string _strLayerMask, Vector3 _vDir)
    {
        RaycastHit hit;
        Vector3 vPickObjPos = m_objPicking.transform.position;
        vPickObjPos.y -= 0.5f;
        Ray ray = new Ray(vPickObjPos, _vDir);

        int iLayerMask = (1 << LayerMask.NameToLayer(_strLayerMask)) | (1 << LayerMask.NameToLayer("UI"));

        if (Physics.Raycast(ray, out hit, 20.0f, iLayerMask))
        {
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
            return _RayHit.collider.gameObject;
        }
        return null;
    }

    private void Picking_Tower()
    {
        GFunc.Function.Print_simpleLog("picking Tower");

        m_objPickTower = m_objPicking;
        m_eNextControlState = DataEnum.ePickingMode.Tile;
        
        Check_TowerInBoard();
        Picking_Tower_ChangeRenderer(true);
        Check_Exception_TowerInBoard();

        m_objPickTower.GetComponent<BaseObj>().m_bSelect = true;
        m_objPicking = null;
    }

    private void Picked_Tower()
    {

    }

    private void Picking_Tower_ChangeRenderer(bool _bRimLight_OnOff)
    {
        if (m_objPickTower == null)
            return;

        if (_bRimLight_OnOff)
        {
            Shader rimlight = GetComponent<ShaderController>().Get_Shader("Rimlight_Shader");
            //Shader rimlight = Shader.Find("Custom/Rimlight_Shader");
            if (rimlight == null)
            {
                GFunc.Function.Print_Log("rimlight null.");
                return;
     
            }
            //rimlight.
            m_objPickTower.GetComponentInChildren<Renderer>().material.shader = rimlight;
            //float fPow = m_objPickTower.GetComponentInChildren<Renderer>().material.GetFloat("_Pow");
            m_objPickTower.GetComponentInChildren<Renderer>().material.SetFloat("_Pow", 1.0f);

            m_objPickTower.GetComponentInChildren<Renderer>().material.SetColor("_RimCol", new Color(0, 1, 0));

            if (m_iCheckPickingTower == OUTBOARD)
            {
                m_objPickTower.GetComponentInChildren<Renderer>().material.SetFloat("_Holo", 1);
            }
            else
            {
                m_objPickTower.GetComponentInChildren<Renderer>().material.SetFloat("_Holo", 0);
            }
        }
        else
        {
            //if (m_iCheckPickingTower == OUTBOARD)
            //{
            //    Shader rimlight = gameObject.GetComponent<ShaderController>().Get_Shader("Rimlight_Shader");
            //    //Shader rimlight = Shader.Find("Custom/Rimlight_Shader");
            //    if (rimlight == null)
            //        return;

            //    m_objPickTower.GetComponentInChildren<Renderer>().material.SetFloat("_Pow", 1.0f);
            //    m_objPickTower.GetComponentInChildren<Renderer>().material.SetColor("_RimCol", new Color(1, 0, 0));
            //    m_objPickTower.GetComponentInChildren<Renderer>().material.SetFloat("_Hologram", 0);
            //}
            //else
            //{
            Shader defaultshader = GetComponent<ShaderController>().Get_Shader("Default_Shader"); ;
            //}

            if (defaultshader == null)
            {
                GFunc.Function.Print_Log("defaultshader null.");
                
                return;

            }
            m_objPickTower.GetComponentInChildren<Renderer>().material.shader = defaultshader;
        }
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

        GFunc.Function.Print_simpleLog("picking Tile");
        
        if (INBOARD == m_iCheckPickingTower)
        {
            if (Tile_Check_InTower(false))
                return;

            Vector3 vPickPos = m_objPicking.transform.position;
            vPickPos.y = 0;
            m_objPickTower.transform.position = vPickPos;

            m_eNextControlState = DataEnum.ePickingMode.Obj_Tower;
        }
        else if(OUTBOARD == m_iCheckPickingTower) //보드 밖에 있는 것을 안으로 옮기는 경우
        {
            bool bCheck = Tile_Check_InTower(true);
            if (bCheck) // 한번더 레이피킹을 하여 해당 타일에 오브젝트가 있는 지 확인.
            {
               
                return; // 오브젝트가 있다면 바로 리턴함.
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

    private bool Tile_Check_InTower(bool _bSort)
    {
        if (TileUpto_RayPicking("Tower",new Vector3(0,1,0))) // 한번더 레이피킹을 하여 해당 타일에 오브젝트가 있는 지 확인.
        {
            //타워 업글 -> 스텟 상승
            if (m_objPicking.GetComponent<TowerAI>().m_strPrefabName ==
                m_objPickTower.GetComponent<TowerAI>().m_strPrefabName)
            {
                //클론명도 체크해야함.
                if(m_objPicking.name == m_objPickTower.name)
                {
                    GFunc.Function.Print_simpleLog("sameObject");

                    return false;
                }
                TowerStatUp(_bSort);
                //타워가 같은 타워라면 타워 업글
            }
            else //다르다면 티어 체크 후 같은 티어라면 티어를 업글
            {
                
                CheckTowerRank(_bSort);
            }

            m_eNextControlState = DataEnum.ePickingMode.Obj_Tower;
            return true; // 오브젝트가 있다면 바로 리턴함.
        }      
        return false;
    }

    private void CheckTowerRank(bool _bSort)
    {
        //
        if (m_objPicking.GetComponent<TowerAI>().Get_TowerInfo.iTowerId < 0)
            return;
        int iPickingTowerRank = (int)((m_objPicking.GetComponent<TowerAI>().Get_TowerInfo.iTowerId + 10) *0.1f);
        int iPickedTowerRank = (int)((m_objPickTower.GetComponent<TowerAI>().Get_TowerInfo.iTowerId + 10) * 0.1f);

        if (iPickedTowerRank != iPickingTowerRank)  //타워 랭크가 다르다면 종료
            return;

        if(iPickingTowerRank >0 && iPickingTowerRank<5)
        {
            GFunc.Function.Print_simpleLog("TowerRankUp");
            CreateRankUpTower(_bSort, iPickingTowerRank);
        }
        else
        {
            return;
        }
    }

    private void CreateRankUpTower(bool _bSort, int _iPickiTowerRank)
    {
        GameObject objEvent = GameObject.FindGameObjectWithTag("EventActor");
        if(_bSort && (m_iCheckPickingTower == OUTBOARD))
            GameObject.FindWithTag("TotalController").GetComponent<ConstructionController>().Sort_AwaitList(m_iPick_AwaitBoxNumber);
        if(_iPickiTowerRank >= 1)
        {
            if (objEvent.GetComponent<Constructor>().Construction_Tower_NoAwait((DataEnum.eRankID)(1 << (_iPickiTowerRank)), m_objPicking.transform.position))
            {
                Destroy(m_objPicking);
                Destroy(m_objPickTower);
                return;
            }
            GFunc.Function.Print_simpleLog("TowerRankUpFailed");
        }
        else
        {
            GFunc.Function.Print_simpleLog("PickRank : " + _iPickiTowerRank);
        }
        //if (objEvent.GetComponent<Constructor>().Construction_Tower_NoAwait((DataEnum.eRankID)(1<<(_iPickiTowerRank+1)), m_objPicking.transform.position))
        //{
        //    Destroy(m_objPicking);
        //    Destroy(m_objPickTower);
        //}
    }
    private void CreateRankUpTower(bool _bSort, int _iPickiTowerRank, bool _bTest)
    {
        GameObject objEvent = GameObject.FindGameObjectWithTag("EventActor");
        if (_bSort)
            GameObject.FindWithTag("TotalController").GetComponent<ConstructionController>().Sort_AwaitList(m_iPick_AwaitBoxNumber);
        if (_iPickiTowerRank == 1)
        {
            if (objEvent.GetComponent<Constructor>().Construction_Tower((DataEnum.eRankID)(1 << (_iPickiTowerRank)), 0))
            {
                Destroy(m_objPicking);
                Destroy(m_objPickTower);
            }
        }
        if (objEvent.GetComponent<Constructor>().Construction_Tower((DataEnum.eRankID)(1 << (_iPickiTowerRank + 1))))
        {
            Destroy(m_objPicking);
            Destroy(m_objPickTower);
        }
    }

    private void TowerStatUp(bool _bSort)
    {
        DataStruct.tagTowerStatus towerinfo =    m_objPicking.GetComponent<TowerAI>().Get_TowerInfo;
        ++towerinfo.iLvl;
        m_objPicking.GetComponent<TowerAI>().Set_TowerInfo = towerinfo;
        Base_Effect effect = m_objPicking.GetComponentInChildren<Base_Effect>();
        ParticleSystem ps = m_objPicking.GetComponentInChildren<ParticleSystem>();
        var mainModule = ps.main;
        var vsize = mainModule.startSize;
        effect.m_tEffectInfo.fParticleScale += 1;
        vsize.constantMax = effect.m_tEffectInfo.fParticleScale;
        mainModule.startSize = vsize;
        mainModule.simulationSpeed += 0.5f;
        if (_bSort)
            GameObject.FindWithTag("TotalController").GetComponent<ConstructionController>().Sort_AwaitList(m_iPick_AwaitBoxNumber);
        Destroy(m_objPickTower);
    }

    public bool Sell_Tower()
    {
        if(m_eCurControlState == DataEnum.ePickingMode.Tile
            && m_objPickTower != null)
        {
            if (!Calculate_SellingTower())
            {
                
                return false;
            }
            if (OUTBOARD == m_iCheckPickingTower) //보드 밖에 있는 것을 안으로 옮기는 경우
            {
                GameObject.FindWithTag("TotalController").GetComponent<ConstructionController>().Sort_AwaitList(m_iPick_AwaitBoxNumber);
            }
            Destroy(m_objPickTower);
            m_eNextControlState = DataEnum.ePickingMode.Obj_Tower;
            return true;
        }

        GFunc.Function.Print_simpleLog("Pick is null and State is Not Tile");
        return false;
    }
    bool Calculate_SellingTower()
    {
        DataStruct.tagTowerStatus towerinfo = m_objPickTower.GetComponent<TowerAI>().Get_TowerInfo;
        DataEnum.eRankID eRank = m_objPickTower.GetComponent<TowerAI>().Get_TowerRank;

        if(DataEnum.eRankID.End == eRank)
        {
            GFunc.Function.Print_simpleLog("Rank is End. " + m_objPickTower.name);
            return false;
        }

        //추후 랭크 별로 다른 골드를 지급할 시 필요함.
        //if (DataEnum.eRankID.Normal == eRank)
        //{
        //    m_tPlayerData.iGold += GConst.BaseValue.iTowerGold;
        //}
        //else if (DataEnum.eRankID.Magic == eRank)
        //{
        //    m_tPlayerData.iGold += GConst.BaseValue.iTowerGold;
        //}
        //else if (DataEnum.eRankID.Rare == eRank)
        //{
        //    m_tPlayerData.iGold += GConst.BaseValue.iTowerGold;
        //}
        //else if (DataEnum.eRankID.Epic == eRank)
        //{
        //    m_tPlayerData.iGold += GConst.BaseValue.iTowerGold;
        //}
        //else if (DataEnum.eRankID.Unique == eRank)
        //{
        //    m_tPlayerData.iGold += GConst.BaseValue.iTowerGold;
        //}
        //else
        //    return false;

        if (DataEnum.eRankID.Normal == eRank ||
            DataEnum.eRankID.Magic == eRank ||
            DataEnum.eRankID.Rare == eRank ||
            DataEnum.eRankID.Epic == eRank ||
            DataEnum.eRankID.Unique == eRank
            )
        {
            //특정 타워는 비싸게 팔 수 있는 기믹이 필요함.

            m_tPlayerData.iGold += GConst.BaseValue.iTowerGold;
        }
        else
        {
            GFunc.Function.Print_simpleLog("Rank is Other");
            return false;
        }

        return true;
    }

}
