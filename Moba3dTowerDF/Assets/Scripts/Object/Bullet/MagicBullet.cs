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

    public GameObject[] m_objTrailEffect;
    public float fAngle;
    protected override void Start()
    {
        base.Start();
        GetComponentInChildren<Renderer>().material.SetColor("_RimCol", new Color(0, 0, 1));
       
    }
    private void OnEnable()
    {
        fAngle = Mathf.PI;
        for (int i = 0; i < 2; ++i)
        {
            //x는 cosT
            //y는 sinT
            Vector3 vInitPos = new Vector3(Mathf.Cos(fAngle * (i + 1)), Mathf.Sin(fAngle * (i + 1)), 0);
            vInitPos *= 0.3f;
            m_objTrailEffect[i].transform.localPosition = vInitPos;
        }
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

        RotateTrail();

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

    private void RotateTrail()
    {
        for(int i=0; i<2;++i)
        {
            fAngle += m_tagStatus.fLifeTime*Mathf.Deg2Rad*15;
            Vector3 vRotPos = new Vector3(Mathf.Cos(fAngle * (i+1)), Mathf.Sin(fAngle * (i + 1)), 0);
            vRotPos *= 0.3f;
            m_objTrailEffect[i].transform.localPosition = vRotPos;
        }
       // m_objTrailEffect[0]
    }
}

