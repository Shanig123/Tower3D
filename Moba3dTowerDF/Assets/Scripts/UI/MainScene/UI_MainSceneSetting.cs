using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainSceneSetting : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_listDimmed;
    public List<GameObject> m_BarList;
    public List<UnityEngine.UI.Text> m_TextsList;

    public List<UnityEngine.UI.Text> m_ValueTextList;

    private bool m_bActive = false;
    public bool Get_Active { get { return m_bActive; } }

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!m_bActvie)
        //    this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        m_bActive = true;
        AllChangeText(Option_Manager.Instance.m_tOptiondata.bKor);
        foreach(GameObject iter in m_listDimmed)
        {
            iter.SetActive(false);
        }
        Game_Manager.Instance.Game_Pause(true);
    }

    private void OnDisable()
    {
        m_bActive = false;
        foreach (GameObject iter in m_listDimmed)
        {
            iter.SetActive(true);
        }
        Game_Manager.Instance.Game_Pause(false);
    }

    public void OnClick_SettingBoard()
    {
        if (m_bActive)
            this.gameObject.SetActive(false);
        else
            this.gameObject.SetActive(true);
    }

    public void OnClick_MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");

    }
    public void Scroll_MasterVol(UnityEngine.UI.Scrollbar _scrollbar)
    {
        float fVol = _scrollbar.value;
        Option_Manager.Instance.m_tOptiondata.fMasterVol = fVol;
        ChangeMasterVolText();
    }

    public void Scroll_SFXVol(UnityEngine.UI.Scrollbar _scrollbar)
    {
        float fVol = _scrollbar.value;
        Option_Manager.Instance.m_tOptiondata.fSfxVol = fVol;
        ChangeSFXVolText();
    }

    public void Scroll_BGMVol(UnityEngine.UI.Scrollbar _scrollbar)
    {
        float fVol = _scrollbar.value;
        Option_Manager.Instance.m_tOptiondata.fBgmVol = fVol;
        ChangeBGMVolText();
    }

    private void AllChangeText(bool _bKor)
    {
        if (_bKor)
        {
            m_TextsList[0].text = "일시정지 !";
            m_TextsList[1].text = "마스터 볼륨";
            m_TextsList[2].text = "SFX 볼륨";
            m_TextsList[3].text = "BGM 볼륨";

           //버튼 텍스트
        }
        else
        {
            m_TextsList[0].text = "PAUSE !";
            m_TextsList[1].text = "Master Vol";
            m_TextsList[2].text = "SFX Vol";
            m_TextsList[3].text = "BGM Vol";

            //버튼 텍스트
        }
    }

    private void ChangeMasterVolText()
    {
        if (Option_Manager.Instance)
        {
            float fVol = Option_Manager.Instance.m_tOptiondata.fMasterVol;
            m_BarList[0].GetComponent<UnityEngine.UI.Scrollbar>().value = fVol;
            int iConvert = (int)(fVol * 100f);
            m_ValueTextList[0].text = "" + iConvert;
        }
    }

    private void ChangeSFXVolText()
    {
        if (Option_Manager.Instance)
        {
            float fVol = Option_Manager.Instance.m_tOptiondata.fSfxVol;
            m_BarList[1].GetComponent<UnityEngine.UI.Scrollbar>().value = fVol;
            int iConvert = (int)(fVol * 100f);
            m_ValueTextList[1].text = "" + iConvert;
        }
    }

    private void ChangeBGMVolText()
    {
        if (Option_Manager.Instance)
        {
            float fVol = Option_Manager.Instance.m_tOptiondata.fBgmVol;
            m_BarList[2].GetComponent<UnityEngine.UI.Scrollbar>().value = fVol;
            int iConvert = (int)(fVol * 100f);
            m_ValueTextList[2].text = "" + iConvert;
        }
    }

}
