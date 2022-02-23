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
    protected override void Start()
    {
        base.Start();
        GetComponentInChildren<Renderer>().material.SetColor("_RimCol", new Color(0, 0, 1));

    }

    protected override void Update()
    {
        
        if (!m_bFirstInit)
            FirstInit();
        base.Update();
        if (m_objTargetMob == null)
            m_iTargetID = 0;
     

    }
    private void FirstInit()
    {

        m_bFirstInit = true;
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
    protected void OnTriggerEnter(Collider other)
    {
        if (!m_bCheckDead)
        {
            GameObject obj = other.gameObject;
            obj.GetComponent<MobAI>().Add_HP = (-m_tagStatus.iAtk);
            m_tagStatus.fLifeTime = 0;
            m_eNextState = DataEnum.eState.Dead;
        }
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
        //d����Ʈ ����

        GameObject.FindGameObjectWithTag("TotalController").GetComponent<EffectPoolController>().
            Get_ObjPool(transform.position, "CornBust");

        m_tagStatus.fMaxLifeTime = 0;
        m_tagStatus.fLifeTime = 0;
        m_objTargetMob = null;
        m_eNextState = DataEnum.eState.NoActive;

    }
}
