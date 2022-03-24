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

   // public AudioClip audioClip;
  //  public AudioSource m_audioSource;

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
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void DoReadyState()
    {
        base.DoReadyState();
        LookAt_Target();
    }

    protected override void DoActiveState()
    {
        m_tBulletInfo.fLifeTime += Time.deltaTime;

        RotateTrail();

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
        //Instantiate()
        GameObject.FindGameObjectWithTag("TotalController").GetComponent<EffectPoolController>().
            Get_ObjPool(transform.position, "MagicDead");

        //AudioSource audioSource = 


        m_tBulletInfo.fMaxLifeTime = 0;
        m_tBulletInfo.fLifeTime = 0;
        m_objTargetMob = null;
        m_eNextState = DataEnum.eState.NoActive;

    }
    protected override void PlaySound_create()
    {
        //m_audioSource = GetComponent<AudioSource>();
        Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.Sfx, 6,this.transform.position);
    }
    private void RotateTrail()
    {
        for(int i=0; i<2;++i)
        {
            fAngle += m_tBulletInfo.fLifeTime*Mathf.Deg2Rad*15;
            Vector3 vRotPos = new Vector3(Mathf.Cos(fAngle * (i+1)), Mathf.Sin(fAngle * (i + 1)), 0);
            vRotPos *= 0.3f;
            m_objTrailEffect[i].transform.localPosition = vRotPos;
        }
       // m_objTrailEffect[0]
    }
}

