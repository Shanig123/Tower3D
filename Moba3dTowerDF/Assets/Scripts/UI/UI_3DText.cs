using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_3DText : MonoBehaviour
{
    [SerializeField] private TextMesh m_textMesh;
    [SerializeField] private Transform m_transform;
   
    private float m_fcolorAlpha = 1f;
    private float m_fLifetime;
    [SerializeField] private float m_fMaxLifetime;
    public bool m_bEffect;

    public void Set_TextInfo(string _strText, Color _textColor, float fScale =1f)
    {
        m_transform.localScale *= fScale;
        m_textMesh.text = _strText;
        m_textMesh.color = _textColor;
    }
    public void Set_TextInfo(string _strText, float fScale = 1f)
    {
        m_transform.localScale *= fScale;
        m_textMesh.text = _strText;
    }
    private void Start()
    {
        if(m_bEffect)
        {
            if(m_fMaxLifetime == 0)
            {
                m_fMaxLifetime = 1f;
            }
        }
    }
    public void Update()
    {
        if(m_bEffect)
        {
            m_fLifetime += Time.deltaTime;
            if(m_fLifetime > m_fMaxLifetime)
            {
                Destroy(this.gameObject);
            }
            else
            {
                float fAspect = 1f-( m_fLifetime / m_fMaxLifetime);

                m_transform.localScale *= fAspect;

                Color lcolor = m_textMesh.color;
                lcolor.a = fAspect;
                m_textMesh.color = lcolor;
            }
        }
    }
}
