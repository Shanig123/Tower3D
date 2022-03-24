
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region DATA

namespace DataEnum
{
    public enum eState
    { NoActive, Ready, Active, Dead, End };
    public enum eTowerType { Atk, Atk_HitScan,Buff, Scrl, End }
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

    public enum eTextEffect
    {
        Default_AlphaDown       = 1,            //1
        Volcano                 = (1 << 1),     //2
        Up                      = (1 << 2),     //4
        SizeDown                = (1 << 3),     //8
        BillBoard               = (1 << 4),     //16
        End                     = 0
    };

    public enum eClip { Ambi, Sfx, UI, Bgm, _End };

    public enum ePickingMode { Tile, Obj_Tower, End };

    public enum eStatus
    {
        Poison      = 1,        //1
        Fire        = (1 << 1), //2
        Slow        = (1 << 2), //4
        Stun       = (1 << 3),  //8
        End         = 0
    }
}
namespace DataStruct
{
    [System.Serializable]
    public class tagStatusInfo
    {
        public tagStatusInfo() { var a = this; a = null; }

        public float fFireStatusTime;
        public float fFireMaxStatusTime;
        public float fFireRatio;

        public float fPoisonStatusTime;
        public float fPoisonMaxStatusTime;
        public float fPoisonDotTime;
        public float fPoisonDotMaxTime;
        public int iPoisonDamage;

        public float fSlowStatusTime;
        public float fSlowMaxStatusTime;
        public float fSlowSpeed;

        public float fStunStatusTime;
        public float fStunMaxStatusTime;



    }
    [System.Serializable]
    public class tagBaseStatus
    {
        public int iHp;
        public int iMaxHp;

        public int iAtk;
        public int iLvl;

        public int iStatus;
    }

    [System.Serializable]
    public class tagMobStatus : tagBaseStatus
    {
 
        public float fMoveSpeed;
        public float fMoveSpeed_Stun;
        public float fMoveSpeed_Slow;
        public float fMoveSpeed_Fast;
        public float fDeadTime;
    }

    [System.Serializable]
    public class tagTowerStatus : tagBaseStatus
    {
        //DataEnum.eRankID

        public int iBulletStatus;
        public DataEnum.eTowerType eType;
        public int iTowerId;
      
        public float fRange;

        public float fAtkCoolTime;
        public float fMaxAtkCoolTime;

        public tagStatusInfo tStatusInfo;
    }

    [System.Serializable]
    public class tagBulletStatus : tagBaseStatus
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

        public bool[] bArrUnlockAbility;
    }

    [System.Serializable]
    public struct tagDamageStruct
    {
        public int iDamage_Buffer;
        public int iDotDamage_Buffer;
        public int iFireStatusDamage_Buffer;

        public int iTotalDamage;
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
        //public  System.Array iArrAbility;
        public int[] iArrAbility;
    }

    [System.Serializable]
    public struct tagStageInfo
    {
        public DataEnum.eDifficulty eDifficulty;
        public int iStartAbility;

       // public bool bKor; //1 : kor 0 : eng
    }

    [System.Serializable]
    public class tagEffectInfo
    {

        public Color colorEffect =  new Color(1,1,1,1);
        public Vector3 vScale = new Vector3(1,1,1);
        public float fParticleScale = 1;
        public float fSpeed = 1;

        public float fLifeTime = 0;
        public float fTimerSpeed = 1f;
        public float fMaxLifeTime = 0;

        public bool bLoop = true;

        
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


        public static DataStruct.tagGameData InitGameData()
        {
            DataStruct.tagGameData tagData = new DataStruct.tagGameData();
            tagData.iBestWave = 0;
            tagData.iWave = 0;
            tagData.iTotalKillCount = 0;
            tagData.iUnLockLevel = 0;
            tagData.bArrUnlockAbility = new bool[21];
            return tagData;
        }
        
        public static float Jump(float _fYIn, float _fDeltaTime, float _fPower)
        {
            float fYTemp = _fYIn;
            return fYTemp = fYTemp + (_fPower * _fDeltaTime - 0.5f * 9.807f * _fDeltaTime * _fDeltaTime);
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
