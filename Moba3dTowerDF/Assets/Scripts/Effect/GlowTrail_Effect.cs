using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowTrail_Effect : MonoBehaviour
{
    [SerializeField] private TrailRenderer m_TrailRender;
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
        if (m_EffectColor == null)
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
        //detailcolor가 없다면 false 있으면 true 
        //false라면 effectColor값 사용 true라면 디테일 컬러값을 사용한다.
        bool bCheckDetailColor = true;
        if (m_DetailColor[0] == null &&
                m_DetailColor[1] == null &&
                m_DetailColor[2] == null)
            bCheckDetailColor = false;

        if (m_TrailRender != null)
        {

        }
        if(m_TrailParticle != null)
        {

        }
        if (m_GlowParticle != null)
        {

        }
        if (m_DustParticle != null)
        {

        }
    }
}
