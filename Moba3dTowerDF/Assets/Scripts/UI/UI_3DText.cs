using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_3DText : MonoBehaviour
{
    [SerializeField] private TextMesh m_textMesh;
    [SerializeField] private Transform m_transform;
   
    private float m_fcolorAlpha = 1f;
    private float m_fLifetime;
    [SerializeField]
    private int m_iTextEffect = 1;

    private Vector3 m_vCreatePos;
    private Vector2 m_vRandAxis;
    //private float m_fRandXAxis;
    //private float m_fRandZAxis;

    [SerializeField] private float m_fMaxLifetime;
    public bool m_bEffect;

    

    public void Set_TextInfo(string _strText, Color _textColor, DataEnum.eTextEffect _ePatern = (DataEnum.eTextEffect)1, float fScale =1f)
    {
        m_iTextEffect = (int)_ePatern;
        m_transform.localScale *= fScale;
        m_textMesh.text = _strText;
        m_textMesh.color = _textColor;
    }
    public void Set_TextInfo(string _strText, DataEnum.eTextEffect _ePatern = (DataEnum.eTextEffect)1, float fScale = 1f)
    {
        m_iTextEffect = (int)_ePatern;
        m_transform.localScale *= fScale;
        m_textMesh.text = _strText;
    }
    private void Start()
    {
        m_vCreatePos = transform.position;
        m_vRandAxis.x = GameObject.FindGameObjectWithTag("TotalController").GetComponent<DataController>().ExtractRandomNumberFromSeed_NoCount();
        m_vRandAxis.y = GameObject.FindGameObjectWithTag("TotalController").GetComponent<DataController>().ExtractRandomNumberFromSeed_NoCount();
        m_vRandAxis.x = m_vRandAxis.x * 2f - 1f;

        m_vRandAxis.y = m_vRandAxis.y * 2f - 1f;
        m_vRandAxis.Normalize();
        if (m_bEffect)
        {
            if(m_fMaxLifetime == 0)
            {
                m_fMaxLifetime = 1f;
            }
        }
    }
    private void Update()
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
                Do_EffectOption();
      

             
            }
        }
    }

    private void Do_EffectOption()
    {
        if (m_iTextEffect == 0)
            return;
        
        if ((m_iTextEffect & ((int)DataEnum.eTextEffect.Default_AlphaDown)) == ((int)DataEnum.eTextEffect.Default_AlphaDown))
        {
            float fAspect = 1f - (m_fLifetime / m_fMaxLifetime);
            Color lcolor = m_textMesh.color;
            lcolor.a = fAspect;
            m_textMesh.color = lcolor;
        }
        if ((m_iTextEffect & ((int)DataEnum.eTextEffect.Volcano)) == ((int)DataEnum.eTextEffect.Volcano))
        {
            var vPos = transform.position;
            vPos.x += m_vRandAxis.x * Time.deltaTime;
            vPos.z += m_vRandAxis.y * Time.deltaTime;
            vPos.y = GFunc.Function.Jump(m_vCreatePos.y,m_fLifetime,5f);
            transform.position = vPos;
        }
        if ((m_iTextEffect & ((int)DataEnum.eTextEffect.Up)) == ((int)DataEnum.eTextEffect.Up))
        {
            var vPos = transform.position;
            vPos.y += Time.fixedDeltaTime;
            transform.position = vPos;
        }
        if ((m_iTextEffect & ((int)DataEnum.eTextEffect.SizeDown)) == ((int)DataEnum.eTextEffect.SizeDown))
        {
            m_transform.localScale *= 1f - ((m_fLifetime*0.1f) / m_fMaxLifetime);
        }
    }
}
