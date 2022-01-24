using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTrail_Effect : Base_Effect
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ParticleSystem ps =   GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        mainModule.startColor = m_tEffectInfo.colorEffect;
        mainModule.simulationSpeed = m_tEffectInfo.fSpeed;

        var scale = GetComponent<Transform>().localScale;
        scale.x *= m_tEffectInfo.vScale.x; scale.y *= m_tEffectInfo.vScale.y; scale.z *= m_tEffectInfo.vScale.z;
        GetComponent<Transform>().localScale = scale;

    }

    // Update is called once per frame
    void Update()
    {
        //ParticleSystem ps = GetComponent<ParticleSystem>();
        //var mainModule = ps.main;
        //mainModule.startColor = m_tEffectInfo.colorEffect;
        //mainModule.simulationSpeed = m_tEffectInfo.fSpeed;
    }

  
}
