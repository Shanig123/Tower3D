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



    public DataStruct.tagMobStatus m_tagStatus;

    [SerializeField] private PlayerController m_playerController;

    [SerializeField] private Vector3[] m_arrWaypoints;
    [SerializeField] private int m_iCurTargetWaypoint = 0;
    [SerializeField] private int m_iNextTargetWaypoint = 0;


    [SerializeField] private DataEnum.eState m_eNextMobState = DataEnum.eState.End;
    [SerializeField] private DataEnum.eState m_eCurMobState = DataEnum.eState.End;

    #endregion
    #region Property
    public DataEnum.eState GetState
    {
        get
        {
            return m_eCurMobState;
        }
    }
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
            m_tagStatus.iHp += value;
        }
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
        if (m_tagStatus.fMoveSpeed == 0)
            m_tagStatus.fMoveSpeed = 3f;
        if (m_tagStatus.iHp == 0)
        {
            StageController stageController= GameObject.FindGameObjectWithTag("TotalController").GetComponent<StageController>();
            m_tagStatus.iHp = 5 + (stageController.Get_Wave * 2);
            m_tagStatus.iMaxHp = m_tagStatus.iHp;

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
      
        if(m_tagStatus.iHp < 0)
        {
            CheckDead();
        }
        else
            CheckTarget();
    }
    private void DoDeadState()
    {
        m_tagStatus.fDeadTime += Time.deltaTime;
        if (m_tagStatus.fDeadTime > 5.0f)
        {
            StageController temp = GameObject.FindWithTag("TotalController").GetComponent<StageController>();
            temp.Min_MobCount();
            Destroy(gameObject);
            m_tagStatus.fDeadTime = 0;

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

    private void CheckTarget()
    {
        Vector3 vTargetPos = m_arrWaypoints[m_iCurTargetWaypoint];
        vTargetPos.y = this.transform.position.y;
        Vector3 vecTargetToLength = vTargetPos - this.transform.position;
        float fLength = vecTargetToLength.magnitude;

        if (fLength < 0.1)
        {
            if (m_arrWaypoints[m_iCurTargetWaypoint] == m_arrWaypoints[m_arrWaypoints.Length-1] && fLength < 1.0)
            {
                m_eNextMobState = DataEnum.eState.NoActive;
            }
            else
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

    private void DoMove()
    {
        m_Transform.position += Time.deltaTime * m_tagStatus.fMoveSpeed * m_Transform.forward;
        m_Ani.SetBool("isRunning", true);
        m_Ani.speed = m_tagStatus.fMoveSpeed * 0.5f;
    }
}
