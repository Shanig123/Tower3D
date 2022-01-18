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

    //�� �Լ��� ȣ��Ǹ� Ÿ���� ������.
    private bool Construction_Tower(DataEnum.eRankID _eRankID) //���� ������ �� ���н� ������ȯ
    {
        GameObject obj = GameObject.FindWithTag("TotalController");
        int iPlayerGold = obj.GetComponent<PlayerController>().Get_Gold;
        print("list count : " + m_listAwaitObj.Count + " \n Const :  " + GConst.BaseValue.iHorizontal);
        if (m_listAwaitObj.Count < GConst.BaseValue.iAwaitBoxMax)
        {
            if (iPlayerGold >= GConst.BaseValue.iTowerGold)
            {
                if ((_eRankID & DataEnum.eRankID.Normal) == DataEnum.eRankID.Normal)       //�Ϲ�Ÿ�� ��ȯ
                {
                    int iRandomID = obj.GetComponent<DataController>().ExtractRandomNumberFromSeed(0, 8); // Ÿ�� Ű�� 0-8�� ����

                    int iRankID = (int)DataEnum.eRankID.Normal
                        | (1 << (iRandomID + GConst.BaseValue.iMaxRank_Lvl_Count));//�븻Ÿ��Ÿ�԰� Ű�� ȥ��

                    InstanceTower(iRankID); //�ν��Ͻ� Ÿ��
                    return true;
                }
                else  //Ư�� Ÿ�� ��ȯ
                {
                    if (obj.GetComponent<PlayerController>().Get_SpecialCost(_eRankID) > 0)//Ư�� �ڽ�Ʈ ���� ���� ��
                    {
                        int iRandomID = obj.GetComponent<DataController>().ExtractRandomNumberFromSeed(0, 8); // Ÿ�� Ű�� 0-8�� ����
                        int iRankID = (int)_eRankID
                            | (1 << (iRandomID + GConst.BaseValue.iMaxRank_Lvl_Count)); //��ǲŸ��Ÿ�԰� Ÿ�� Ű�� ȥ��

                        InstanceTower(iRankID);//�ν��Ͻ� Ÿ��      
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

    //�� �Լ��� ȣ��Ǵ� ���� �÷��̾ ��ư�� ������ �� ������.
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
        //Ÿ�� �ν��Ͻ�
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
            //��� ���
            iPlayerGold -= GConst.BaseValue.iTowerGold;
            obj.GetComponent<PlayerController>().Set_Gold = iPlayerGold;
        }
        else
        {
            //��� ���
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
