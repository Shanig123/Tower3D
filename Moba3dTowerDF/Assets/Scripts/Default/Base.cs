using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum eControl_Mode { NoControl, Construction, End };

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
        public int iMobId;

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
    public struct tagGameData
    {
        public int iWave;
        public int iBestWave;
        public int iTotalKillCount;

        public uint iUnLockLevel;
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
    }

    public struct tagPrefab
    {
        public GameObject objPrefabs;
        public string strKeys;
    }


}

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
