using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SoundOptionBoard : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> m_BarList;
    public List<UnityEngine.UI.Text> m_TextsList;
    public GameObject m_objResetButton;
    
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
            ChangeMasterVolText();
            ChangeSFXVolText();
            ChangeBGMVolText();
        }
    }

    private void AllChangeText(bool _bKor)
    {
        if(_bKor)
        {
            m_TextsList[0].text = "사운드 옵션";
            m_TextsList[1].text = "마스터 볼륨";
            m_TextsList[2].text = "SFX 볼륨";
            m_TextsList[3].text = "BGM 볼륨";
            
            m_objResetButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "기본값";
        }
        else
        {
            m_TextsList[0].text = "Sound Option";
            m_TextsList[1].text = "Master Vol";
            m_TextsList[2].text = "SFX Vol";
            m_TextsList[3].text = "BGM Vol";

            m_objResetButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Reset";
        }
    }


    private void ChangeMasterVolText()
    {
        if(Option_Manager.Instance)
        {
            float fVol =  Option_Manager.Instance.m_tOptiondata.fMasterVol;
            if (fVol > 1)
                fVol = 1;
            m_BarList[0].GetComponent<UnityEngine.UI.Scrollbar>().value = fVol;
            int iConvert = (int)(fVol*100f);
            m_ValueTextList[0].text =""+iConvert;
        }
    }   
    
    private void ChangeSFXVolText()
    {
        if(Option_Manager.Instance)
        {
            float fVol =  Option_Manager.Instance.m_tOptiondata.fSfxVol;
            if (fVol > 1)
                fVol = 1;
            m_BarList[1].GetComponent<UnityEngine.UI.Scrollbar>().value = fVol;
            int iConvert = (int)(fVol*100f);
            m_ValueTextList[1].text =""+iConvert;
        }
    }   
    
    private void ChangeBGMVolText()
    {
        if(Option_Manager.Instance)
        {
            float fVol =  Option_Manager.Instance.m_tOptiondata.fBgmVol;
            if (fVol > 1)
                fVol = 1;
            m_BarList[2].GetComponent<UnityEngine.UI.Scrollbar>().value = fVol;
            int iConvert = (int)(fVol*100f);
            m_ValueTextList[2].text =""+iConvert;
        }
    }    

    public void Scroll_MasterVol(UnityEngine.UI.Scrollbar _scrollbar)
    {
       float fVol = _scrollbar.value;
        if (fVol > 1)
            fVol = 1;
        Option_Manager.Instance.m_tOptiondata.fMasterVol = fVol;
       ChangeMasterVolText();
    }
       
    public void Scroll_SFXVol(UnityEngine.UI.Scrollbar _scrollbar)
    {
        float fVol = _scrollbar.value;
        if (fVol > 1)
            fVol = 1;
        Option_Manager.Instance.m_tOptiondata.fSfxVol = fVol;
       ChangeSFXVolText();
    }
        
    public void Scroll_BGMVol(UnityEngine.UI.Scrollbar _scrollbar)
    {
        float fVol = _scrollbar.value;
        if (fVol > 1)
            fVol = 1;
        Option_Manager.Instance.m_tOptiondata.fBgmVol = fVol;
       ChangeBGMVolText();
    }


    public void OnClickReset()
    {
        Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.UI, 9, new Vector3(100, 100, 100));
        if (Option_Manager.Instance)
        {
            Option_Manager.Instance.m_tOptiondata.fMasterVol = Option_Manager.Instance.Get_DefaultData.fMasterVol;
            Option_Manager.Instance.m_tOptiondata.fBgmVol = Option_Manager.Instance.Get_DefaultData.fBgmVol;
            Option_Manager.Instance.m_tOptiondata.fSfxVol = Option_Manager.Instance.Get_DefaultData.fSfxVol;

            
        }
        BoardInit();
    }
}
