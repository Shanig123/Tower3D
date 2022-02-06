using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ArchiveBoard : MonoBehaviour
{
    public List<UnityEngine.UI.Text> m_listText;
    public List<GameObject> m_listItem;
    public GameObject m_objItemButtonOrigin;
    public List<Sprite> m_sprites;

    public int m_iWidth;
    public int m_iHeight;

    // Start is called before the first frame update
    private void Awake()
    {
        m_listItem = new List<GameObject>();


        for (int i = 0; i < m_iHeight; ++i)
        {
            for (int j = 0; j < m_iWidth; ++j)
            {
                int iIndex = i * m_iWidth + j;
                GameObject objCreate = GameObject.Instantiate(m_objItemButtonOrigin);
                objCreate.transform.parent = this.transform;

                objCreate.name = "Ability_" + iIndex;
                objCreate.SetActive(true);
                objCreate.GetComponent<UI_AbilityMainMenu>().m_iKey = iIndex;
                m_listItem.Add(objCreate);
            }
        }
    }
    void Start()
    {
        RectTransform rectTransform = m_objItemButtonOrigin.GetComponent<RectTransform>();

        for (int i = 0; i < m_iHeight; ++i)
        {
            for (int j = 0; j < m_iWidth; ++j)
            {
                int iIndex = i * m_iWidth + j;
                Vector3 pos = new Vector3();
                pos.x = -220 + (75 * j);
                pos.y = 240 - (75 * i);
                m_listItem[iIndex].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                m_listItem[iIndex].GetComponent<RectTransform>().localPosition = pos;
            }
        }
        //  ChangeBanner();
        //  ChangeScript();
        //  ChangeSprite();
    }
    private void OnEnable()
    {
        ChangeBanner();
        ChangeScript(0);
        if (GameObject.Find("Log_File"))
        {
            UnityEngine.UI.Text tLog =
           GameObject.Find("Log_File").
           GetComponent<UnityEngine.UI.Text>();
            tLog.text = "OnEnable_sprite";
        }
        ChangeSprite();
    }

    private void ChangeSprite()
    {
        if (Game_Manager.Instance)
        {
            for (int i = 0; i < m_listItem.Count; ++i)
            {
                if (Game_Manager.Instance.m_tGameData.bArrUnlockAbility[i])
                {
                    m_listItem[i].GetComponent<UnityEngine.UI.Image>().sprite = m_sprites[1];
                }
                else
                {


                    m_listItem[i].GetComponent<UnityEngine.UI.Image>().sprite = m_sprites[0];
                }
            }
        }

    }
    private void ChangeBanner()
    {
        GameObject objBanner = gameObject.transform.Find("ArchiveBanner").gameObject;

        if (Option_Manager.Instance)
        {
            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                objBanner.GetComponent<UnityEngine.UI.Text>().text = "아 카 이 브";
            }
            else
            {
                objBanner.GetComponent<UnityEngine.UI.Text>().text = "ARCHIVE";
            }
        }
    }
    private void ChangeScript(int _iAbilityNumber)
    {
        GameObject objScript = gameObject.transform.Find("ArchiveScriptTxt").gameObject;
        if (Option_Manager.Instance)
        {
            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                if (Game_Manager.Instance.m_tGameData.bArrUnlockAbility[_iAbilityNumber])
                {
                   // string scripts = Resource_Manager.Instance.Get_Scripts[0][_iAbilityNumber];

                    objScript.GetComponent<UnityEngine.UI.Text>().text = "해금됨";

                }
                else
                {
                    string scripts = Resource_Manager.Instance.Get_Scripts[1][_iAbilityNumber];
                    objScript.GetComponent<UnityEngine.UI.Text>().text =
                        scripts;
                }
            }
            else
            {
                if (Game_Manager.Instance.m_tGameData.bArrUnlockAbility[_iAbilityNumber])
                {
                    objScript.GetComponent<UnityEngine.UI.Text>().text = "Unlocked";

                }
                else
                {
                    string scripts = Resource_Manager.Instance.Get_Scripts[3][_iAbilityNumber];
                    objScript.GetComponent<UnityEngine.UI.Text>().text =
                        scripts;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ClickItem(UnityEngine.UI.Button _objButton)
    {
        UI_AbilityMainMenu uI_AbilityMainMenu = _objButton.GetComponent<UI_AbilityMainMenu>();

        if (uI_AbilityMainMenu)
        {
            if (Game_Manager.Instance)
            {
                ChangeSprite();
                ChangeScript(uI_AbilityMainMenu.m_iKey);
            }
        }

    }
}
