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

    void Update()
    {
        if (!m_bFirstInit)
            FirstInit();
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
    private void OnTriggerEnter(Collider other)
    {
        if (!m_bCheckDead)
        {
            GameObject obj = other.gameObject;
            obj.GetComponent<MobAI>().Add_HP = (-m_tagStatus.iAtk);
            m_tagStatus.fLifeTime = 0;
            m_eNextState = DataEnum.eState.Dead;

        }

    }
}
