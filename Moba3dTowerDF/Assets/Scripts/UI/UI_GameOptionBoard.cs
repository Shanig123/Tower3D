using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOptionBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Sprite> m_Arrsprite;
    public List<GameObject> m_ButtonsList;
    public List<UnityEngine.UI.Text> m_TextsList;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        BoardInit();
    }
    private void OnDisable()
    {
     
    }
    private void BoardInit()
    {
        if (Option_Manager.Instance)
        {
            AllChangeText(Option_Manager.Instance.m_tOptiondata.bKor);
            Change_LangSprite();
            Change_EnvSprite();
            Change_ShadowSprite();
        }
    }

    private void AllChangeText(bool _bKor)
    {
        if(_bKor)
        {
            m_ButtonsList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = "항상 밤";
            m_ButtonsList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = "랜덤";
            m_ButtonsList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = "항상 낮";
            m_ButtonsList[3].GetComponentInChildren<UnityEngine.UI.Text>().text = "낮음";
            m_ButtonsList[4].GetComponentInChildren<UnityEngine.UI.Text>().text = "중간";
            m_ButtonsList[5].GetComponentInChildren<UnityEngine.UI.Text>().text = "높음";
            m_ButtonsList[6].GetComponentInChildren<UnityEngine.UI.Text>().text = "기본값";

            m_TextsList[0].text = "게임 옵션";
            m_TextsList[1].text = "언어";
            m_TextsList[2].text = "환경";
            m_TextsList[3].text = "그림자 품질";
        }
        else
        {
            m_ButtonsList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = "Night";
            m_ButtonsList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = "Random";
            m_ButtonsList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = "Day";
            m_ButtonsList[3].GetComponentInChildren<UnityEngine.UI.Text>().text = "Low";
            m_ButtonsList[4].GetComponentInChildren<UnityEngine.UI.Text>().text = "Mid";
            m_ButtonsList[5].GetComponentInChildren<UnityEngine.UI.Text>().text = "High";
            m_ButtonsList[6].GetComponentInChildren<UnityEngine.UI.Text>().text = "Reset";

            m_TextsList[0].text = "Game Option";
            m_TextsList[1].text = "Language";
            m_TextsList[2].text = "Environment";
            m_TextsList[3].text = "Shadow Quality";
        }
    }

    private void Change_LangSprite()
    {
        if(Option_Manager.Instance)
        {
            if(Option_Manager.Instance.m_tOptiondata.bKor)
            {
                m_ButtonsList[7].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[1];
                m_ButtonsList[8].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
            }
            else
            {
                m_ButtonsList[7].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
                m_ButtonsList[8].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[1];
            }
        }
    }
    private void Change_EnvSprite()
    {
        if (Option_Manager.Instance)
        {
            if (Option_Manager.Instance.m_tOptiondata.iDayNight == 1)
            {
                m_ButtonsList[0].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
                m_ButtonsList[1].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
                m_ButtonsList[2].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[1];
            }
            else if(Option_Manager.Instance.m_tOptiondata.iDayNight == 0)
            {
                m_ButtonsList[0].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
                m_ButtonsList[1].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[1];
                m_ButtonsList[2].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
            }
            else
            {
                m_ButtonsList[0].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[1];
                m_ButtonsList[1].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
                m_ButtonsList[2].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
            }
        }
    }
    private void Change_ShadowSprite()
    {
        if (Option_Manager.Instance)
        {
            if (Option_Manager.Instance.m_tOptiondata.iShadow == 1)
            {
                m_ButtonsList[3].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
                m_ButtonsList[4].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
                m_ButtonsList[5].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[1];
            }
            else if (Option_Manager.Instance.m_tOptiondata.iShadow == 0)
            {
                m_ButtonsList[3].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
                m_ButtonsList[4].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[1];
                m_ButtonsList[5].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
            }
            else
            {
                m_ButtonsList[3].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[1];
                m_ButtonsList[4].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
                m_ButtonsList[5].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
            }
        }
    }

    public void OnClickKor()
    {
        if (Option_Manager.Instance)
        {
            Option_Manager.Instance.m_tOptiondata.bKor = true;
            AllChangeText(Option_Manager.Instance.m_tOptiondata.bKor);
            Change_LangSprite();
        }

    }
    public void OnClickEng()
    {
        if (Option_Manager.Instance)
        {
            Option_Manager.Instance.m_tOptiondata.bKor = false;
            AllChangeText(Option_Manager.Instance.m_tOptiondata.bKor);
            Change_LangSprite();
        }
    }
    public void OnClickNight()
    {
        if (Option_Manager.Instance)
            Option_Manager.Instance.m_tOptiondata.iDayNight = -1;
        Change_EnvSprite();
    }
    public void OnClickRandom()
    {
        if (Option_Manager.Instance)
            Option_Manager.Instance.m_tOptiondata.iDayNight = 0;
        Change_EnvSprite();
    }
    public void OnClickDay()
    {
        if (Option_Manager.Instance)
            Option_Manager.Instance.m_tOptiondata.iDayNight = 1;
        Change_EnvSprite();
    }
    public void OnClickLow()
    {
        if (Option_Manager.Instance)
            Option_Manager.Instance.m_tOptiondata.iShadow = -1;
        Change_ShadowSprite();
    }
    public void OnClickMid()
    {
        if (Option_Manager.Instance)
            Option_Manager.Instance.m_tOptiondata.iShadow = 0;
        Change_ShadowSprite();
    }
    public void OnClickHigh()
    {
        if (Option_Manager.Instance)
            Option_Manager.Instance.m_tOptiondata.iShadow = 1;
        Change_ShadowSprite();
    }
    public void OnClickReset()
    {
        if (Option_Manager.Instance)
        {
            Option_Manager.Instance.m_tOptiondata.iShadow = Option_Manager.Instance.Get_DefaultData.iShadow;
            Option_Manager.Instance.m_tOptiondata.iDayNight = Option_Manager.Instance.Get_DefaultData.iDayNight;
        }
        BoardInit();
    }

}
