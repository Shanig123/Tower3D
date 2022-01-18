using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{
    ConstructionController()
        : base()
    {
        m_eLevel = DataEnum.eDifficulty.End;
        m_listAwaitObj = new List<GameObject>();
    }
    #region Value

    [SerializeField] private DataEnum.eDifficulty m_eLevel;
    [SerializeField] private List<GameObject> m_listAwaitObj;
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

    //이 함수가 호출되면 타워가 생성됨.
    private bool Construction_Tower(DataEnum.eRankID _eRankID) //생성 성공시 참 실패시 거짓반환
    {
        GameObject obj = GameObject.FindWithTag("TotalController");
        int iPlayerGold = obj.GetComponent<PlayerController>().Get_Gold;
        print("list count : " + m_listAwaitObj.Count + " \n Const :  " + GConst.BaseValue.iHorizontal);
        if (m_listAwaitObj.Count < GConst.BaseValue.iAwaitBoxMax)
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
                print("Not enough gold.");
            }
        }
        else
        {
            print("list count(" + m_listAwaitObj.Count + ") is over " + GConst.BaseValue.iHorizontal);
        }
        return false;
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
            print("list is full or not enough gold.");
        }
        else
        {
            print("Tower Call input Err.");

        }
        return false;
    }

    private void InstanceTower(int _iTowerId)
    {
        //타워 인스턴스
        int iListCount = m_listAwaitObj.Count;
        Vector3 vPos;
        if (Object_Manager.Instance.m_dictClone_Object.ContainsKey("Box"))
        {
            vPos = Object_Manager.Instance.m_dictClone_Object["Box"][iListCount].transform.position;
            vPos.y += 0.5f;
            string strObjKey = ObjKeyTostrTowerID(_iTowerId);
            print(strObjKey + "_" + _iTowerId);
            GameObject objInstance = Object_Manager.Instance.InstanceObject(vPos, "Tower", "Object", strObjKey);

            objInstance.GetComponent<TowerAI>().Set_TowerID = (ObjKeyTointTowerID(_iTowerId));

            m_listAwaitObj.Add(objInstance);
        }
        else
        {
            print("err not found Key : Box");
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
        int iListCount = m_listAwaitObj.Count;
        if(iListCount<= _iIndex)
        {
            print("InputErr");
            return;
        }
        m_listAwaitObj.RemoveAt(_iIndex);
        Vector3 vPos;

        for (int i = _iIndex; i<iListCount-1;++i)
        {
            vPos = Object_Manager.Instance.m_dictClone_Object["Box"][i].transform.position;
            vPos.y += 0.5f;
            m_listAwaitObj[i].transform.position = vPos;
        }
       

        //if (Object_Manager.Instance.m_dictClone_Object.ContainsKey("Box"))
        //{
        //    vPos = Object_Manager.Instance.m_dictClone_Object["Box"][iListCount].transform.position;
        //    vPos.y += 0.5f;
        //    string strObjKey = ObjKeyTostrTowerID(_iTowerId);
        //    print(strObjKey + "_" + _iTowerId);
        //    GameObject objInstance = Object_Manager.Instance.InstanceObject(vPos, "Tower", "Object", strObjKey);

        //    objInstance.GetComponent<TowerAI>().Set_TowerID = (ObjKeyTointTowerID(_iTowerId));

        //    m_listAwaitObj.Add(objInstance);
        //}
    }
}
