using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : BaseObj
{
    TowerAI()
        : base() { }

    #region Value

    [SerializeField] private DataStruct.tagTowerStatus m_tagStatus;

    [SerializeField] private GameObject m_objTargetMob;
    [SerializeField] private int m_iTargetID;

    [SerializeField] private DataEnum.eState m_eNextState = DataEnum.eState.End;
    [SerializeField] private DataEnum.eState m_eCurState = DataEnum.eState.End;


    [SerializeField] private Vector3 m_vCurModifyPos;
    [SerializeField] private Vector3 m_vNextModifyPos;

    #endregion

    #region Property
    public DataEnum.eState GetStat { get { return m_eCurState; } }
    public DataStruct.tagTowerStatus Get_TowerInfo { get { return m_tagStatus; } }


    public DataEnum.eState SetState { set { m_eNextState = value; } }
    public int Set_TowerID { set { m_tagStatus.iTowerId = value; } }
    public DataStruct.tagTowerStatus Set_TowerInfo { set { m_tagStatus = value; } }

    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
      //  m_tagStatus.strTowerName = gameObject.name;
       //EditorUtility
    }


    // Update is called once per frame
    void Update()
    {
        if (!m_bFirstInit)
            UpdateInit();
        if (m_objTargetMob == null)
            m_iTargetID = 0;
        CheckState();
        DoController();
    }
    private void UpdateInit()
    {
        Rename_Clone();
       
        if (0 == m_fReadyTimerMax)
        {
            m_fReadyTimer = 3;
        }
        if (m_tagStatus.fMaxAtkCoolTime == 0)
            m_tagStatus.fMaxAtkCoolTime = 1f;
        if (m_tagStatus.fRange == 0)
            m_tagStatus.fRange = 2f;
        if (m_tagStatus.iAtk == 0)
            m_tagStatus.iAtk = 5;
        m_bFirstInit = true;
    }

    private void Rename_Clone()
    {
        string name = gameObject.name;
        int iCount = GameObject.FindGameObjectWithTag("TotalController").GetComponent<DataController>().Get_TotalCallCount;
        gameObject.name = name + "_" + iCount;
    }

    #region ControllerFunc

    private void CheckState()
    {
        if (m_eCurState != m_eNextState)
        {
            switch (m_eNextState)
            {
                case DataEnum.eState.NoActive:
                    {
                        m_bObjActiveOnOff = false;
                    }
                    break;
                case DataEnum.eState.Ready:
                    {
                        m_bObjActiveOnOff = true;
                        m_bCheckDead = false;
                    }
                    break;
                case DataEnum.eState.Active:
                    {

                    }
                    break;
                case DataEnum.eState.Dead:
                    {
                        m_bCheckDead = true;
                    }
                    break;
                default:
                    {
                  
                    }
                    break;
            }

            m_eCurState = m_eNextState;
        }
    }

    private void DoController()
    {
        switch (m_eCurState)
        {
            case DataEnum.eState.NoActive:
                {
                    DoNoActiveState();
                }
                break;
            case DataEnum.eState.Ready:
                {
                    DoReadyState();
                }
                break;
            case DataEnum.eState.Active:
                {
                    DoActiveState();
                }
                break;
            case DataEnum.eState.Dead:
                {
                    DoDeadState();
                }
                break;
            default:
                {
                    m_fReadyTimer += Time.deltaTime;

                    if (m_fReadyTimer > m_fReadyTimerMax)
                    {
                        m_fReadyTimer = 0;
                        m_eNextState = DataEnum.eState.Ready;

                    }
                }
                break;
        }
    }

    #endregion
    private void DoNoActiveState()
    {
       
    }
    private void DoReadyState()
    {
        CheckInStageBoard();
        InStageBoard();
        ReadyToActive();
    }
    private void DoActiveState()
    {
        CheckTarget();
    }
    private void DoDeadState()
    {

    }

    private void CheckDead()
    {

    }

    #region ReadyState_Func

    private void CheckInStageBoard()
    {
        if ((m_tagStatus.iStatus & GConst.BaseValue.iStatFlag_CheckInStage)
            != GConst.BaseValue.iStatFlag_CheckInStage)
        {
            Vector3 vPos = transform.position;
            if (GConst.BaseValue.vLeftTop.x < vPos.x &&
                GConst.BaseValue.vRightBottom.x > vPos.x)
            {
                if (GConst.BaseValue.vLeftTop.z > vPos.z &&
                GConst.BaseValue.vRightBottom.z < vPos.z)
                {
                    m_vCurModifyPos = vPos;
                    m_vNextModifyPos = vPos;

                    m_tagStatus.iStatus |= GConst.BaseValue.iStatFlag_CheckInStage;
                }
            }
        }
    }

    private void InStageBoard() 
    {
        //타워가 게임 보드 안으로 들어왔을 때

        if ((m_tagStatus.iStatus & GConst.BaseValue.iStatFlag_CheckInStage)
            == GConst.BaseValue.iStatFlag_CheckInStage)
        {
            if ((m_tagStatus.iStatus & GConst.BaseValue.iStatFlag_ReadyToIdle)
             != GConst.BaseValue.iStatFlag_ReadyToIdle)
            {
                if(m_vCurModifyPos == m_vNextModifyPos)
                {
                    m_fReadyTimer += Time.deltaTime;
                    if (m_fReadyTimer > m_fReadyTimerMax)
                    {
                        m_fReadyTimer = 0;
                        m_eNextState = DataEnum.eState.Active;
                        m_tagStatus.iStatus |= GConst.BaseValue.iStatFlag_ReadyToIdle;
                    }
                }
                else
                {
                    Modify_Pos();
                    m_fReadyTimer = 0;
                }
                
              
            }
           
        }
    }

    private void ReadyToActive()
    {
        if ((m_tagStatus.iStatus & GConst.BaseValue.iStatFlag_ReadyToIdle)
            == GConst.BaseValue.iStatFlag_ReadyToIdle)
        {
            EndModifyPos();
            m_eNextState = DataEnum.eState.Active;
        }
    }

    private void Construction_Ready()
    {

    }
    #endregion

    #region ActiveState_Func

    private void CheckTarget()
    {
        if (m_objTargetMob == null)
        {
            StageController stageController = GameObject.FindWithTag("TotalController").GetComponent<StageController>();
            int iMobCount = stageController.Get_MobCount;
            if (iMobCount < 1)
                return;
            int iMaxCount = stageController.Get_WaveMaxCreateCount;
            string strWaveName = "Wave_" + stageController.Get_Wave;
            if (Object_Manager.Instance.m_dictClone_Object.ContainsKey(strWaveName))
            {
                for (int i = 0; i < iMaxCount; ++i)
                {
                    //GameObject temp = GameObject.FindWithTag("Management").GetComponent<Object_Manager>().m_dictClone_Object[strWaveName][m_iTargetID];

                    if (i >= Object_Manager.Instance.m_dictClone_Object[strWaveName].Count)
                    {
                        break;
                    }
                    GameObject temp = Object_Manager.Instance.m_dictClone_Object[strWaveName][i];

                    if (temp == null)
                    {
                        continue;
                    }
                    if (temp.GetComponent<MobAI>().GetState != DataEnum.eState.Active)
                        continue;

                    Vector3 vecTargetToLength = temp.transform.position - m_Transform.position;
                    float fLength = vecTargetToLength.magnitude;

                    if (fLength > m_tagStatus.fRange)
                        continue;

                    m_objTargetMob = temp;
                    m_iTargetID = i;
                }
            }
        }

        if (m_objTargetMob)
            DoAttack();
        else
            DoIdle();
    }

    private void DoAttack()
    {
        m_tagStatus.fAtkCoolTime += Time.deltaTime;

        if (m_objTargetMob != null)
        {
            if (m_objTargetMob.GetComponent<MobAI>().GetState != DataEnum.eState.Active)
            {
                m_objTargetMob = null;
                m_iTargetID = 0;
                return;

            }
            m_Transform.LookAt(m_objTargetMob.transform);
            Vector3 vecTargetToLength = m_objTargetMob.transform.position - m_Transform.position;
            float fLength = vecTargetToLength.magnitude;

            if (fLength > m_tagStatus.fRange)
            {
                //  Debug.Log("RangeOut");
                m_objTargetMob = null;
                m_iTargetID = 0;
            }
        }

        if (m_tagStatus.fAtkCoolTime > m_tagStatus.fMaxAtkCoolTime)
        {
            m_tagStatus.fAtkCoolTime = 0;
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
       
        // Debug.Log("Attack");
        DataStruct.tagBulletStatus tagTemp = new DataStruct.tagBulletStatus();
        tagTemp.iAtk = m_tagStatus.iAtk;
        if (m_tagStatus.fRange > 0 || tagTemp.fMaxLifeTime<0)
            tagTemp.fMaxLifeTime = m_tagStatus.fRange;
        else
            tagTemp.fMaxLifeTime = 1f;

        tagTemp.fLifeTime = 0;
        tagTemp.strObjTagName = "Empty_Bullet";
        tagTemp.objTarget = m_objTargetMob;
        tagTemp.fMoveSpeed = 10.0f;

        GameObject retObj = ObjPool_Manager.Instance.Get_ObjPool(this.transform.position, tagTemp);
        retObj.GetComponent<BaseBullet>().SetState = DataEnum.eState.Ready;
        //공격
        //공격 중 타겟팅이 벗어나면 해제
        //  m_objTargetMob
    }

    private void DoIdle()
    {
        m_tagStatus.fAtkCoolTime = 0;
    }

    #endregion

    public void StartModifyPos()
    {
        m_tagStatus.iStatus |= GConst.BaseValue.iStatFlag_CheckModifyStart;

    }
    public void EndModifyPos()
    {
        m_tagStatus.iStatus |= GConst.BaseValue.iStatFlag_CheckModifyEnd;
    }
    private void Modify_Pos()
    {


        if (m_vCurModifyPos != m_vNextModifyPos)
        {
            m_vCurModifyPos = m_vNextModifyPos;
            transform.position = m_vCurModifyPos;
        }
    }
}
