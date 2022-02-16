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
        GameObject.FindGameObjectWithTag("TotalController").GetComponent<AbilityController>().Add_Ability = 1;
  
        Game_Manager.Instance.Game_Pause(true);
        Dissiable_UseScrollButton();
        m_bActive = true;
    }

    private void OnDisable()
    {
        ResetInfo();
        Game_Manager.Instance.Game_Pause(false);
        m_bActive = false;
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

        }


    }

    private void Dissiable_UseScrollButton()
    {
        m_objUseScrollButton.SetActive(false);
    }
}
