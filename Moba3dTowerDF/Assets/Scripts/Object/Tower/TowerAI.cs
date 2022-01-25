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

    public DataStruct.tagEffectInfo m_tEffectInfo;

    #endregion

    #region Property
    public DataEnum.eState GetStat { get { return m_eCurState; } }
    public DataStruct.tagTowerStatus Get_TowerInfo { get { return m_tagStatus; } }
    public DataEnum.eRankID Get_TowerRank { get
        {
            int id = m_tagStatus.iTowerId + 10;
            int num = (int)(id * 0.1);

            if (1 == num)
                return DataEnum.eRankID.Normal;
            else if (2 == num)
                return DataEnum.eRankID.Magic;
            else if (3 == num)
                return DataEnum.eRankID.Rare;
            else if (4 == num)
                return DataEnum.eRankID.Epic;
            else if (5 == num)
                return DataEnum.eRankID.Unique;
            else
                return DataEnum.eRankID.End;
        }
    }
    public DataEnum.eRankID Get_TowerNum
    {
        get
        {
            if ((m_tagStatus.iTowerId & (int)DataEnum.eRankID.RankID_0) == (int)DataEnum.eRankID.RankID_0)
                return DataEnum.eRankID.RankID_0;
            else if ((m_tagStatus.iTowerId & (int)DataEnum.eRankID.RankID_1) == (int)DataEnum.eRankID.RankID_1)
                return DataEnum.eRankID.RankID_1;
            else if ((m_tagStatus.iTowerId & (int)DataEnum.eRankID.RankID_2) == (int)DataEnum.eRankID.RankID_2)
                return DataEnum.eRankID.RankID_2;
            else if ((m_tagStatus.iTowerId & (int)DataEnum.eRankID.RankID_3) == (int)DataEnum.eRankID.RankID_3)
                return DataEnum.eRankID.RankID_3;
            else if ((m_tagStatus.iTowerId & (int)DataEnum.eRankID.RankID_4) == (int)DataEnum.eRankID.RankID_4)
                return DataEnum.eRankID.RankID_4;
            else if ((m_tagStatus.iTowerId & (int)DataEnum.eRankID.RankID_5) == (int)DataEnum.eRankID.RankID_5)
                return DataEnum.eRankID.RankID_5;
            else if ((m_tagStatus.iTowerId & (int)DataEnum.eRankID.RankID_6) == (int)DataEnum.eRankID.RankID_6)
                return DataEnum.eRankID.RankID_6;
            else if ((m_tagStatus.iTowerId & (int)DataEnum.eRankID.RankID_7) == (int)DataEnum.eRankID.RankID_7)
                return DataEnum.eRankID.RankID_7;
            else
                return DataEnum.eRankID.End;
        }
    }

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
        RenderFunc();
        CheckState();
        DoController();
    }

    private void UpdateInit()
    {
        Rename_Clone();
        UpdateInit_Effect();

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

    private void UpdateInit_Effect()
    {
        Base_Effect effect = GetComponentInChildren<Base_Effect>();
        if (effect != null)
        {
            effect.m_tEffectInfo.fParticleScale = 5f;
            effect.m_tEffectInfo.vScale = new Vector3(0.5f, 0.5f, 0.5f);
            Vector3 vPos = effect.transform.position;
            vPos.y += 0.5f;
            effect.transform.position = vPos;
            if (DataEnum.eRankID.Normal == Get_TowerRank)
            {
            }
            else if (DataEnum.eRankID.Magic == Get_TowerRank)
            {
                effect.m_tEffectInfo.colorEffect = Color.yellow;
                effect.m_tEffectInfo.fSpeed = 1.25f;
                //ps.shape = sha;
            }
            else if (DataEnum.eRankID.Rare == Get_TowerRank)
            {
                effect.m_tEffectInfo.colorEffect = Color.blue;
                effect.m_tEffectInfo.fSpeed = 1.5f;
            }
            else if (DataEnum.eRankID.Epic == Get_TowerRank)
            {
                effect.m_tEffectInfo.colorEffect = Color.magenta;
                effect.m_tEffectInfo.fSpeed = 1.75f;
            }
            else if (DataEnum.eRankID.Unique == Get_TowerRank)
            {
                effect.m_tEffectInfo.colorEffect = Color.black;
                effect.m_tEffectInfo.fSpeed = 2f;
                ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                ParticleSystem.ShapeModule sha = ps.shape;
                sha.radius = 0;
                sha.radiusThickness = 1;
                effect.m_tEffectInfo.vScale = new Vector3(1.25f, 1.25f, 1.25f);

            }
            effect.Copy_ClassInfoToParticleSys();
        }

    }

    private void RenderFunc()
    {
        if(m_bSelect)
        {
            Shader rimlight = GameObject.FindWithTag("TotalController").GetComponent<ShaderController>().Get_Shader("Rimlight_Shader");
            //Shader rimlight = Shader.Find("Custom/Rimlight_Shader");
            if (rimlight == null)
            {
                GFunc.Function.Print_Log("rimlight null.");
                return;

            }
            //rimlight.
            GetComponentInChildren<SkinnedMeshRenderer>().material.shader = rimlight;
            //float fPow = m_objPickTower.GetComponentInChildren<Renderer>().material.GetFloat("_Pow");
            GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat("_Pow", 1.0f);

            GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_RimCol", new Color(0, 1, 0));
        }
        else
        {
            Shader rimlight =GameObject.FindWithTag("TotalController"). GetComponent<ShaderController>().Get_Shader("Default_Shader");
            //Shader rimlight = Shader.Find("Custom/Rimlight_Shader");
            if (rimlight == null)
            {
                GFunc.Function.Print_Log("rimlight null.");
                return;

            }
            //rimlight.
            GetComponentInChildren<SkinnedMeshRenderer>().material.shader = rimlight;
            ////float fPow = m_objPickTower.GetComponentInChildren<Renderer>().material.GetFloat("_Pow");
            //GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat("_Pow", 8.0f);
            ////GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat("_Holo", 0);
            //if (Get_TowerRank == DataEnum.eRankID.Normal)
            //    GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_RimCol", new Color(0, 0, 1));
        }
      
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
                    gameObject.layer = 0;
                    DoNoActiveState();
                }
                break;
            case DataEnum.eState.Ready:
                {
                    gameObject.layer = LayerMask.NameToLayer("Tower");
                    DoReadyState();
                }
                break;
            case DataEnum.eState.Active:
                {
                    gameObject.layer = LayerMask.NameToLayer("Tower");
                    DoActiveState();
                }
                break;
            case DataEnum.eState.Dead:
                {
                    gameObject.layer = 0;
                    DoDeadState();
                }
                break;
            default:
                {
                    gameObject.layer = 0;
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

            RaycastHit hit;

            Vector3 vector3Orin = vPos;

            vector3Orin.y += 0.5f;
            Ray ray = new Ray(vector3Orin,
               new Vector3(0, -1, 0));

            if (Physics.Raycast(ray, out hit, 3f, (1 << LayerMask.NameToLayer("Tile"))))
            {
                m_vCurModifyPos = vPos;
                m_vNextModifyPos = vPos;

                m_tagStatus.iStatus |= GConst.BaseValue.iStatFlag_CheckInStage;
            }


            //if (GConst.BaseValue.vLeftTop.x < vPos.x &&
            //    GConst.BaseValue.vRightBottom.x > vPos.x)
            //{
            //    if (GConst.BaseValue.vLeftTop.z > vPos.z &&
            //    GConst.BaseValue.vRightBottom.z < vPos.z)
            //    {
            //        m_vCurModifyPos = vPos;
            //        m_vNextModifyPos = vPos;

            //        m_tagStatus.iStatus |= GConst.BaseValue.iStatFlag_CheckInStage;
            //    }
            //}
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
                    //m_fReadyTimer += Time.deltaTime;
                    //if (m_fReadyTimer > m_fReadyTimerMax)
                    //{
                        m_fReadyTimer = 0;
                        m_eNextState = DataEnum.eState.Active;
                        m_tagStatus.iStatus |= GConst.BaseValue.iStatFlag_ReadyToIdle;
                   // }
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
