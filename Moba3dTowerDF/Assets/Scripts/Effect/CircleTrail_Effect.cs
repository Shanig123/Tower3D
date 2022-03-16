using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTrail_Effect : Base_Effect
{
    // Start is called before the first frame update
    [SerializeField]
    private Shader m_baseShader;
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

        m_baseShader = Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply");
    }

    // Update is called once per frame
    void Update()
    {
        if(m_baseShader != GetComponent<ParticleSystemRenderer>().material.shader)
        {
            GetComponent<ParticleSystemRenderer>().material.shader = m_baseShader;
        }
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if(!ps.isPlaying)
        {
        
            GameObject.FindGameObjectWithTag("TotalController").GetComponent<EffectPoolController>().ReturnPool(this.gameObject, m_strPrefabName);
            m_bIsOn = false;
            ps.Clear();
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        mainModule.startColor = m_tEffectInfo.colorEffect;
        mainModule.simulationSpeed = m_tEffectInfo.fSpeed;

        var scale = GetComponent<Transform>().localScale;
        scale.x *= m_tEffectInfo.vScale.x; scale.y *= m_tEffectInfo.vScale.y; scale.z *= m_tEffectInfo.vScale.z;
        GetComponent<Transform>().localScale = scale;

        m_baseShader = Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply");

    }
    protected override void ActiveInit()
    {
        // throw new System.NotImplementedException();
    }

}
