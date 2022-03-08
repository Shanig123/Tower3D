using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_Effect : MonoBehaviour
{
    public DataStruct.tagEffectInfo m_tEffectInfo = new DataStruct.tagEffectInfo();
    public string m_strPrefabName;

    public  bool m_bIsOn = true;
    public AudioClip m_audioClip;
    public AudioSource m_audioSource;
    // Start is called before the first frame update
    protected bool m_bIsActiveInit = false;

    protected void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
  
    protected virtual void Start()
    {

    }
    protected abstract void ActiveInit();

    protected bool Do_Timer()
    {
        if (!m_tEffectInfo.bLoop)
        {
            m_tEffectInfo.fLifeTime += (Time.deltaTime* m_tEffectInfo.fTimerSpeed);
            if (m_tEffectInfo.fLifeTime > m_tEffectInfo.fMaxLifeTime)
            {
                m_bIsOn = false;
                m_tEffectInfo.fLifeTime = 0;
               
                return true;
            }
            return false;
        }
        else
            return true;
       
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