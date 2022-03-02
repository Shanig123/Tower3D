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
    protected override void Start()
    {
        base.Start();
        Shader RimShader = GameObject.FindWithTag("TotalController").GetComponent<ShaderController>().Get_Shader("Rimlight_Shader");
        //Shader defaultShader = Shader.Find("Custom/Default_Shader");
        if (RimShader == null)
        {
            print("defaultShader null");
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
        //CheckInStageBoard();
        //InStageBoard();
        //ReadyToActive();
    }
    protected abstract void DoNoActiveState();
    protected abstract void DoActiveState();
    protected abstract void DoDeadState();
    //protected abstract void CheckDead();

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

    protected void DoMove()
    {
        m_Transform.position += Time.deltaTime * m_tagStatus.fMoveSpeed * m_Transform.forward;
        // m_Ani.SetBool("isRunning", true);
    }
}
