using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_RandomMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    
    }
    bool m_bInit = false;
    // Update is called once per frame
    void Update()
    {
        if(!m_bInit)
        {
            m_bInit = true;
               Vector3 _vCreatePos = transform.position;
            string strObjKey = "";

            int iSeed = Time.deltaTime.GetHashCode();

            System.Random rd = new System.Random(iSeed);

            int iTear = rd.Next(2, 4);
            int iRandomID =0;// 타워 키값 0-8번 추출
            if (iTear == 3)
            {
                iRandomID = rd.Next(0, 4);
                if (iRandomID == 2)
                    iRandomID = 4;
                else if (iRandomID == 3)
                    iRandomID = 7;
            }
            else
                iRandomID = rd.Next(0, 7);
            int _iTowerId = (1 << iTear)
                   | (1 << (iRandomID + GConst.BaseValue.iMaxRank_Lvl_Count));//노말타워타입과 키값 혼합

            for (int i = 0; i < GConst.BaseValue.iMaxRank_Lvl_Count; ++i)
            {
                if ((_iTowerId & (1 << i)) == (1 << i))
                {
                    for (int j = 0; j < GConst.BaseValue.iMaxRank_ID_Count; ++j)
                    {
                        if ((_iTowerId & (1 << (j + GConst.BaseValue.iMaxRank_Lvl_Count))) == (1 << (j + GConst.BaseValue.iMaxRank_Lvl_Count)))
                        {
                            string strnumid = "_" + i + j;
                            strObjKey = "Tower" + strnumid;
                        }
                    }

                }

            }

            //  GFunc.Function.Print_Log("X: " + _vCreatePos.x + "\n" + "X: " + _vCreatePos.y + "\n" + "X: " + _vCreatePos.z + "\n");
            // GameObject objInstance = Object_Manager.Instance.InstanceObject(_vCreatePos, "Tower", "Tower", strObjKey);
            Vector3 vCamPos = new Vector3(-5,-4,0);
            //Vector3 vCamPos = Camera.main.transform.position;
            Vector3 vDir = vCamPos - transform.position;
            vDir.y = 0;
            vDir.Normalize();

            Quaternion temp = Quaternion.LookRotation(vDir);
            GameObject obj = Instantiate(Resource_Manager.Instance.m_dictPrefabs["Tower"][strObjKey].objPrefabs, _vCreatePos, temp);
            obj.GetComponent<TowerAI>().enabled = false;
            obj.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
        }
    }
}
