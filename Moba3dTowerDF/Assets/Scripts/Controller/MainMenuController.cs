using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    #region Value
    public enum eMenuState { Main, StageSelect, Option, Exit, End };
    enum eMenuInFoState { DifficultySelect, AbilitySelect, Archive, GeneralOption, SoundOption, Emty2, Main, End }

    [SerializeField] eMenuState m_eCurMenuState = eMenuState.End;
    [SerializeField] eMenuState m_eNextMenuState = eMenuState.End;
    [SerializeField] eMenuInFoState m_eCurMenuInfoState = eMenuInFoState.End;
    [SerializeField] eMenuInFoState m_eNextMenuInfoState = eMenuInFoState.End;

    bool m_bUpdateInit = false;

    string[] m_strKor = { "게 임 시 작", "옵 션", "종 료", "특 성", "난 이 도", "아카이브", "일 반", "사 운 드", " " };
    string[] m_strEng = { "GameStart", "Option", "Quit", "Ability", "Difficulty", "Archive", "General", "Sound", " " };

    [SerializeField] List<GameObject> m_objUIList;
    [SerializeField] List<GameObject> m_objBoardUIList;
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        //m_objUIList = new List<GameObject>();
        //m_objBoardUIList = new List<GameObject>();

        //m_objUIList.Add(GameObject.Find("MainTop"));
        //m_objUIList.Add(GameObject.Find("MainMid"));
        //m_objUIList.Add(GameObject.Find("MainBottom"));

        //m_objUIList.Add(GameObject.Find("InfoBackground"));
        //m_objUIList.Add(GameObject.Find("ExitQuestion")); //ExitQuestion

        //m_objBoardUIList.Add(GameObject.Find("Ability_Board"));
        //m_objBoardUIList.Add(GameObject.Find("Difficulty_Board"));
        //m_objBoardUIList.Add(GameObject.Find("GeneralOption_Board"));
        //m_objBoardUIList.Add(GameObject.Find("SoundOption_Board"));


    }
    void Start()
    {
        m_eNextMenuState = eMenuState.Main;
        m_eNextMenuInfoState = eMenuInFoState.Main;
       
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


        m_bUpdateInit = true;
    }
    #region UiClick
    public void ClickTopButton()
    {
        Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.UI, 8, new Vector3(100, 100, 100));
        if (m_eCurMenuState == eMenuState.Main) // 메인화면에서 게임시작 버튼 눌렀을 때
        {
            m_eNextMenuState = eMenuState.StageSelect;
        }
        else if (m_eCurMenuState == eMenuState.Option) // 옵션화면에서 제네럴 눌렀을 때
        {
            m_eNextMenuInfoState = eMenuInFoState.GeneralOption;
        }
        else if (m_eCurMenuState == eMenuState.Exit) //엑시트 (메인화면에서) 엑시트 눌렀을 때
        {
            m_eNextMenuState = eMenuState.StageSelect;
        }
        else if (m_eCurMenuState == eMenuState.StageSelect) // 게임시작화면에서 특성 선택 눌렀을 때
        {
            m_eNextMenuInfoState = eMenuInFoState.AbilitySelect;
        }
        return;
    }
    public void ClickMidButton()
    {
        Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.UI, 8, new Vector3(100, 100, 100));
        if (m_eCurMenuState == eMenuState.Main) // 메인화면에서 옵션 버튼 눌렀을 때
        {
            m_eNextMenuState = eMenuState.Option;
        }
        else if (m_eCurMenuState == eMenuState.StageSelect)// 게임시작화면에서 난이도 선택 눌렀을 때
        {
            m_eNextMenuInfoState = eMenuInFoState.DifficultySelect;
        }
        else if (m_eCurMenuState == eMenuState.Exit) //엑시트 (메인화면에서) 엑시트 눌렀을 때
        {
            m_eNextMenuState = eMenuState.Option;
        }
        else if (m_eCurMenuState == eMenuState.Option) // 옵션화면에서 사운드 눌렀을 때
        {
            m_eNextMenuInfoState = eMenuInFoState.SoundOption;
        }
        return;
    }
    public void ClickBottomButton()
    {
        Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.UI, 8, new Vector3(100, 100, 100));
        if (m_eCurMenuState == eMenuState.Main)// 메인화면에서 엑시트 눌렀을 때
        {
            // Game_Manager.Instance.AppQuit();
            m_eNextMenuState = eMenuState.Exit;
        }
        else if (m_eCurMenuState == eMenuState.StageSelect) // 게임시작화면에서 특성 선택 눌렀을 때
        {
            m_eNextMenuInfoState = eMenuInFoState.Archive;
        }
        else if (m_eCurMenuState == eMenuState.Exit) //엑시트 (메인화면에서) 엑시트 눌렀을 때
        {
            m_eNextMenuState = eMenuState.Main;
        }
        else
        {
            m_eNextMenuInfoState = eMenuInFoState.Main;
        }
        return;
    }

    public void ClickYesButton()
    {
        Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.UI, 8, new Vector3(100, 100, 100));
        if (m_eCurMenuState == eMenuState.StageSelect)
        {
            SceneManager.LoadScene("MainScene");
        }
        else if (m_eCurMenuState == eMenuState.Option)
        {
            m_eNextMenuState = eMenuState.Main;
        }
        else if (m_eCurMenuState == eMenuState.Exit)
        {
            Game_Manager.Instance.AppQuit();
        }
        return;
    }
    public void ClickNoButton()
    {
        Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.UI, 9, new Vector3(100, 100, 100));
        m_eNextMenuState = eMenuState.Main;
        return;
    }

    public void ClickKorButton()
    {
        Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.UI, 8, new Vector3(100, 100, 100));
        Option_Manager.Instance.m_tOptiondata.bKor = true;
        ChangeAllText();
        return;
    }
    public void ClickEngButton()
    {
        Sound_Manager.Instance.Play_AudioClip(DataEnum.eClip.UI, 8, new Vector3(100, 100, 100));
        Option_Manager.Instance.m_tOptiondata.bKor = false;
        ChangeAllText();
        return;
    }
    #endregion


    void ChangeAllText()
    {
        if(Option_Manager.Instance)
        {
            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[6];
                m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[7];
                m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[8];
                m_objUIList[5].GetComponentInChildren<UnityEngine.UI.Text>().text = "보상광고";

            }
            else
            {
                m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[6];
                m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[7];
                m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[8];
                m_objUIList[5].GetComponentInChildren<UnityEngine.UI.Text>().text = "RewardAd";
            }
        }
    }

    void ChangeMenuState()
    {
        if(m_eNextMenuState != m_eCurMenuState)
        {
            switch(m_eNextMenuState)
            {
                case eMenuState.Main:
                    if(Option_Manager.Instance.m_tOptiondata.bKor)
                    {
                        m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[0];
                        m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[1];
                        m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[2];                 
                    }
                    else
                    {
                        m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[0];
                        m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[1];
                        m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[2];

                    }
                    m_eNextMenuInfoState = eMenuInFoState.Main;
                    m_objUIList[3].SetActive(false);
                    m_objUIList[4].SetActive(false);
                    break;
                case eMenuState.StageSelect:
                    if (Option_Manager.Instance.m_tOptiondata.bKor)
                    {
                        m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[3];
                        m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[4];
                        m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[5];
                    }
                    else
                    {
                        m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[3];
                        m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[4];
                        m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[5];

                    }
                    m_eNextMenuInfoState = eMenuInFoState.AbilitySelect;
                    m_objUIList[3].SetActive(true);
                    m_objUIList[4].SetActive(false);
                    break;
                case eMenuState.Option:
                    if (Option_Manager.Instance.m_tOptiondata.bKor)
                    {
                        m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[6];
                        m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[7];
                        m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[8];
                    }
                    else
                    {
                        m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[6];
                        m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[7];
                        m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[8];

                    }
                    m_eNextMenuInfoState = eMenuInFoState.GeneralOption;
                    m_objUIList[3].SetActive(true);
                    m_objUIList[4].SetActive(false);
                    break;
                case eMenuState.Exit:
                    if (Option_Manager.Instance.m_tOptiondata.bKor)
                    {
                        m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[0];
                        m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[1];
                        m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strKor[2];
                    }
                    else
                    {
                        m_objUIList[0].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[0];
                        m_objUIList[1].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[1];
                        m_objUIList[2].GetComponentInChildren<UnityEngine.UI.Text>().text = m_strEng[2];

                    }
                    m_eNextMenuInfoState = eMenuInFoState.Main;
                    m_objUIList[3].SetActive(false);
                    m_objUIList[4].SetActive(true);
                    break;
                default:
                    break;
            }
            m_eCurMenuState = m_eNextMenuState;
        }
        ChangeMenuInfo();
    }

    void ChangeMenuInfo()
    {
        if(m_eNextMenuInfoState != m_eCurMenuInfoState)
        {
            switch(m_eNextMenuInfoState)
            {
                case eMenuInFoState.AbilitySelect:
                    m_objBoardUIList[0].SetActive(true);
                    m_objBoardUIList[1].SetActive(false);
                    m_objBoardUIList[4].SetActive(false);
                    break;
                case eMenuInFoState.DifficultySelect:
                    m_objBoardUIList[0].SetActive(false);
                    m_objBoardUIList[1].SetActive(true);
                    m_objBoardUIList[4].SetActive(false);
                    break;
                case eMenuInFoState.Archive:
                    //if (m_eCurMenuInfoState == eMenuInFoState.AbilitySelect)
                    //    m_eNextMenuInfoState = eMenuInFoState.AbilitySelect;
                    //else if (m_eCurMenuInfoState == eMenuInFoState.DifficultySelect)
                    //    m_eNextMenuInfoState = eMenuInFoState.DifficultySelect;

                    m_objBoardUIList[0].SetActive(false);
                    m_objBoardUIList[1].SetActive(false);
                    m_objBoardUIList[4].SetActive(true);
                    break;
                case eMenuInFoState.GeneralOption:
                    m_objBoardUIList[2].SetActive(true);
                    m_objBoardUIList[3].SetActive(false);
                    break;
                case eMenuInFoState.SoundOption:
                    m_objBoardUIList[2].SetActive(false);
                    m_objBoardUIList[3].SetActive(true);
                    break;
                case eMenuInFoState.Emty2:
                    if (m_eCurMenuInfoState == eMenuInFoState.GeneralOption)
                        m_eNextMenuInfoState = eMenuInFoState.GeneralOption;
                    else if (m_eCurMenuInfoState == eMenuInFoState.SoundOption)
                        m_eNextMenuInfoState = eMenuInFoState.SoundOption;
                    break;
                case eMenuInFoState.Main:
                    foreach(GameObject iter in m_objBoardUIList)
                    {
                        iter.SetActive(false);
                    }
                    break;
                default:
                    foreach (GameObject iter in m_objBoardUIList)
                    {
                        iter.SetActive(false);
                    }
                    break;
            }
            m_eCurMenuInfoState = m_eNextMenuInfoState;
        }
    }

}
