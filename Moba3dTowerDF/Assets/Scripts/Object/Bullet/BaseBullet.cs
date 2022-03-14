using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : BaseObj
{
    #region Values

    public DataStruct.tagBulletStatus m_tagStatus;

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
            return m_tagStatus.strObjTagName;
        }

    }
    public DataStruct.tagBulletStatus Set_Data
    {
        set
        {
            m_tagStatus = value;
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
   protected virtual void Update()
    {
        CheckState();
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
                        m_objTargetMob = m_tagStatus.objTarget;
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
            obj.GetComponent<MobAI>().Add_HP = (-m_tagStatus.iAtk);
            m_tagStatus.fLifeTime = 0;
            m_eNextState = DataEnum.eState.Dead;
            Create_DamageEffect();
        }
    }
    protected void DoMove()
    {
        m_Transform.position += Time.deltaTime * m_tagStatus.fMoveSpeed * m_Transform.forward;
        // m_Ani.SetBool("isRunning", true);
    }

    protected void Create_DamageEffect()
    {
        GameObject obj3DText = Resource_Manager.Instance.InstanceObj("3DText", "3DTextObject", transform.position);
        obj3DText.GetComponent<UI_3DText>().m_bEffect = true;
        obj3DText.GetComponent<UI_3DText>().Set_TextInfo(m_tagStatus.iAtk.ToString(), new Color(1, 0, 0), (DataEnum.eTextEffect)11);
    }

    protected void Return_Pool()
    {
        if (!m_bObjActiveOnOff)
        {
            //ref GameObject temp = this.gameObject;
            GameObject temp = this.gameObject;
            m_tagStatus.objTarget = null;
            ObjPool_Manager.Instance.ReturnPool(ref temp, this.m_tagStatus.strObjTagName); //? 
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
