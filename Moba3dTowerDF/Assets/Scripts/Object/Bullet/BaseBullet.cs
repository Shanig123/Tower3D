using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : BaseObj
{
    #region Values

    public DataStruct.tagBulletStatus m_tBulletInfo;
    public DataStruct.tagStatusInfo m_tStatusInfo;
    [SerializeField] protected GameObject m_objTargetMob;
    [SerializeField] protected int m_iTargetID;

    [SerializeField] protected DataEnum.eState m_eNextState = DataEnum.eState.End;
    [SerializeField] protected DataEnum.eState m_eCurState = DataEnum.eState.End;

    [SerializeField] protected Vector3 m_vCreatePos;

    [SerializeField] protected string m_strDeadEffectName;
    #endregion

    #region Property
    public DataEnum.eState GetState
    {
        get
        {
            return m_eCurState;
        }
    }

    public string GetTagName
    {
        get
        {
            return m_tBulletInfo.strObjTagName;
        }

    }
    public DataStruct.tagBulletStatus Set_Data
    {
        set
        {
            DataStruct.tagBulletStatus temp = new DataStruct.tagBulletStatus();
            temp.fLifeTime = value.fLifeTime;
            temp.fMaxLifeTime = value.fMaxLifeTime;
            temp.fMoveSpeed = value.fMoveSpeed;
       
            temp.objTarget = value.objTarget;
            temp.strObjTagName = value.strObjTagName;

            temp.iHp = value.iHp;
            temp.iMaxHp = value.iMaxHp;
            temp.iAtk = value.iAtk;
            temp.iLvl = value.iLvl;
            temp.iStatus = value.iStatus;

            m_tBulletInfo = temp;
        }
    }

    public DataEnum.eState SetState
    {
        set
        {
            m_eNextState = value;
        }
    }

    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    protected override void Start()
    {
        base.Start();
        Shader RimShader = GameObject.FindWithTag("TotalController").GetComponent<ShaderController>().Get_Shader("Rimlight_Shader");
        //Shader defaultShader = Shader.Find("Custom/Default_Shader");
        if (RimShader == null)
        {
            GFunc.Function.Print_Log("defaultShader null");
            return;
        }
        GetComponentInChildren<Renderer>().material.shader = RimShader;
        GetComponentInChildren<Renderer>().material.SetFloat("_Pow", 1.0f);
        GetComponentInChildren<Renderer>().material.SetColor("_RimCol", new Color(0, 1, 0));
        GetComponentInChildren<Renderer>().material.SetFloat("_Holo", 0);

    }

    // Update is called once per frame
   protected virtual void LateUpdate()
    {
        CheckState();
       
    }
    protected virtual void FixedUpdate()
    {
        DoController();
    }

    #region ControllerFunc

    protected virtual void DoReadyState()
    {
        m_eNextState = DataEnum.eState.Active;
        //CheckInStageBoard();
        //InStageBoard();
        //ReadyToActive();
    }
    protected virtual void DoNoActiveState()
    {
        Return_Pool();
    }
    protected abstract void DoActiveState();
    protected abstract void DoDeadState();
    //protected abstract void CheckDead();

    protected virtual void PlaySound_create()
    {

    }

    private void CheckState()
    {
        if (m_eCurState != m_eNextState)
        {
            switch (m_eNextState)
            {
                case DataEnum.eState.NoActive:
                    {
                        m_vCreatePos = Vector3.zero;
                        m_bObjActiveOnOff = false;
                        //ObjPool_Manager.Instance.ReturnPool(this.gameObject, this.m_tagStatus.strObjTagName);
                    }
                    break;
                case DataEnum.eState.Ready:
                    {
                        m_vCreatePos = transform.position;
                        m_objTargetMob = m_tBulletInfo.objTarget;
                        m_bObjActiveOnOff = true;
                        m_bCheckDead = false;
                    }
                    break;
                case DataEnum.eState.Active:
                    {
                        PlaySound_create();
                    }
                    break;
                case DataEnum.eState.Dead:
                    {
                        m_vCreatePos = Vector3.zero;
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
                        m_eNextState = DataEnum.eState.NoActive;

                    }
                }
                break;
        }
    }

    #endregion
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!m_bCheckDead)
        {
            GameObject obj = other.gameObject;
            if (m_tBulletInfo.iAtk > 0)
                obj.GetComponent<MobAI>().Add_Damage = (m_tBulletInfo.iAtk);

            Passing_StatusInfo(obj);

            m_tBulletInfo.fLifeTime = 0;
            m_eNextState = DataEnum.eState.Dead;
            //Create_DamageEffect();
        }
    }

    protected void Passing_StatusInfo(GameObject _objMonster)
    {
        int iMobStatus = _objMonster.GetComponent<MobAI>().m_tMobInfo.iStatus;
        if ((m_tBulletInfo.iStatus & (int)DataEnum.eStatus.Fire) == (int)DataEnum.eStatus.Fire)
        {
            if (!((iMobStatus & (int)DataEnum.eStatus.Fire) == (int)DataEnum.eStatus.Fire))
            {
                _objMonster.GetComponent<MobAI>().Set_FireStatus(m_tStatusInfo);
            }
        }
        if ((m_tBulletInfo.iStatus & (int)DataEnum.eStatus.Poison) == (int)DataEnum.eStatus.Poison)
        {
            if (!((iMobStatus & (int)DataEnum.eStatus.Poison) == (int)DataEnum.eStatus.Poison))
            {
                _objMonster.GetComponent<MobAI>().Set_PoisonStatus(m_tStatusInfo);
            }
        }
        if ((m_tBulletInfo.iStatus & (int)DataEnum.eStatus.Slow) == (int)DataEnum.eStatus.Slow)
        {
            if (!((iMobStatus & (int)DataEnum.eStatus.Slow) == (int)DataEnum.eStatus.Slow))
            {
                _objMonster.GetComponent<MobAI>().Set_SlowStatus(m_tStatusInfo);
            }
        }
        if ((m_tBulletInfo.iStatus & (int)DataEnum.eStatus.Stun) == (int)DataEnum.eStatus.Stun)
        {
            if (!((iMobStatus & (int)DataEnum.eStatus.Stun) == (int)DataEnum.eStatus.Stun))
            {
                _objMonster.GetComponent<MobAI>().Set_StunStatus(m_tStatusInfo);
            }
        }
    }

    protected void DoMove()
    {
        m_Transform.position += Time.deltaTime * m_tBulletInfo.fMoveSpeed * m_Transform.forward;
        // m_Ani.SetBool("isRunning", true);
    }

    //protected void Create_DamageEffect()
    //{
    //    GameObject obj3DText = Resource_Manager.Instance.InstanceObj("3DText", "3DTextObject", transform.position);
    //    obj3DText.GetComponent<UI_3DText>().m_bEffect = true;
    //    obj3DText.GetComponent<UI_3DText>().Set_TextInfo(m_tagStatus.iAtk.ToString(), new Color(1, 0, 0), (DataEnum.eTextEffect)11);
    //}

    protected void Return_Pool()
    {
        if (!m_bObjActiveOnOff)
        {
            //ref GameObject temp = this.gameObject;
            GameObject temp = this.gameObject;
            m_tBulletInfo.objTarget = null;
            ObjPool_Manager.Instance.ReturnPool(ref temp, this.m_tBulletInfo.strObjTagName); //? 
            this.gameObject.SetActive(false);
        }
        else
        {

            m_eNextState = DataEnum.eState.Ready;
        }
    }
    protected void LookAt_Target()
    {
        Vector3 TargetPos = m_objTargetMob.transform.position;
        TargetPos.y = transform.position.y;
        transform.LookAt(TargetPos);
    }

}
