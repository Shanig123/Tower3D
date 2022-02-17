using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AbilityMainMenu : MonoBehaviour
{
    public int m_iKey = 0;

    private string m_strScripts;
    [SerializeField] private UnityEngine.UI.Image m_image_Child_1;
    [SerializeField] private UnityEngine.UI.Image m_image_Child_2;

    private void Start()
    {
        if (m_image_Child_1 != null &&
         m_image_Child_2 != null)
        {
            m_image_Child_2.sprite = this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            m_image_Child_1.color = new Color(1, 1, 1);
        }
    }
    private void OnEnable()
    {
        if(m_image_Child_1 != null && 
            m_image_Child_2 != null)
        {
            m_image_Child_2.sprite = this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            m_image_Child_1.color = new Color(1, 1, 1);
        }
    }

    private void OnDisable()
    {
        
    }

    private void SettoScripts_Kor()
    {

    }

    private void SettoScripts_Eng()
    {

    }
}
