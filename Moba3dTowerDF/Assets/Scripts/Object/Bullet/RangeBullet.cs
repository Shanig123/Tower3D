using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBullet : BaseBullet
{

    #region Value
    //[SerializeField]
    private float m_fDeadTime;
    //[SerializeField]
    private float m_fAtkCoolTime;
    [SerializeField]
    private float m_fAtkMaxCoolTime;

    [SerializeField]
    private Collider m_collider;

    #endregion
    protected override void Start()
    {
        return;
    }
    private void OnEnable()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);

    }

    protected override void DoActiveState()
    {
        m_tagStatus.fLifeTime += Time.deltaTime;

        if (m_tagStatus.fLifeTime > m_tagStatus.fMaxLifeTime)
        {
            m_tagStatus.fLifeTime = 0;
            m_eNextState = DataEnum.eState.Dead;
        }
        else
        {
            if(m_collider.enabled == false)
            {
                m_fAtkCoolTime += Time.deltaTime;
                if (m_fAtkCoolTime > m_fAtkMaxCoolTime)
                {
                    m_fAtkCoolTime = 0;
                    m_collider.enabled = true;
                }
            }
            else
            {
                m_fAtkCoolTime += Time.deltaTime;
                if (m_fAtkCoolTime > 0.1)
                {
                    m_fAtkCoolTime = 0;
                    m_collider.enabled = false;
                }
            }
        }


    }

    protected override void DoDeadState()
    {
        m_fDeadTime += Time.deltaTime;
        m_collider.enabled = false;
        if (m_fDeadTime > 4)
        {
            m_fDeadTime = 0;
            m_fAtkCoolTime = 0;
            m_tagStatus.fLifeTime = 0;

            m_tagStatus.fMaxLifeTime = 0;
            m_objTargetMob = null;
            m_eNextState = DataEnum.eState.NoActive;
        }
        else
        {
            float fAspect = 1f - (m_fDeadTime/4f);
            transform.localScale = new Vector3(fAspect, fAspect, fAspect);
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (!m_bCheckDead)
        {
            GameObject obj = other.gameObject;
            obj.GetComponent<MobAI>().Add_HP = (-m_tagStatus.iAtk);

            Create_DamageEffect();
        }
    }
}
