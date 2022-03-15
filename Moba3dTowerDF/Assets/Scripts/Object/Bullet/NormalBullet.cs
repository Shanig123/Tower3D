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
 
    protected override void DoReadyState()
    {
        base.DoReadyState();
        LookAt_Target();
    }

    protected override void DoActiveState()
    {
        m_tBulletInfo.fLifeTime += Time.deltaTime;


        if ((m_vCreatePos - transform.position).magnitude > m_tBulletInfo.fMaxLifeTime)
        {
            m_tBulletInfo.fLifeTime = 0;
            m_eNextState = DataEnum.eState.Dead;
        }
        else
            DoMove();
    }
   
    protected override void DoDeadState()
    {
        //d이펙트 생성

        //GameObject.FindGameObjectWithTag("TotalController").GetComponent<EffectPoolController>().
        //    Get_ObjPool(transform.position, "CornBust");

        m_tBulletInfo.fMaxLifeTime = 0;
        m_tBulletInfo.fLifeTime = 0;
        m_objTargetMob = null;
        m_eNextState = DataEnum.eState.NoActive;

    }
}
