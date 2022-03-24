using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerAI : BaseObj
{
    //TowerAI()
    //    : base() { }

    #region Value

    [SerializeField] protected DataStruct.tagTowerStatus m_tTowerInfo;

    public DataStruct.tagEffectInfo m_tEffectInfo;
    //[SerializeField] protected DataStruct.tagStatusInfo m_tStatusInfo;

    [SerializeField] protected GameObject m_objTargetMob;
    [SerializeField] protected int m_iTargetID;

    [SerializeField] protected DataEnum.eState m_eNextState = DataEnum.eState.End;
    [SerializeField] protected DataEnum.eState m_eCurState = DataEnum.eState.End;


    [SerializeField] protected Vector3 m_vCurModifyPos;
    [SerializeField] protected Vector3 m_vNextModifyPos;


    [SerializeField] protected string m_strBulletName;
    #endregion

    #region Property
    public DataEnum.eState GetStat { get { return m_eCurState; } }
    public DataStruct.tagTowerStatus Get_TowerInfo { get { return m_tTowerInfo; } }
    public DataEnum.eRankID Get_TowerRank { get
        {
            int id = m_tTowerInfo.iTowerId + 10;
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
            if ((m_tTowerInfo.iTowerId & (int)DataEnum.eRankID.RankID_0) == (int)DataEnum.eRankID.RankID_0)
                return DataEnum.eRankID.RankID_0;
            else if ((m_tTowerInfo.iTowerId & (int)DataEnum.eRankID.RankID_1) == (int)DataEnum.eRankID.RankID_1)
                return DataEnum.eRankID.RankID_1;
            else if ((m_tTowerInfo.iTowerId & (int)DataEnum.eRankID.RankID_2) == (int)DataEnum.eRankID.RankID_2)
                return DataEnum.eRankID.RankID_2;
            else if ((m_tTowerInfo.iTowerId & (int)DataEnum.eRankID.RankID_3) == (int)DataEnum.eRankID.RankID_3)
                return DataEnum.eRankID.RankID_3;
            else if ((m_tTowerInfo.iTowerId & (int)DataEnum.eRankID.RankID_4) == (int)DataEnum.eRankID.RankID_4)
                return DataEnum.eRankID.RankID_4;
            else if ((m_tTowerInfo.iTowerId & (int)DataEnum.eRankID.RankID_5) == (int)DataEnum.eRankID.RankID_5)
                return DataEnum.eRankID.RankID_5;
            else if ((m_tTowerInfo.iTowerId & (int)DataEnum.eRankID.RankID_6) == (int)DataEnum.eRankID.RankID_6)
                return DataEnum.eRankID.RankID_6;
            else if ((m_tTowerInfo.iTowerId & (int)DataEnum.eRankID.RankID_7) == (int)DataEnum.eRankID.RankID_7)
                return DataEnum.eRankID.RankID_7;
            else
                return DataEnum.eRankID.End;
        }
    }

    public DataEnum.eState SetState { set { m_eNextState = value; } }
    public int Set_TowerID { set { m_tTowerInfo.iTowerId = value; } }
    public DataStruct.tagTowerStatus Set_TowerInfo { set { m_tTowerInfo = value; } }

    #endregion

    protected virtual void Update()
    {
        if (!m_bFirstInit)
            UpdateInit();
        if (m_objTargetMob == null)
            m_iTargetID = 0;
        EffectFunc();
        RenderFunc();
        CheckState();
        DoController();
    }

    protected virtual void UpdateInit()
    {
        Rename_Clone();
        UpdateInit_Effect();

        if (0 == m_fReadyTimerMax)
        {
            m_fReadyTimer = 3;
        }
        if (m_tTowerInfo.fMaxAtkCoolTime == 0)
            m_tTowerInfo.fMaxAtkCoolTime = 1f;
        if (m_tTowerInfo.fRange == 0)
            m_tTowerInfo.fRange = 3f;
        if (m_tTowerInfo.iAtk == 0)
            m_tTowerInfo.iAtk = 5;
        //if( m_tTowerInfo.eType == DataEnum.eTowerType.Buff)
        //{

        //}
        m_bFirstInit = true;
    }

    protected virtual void EffectFunc()
    {
        Base_Effect effect = GetComponentInChildren<Base_Effect>();
        if (effect != null)
        {
            if (DataEnum.eRankID.Normal == Get_TowerRank)
            {

            }
            else if (DataEnum.eRankID.Magic == Get_TowerRank)
            {

                //ps.shape = sha;
            }
            else if (DataEnum.eRankID.Rare == Get_TowerRank)
            {
                //var rot = effect.transform.rotation;
                //var vAngle = rot.eulerAngles;
                //vAngle.y += (Time.deltaTime*90f);
                //if (vAngle.y > 360)
                //    vAngle.y = 0;
                //rot.eulerAngles = vAngle;
                //effect.transform.rotation = rot;
            }
            else if (DataEnum.eRankID.Epic == Get_TowerRank)
            {
                //var rot = effect.transform.rotation;
                //var vAngle = rot.eulerAngles;
                //vAngle.y += (Time.deltaTime * 90f);
                //if (vAngle.y > 360)
                //    vAngle.y = 0;
                //rot.eulerAngles = vAngle;
                //effect.transform.rotation = rot;
            }
            else if (DataEnum.eRankID.Unique == Get_TowerRank)
            {
                var rot = effect.transform.rotation;
                var vAngle = rot.eulerAngles;
                vAngle.y += (Time.deltaTime * 90f);
                if (vAngle.y > 360)
                    vAngle.y = 0;
                vAngle.x = -vAngle.y;
                rot.eulerAngles = vAngle;
                effect.transform.rotation = rot;

            }
        }
    }

    protected virtual void UpdateInit_Effect()
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
                effect.m_tEffectInfo.colorEffect = Color.green;
                effect.m_tEffectInfo.fSpeed = 1.5f;
            }
            else if (DataEnum.eRankID.Epic == Get_TowerRank)
            {
                effect.m_tEffectInfo.colorEffect = new Color(30f / 255f, 30f/255f,1f);
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
        if (m_bSelect)
        {
            Shader rimlight = GameObject.FindWithTag("TotalController").GetComponent<ShaderController>().Get_Shader("Rimlight_Shader");
            //Shader rimlight = Shader.Find("Custom/Rimlight_Shader");
            if (rimlight == null)
            {
                GFunc.Function.Print_Log("rimlight null.");
                return;

            }
            //rimlight.
            Renderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (renderer == null)
                renderer = GetComponentInChildren<MeshRenderer>();
            renderer.material.shader = rimlight;
            renderer.material.SetFloat("_Pow", 1.0f);
            renderer.material.SetColor("_RimCol", new Color(0, 1, 0));
        }
        else
        {
            Shader rimlight = GameObject.FindWithTag("TotalController").GetComponent<ShaderController>().Get_Shader("Default_Shader");
            //Shader rimlight = Shader.Find("Custom/Rimlight_Shader");
            if (rimlight == null)
            {
                GFunc.Function.Print_Log("rimlight null.");
                return;

            }
            //rimlight.
            Renderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (renderer == null)
                renderer = GetComponentInChildren<MeshRenderer>();
            renderer.material.shader = rimlight;
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

    protected virtual void DoReadyState()
    {
        CheckInStageBoard();
        InStageBoard();
        ReadyToActive();
    }
    protected abstract void DoNoActiveState();
    protected abstract void DoActiveState();
    protected abstract void DoDeadState();
    protected abstract void CheckDead();


    #region ReadyState_Func

    private void CheckInStageBoard()
    {
        if ((m_tTowerInfo.iStatus & GConst.BaseValue.iStatFlag_CheckInStage)
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

                m_tTowerInfo.iStatus |= GConst.BaseValue.iStatFlag_CheckInStage;
            }


            //if (GConst.BaseValue.vLeftTop.x < vPos.x &&
            //    GConst.BaseValue.vRightBottom.x > vPos.x)
            //{
            //    if (GConst.BaseValue.vLeftTop.z > vPos.z &&
            //    GConst.BaseValue.vRightBottom.z < vPos.z)
            //    {
            //        m_vCurModifyPos = vPos;
            //        m_vNextModifyPos = vPos;

            //        m_tTowerInfo.iStatus |= GConst.BaseValue.iStatFlag_CheckInStage;
            //    }
            //}
        }
    }

    private void InStageBoard() 
    {
        //타워가 게임 보드 안으로 들어왔을 때

        if ((m_tTowerInfo.iStatus & GConst.BaseValue.iStatFlag_CheckInStage)
            == GConst.BaseValue.iStatFlag_CheckInStage)
        {
            if ((m_tTowerInfo.iStatus & GConst.BaseValue.iStatFlag_ReadyToIdle)
             != GConst.BaseValue.iStatFlag_ReadyToIdle)
            {
                if(m_vCurModifyPos == m_vNextModifyPos)
                {
                    //m_fReadyTimer += Time.deltaTime;
                    //if (m_fReadyTimer > m_fReadyTimerMax)
                    //{
                        m_fReadyTimer = 0;
                        m_eNextState = DataEnum.eState.Active;
                        m_tTowerInfo.iStatus |= GConst.BaseValue.iStatFlag_ReadyToIdle;
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
        if ((m_tTowerInfo.iStatus & GConst.BaseValue.iStatFlag_ReadyToIdle)
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

    protected void CheckTarget()
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

                    if (fLength > m_tTowerInfo.fRange)
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
        m_tTowerInfo.fAtkCoolTime += Time.deltaTime;

        if (m_objTargetMob != null)
        {
            if (m_objTargetMob.GetComponent<MobAI>().GetState != DataEnum.eState.Active)
            {
                m_objTargetMob = null;
                m_iTargetID = 0;
                return;

            }
            var tran = m_objTargetMob.transform;
            var pos = tran.position;
            pos.y= this.transform.position.y;
            tran.position = pos;
            m_Transform.LookAt(tran);
            Vector3 vecTargetToLength = m_objTargetMob.transform.position - m_Transform.position;
            float fLength = vecTargetToLength.magnitude;

            if (fLength > m_tTowerInfo.fRange)
            {
                //  Debug.Log("RangeOut");
                m_objTargetMob = null;
                m_iTargetID = 0;
            }
        }

        //추후 쿨타임이 아닌 애니메이션 기반으로 변경 필요함.
        if (m_tTowerInfo.fAtkCoolTime > m_tTowerInfo.fMaxAtkCoolTime)
        {
            m_tTowerInfo.fAtkCoolTime = 0;
            if(m_tTowerInfo.eType == DataEnum.eTowerType.Atk)
                CreateBullet(/*"Magic_Bullet_0"*/);
            else if(m_tTowerInfo.eType == DataEnum.eTowerType.Atk_HitScan)
                HitScan();
        }
    }

    private void CreateBullet()
    {
        if (m_objTargetMob == null)
            return;
       
        // Debug.Log("Attack");
        DataStruct.tagBulletStatus tagTemp = new DataStruct.tagBulletStatus();
        if (m_tTowerInfo.iAtk < 0)
            tagTemp.iAtk = 0;
        else
            tagTemp.iAtk = m_tTowerInfo.iAtk;

        if (m_tTowerInfo.fRange > 0 || tagTemp.fMaxLifeTime<0)
            tagTemp.fMaxLifeTime = m_tTowerInfo.fRange;
        else
            tagTemp.fMaxLifeTime = 1f;

        tagTemp.fLifeTime = 0;
        tagTemp.strObjTagName = m_strBulletName;
        tagTemp.objTarget = m_objTargetMob;
        tagTemp.fMoveSpeed = 5.0f;
        Vector3 vDir = m_objTargetMob.transform.position - this.transform.position;
        vDir.Normalize();
        Vector3 vCreatePos = (vDir * 0.85f) + this.transform.position;
        GameObject retObj = ObjPool_Manager.Instance.Get_ObjPool(vCreatePos, tagTemp);

        if (retObj)
        {
            retObj.GetComponent<BaseBullet>().SetState = DataEnum.eState.Ready;
            retObj.GetComponent<BaseBullet>().m_tStatusInfo = m_tTowerInfo.tStatusInfo;
            retObj.GetComponent<BaseBullet>().m_tBulletInfo.iStatus = m_tTowerInfo.iBulletStatus;
        }

        //공격
        //공격 중 타겟팅이 벗어나면 해제
        //  m_objTargetMob
    }

   protected void HitScan()
    {
        GFunc.Function.Print_Log("Called HitScan");
        
        if (m_objTargetMob == null)
            return;
 
        Vector3 vCreateEffectPos = m_objTargetMob.transform.position;
        vCreateEffectPos.y += 1f;

        int iTempAtk = 0;
    
        if (m_tTowerInfo.iAtk < 0)
            iTempAtk = 0;
        else
            iTempAtk = m_tTowerInfo.iAtk;
        
        if (iTempAtk > 0)
            m_objTargetMob.GetComponent<MobAI>().Add_Damage = (iTempAtk);

        Passing_StatusInfo(m_objTargetMob);

        //이펙트 추가
        GFunc.Function.Print_Log("HitScan");
        GameObject objEffect=  GameObject.FindGameObjectWithTag("TotalController").GetComponent<EffectPoolController>().Get_ObjPool(vCreateEffectPos, m_strBulletName);
        objEffect.GetComponent<Base_Effect>().Set_Color(m_tEffectInfo.colorEffect);
    }
    
    protected void Passing_StatusInfo(GameObject _objMonster)
    {
        int iMobStatus = _objMonster.GetComponent<MobAI>().m_tMobInfo.iStatus;
        if (( m_tTowerInfo.iBulletStatus & (int)DataEnum.eStatus.Fire) == (int)DataEnum.eStatus.Fire)
        {
            if (!((iMobStatus & (int)DataEnum.eStatus.Fire) == (int)DataEnum.eStatus.Fire))
            {
                _objMonster.GetComponent<MobAI>().Set_FireStatus(m_tTowerInfo.tStatusInfo);
            }
        }
        if (( m_tTowerInfo.iBulletStatus & (int)DataEnum.eStatus.Poison) == (int)DataEnum.eStatus.Poison)
        {
            if (!((iMobStatus & (int)DataEnum.eStatus.Poison) == (int)DataEnum.eStatus.Poison))
            {
                _objMonster.GetComponent<MobAI>().Set_PoisonStatus(m_tTowerInfo.tStatusInfo);
            }
        }
        if (( m_tTowerInfo.iBulletStatus & (int)DataEnum.eStatus.Slow) == (int)DataEnum.eStatus.Slow)
        {
            if (!((iMobStatus & (int)DataEnum.eStatus.Slow) == (int)DataEnum.eStatus.Slow))
            {
                _objMonster.GetComponent<MobAI>().Set_SlowStatus(m_tTowerInfo.tStatusInfo);
            }
        }
        if (( m_tTowerInfo.iBulletStatus & (int)DataEnum.eStatus.Stun) == (int)DataEnum.eStatus.Stun)
        {
            if (!((iMobStatus & (int)DataEnum.eStatus.Stun) == (int)DataEnum.eStatus.Stun))
            {
                _objMonster.GetComponent<MobAI>().Set_StunStatus(m_tTowerInfo.tStatusInfo);
            }
        }
    }

    private void DoIdle()
    {
        m_tTowerInfo.fAtkCoolTime = 0;
    }

    #endregion

    public void StartModifyPos()
    {
        m_tTowerInfo.iStatus |= GConst.BaseValue.iStatFlag_CheckModifyStart;

    }
    public void EndModifyPos()
    {
        m_tTowerInfo.iStatus |= GConst.BaseValue.iStatFlag_CheckModifyEnd;
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
