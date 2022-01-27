using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public enum eMenuState { Main , StageSelect, Option, Exit, End};
    enum eMenuInFoState {DifficultySelect, SpecilitySelect, NoEmty, GeneralOption, SoundOption,GraphicOption, Main, End }
    [SerializeField] eMenuState m_eCurMenuState = eMenuState.End;
    [SerializeField] eMenuState m_eNextMenuState = eMenuState.End;
    [SerializeField] eMenuInFoState m_eMenuInfoState = eMenuInFoState.End;
    bool m_bUpdateInit = false;
    string[] m_strKor = { "게 임 시 작" , "옵 션","종 료" ,"난 이 도", "특 성", "","일 반", "사 운 드", "그 래 픽"  };
    string[] m_strEng = { "게 임 시 작", "옵 션", "종 료", "난 이 도", "특 성", "", "일 반", "사 운 드", "그 래 픽" };
    [SerializeField] List<GameObject> m_objUIList;
    // Start is called before the first frame update
    void Start()
    {
        m_eNextMenuState = eMenuState.Main;
        m_eMenuInfoState = eMenuInFoState.Main;
        m_objUIList = new List<GameObject>();

    }
    // Update is called once per frame
    void Update()
    {
        if (!m_bUpdateInit)
            UpdateInit();
        ChangeMenuState();
    }

    void UpdateInit()
    {
        m_objUIList.Add(GameObject.Find("MainTop"));
        m_objUIList.Add(GameObject.Find("MainMid"));
        m_objUIList.Add(GameObject.Find("MainBottom"));
        m_objUIList.Add(GameObject.Find("InfoBackground"));
        m_objUIList.Add(GameObject.Find("ExitQuestion"));
        
        m_bUpdateInit = true;
    }

    public void ClickTopButton()
    {
        if (m_eCurMenuState == eMenuState.Main)
        {
            m_eNextMenuState = eMenuState.StageSelect;
        }
        else if(m_eCurMenuState == eMenuState.Option)
        {
           // m_e
        }
        else if (m_eCurMenuState == eMenuState.StageSelect)
        {

        }
        return;
    }
    public void ClickMidButton()
    {
        if (m_eCurMenuState == eMenuState.Main)
        {
            m_eNextMenuState = eMenuState.Option;
        }
        else if (m_eCurMenuState == eMenuState.StageSelect)
        {

        }
        else if (m_eCurMenuState == eMenuState.Option)
        {

        }
        return;
    }
    public void ClickBottomButton()
    {
        if (m_eCurMenuState == eMenuState.Main)
        {
            // Game_Manager.Instance.AppQuit();
            m_eNextMenuState = eMenuState.Exit;
        }
        else if(m_eCurMenuState == eMenuState.Exit)
        {
            m_eNextMenuState = eMenuState.Main;
        }
        else
        {

        }
        return;
    }

    public void ClickYesButton()
    {
        if (m_eCurMenuState == eMenuState.StageSelect)
        {
            SceneManager.LoadScene("MainScene");
        }
        else if(m_eCurMenuState == eMenuState.Option)
        {
            m_eNextMenuState = eMenuState.Main;
        }
        else if(m_eCurMenuState == eMenuState.Exit)
        {
            Game_Manager.Instance.AppQuit();
        }
        return;
    }
    public void ClickNoButton()
    {
        m_eNextMenuState = eMenuState.Main;
        return;
    }


    

    void ChangeMenuState()
    {
        if(m_eNextMenuState != m_eCurMenuState)
        {
            switch(m_eNextMenuState)
            {
                case eMenuState.Main:
                    m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[0];
                    m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[1];
                    m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[2];
                    m_eMenuInfoState = eMenuInFoState.Main;
                    m_objUIList[3].SetActive(false);
                    m_objUIList[4].SetActive(false);
                    break;
                case eMenuState.StageSelect:
                    m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[3];
                    m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[4];
                    m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[5];
                    m_eMenuInfoState = eMenuInFoState.DifficultySelect;
                    m_objUIList[3].SetActive(true);
                    m_objUIList[4].SetActive(false);
                    break;
                case eMenuState.Option:
                    m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[6];
                    m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[7];
                    m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[8];
                    m_eMenuInfoState = eMenuInFoState.GeneralOption;
                    m_objUIList[3].SetActive(true);
                    m_objUIList[4].SetActive(false);
                    break;
                case eMenuState.Exit:
                    m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[0];
                    m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[1];
                    m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[2];
                    m_eMenuInfoState = eMenuInFoState.Main;
                    m_objUIList[3].SetActive(false);
                    m_objUIList[4].SetActive(true);
                    break;
                default:
                    break;
            }
            m_eCurMenuState = m_eNextMenuState;
        }
    }



}
