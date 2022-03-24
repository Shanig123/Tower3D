using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAI : BaseObj
{
    MobAI()
        :base()
    {
       
    }


    #region Values



    public DataStruct.tagMobStatus m_tMobInfo;
    public DataStruct.tagStatusInfo m_tStatusInfo;
    [SerializeField] private PlayerController m_playerController;

    [SerializeField] private Vector3[] m_arrWaypoints;
    [SerializeField] private int m_iCurTargetWaypoint = 0;
    [SerializeField] private int m_iNextTargetWaypoint = 0;


    [SerializeField] private DataEnum.eState m_eNextMobState = DataEnum.eState.End;
    [SerializeField] private DataEnum.eState m_eCurMobState = DataEnum.eState.End;

    //private int m_iDamage_Buffer;
    //private int m_iDotDamage_Buffer;
    //private int m_iFireStatusDamage_Buffer;

    private DataStruct.tagDamageStruct m_tDamageStruct;
    #endregion
    #region Property
    public DataEnum.eState GetState
    {
        get
        {
            return m_eCurMobState;
        }
    }
    public int Get_Status { get { return m_tMobInfo.iStatus; } }


    public DataEnum.eState SetState
    {
        set
        {
            m_eNextMobState = value;
        }
    }
    public int Add_HP
    {
        set
        {
            m_tMobInfo.iHp += value;
        }
    }

    public int Add_Damage
    {
        set
        {
            m_tDamageStruct.iDamage_Buffer += value;
            //m_iFireStatusDamage_Buffer += value;
        }
    }

    public void Set_FireStatus(DataStruct.tagStatusInfo _tStatusInfo)
    {
        m_tMobInfo.iStatus |= (int)DataEnum.eStatus.Fire;
        m_tStatusInfo.fFireStatusTime = 0;
        m_tStatusInfo.fFireMaxStatusTime = _tStatusInfo.fFireMaxStatusTime;
        m_tStatusInfo.fFireRatio = _tStatusInfo.fFireRatio;
    }
    public void Set_PoisonStatus(DataStruct.tagStatusInfo _tStatusInfo)
    {
        m_tMobInfo.iStatus |= (int)DataEnum.eStatus.Poison;
        m_tStatusInfo.fPoisonStatusTime = 0;
        m_tStatusInfo.fPoisonDotTime = 0;
        if (_tStatusInfo.fPoisonMaxStatusTime > 0)
            m_tStatusInfo.fPoisonMaxStatusTime = _tStatusInfo.fPoisonMaxStatusTime;
        else
            m_tStatusInfo.fPoisonMaxStatusTime = 1f;

        if (_tStatusInfo.fPoisonDotMaxTime > 0)
            m_tStatusInfo.fPoisonDotMaxTime = _tStatusInfo.fPoisonDotMaxTime;
        else
            m_tStatusInfo.fPoisonDotMaxTime = 0.5f;

        if (_tStatusInfo.iPoisonDamage > 0)
            m_tStatusInfo.iPoisonDamage = _tStatusInfo.iPoisonDamage;
        else
            m_tStatusInfo.iPoisonDamage = 3;

    }

    public void Set_SlowStatus(DataStruct.tagStatusInfo _tStatusInfo)
    {
        m_tMobInfo.iStatus |= (int)DataEnum.eStatus.Slow;
        m_tStatusInfo.fSlowSpeed = 0;

        if (_tStatusInfo.fSlowMaxStatusTime > 0)
            m_tStatusInfo.fSlowMaxStatusTime = _tStatusInfo.fSlowMaxStatusTime;
        else
            m_tStatusInfo.fSlowMaxStatusTime = 1f;

        if (_tStatusInfo.fSlowSpeed > 0)
            m_tStatusInfo.fSlowSpeed = _tStatusInfo.fSlowSpeed;
        else
            m_tStatusInfo.fSlowSpeed = 0.25f;      
        
    }

    public void Set_StunStatus(DataStruct.tagStatusInfo _tStatusInfo)
    {
        m_tMobInfo.iStatus |= (int)DataEnum.eStatus.Stun;
        m_tStatusInfo.fStunStatusTime = 0;

        if (_tStatusInfo.fStunMaxStatusTime > 0)
            m_tStatusInfo.fStunMaxStatusTime = _tStatusInfo.fStunMaxStatusTime;
        else
            m_tStatusInfo.fStunMaxStatusTime = 1f;

    }
    #endregion
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GetComponentInChildren<Renderer>().material.shader = Shader.Find("Standard");
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_bFirstInit)
            FirstInit();
        CheckMobState();
        MobController();
    }
    private void FirstInit()
    {
        if (!Object_Manager.Instance.m_dictClone_Object.ContainsKey("Waypoints"))
            return;
        //Object_Manager.Instance.m_dictClone_Object["Waypoints"].Count
        m_arrWaypoints = new Vector3[12];
        for(int i=0; i<m_arrWaypoints.Length;++i)
        {
            m_arrWaypoints[i] = Object_Manager.Instance.m_dictClone_Object["Waypoints"][i].GetComponent<Transform>().position;
        }

        if(null == m_playerController)
        {
            m_playerController = GameObject.FindWithTag("TotalController").GetComponent<PlayerController>();
        }
        DefaultStat();

        m_bFirstInit = true;
    }

    private void DefaultStat()
    {
        if (m_tMobInfo.fMoveSpeed == 0)
            m_tMobInfo.fMoveSpeed = 3f;
        if (m_tMobInfo.iHp == 0)
        {
            StageController stageController= GameObject.FindGameObjectWithTag("TotalController").GetComponent<StageController>();
            m_tMobInfo.iHp = 5 + (stageController.Get_Wave * 2);
            m_tMobInfo.iMaxHp = m_tMobInfo.iHp;

        }
    }
    private void CheckMobState()
    {
        if(m_eCurMobState != m_eNextMobState)
        {
            switch(m_eNextMobState)
            {
                case DataEnum.eState.NoActive:
                    {
                        m_bObjActiveOnOff = false;
                        m_bCheckDead = false;
                    }
                    break;
                case DataEnum.eState.Ready:
                    {
                        m_bObjActiveOnOff = true;
                        m_bCheckDead = false;
                        gameObject.GetComponent<Collider>().enabled = true;
                    }
                    break;
                case DataEnum.eState.Active:
                    {
                        var pos = this.transform.position;
                        pos.y = m_arrWaypoints[m_iCurTargetWaypoint].y;
                        this.transform.position = pos;
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

            m_eCurMobState = m_eNextMobState;
        }
    }

    private void MobController()
    {
        switch (m_eCurMobState)
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

                }
                break;
        }
    }

    private void DoNoActiveState()
    {
        m_iNextTargetWaypoint = 0;

        StageController temp = GameObject.FindWithTag("TotalController").GetComponent<StageController>();
        temp.Min_MobCount();

        Destroy(gameObject);
    }
    private void DoReadyState()
    {
        m_fReadyTimer += Time.deltaTime;

        if (m_fReadyTimer > m_fReadyTimerMax)
        {
            m_fReadyTimer = 0;
            m_eNextMobState = DataEnum.eState.Active;
            m_iNextTargetWaypoint = 1;
            m_iCurTargetWaypoint = 1;

  
        }
    }
    private void DoActiveState()
    {
        Status_Phase();
        Damage_Phase();

        if (m_tMobInfo.iHp < 0)
        {
            CheckDead();
        }
        else
            CheckTarget();
    }
    private void DoDeadState()
    {
        m_tMobInfo.fDeadTime += Time.deltaTime;
        if (m_tMobInfo.fDeadTime > 5.0f)
        {
            StageController temp = GameObject.FindWithTag("TotalController").GetComponent<StageController>();
            temp.Min_MobCount();
            Destroy(gameObject);
            m_tMobInfo.fDeadTime = 0;

        }
        //m_Ani.GetCurrentAnimatorStateInfo().normalizedTime
    
    }

    private void CheckDead()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        m_playerController.Add_KillCount();
        m_eNextMobState = DataEnum.eState.Dead;
       
        m_Ani.SetBool("isDead1", true);
        m_Ani.SetBool("isDead", true);
        //m_Ani.GetBool()
    }

    #region Move_Func

    private void CheckTarget()
    {
        Vector3 vTargetPos = m_arrWaypoints[m_iCurTargetWaypoint];
        vTargetPos.y = this.transform.position.y;
        Vector3 vecTargetToLength = vTargetPos - this.transform.position;
        float fLength = vecTargetToLength.magnitude;

        if (fLength < 0.1)
        {
            if (!EndCheckPoint(fLength))
            {
                ++m_iNextTargetWaypoint;
            }
        }
        else
        {
            if(m_iCurTargetWaypoint == m_iNextTargetWaypoint)
            {
                m_Transform.LookAt(vTargetPos);
                DoMove();
            }       
        }
        if (m_iNextTargetWaypoint != m_iCurTargetWaypoint)
        {
            m_iCurTargetWaypoint = m_iNextTargetWaypoint;
        }
    }

    private bool EndCheckPoint(float _fLengTh)
    {  //마지막 체크포인트 도달했을 때
        if (m_arrWaypoints[m_iCurTargetWaypoint] == m_arrWaypoints[m_arrWaypoints.Length - 1] && _fLengTh < 1.0)
        {
            GameObject.FindGameObjectWithTag("TotalController").GetComponent<PlayerController>().Add_Life(-1);
    
            m_eNextMobState = DataEnum.eState.NoActive;
            return true;
        }
        return false;
    }
    private float Calculate_Speed()
    {
        return  (float)((1-(int)m_tMobInfo.fMoveSpeed_Stun)) *
            m_tMobInfo.fMoveSpeed * (1f + (m_tMobInfo.fMoveSpeed_Fast - m_tMobInfo.fMoveSpeed_Slow));

    }
    private void DoMove()
    {
        float fResultSpeed = Calculate_Speed();
        if(fResultSpeed >0)
        {
            m_Transform.position += Time.deltaTime * fResultSpeed * m_Transform.forward;
            Ani_Run();
            m_Ani.speed = fResultSpeed * 0.5f;
        }
        else if(fResultSpeed ==0)
        {
            // m_Transform.position += Time.deltaTime * fResultSpeed * m_Transform.forward;
            Ani_Idle();
            m_Ani.speed = fResultSpeed * 0.5f;
        }
        
    }
    private void Ani_Run()
    {
        m_Ani.SetBool("isRunning", true);
    }
    private void Ani_Idle()
    {
        m_Ani.SetBool("isIdle", true);
        m_Ani.SetBool("Idle", true);
    }

    #endregion

    #region Damage+Status_Fuc

    private void Create_DamageEffect()
    {
        GameObject obj3DText = Resource_Manager.Instance.InstanceObj("3DText", "3DTextObject", transform.position);
        obj3DText.GetComponent<UI_3DText>().m_bEffect = true;
        obj3DText.GetComponent<UI_3DText>().Set_TextInfo(m_tDamageStruct.iDamage_Buffer.ToString(), new Color(1, 0, 0), (DataEnum.eTextEffect)11);
    }
    private void Create_DamageEffect(int _iDamage, Color _color)
    {
        GameObject obj3DText = Resource_Manager.Instance.InstanceObj("3DText", "3DTextObject", transform.position);
        obj3DText.GetComponent<UI_3DText>().m_bEffect = true;
        float fScale = 1.5f + (GameObject.FindGameObjectWithTag("TotalController").GetComponent<DataController>().ExtractRandomNumberFromSeed_NoCount()*0.5f);
        obj3DText.GetComponent<UI_3DText>().Set_TextInfo(_iDamage.ToString(), _color, (DataEnum.eTextEffect)27, fScale);
    }
    private void Create_StatusEffect(string _strStatusMessage, Color _color)
    {
        GameObject obj3DText = Resource_Manager.Instance.InstanceObj("3DText", "3DTextObject", transform.position);
        obj3DText.GetComponent<UI_3DText>().m_bEffect = true;
        obj3DText.GetComponent<UI_3DText>().Set_TextInfo(_strStatusMessage, _color, (DataEnum.eTextEffect.Up | DataEnum.eTextEffect.BillBoard | DataEnum.eTextEffect.SizeDown | DataEnum.eTextEffect.Default_AlphaDown));
    }

    private void Status_Phase()
    {
        int iStatus = m_tMobInfo.iStatus;
        if (iStatus == 0)
        {
            Reset_Status();
            return;
        }

        if ((iStatus & (int)DataEnum.eStatus.Stun) == (int)DataEnum.eStatus.Stun)
        {
            if (m_tStatusInfo.fStunStatusTime == 0)
            {
                Create_StatusEffect("STUN!", Color.gray);
            }
            m_tStatusInfo.fStunStatusTime += Time.deltaTime;
            if (m_tStatusInfo.fStunStatusTime > m_tStatusInfo.fStunMaxStatusTime)
            {
                m_tStatusInfo.fStunStatusTime = 0;
                m_tStatusInfo.fStunMaxStatusTime = 0;
                m_tMobInfo.iStatus &= ~((int)DataEnum.eStatus.Stun);
                m_tMobInfo.fMoveSpeed_Stun = 0;
            }
            else
            {
                //기절상태이상 처리
                m_tMobInfo.fMoveSpeed_Stun = 1;
            }
        }
        if ((iStatus & (int)DataEnum.eStatus.Slow) == (int)DataEnum.eStatus.Slow)
        {
            if (m_tStatusInfo.fSlowStatusTime == 0)
            {
                Create_StatusEffect("SLOW!", Color.blue);
            }
            m_tStatusInfo.fSlowStatusTime += Time.deltaTime;
            if (m_tStatusInfo.fSlowStatusTime > m_tStatusInfo.fSlowMaxStatusTime)
            {
                m_tStatusInfo.fSlowStatusTime = 0;
                m_tStatusInfo.fSlowMaxStatusTime = 0;
                m_tMobInfo.iStatus &= ~((int)DataEnum.eStatus.Slow);

                m_tMobInfo.fMoveSpeed_Slow = 0;
                m_tStatusInfo.fSlowSpeed = 0;
            }
            else
            {
                //슬로우상태이상 처리
                m_tMobInfo.fMoveSpeed_Slow = m_tStatusInfo.fSlowSpeed;
            }
        }
        if ((iStatus & (int)DataEnum.eStatus.Fire) == (int)DataEnum.eStatus.Fire)
        {
            if (m_tStatusInfo.fFireStatusTime == 0)
            {
                Create_StatusEffect("FIRE!", Color.yellow);
            }
            m_tStatusInfo.fFireStatusTime += Time.deltaTime;
            if (m_tStatusInfo.fFireStatusTime > m_tStatusInfo.fFireMaxStatusTime)
            {
                m_tStatusInfo.fFireStatusTime = 0;
                m_tStatusInfo.fFireMaxStatusTime = 0;

                m_tStatusInfo.fFireRatio = 0;

                m_tMobInfo.iStatus &= ~((int)DataEnum.eStatus.Fire);
            }
            else
            {
                //화염상태이상 처리
                if (m_tDamageStruct.iDamage_Buffer > 0)
                    m_tDamageStruct.iFireStatusDamage_Buffer =(int)((float)(m_tDamageStruct.iDamage_Buffer)* m_tStatusInfo.fFireRatio);
            }
        }
        if ((iStatus & (int)DataEnum.eStatus.Poison) == (int)DataEnum.eStatus.Poison)
        {
            if (m_tStatusInfo.fPoisonStatusTime == 0)
            {
                Create_StatusEffect("POISON!", Color.green);
            }
            m_tStatusInfo.fPoisonStatusTime += Time.deltaTime;
            if (m_tStatusInfo.fPoisonStatusTime > m_tStatusInfo.fPoisonMaxStatusTime)
            {
                m_tStatusInfo.fPoisonStatusTime = 0;
                m_tStatusInfo.fPoisonMaxStatusTime = 0;
                m_tMobInfo.iStatus &= ~((int)DataEnum.eStatus.Poison);
                m_tStatusInfo.iPoisonDamage = 0;
                m_tStatusInfo.fPoisonDotTime = 0;
            }
            else
            {
                //독상태이상 처리
                m_tStatusInfo.fPoisonDotTime += Time.deltaTime;
                if (m_tStatusInfo.fPoisonDotTime > m_tStatusInfo.fPoisonDotMaxTime)
                {
                    m_tDamageStruct.iDotDamage_Buffer += m_tStatusInfo.iPoisonDamage;
                    m_tStatusInfo.fPoisonDotTime = 0;
                }

            }
        }

    }

    private void Damage_Phase()
    {
        StatusDamage_Pahse();
        if(m_tDamageStruct.iDamage_Buffer > 0)
        {
            m_tDamageStruct.iTotalDamage += m_tDamageStruct.iDamage_Buffer;
            Color color_ = Color.white;
           // color_.r -= 0.1f; color_.g -= 0.1f; color_.b -= 0.1f;
            Create_DamageEffect(m_tDamageStruct.iDamage_Buffer, color_);
            m_tDamageStruct.iDamage_Buffer = 0;

        }
        if( m_tDamageStruct.iTotalDamage>0)
        {
        

            m_tMobInfo.iHp -= m_tDamageStruct.iTotalDamage;
           // Create_DamageEffect();
            m_tDamageStruct.iTotalDamage = 0;

        }

    }

    private void StatusDamage_Pahse()
    {
        int iStatus = m_tMobInfo.iStatus;
        if ((iStatus & (int)DataEnum.eStatus.Fire) == (int)DataEnum.eStatus.Fire)
        {
            if (m_tDamageStruct.iFireStatusDamage_Buffer > 0)
            {
                // m_tDamageStruct.iFireStatusDamage_Buffer
                m_tDamageStruct.iTotalDamage += m_tDamageStruct.iFireStatusDamage_Buffer;
                Create_DamageEffect(m_tDamageStruct.iFireStatusDamage_Buffer, Color.red);
                m_tDamageStruct.iFireStatusDamage_Buffer = 0;

                m_tStatusInfo.fFireStatusTime = 0;
                m_tStatusInfo.fFireMaxStatusTime = 0;

                m_tStatusInfo.fFireRatio = 0;

                m_tMobInfo.iStatus &= ~((int)DataEnum.eStatus.Fire);
            }
        }
        if ((iStatus & (int)DataEnum.eStatus.Poison) == (int)DataEnum.eStatus.Poison)
        {
            if(  m_tDamageStruct.iDotDamage_Buffer > 0)
            {
                m_tDamageStruct.iTotalDamage += m_tDamageStruct.iDotDamage_Buffer;
                Create_DamageEffect(m_tDamageStruct.iDotDamage_Buffer, Color.green);
                m_tDamageStruct.iDotDamage_Buffer = 0;
            }
        }
    }
    private void Reset_Status()
    {
        m_tStatusInfo = new DataStruct.tagStatusInfo();
        //m_tStatusInfo.fDotMaxTime = 0;
        //m_tStatusInfo.fFireStatusTime = 0;
        //m_tStatusInfo.fFireMaxStatusTime = 0;
        //m_tStatusInfo.fFireCoolTime = 0;
        //m_tStatusInfo.fPoisonStatusTime = 0;
        //m_tStatusInfo.fPoisonMaxStatusTime = 0;
        //m_tStatusInfo.fPoisonCoolTime = 0;
        //m_tStatusInfo.fSlowStatusTime = 0;
        //m_tStatusInfo.fSlowMaxStatusTime = 0;
        //m_tStatusInfo.fStunStatusTime = 0;
        //m_tStatusInfo.fStunMaxStatusTime = 0;

        //m_tStatusInfo.fDotTime = 0;
        //m_tStatusInfo.fDotMaxTime = 0;

        //m_tStatusInfo.iPoisonDamage = 0;
        //m_tStatusInfo.iFireDamage = 0;

    }

    #endregion
}
