using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GConst
{
    static class BaseValue
    {
        public const float fBulletSpeed = 10f;

        public const int iTowerGold = 10;
        public const int iHorizontal = 11;
        public const int iMaxRank_Lvl_Count = 5;
        public const int iMaxRank_ID_Count = 10;

        public const int iAwaitBoxMax = 6;

        public const int iStatFlag_NULL = 0;
        public const int iStatFlag_CheckInStage = 1;
        public const int iStatFlag_ReadyToIdle = (1<<1);
        public const int iStatFlag_CheckModifyStart = (1 << 2);
        public const int iStatFlag_CheckModifyEnd = (1 << 3);

        static public Vector3 vLeftTop { get { return new Vector3(-5.5f, 0, 5.5f); } }
        static public Vector3 vRightBottom { get { return new Vector3(5.5f, 0, -5.5f); } }
    }



};

//extern const int iTowerGold = 100;

//public class BaseConstant : MonoBehaviour
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
