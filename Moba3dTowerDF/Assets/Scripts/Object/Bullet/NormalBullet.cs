using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BaseBullet
{
    NormalBullet()
           : base()
    {

    }
 

    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (!m_bFirstInit)
            FirstInit();
        if (m_objTargetMob == null)
            m_iTargetID = 0;
        CheckState();
        DoController();

    }
    private void FirstInit()
    {

        m_bFirstInit = true;
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
                        //ObjPool_Manager.Instance.ReturnPool(this.gameObject, this.m_tagStatus.strObjTagName);
                    }
                    break;
                case DataEnum.eState.Ready:
                    {
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
    private void DoNoActiveState()
    {
        if(!m_bObjActiveOnOff)
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
    private void DoReadyState()
    {
        Vector3 TargetPos =  m_objTargetMob.transform.position;
        TargetPos.y = transform.position.y;
        transform.LookAt(TargetPos);

        m_eNextState = DataEnum.eState.Active;
    }
    private void DoActiveState()
    {
        m_tagStatus.fLifeTime += Time.deltaTime;
       

        if (m_tagStatus.fLifeTime > m_tagStatus.fMaxLifeTime)
        {
            print("LifeEnd");
            m_tagStatus.fLifeTime = 0;
            m_eNextState = DataEnum.eState.Dead;
        }
        else
            DoMove();
    }
    private void DoMove()
    {
        m_Transform.position += Time.deltaTime * m_tagStatus.fMoveSpeed * m_Transform.forward;
       // m_Ani.SetBool("isRunning", true);
    }

    //public LayerMask layerMask;
    //private void OnTriggerEnter(Collider other)
    //{
    //    if ((layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
    //    {
    //        Debug.Log("Hit with Layermask");
    //    }
    //    else
    //    {
    //        Debug.Log("Not in Layermask" + other.gameObject.name);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (!m_bCheckDead)
        {
            print("Collision");
            GameObject obj = other.gameObject;
            obj.GetComponent<MobAI>().Add_HP = (-m_tagStatus.iAtk);
            m_tagStatus.fLifeTime = 0;
            m_eNextState = DataEnum.eState.Dead;

        }

    }
    private void DoDeadState()
    {
        //d¿Ã∆Â∆Æ ª˝º∫

        m_tagStatus.fMaxLifeTime = 0;
        m_tagStatus.fLifeTime = 0;
        m_objTargetMob = null;
        m_eNextState = DataEnum.eState.NoActive;
       
    }
}
