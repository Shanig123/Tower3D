using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short_Effect : Base_Effect
{
    protected override void Start()
    {
        base.Start();
        //ParticleSystem ps = GetComponent<ParticleSystem>();
        //var mainModule = ps.main;
        //mainModule.startColor = m_tEffectInfo.colorEffect;
        //mainModule.simulationSpeed = m_tEffectInfo.fSpeed;

        //var scale = GetComponent<Transform>().localScale;
        //scale.x *= m_tEffectInfo.vScale.x; scale.y *= m_tEffectInfo.vScale.y; scale.z *= m_tEffectInfo.vScale.z;
        //GetComponent<Transform>().localScale = scale;

       // m_baseShader = Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply");
    }
    private void OnEnable() 
    {
        var _main =   m_ps.main;
        _main.startColor = m_tEffectInfo.colorEffect;
    }
    private void OnDisable() 
    {
        var _main =   m_ps.main;
        _main.startColor = Color.white;
    }
    // Update is called once per frame
    protected override void LateUpdate()
    {
        //ParticleSystem ps = GetComponent<ParticleSystem>();

        if (!m_ps.IsAlive())
        {
            m_bIsOn = false;
            m_ps.Clear();
            gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("TotalController").GetComponent<EffectPoolController>().ReturnPool(gameObject, m_strPrefabName);
            
            //Destroy(gameObject);
        }
        // if()
    }

    protected override void ActiveInit()
    {
        // throw new System.NotImplementedException();
    }
}
