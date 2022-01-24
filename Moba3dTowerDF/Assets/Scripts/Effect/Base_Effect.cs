using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Effect : MonoBehaviour
{
    public DataStruct.tagEffectInfo m_tEffectInfo = new DataStruct.tagEffectInfo();

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }
    public void Copy_ClassInfoToParticleSys()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        mainModule.startColor = m_tEffectInfo.colorEffect;
        mainModule.simulationSpeed = m_tEffectInfo.fSpeed;
        //mainModule.startSize.constantMin *= m_tEffectInfo.fParticleScale;

        var vsize = mainModule.startSize;
        vsize.constantMax = m_tEffectInfo.fParticleScale;
        mainModule.startSize = vsize;

        var scale = GetComponent<Transform>().localScale;
        scale.x *= m_tEffectInfo.vScale.x; scale.y *= m_tEffectInfo.vScale.y; scale.z *= m_tEffectInfo.vScale.z;
        GetComponent<Transform>().localScale = scale;
    }
}