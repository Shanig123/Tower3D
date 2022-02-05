using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AbilityBoard : MonoBehaviour
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
                m_listItem[iIndex].GetComponent<RectTransform>().localScale =new Vector3(1, 1, 1);
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
        ChangeScript();
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
            string tf = "";
            if (GameObject.Find("Log_File"))
            {
                UnityEngine.UI.Text tLog =
               GameObject.Find("Log_File").
               GetComponent<UnityEngine.UI.Text>();
                tLog.text = "Instance GameManger";
            }
            for (int i = 0; i < m_listItem.Count; ++i)
            {
                if (GameObject.Find("Log_File"))
                {
                    UnityEngine.UI.Text tLog =
                   GameObject.Find("Log_File").
                   GetComponent<UnityEngine.UI.Text>();
                    tLog.text = "for"+i;
                }
                if (Game_Manager.Instance.m_tGameData.bArrUnlockAbility[i])
                {
                    tf += "1";
                    if (GameObject.Find("Log_File"))
                    {
                        UnityEngine.UI.Text tLog =
                       GameObject.Find("Log_File").
                       GetComponent<UnityEngine.UI.Text>();
                        tLog.text = "for" + i +"_true";
                    }
                    if (i == Game_Manager.Instance.m_tStageInfo.iStartAbility)
                        m_listItem[i].GetComponent<UnityEngine.UI.Image>().sprite = m_sprites[2];
                    else
                        m_listItem[i].GetComponent<UnityEngine.UI.Image>().sprite = m_sprites[1];
                }
                else
                {
                    tf += "0";
                    if (GameObject.Find("Log_File"))
                    {
                        UnityEngine.UI.Text tLog =
                       GameObject.Find("Log_File").
                       GetComponent<UnityEngine.UI.Text>();
                        tLog.text = "for" + i + "_false";
                    }
                    m_listItem[i].GetComponent<UnityEngine.UI.Image>().sprite = m_sprites[0];
                }
            }

            if (GameObject.Find("Log_File"))
            {
                UnityEngine.UI.Text tLog =
               GameObject.Find("Log_File").
               GetComponent<UnityEngine.UI.Text>();
                tLog.text = tf;
            }
        }

    }
    private void ChangeBanner()
    {
        GameObject objBanner = gameObject.transform.Find("AbilityBanner").gameObject;

        if (Option_Manager.Instance)
        {
            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                objBanner.GetComponent<UnityEngine.UI.Text>().text = "특 성 선 택"; 
            }
            else
            {
                objBanner.GetComponent<UnityEngine.UI.Text>().text = "ABILITY";
            }
        }
    }
    private void ChangeScript()
    {     
        GameObject objScript = gameObject.transform.Find("AbilityScriptTxt").gameObject;
        if (Option_Manager.Instance)
        {
            if(Option_Manager.Instance.m_tOptiondata.bKor)
            {
                if (Game_Manager.Instance.m_tGameData.bArrUnlockAbility[Game_Manager.Instance.m_tStageInfo.iStartAbility])
                    objScript.GetComponent<UnityEngine.UI.Text>().text = "특성_" + Game_Manager.Instance.m_tStageInfo.iStartAbility;
                else
                    objScript.GetComponent<UnityEngine.UI.Text>().text = "잠김";
            }
            else
            {
                if (Game_Manager.Instance.m_tGameData.bArrUnlockAbility[Game_Manager.Instance.m_tStageInfo.iStartAbility])
                    objScript.GetComponent<UnityEngine.UI.Text>().text = "ABILITY_" + Game_Manager.Instance.m_tStageInfo.iStartAbility;
                else
                    objScript.GetComponent<UnityEngine.UI.Text>().text = "LOCK";
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

        if(uI_AbilityMainMenu)
        {
            if(Game_Manager.Instance)
            {

                Game_Manager.Instance.m_tStageInfo.iStartAbility = uI_AbilityMainMenu.m_iKey;
                ChangeSprite();
                ChangeScript();
            }
        }

    }
}
