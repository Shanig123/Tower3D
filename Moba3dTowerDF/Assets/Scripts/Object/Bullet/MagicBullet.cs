using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : BaseBullet
{
    MagicBullet()
        : base(){}
    // Start is called before the first frame update 
    public Color m_colorEffect;
    public Color m_colorGlow;
     protected override void Start()
    {
        base.Start();
        GetComponentInChildren<Renderer>().material.SetColor("_RimCol", new Color(0, 0, 1));

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void DoNoActiveState()
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
    protected override void DoReadyState()
    {
        Vector3 TargetPos = m_objTargetMob.transform.position;
        TargetPos.y = transform.position.y;
        transform.LookAt(TargetPos);

        m_eNextState = DataEnum.eState.Active;
    }
    protected override void DoActiveState()
    {
        m_tagStatus.fLifeTime += Time.deltaTime;


        if ((m_vCreatePos - transform.position).magnitude > m_tagStatus.fMaxLifeTime)
        {
            m_tagStatus.fLifeTime = 0;
            m_eNextState = DataEnum.eState.Dead;
        }
        else
            DoMove();
    }

    protected override void DoDeadState()
    {
        //d이펙트 생성

        GameObject.FindGameObjectWithTag("TotalController").GetComponent<EffectPoolController>().
            Get_ObjPool(transform.position, "CornBust");

        m_tagStatus.fMaxLifeTime = 0;
        m_tagStatus.fLifeTime = 0;
        m_objTargetMob = null;
        m_eNextState = DataEnum.eState.NoActive;

    }
}

