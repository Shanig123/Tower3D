using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTower : TowerAI
{
    ScrollTower()
          : base() { }

    // Start is called before the first frame update
    protected override void Start()
    {
       
        base.Start();
        m_tTowerInfo.eType = DataEnum.eTowerType.Scrl;
        //  m_tagStatus.strTowerName = gameObject.name;
        //EditorUtility
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Scroll_RotateY();
    }

    protected override void UpdateInit()
    {
        base.UpdateInit();

    }
    protected override void UpdateInit_Effect()
    {
        Base_Effect effect = GetComponentInChildren<Base_Effect>();
        if (effect != null)
        {
            effect.m_tEffectInfo.fParticleScale = 15f;
            effect.m_tEffectInfo.vScale = new Vector3(0.5f, 0.5f, 0.5f);
            Vector3 vPos = effect.transform.position;
            vPos.y += 0.5f;
            effect.transform.position = vPos;
            Color color = new Color(129 / 255f, 20 / 255f,1);
            effect.m_tEffectInfo.colorEffect = color;
            effect.m_tEffectInfo.fSpeed = 1.5f;
           
            effect.Copy_ClassInfoToParticleSys();
        }
    }

    protected override void CheckDead()
    {
       
    }

    protected override void DoActiveState()
    {
       
    }

    protected override void DoDeadState()
    {
       
    }

    protected override void DoNoActiveState()
    {
        
    }
    private void Scroll_RotateY()
    {
        var rot = gameObject.transform.Find("scroll").gameObject.transform.rotation;
        var Angle = rot.eulerAngles;
        Angle.y += Time.deltaTime * 45f;
        rot.eulerAngles = Angle;
        gameObject.transform.Find("scroll").gameObject.transform.rotation = rot ;

    }

}
