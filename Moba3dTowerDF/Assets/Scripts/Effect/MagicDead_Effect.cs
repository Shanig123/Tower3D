using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDead_Effect : Base_Effect
{
    // Start is called before the first frame update
    protected override void Start()
    {
        m_tEffectInfo.fMaxLifeTime = 3f;
        m_tEffectInfo.bLoop = false;
        transform.localScale = new Vector3(1, 1, 1);
        m_tEffectInfo.fTimerSpeed = 2f;
        gameObject.SetActive(false);
       
    }
    private void OnEnable()
    {
        m_tEffectInfo.fMaxLifeTime = 3f;
        m_tEffectInfo.bLoop = false;
        transform.localScale = new Vector3(1, 1, 1);
        m_bIsActiveInit = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(m_bIsOn)
        {
            ActiveInit();
            if (Do_Timer())
            {
                m_bIsOn = false;
                m_tEffectInfo.fLifeTime = 0;
            }
            else
            {
                float fSize = (Mathf.Sin(m_tEffectInfo.fLifeTime + 1));

                if (fSize < 0)
                {
                    m_tEffectInfo.bLoop = true;
                }
                transform.localScale = new Vector3(fSize, fSize, fSize);
            }
        }
        else
        {
            gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("TotalController").GetComponent<EffectPoolController>().ReturnPool(gameObject, "MagicDead");

        }

    }
    protected override void ActiveInit()
    {
        // throw new System.NotImplementedException();
        if(m_bIsActiveInit)
        {
            m_bIsActiveInit = false;

            Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.Sfx,7, this.transform.position);     
        }
    }
}
