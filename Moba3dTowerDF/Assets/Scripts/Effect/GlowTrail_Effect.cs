using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowTrail_Effect : MonoBehaviour
{
    [SerializeField] private TrailRenderer m_TrailRender;
    //[SerializeField] private Material m_TrailRenderMaterial;
    [SerializeField] private ParticleSystem m_TrailParticle;
    [SerializeField] private ParticleSystem m_GlowParticle;
    [SerializeField] private ParticleSystem m_DustParticle;
 

    public Color m_EffectColor;
    public Color m_DefaultColor;

    public Color[] m_DetailColor = new Color[3];

    // Start is called before the first frame update
    void Start()
    {
        InitParticleColorInfo();
        Copy_Info_inParticlesys();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitParticleColorInfo()
    {
        if (m_EffectColor.r == 0 &&
            m_EffectColor.g == 0 &&
            m_EffectColor.b == 0)
        {
            m_EffectColor = m_DefaultColor;
        }
        else
        {
            if (m_DetailColor[0] == null &&
                m_DetailColor[1] == null &&
                m_DetailColor[2] == null)
            {
                m_DetailColor[0] = m_DefaultColor;
                m_DetailColor[1] = m_DefaultColor;
                m_DetailColor[2] = m_DefaultColor;
            }
        }
    }

    private void Copy_Info_inParticlesys()
    {
        //effect�� ���ٸ� true ������ false 
        //false��� effectColor�� ��� true��� ������ �÷����� ����Ѵ�.
        bool bCheckDetailColor = false;
        if (m_EffectColor.r == 0 &&
            m_EffectColor.g == 0 &&
            m_EffectColor.b == 0)
            bCheckDetailColor = true;

        if (m_TrailRender != null)
        {
            if(bCheckDetailColor)
            {
                //m_TrailRender.colorGradient *= m_DetailColor[0];
                m_TrailRender.material.color = m_DetailColor[0];
            }
            else
            {
                m_TrailRender.material.color = m_EffectColor;
            }
        }
        if(m_TrailParticle != null)
        {
            if (bCheckDetailColor)
            {
                //m_TrailRender.colorGradient *= m_DetailColor[0];
                var ParticleMain = m_TrailParticle.main;
                ParticleMain.startColor = m_DetailColor[0];
                //m_TrailParticle.main = ParticleMain;
                //m_TrailParticle.main = ParticleMain;
            }
            else
            {
                var ParticleMain = m_TrailParticle.main;
                ParticleMain.startColor = m_EffectColor;
            }
        }
        if (m_GlowParticle != null)
        {
            if (bCheckDetailColor)
            {
                //m_TrailRender.colorGradient *= m_DetailColor[0];
                var ParticleMain = m_GlowParticle.main;
                ParticleMain.startColor = m_DetailColor[1];
            }
            else
            {
                var ParticleMain = m_GlowParticle.main;
                ParticleMain.startColor = m_EffectColor;
            }
        }
        if (m_DustParticle != null)
        {
            if (bCheckDetailColor)
            {
                var ParticleMain = m_DustParticle.main;
                ParticleMain.startColor = m_DetailColor[2];
            }
            else
            {
                var ParticleMain = m_DustParticle.main;
                ParticleMain.startColor = m_EffectColor;
            }
        }
    }
}
