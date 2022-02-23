using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Quit : MonoBehaviour
{
    public UnityEngine.UI.Text m_QuitText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        if(Option_Manager.Instance)
        {
            if (Option_Manager.Instance.m_tOptiondata.bKor)
                m_QuitText.text = "종료하시겠습니까?";
            else
                m_QuitText.text = "Are you sure \n"+"quit the program?";
        }
    }
}
