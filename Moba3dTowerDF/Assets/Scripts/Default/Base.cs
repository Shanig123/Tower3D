
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region DATA

namespace DataEnum
{
    public enum eState
    { NoActive, Ready, Active, Dead, End };

    public enum eRankID
    {
        Normal      = 1,
        Magic       = (1 << 1),
        Rare        = (1 << 2),
        Epic        = (1 << 3),
        Unique      = (1 << 4),
        RankID_0    = (1 << 5),
        RankID_1    = (1 << 6),
        RankID_2    = (1 << 7),
        RankID_3    = (1 << 8),
        RankID_4    = (1 << 9),
        RankID_5    = (1 << 10),
        RankID_6    = (1 << 11),
        RankID_7    = (1 << 12),

        End     = 0
    };

    public enum eDifficulty
    {
        Totorial    = 1,
        Easy        = (1 << 1),
        Normal      = (1 << 2),
        Hard        = (1 << 3),
        Infinite    = (1 << 4),
        End         = 0
    };

    public enum ePickingMode { Tile, Obj_Tower, End };

}
namespace DataStruct
{
    [System.Serializable]
    public class tagStatus
    {
        public int iHp;
        public int iMaxHp;

        public int iAtk;
        public int iLvl;
    }

    [System.Serializable]
    public class tagMobStatus : tagStatus
    {
        public float fMoveSpeed;


        public float fDeadTime;
    }

    [System.Serializable]
    public class tagTowerStatus : tagStatus
    {
        //DataEnum.eRankID
        public int iTowerId;
        public int iStatus;

        public float fRange;

        public float fAtkCoolTime;
        public float fMaxAtkCoolTime;
    }

    [System.Serializable]
    public class tagBulletStatus : tagStatus
    {
        //public int iAtk;
        public GameObject objTarget;

        public float fMoveSpeed;
        public float fLifeTime;
        public float fMaxLifeTime;

        public string strObjTagName;
    }
   
    [System.Serializable]
    public class tagGameData
    {
        public int iWave;
        public int iBestWave;
        public int iTotalKillCount;

        public uint iUnLockLevel;

        public bool[] bArrUnlockAbility = new bool[21];
    }

    //public struct tagAbilityInfo
    //{
    //    public int iKey;
    //    public bool bCheckUnlock;
    //}

    [System.Serializable]
    public struct tagOptionData
    {
        public int iDayNight; //-1 항상 밤, 0 랜덤, 1 항상 낮
        public int iShadow; // 저품질 중품질 고품질
        public float fMasterVol; // 0-1
        public float fSfxVol;
        public float fBgmVol;

        public bool bKor; //1 : kor 0 : eng
    }

    [System.Serializable]
    public struct tagPlayerData
    {
        public int iLife;
        public int iGold;
        public int iMagicCost;
        public int iRareCost;
        public int iEpicCost;
        public int iUniqueCost;
        public int iTowerSynergy;
        public int[] iArrAbility;
    }

    [System.Serializable]
    public struct tagStageInfo
    {
        public DataEnum.eDifficulty eDifficulty;
        public int iStartAbility;

        public bool bKor; //1 : kor 0 : eng
    }

    [System.Serializable]
    public class tagEffectInfo
    {

        public Color colorEffect =  new Color(1,1,1,1);
        public Vector3 vScale = new Vector3(1,1,1);
        public float fParticleScale = 1;
        public float fSpeed = 1;
    }

    public struct tagPrefab
    {
        public GameObject objPrefabs;
        public string strKeys;
    }

}

#endregion

#region Function

namespace GFunc
{
    class Function
    {
        public static void Print_Log(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
            if (GameObject.Find("Log_File"))
            {
                UnityEngine.UI.Text tLog =
               GameObject.Find("Log_File").
               GetComponent<UnityEngine.UI.Text>();
                tLog.text = message;
            }
#else
          
#endif
            return;
        }

        public static void Print_simpleLog(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#else
          
#endif
            return;
        }
    }

}

#endregion


//public class Base : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
