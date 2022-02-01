using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SoundOptionBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Sprite> m_Arrsprite;
    public List<GameObject> m_BarList;
    public List<UnityEngine.UI.Text> m_TextsList;
    public GameObject m_objResetButton;

    public GameObject m_Temp;

    public List<UnityEngine.UI.Text> m_ValueTextList;

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
  
  
        }
    }

    private void AllChangeText(bool _bKor)
    {
        if(_bKor)
        {
            // m_ButtonsList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = "�׻� ��";
            // m_ButtonsList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = "����";
            // m_ButtonsList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = "�׻� ��";
            // m_ButtonsList[3].GetComponentInChildren<UnityEngine.UI.Text>().text = "����";
            // m_ButtonsList[4].GetComponentInChildren<UnityEngine.UI.Text>().text = "�߰�";
            // m_ButtonsList[5].GetComponentInChildren<UnityEngine.UI.Text>().text = "����";

            m_TextsList[0].text = "���� �ɼ�";
            m_TextsList[1].text = "���";
            m_TextsList[2].text = "ȯ��";
            m_TextsList[3].text = "�׸��� ǰ��";
            
            m_objResetButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "�⺻��";
        }
        else
        {
            // m_ButtonsList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = "Night";
            // m_ButtonsList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = "Random";
            // m_ButtonsList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = "Day";
            // m_ButtonsList[3].GetComponentInChildren<UnityEngine.UI.Text>().text = "Low";
            // m_ButtonsList[4].GetComponentInChildren<UnityEngine.UI.Text>().text = "Mid";
            // m_ButtonsList[5].GetComponentInChildren<UnityEngine.UI.Text>().text = "High";

            m_TextsList[0].text = "Sound Option";
            m_TextsList[1].text = "Master Vol";
            m_TextsList[2].text = "SFX Vol";
            m_TextsList[3].text = "BGM Vol";

            m_objResetButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Reset";
        }

        ChangeMasterVolText();
        ChangeSFXVolText();
        ChangeBGMVolText();
    }


    private void ChangeMasterVolText()
    {
        if(Option_Manager.Instance)
        {
            float fVol =  Option_Manager.Instance.m_tOptiondata.fMasterVol;
            int iConvert = (int)(fVol*100f);
            m_ValueTextList[0].text =""+iConvert;
        }
    }   
    
    private void ChangeSFXVolText()
    {
        if(Option_Manager.Instance)
        {
            float fVol =  Option_Manager.Instance.m_tOptiondata.fSfxVol;
            int iConvert = (int)(fVol*100f);
            m_ValueTextList[1].text =""+iConvert;
        }
    }   
    
    private void ChangeBGMVolText()
    {
        if(Option_Manager.Instance)
        {
            float fVol =  Option_Manager.Instance.m_tOptiondata.fBgmVol;
            int iConvert = (int)(fVol*100f);
            m_ValueTextList[2].text =""+iConvert;
        }
    }    

    public void Scroll_MasterVol(float _fScollValue)
    {
       float fVol =  _fScollValue   ;
       Option_Manager.Instance.m_tOptiondata.fBgmVol = fVol;
       ChangeMasterVolText();
    }
       
    public void Scroll_SFXVol(float _fScollValue)
    {
       float fVol =  _fScollValue   ;
       Option_Manager.Instance.m_tOptiondata.fSfxVol = fVol;
       ChangeSFXVolText();
    }
        
    public void Scroll_BGMVol(float _fScollValue)
    {
        //UnityEngine.UI.ScrollBar = new UnityEngine.UI.ScrollBar();

       float fVol =  _fScollValue   ;
       Option_Manager.Instance.m_tOptiondata.fBgmVol = fVol;
       ChangeBGMVolText();
    }


    public void OnClickReset()
    {
        if (Option_Manager.Instance)
        {
            Option_Manager.Instance.m_tOptiondata.fMasterVol = Option_Manager.Instance.Get_DefaultData.fMasterVol;
            Option_Manager.Instance.m_tOptiondata.fBgmVol = Option_Manager.Instance.Get_DefaultData.fBgmVol;
            Option_Manager.Instance.m_tOptiondata.fSfxVol = Option_Manager.Instance.Get_DefaultData.fSfxVol;
        }
        BoardInit();
    }
}
