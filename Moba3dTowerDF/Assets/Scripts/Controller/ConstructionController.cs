using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{
    ConstructionController()
        : base()
    {
        m_eLevel = DataEnum.eDifficulty.End;
    }
    #region Value

    [SerializeField] private DataEnum.eDifficulty m_eLevel;
    

    //[SerializeField] private Dictionary<int, string> m_mapIDToObjKey;

    #endregion

    #region Property

    public DataEnum.eDifficulty Get_Difficulty
    {
        get
        {
            return m_eLevel;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //이 함수가 호출되는 것은 플레이어가 버튼을 눌렀을 때 동작함.
    public bool CallTower(DataEnum.eRankID _eRankID)
    {
        int iFlag = (int)DataEnum.eRankID.Normal | (int)DataEnum.eRankID.Magic | (int)DataEnum.eRankID.Rare | (int)DataEnum.eRankID.Epic | (int)DataEnum.eRankID.Unique;
        if (((int)_eRankID & iFlag) > 0)
        {
            if (Construction_Tower(_eRankID))
            {
                MinPlayerGold(_eRankID);
                return true;
            }
            GFunc.Function.Print_Log("list is full or not enough gold.");

        }
        else
        {
            GFunc.Function.Print_Log("Tower Call input Err.");
        }
        return false;
    }

    public bool CallTowerNotInAwait(DataEnum.eRankID _eRankID, Vector3 _vCreatePos)
    {
        int iFlag = (int)DataEnum.eRankID.Normal | (int)DataEnum.eRankID.Magic | (int)DataEnum.eRankID.Rare | (int)DataEnum.eRankID.Epic | (int)DataEnum.eRankID.Unique;
        if (((int)_eRankID & iFlag) > 0)
        {
            if (Construction_Tower(_eRankID,_vCreatePos))
            {
                MinPlayerGold(_eRankID);
                return true;
            }
            GFunc.Function.Print_Log("Tower Call input Err.");
        }
        else
        {
            GFunc.Function.Print_Log("Tower Call input Err.");
        }
        return false;
    }

    public bool CallTower(DataEnum.eRankID _eRankID, int _iTowerNum)
    {
        int iFlag = (int)DataEnum.eRankID.Normal | (int)DataEnum.eRankID.Magic | (int)DataEnum.eRankID.Rare | (int)DataEnum.eRankID.Epic | (int)DataEnum.eRankID.Unique;
        if (((int)_eRankID & iFlag) > 0)
        {
            GameObject obj = GameObject.FindWithTag("TotalController");
            int iPlayerGold = obj.GetComponent<PlayerController>().Get_Gold;

            if (BoxOnObjCount() < GConst.BaseValue.iAwaitBoxMax)
            {
                int iRandomID = _iTowerNum; // 타워 키값 0-8번 추출

                int iRankID = (int)_eRankID
                    | (1 << (iRandomID + GConst.BaseValue.iMaxRank_Lvl_Count));//노말타워타입과 키값 혼합

                InstanceTower(iRankID); //인스턴스 타워
                return true;
            }
            GFunc.Function.Print_Log("list is full.");
            return false;
        }
        else
        {
            GFunc.Function.Print_Log("Tower Call input Err.");
        }
        return false;
    }

    public bool CallTower(DataEnum.eRankID _eRankID, int _iTowerNum, Vector3 _vPos )
    {
        int iFlag = (int)DataEnum.eRankID.Normal | (int)DataEnum.eRankID.Magic | (int)DataEnum.eRankID.Rare | (int)DataEnum.eRankID.Epic | (int)DataEnum.eRankID.Unique;
        if (((int)_eRankID & iFlag) > 0)
        {
            GameObject obj = GameObject.FindWithTag("TotalController");
            int iPlayerGold = obj.GetComponent<PlayerController>().Get_Gold;
            int iRandomID = _iTowerNum; // 타워 키값 0-8번 추출

            int iRankID = (int)_eRankID
                | (1 << (iRandomID + GConst.BaseValue.iMaxRank_Lvl_Count));//노말타워타입과 키값 혼합

            InstanceTower(iRankID, _vPos); //인스턴스 타워
            return true;
           
        }
        else
        {
            GFunc.Function.Print_Log("Tower Call input Err.");
        }
        return false;
    }

    //이 함수가 호출되면 타워가 생성됨.
    private bool Construction_Tower(DataEnum.eRankID _eRankID) //생성 성공시 참 실패시 거짓반환
    {
        GameObject obj = GameObject.FindWithTag("TotalController");
        int iPlayerGold = obj.GetComponent<PlayerController>().Get_Gold;

        print(BoxOnObjCount());
        if (BoxOnObjCount() < GConst.BaseValue.iAwaitBoxMax)
        {
            if (iPlayerGold >= GConst.BaseValue.iTowerGold)
            {
                if ((_eRankID & DataEnum.eRankID.Normal) == DataEnum.eRankID.Normal)       //일반타워 소환
                {
                    int iRandomID = obj.GetComponent<DataController>().ExtractRandomNumberFromSeed(0, 8); // 타워 키값 0-8번 추출

                    int iRankID = (int)DataEnum.eRankID.Normal
                        | (1 << (iRandomID + GConst.BaseValue.iMaxRank_Lvl_Count));//노말타워타입과 키값 혼합

                    InstanceTower(iRankID); //인스턴스 타워
                    return true;
                }
                else  //특수 타워 소환
                {
                    if (obj.GetComponent<PlayerController>().Get_SpecialCost(_eRankID) > 0)//특수 코스트 값이 있을 때
                    {
                        int iRandomID = obj.GetComponent<DataController>().ExtractRandomNumberFromSeed(0, 8); // 타워 키값 0-8번 추출
                        int iRankID = (int)_eRankID
                            | (1 << (iRandomID + GConst.BaseValue.iMaxRank_Lvl_Count)); //인풋타워타입과 타워 키값 혼합

                        InstanceTower(iRankID);//인스턴스 타워      
                        return true;
                    }
                }
            }
            else
            {
                GFunc.Function.Print_Log("Not enough gold.");
            }
        }
        return false;
    }

    private bool Construction_Tower(DataEnum.eRankID _eRankID, Vector3 _vCreatePos) //생성 성공시 참 실패시 거짓반환
    {
        GameObject obj = GameObject.FindWithTag("TotalController");
        int iPlayerGold = obj.GetComponent<PlayerController>().Get_Gold;

        if (iPlayerGold >= GConst.BaseValue.iTowerGold)
        {

            int iRandomID = obj.GetComponent<DataController>().ExtractRandomNumberFromSeed(0, 8); // 타워 키값 0-8번 추출
            int iRankID = (int)_eRankID
                | (1 << (iRandomID + GConst.BaseValue.iMaxRank_Lvl_Count)); //인풋타워타입과 타워 키값 혼합

            InstanceTower(iRankID, _vCreatePos);//인스턴스 타워      
            return true;

        }
        else
        {
            GFunc.Function.Print_Log("Not enough gold.");
        }

        return false;
    }

    private bool Construction_Tower(DataEnum.eRankID _eRankID, Vector3 _vCreatePos,int _iRandomMin, int _iRandomMax) //생성 성공시 참 실패시 거짓반환
    {
        GameObject obj = GameObject.FindWithTag("TotalController");
        int iPlayerGold = obj.GetComponent<PlayerController>().Get_Gold;

        if (iPlayerGold >= GConst.BaseValue.iTowerGold)
        {
            //if ((_eRankID & DataEnum.eRankID.Normal) == DataEnum.eRankID.Normal)       //일반타워 소환
            //{
            //    int iRandomID = obj.GetComponent<DataController>().ExtractRandomNumberFromSeed(_iRandomMin, _iRandomMax); // 타워 키값 0-8번 추출

            //    int iRankID = (int)DataEnum.eRankID.Normal
            //        | (1 << (iRandomID + GConst.BaseValue.iMaxRank_Lvl_Count));//노말타워타입과 키값 혼합

            //    InstanceTower(iRankID,_vCreatePos); //인스턴스 타워
            //    return true;
            //}
            //else  //특수 타워 소환
            //{
                int iRandomID = obj.GetComponent<DataController>().ExtractRandomNumberFromSeed(_iRandomMin, _iRandomMax); // 타워 키값 0-8번 추출
                int iRankID = (int)_eRankID
                    | (1 << (iRandomID + GConst.BaseValue.iMaxRank_Lvl_Count)); //인풋타워타입과 타워 키값 혼합

                InstanceTower(iRankID, _vCreatePos);//인스턴스 타워      
                return true;
            //}
        }
        else
        {
            GFunc.Function.Print_Log("Not enough gold.");
        }

        return false;
    }

    private void InstanceTower(int _iTowerId)
    {
        //타워 인스턴스
        GameObject objBox = null;

        int iOnBoxCount = 0;
        int iBoxIndex = 0;
        foreach (GameObject iter in Object_Manager.Instance.m_dictClone_Object["Box"])
        {
            if (iter.GetComponent<Obj_AwaitListBox>().m_OnTowerObj != null)
            {
                ++iOnBoxCount;
            }
            else
            {
                objBox = iter;
                break;
            }
            ++iBoxIndex;
        }
        Vector3 vPos;
        if (Object_Manager.Instance.m_dictClone_Object.ContainsKey("Box"))
        {
            vPos = Object_Manager.Instance.m_dictClone_Object["Box"][iBoxIndex].transform.position;
            vPos.y += 0.5f;
            string strObjKey = ObjKeyTostrTowerID(_iTowerId);
            GFunc.Function.Print_Log("InAwait");

            GameObject objInstance = Object_Manager.Instance.InstanceObject(vPos, "Tower", "Tower", strObjKey);

            objInstance.GetComponent<TowerAI>().Set_TowerID = (ObjKeyTointTowerID(_iTowerId));

            objBox.GetComponent<Obj_AwaitListBox>().m_OnTowerObj = objInstance;
        }
    }
    private void InstanceTower(int _iTowerId,  Vector3 _vCreatePos)
    {
        if (Object_Manager.Instance.m_dictClone_Object.ContainsKey("Box"))
        {
            string strObjKey = ObjKeyTostrTowerID(_iTowerId);
            GFunc.Function.Print_Log("X: " + _vCreatePos.x + "\n" + "X: " + _vCreatePos.y + "\n" + "X: " + _vCreatePos.z + "\n");
            GameObject objInstance = Object_Manager.Instance.InstanceObject(_vCreatePos, "Tower", "Tower", strObjKey);

            objInstance.GetComponent<TowerAI>().Set_TowerID = (ObjKeyTointTowerID(_iTowerId));
        }
    }

    private void MinPlayerGold(DataEnum.eRankID _eRankID)
    {
        GameObject obj = GameObject.FindWithTag("TotalController");
        int iPlayerGold = obj.GetComponent<PlayerController>().Get_Gold;
        if (_eRankID == DataEnum.eRankID.Normal)
        {
            //골드 계산
            iPlayerGold -= GConst.BaseValue.iTowerGold;
            obj.GetComponent<PlayerController>().Set_Gold = iPlayerGold;
        }
        else
        {
            //골드 계산
            iPlayerGold -= GConst.BaseValue.iTowerGold;
            obj.GetComponent<PlayerController>().Set_Gold = iPlayerGold;
            obj.GetComponent<PlayerController>().Add_SpecialCost(_eRankID, -1);
        }
    }

    private string ObjKeyTostrTowerID(int _iTowerId)
    {
        for (int i = 0; i < GConst.BaseValue.iMaxRank_Lvl_Count; ++i)
        {
            if ((_iTowerId & (1 << i)) == (1 << i))
            {
                for (int j = 0; j < GConst.BaseValue.iMaxRank_ID_Count; ++j)
                {
                    if ((_iTowerId & (1 << (j + GConst.BaseValue.iMaxRank_Lvl_Count))) == (1 << (j + GConst.BaseValue.iMaxRank_Lvl_Count)))
                    {
                        string strnumid = "_" + i + j;
                        return "Tower" + strnumid;
                    }
                }

            }

        }
        return null;
    }

    private int ObjKeyTointTowerID(int _iTowerId)
    {
        for (int i = 0; i < GConst.BaseValue.iMaxRank_Lvl_Count; ++i)
        {
            if ((_iTowerId & (1 << i)) == (1 << i))
            {
                for (int j = 0; j < GConst.BaseValue.iMaxRank_ID_Count; ++j)
                {
                    if ((_iTowerId & (1 << (j + GConst.BaseValue.iMaxRank_Lvl_Count))) == (1 << (j + GConst.BaseValue.iMaxRank_Lvl_Count)))
                    {
                        //string strnumid = "_" + i + j;
                        return (i * 10 + j);
                    }
                }

            }

        }
        return -1;
    }

    public void Sort_AwaitList(int _iIndex)
    {
        if(BoxOnObjCount() <= _iIndex)
        {
            print("InputErr");
            return;
        }

        for (int i = 0; i<6;++i)
        {
            RaycastHit hit;
            if (!BoxToRay(i, out hit))
                Object_Manager.Instance.m_dictClone_Object["Box"][i].GetComponent<Obj_AwaitListBox>().m_OnTowerObj = null;
        }
       
    }

    public bool AutoInBoard()
    {
        if (BoxOnObjCount() > 0)
        {
            //제대로 동작되지 않음.
            List<GameObject> listobj_Boxes= Object_Manager.Instance.m_dictClone_Object["Box"];

            int i = 0;
            foreach (GameObject iter in listobj_Boxes)
            {
                //타일의 레이어 체크 및 레이어 위 오브젝트 있는지 체크 필요

                if (Object_Manager.Instance.m_dictClone_Object.ContainsKey("AlphaBlock"))
                {
                    while (i < Object_Manager.Instance.m_dictClone_Object["AlphaBlock"].Count)
                    {
                        int iIndex = i;

                        if (i < GConst.BaseValue.iHorizontal * 8) //i가 77미만일경우
                            iIndex += GConst.BaseValue.iHorizontal * 3;
                        else // i가 
                            iIndex =i - GConst.BaseValue.iHorizontal * 8;

                        if (Object_Manager.Instance.m_dictClone_Object["AlphaBlock"][iIndex].layer
                            == LayerMask.NameToLayer("Tile"))
                        {
                            //타일위에 오브젝트가 있는 지 판단이 필요함.
                            RaycastHit hit;

                            Vector3 vector3Orin = Object_Manager.Instance.
                               m_dictClone_Object["AlphaBlock"][iIndex].transform.position;

                            vector3Orin.y -= 1f;
                            Ray ray = new Ray(vector3Orin,
                               new Vector3(0, 1, 0));

                            if (!(Physics.Raycast(ray, out hit, 3f, (1 << LayerMask.NameToLayer("Tower")))))
                            {
                                print(Object_Manager.Instance.m_dictClone_Object["AlphaBlock"][iIndex].name);
                                vector3Orin.y += 1.5f;
                                if(null != (iter.GetComponent<Obj_AwaitListBox>().m_OnTowerObj))
                                {
                                    iter.GetComponent<Obj_AwaitListBox>().m_OnTowerObj.transform.position = vector3Orin;
                                    iter.GetComponent<Obj_AwaitListBox>().m_OnTowerObj = null;
                                }
                                ++i;
                                break;
                            }
                        }
                        ++i;
                    }

                }

            }

            return true;
        }

        return false;
    }

    private bool BoxToRay(int _iBoxIndex, out RaycastHit _hit)
    {
        Vector3 vOrinPos = Object_Manager.Instance.
           m_dictClone_Object["Box"][_iBoxIndex].transform.position;

        vOrinPos.y -= 2f;
        Ray ray = new Ray(vOrinPos,
           new Vector3(0, 1, 0));

        return Physics.Raycast(ray, out _hit, 10f, (1 << LayerMask.NameToLayer("Tower")));
    }

    private List<GameObject> BoxOnObjectList()
    {
        List<GameObject> listRayHitObject = new List<GameObject>();
        if (Object_Manager.Instance.m_dictClone_Object.ContainsKey("Box"))
        {
            for (int j = 0; j < Object_Manager.Instance.m_dictClone_Object["Box"].Count; ++j)
            {
                RaycastHit hit;

                if (BoxToRay(j, out hit))
                {
                    listRayHitObject.Add(hit.collider.gameObject);
                }
            }
        }
        return listRayHitObject;
    }

    private int BoxOnObjCount()
    {
        int iBoxOnCount = 0;

        foreach(GameObject iter in Object_Manager.Instance.m_dictClone_Object["Box"])
        {
            if( iter.GetComponent<Obj_AwaitListBox>().m_OnTowerObj != null)
            {
                ++iBoxOnCount;
            }
        }
        return iBoxOnCount;
    }
}
