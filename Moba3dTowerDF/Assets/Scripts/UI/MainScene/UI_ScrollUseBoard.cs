using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScrollUseBoard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool m_bActive = false;
    [SerializeField] private GameObject m_objUseScrollButton;
    [SerializeField] private GameObject m_objClose_Button;
    [SerializeField] private float m_fTime;
    private int m_iPickAbilityNumber = -1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bActive)
            Open_ScrollEffect();
    }
    private void ResetInfo()
    {
        m_fTime = 0;
    }

    private void OnEnable()
    {
        m_objClose_Button.SetActive(false);
        Game_Manager.Instance.Game_Pause(true);
        Dissiable_UseScrollButton();
        m_bActive = true;
    }

    private void OnDisable()
    {
        ResetInfo();
        Game_Manager.Instance.Game_Pause(false);
        m_bActive = false;
        if (m_iPickAbilityNumber < 0)
            return;

        GameObject.FindGameObjectWithTag("TotalController").GetComponent<PlayerController>().Use_Scroll(m_iPickAbilityNumber);

        m_iPickAbilityNumber = -1;
    }

    public void OnClick_UseScroll()
    {
        if (m_bActive)
            this.gameObject.SetActive(false);
        else
            this.gameObject.SetActive(true);
    }

    private void Open_ScrollEffect()
    {
        m_fTime += Time.unscaledDeltaTime;
        if(m_fTime>2f)
        {
            m_objClose_Button.SetActive(true);
            m_iPickAbilityNumber = 1;
        }


    }

    private void Dissiable_UseScrollButton()
    {
        m_objUseScrollButton.SetActive(false);
    }
}
