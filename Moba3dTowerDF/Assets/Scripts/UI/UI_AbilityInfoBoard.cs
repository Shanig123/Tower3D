using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AbilityInfoBoard : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Text m_textScript;
    [SerializeField] private GameObject m_PlayerInfo;
    [SerializeField] private GameObject m_AbilityButton;
    [SerializeField] private List<Sprite> m_AbilityImages;
    [SerializeField] private List<GameObject> m_ButtonList;
    [SerializeField] List<int> m_listUIAbility;
    int m_iSelectIndex;

    [SerializeField] int m_iHeight;
    [SerializeField] int m_iWidth;

    int m_iWidthInit;
    int m_iHeightInit;

    private bool m_bActive = false;
    // Start is called before the first frame update
    void Start()
    {
        m_listUIAbility = new List<int>();
        m_ButtonList = new List<GameObject>();
        m_iWidthInit = m_iWidth;
        m_iHeightInit = m_iHeight;
    }

    private void OnEnable()
    {
        m_bActive = true;
        m_PlayerInfo.SetActive(false);
        m_AbilityButton.SetActive(false);
        CopyAbility();
        PrintScript();
        PrintSprite();
    }
    private void OnDisable()
    {
        m_bActive = false;
        m_PlayerInfo.SetActive(true);
        m_iSelectIndex = 0;
    }

    void PrintScript()
    {
        if (m_listUIAbility.Count < 1)
        {
            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                m_textScript.text = "Ư�� ����";
            }
            else
            {
                m_textScript.text = "No Ability";
            }
            return;
        }
        int iAbiID = m_listUIAbility[m_iSelectIndex];

        if (Option_Manager.Instance.m_tOptiondata.bKor)
        {
            m_textScript.text = "��ò���";
        }
        else
        {
            m_textScript.text = "mosiGGagi";
        }
    }

    void PrintSprite()
    {
        //List<GameObject> templist = m_ButtonList;
        float fScale = 1f;
        if (m_ButtonList.Count > (m_iWidth*m_iHeight))
        {
            float fWidthTemp = m_iWidth;
            float fHeightTemp = m_iHeight;

            fWidthTemp *= 1.5f;
            fHeightTemp *= 1.5f;

            m_iWidth = (int)fWidthTemp;
            m_iHeight = (int)fHeightTemp;

        }
        int InitScale = 75;
        float CurScale = 400 /(float) m_iWidth;
        //if (InitScale != CurScale)
            fScale = CurScale / (float)InitScale;
        //300/4 75--1
        //300 6 ?
        for (int i = 0; i < m_ButtonList.Count; ++i)
        {
            //��ư����
            RectTransform rectTransform = m_AbilityButton.GetComponent<RectTransform>();

            int j = i / m_iWidth;
            int k = i % m_iWidth;

            GameObject objCreate = m_ButtonList[i];
            objCreate.transform.parent = this.transform;

            Vector3 pos = new Vector3();

            pos.x = -160 + (CurScale * k);
            pos.y = 350 - (CurScale * j);

            objCreate.GetComponent<RectTransform>().localScale = new Vector3(fScale, fScale, fScale);
            objCreate.GetComponent<RectTransform>().localPosition = pos;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick_AbilityInfoBoard()
    {
        if (m_bActive)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    public void OnClick_Ability(UnityEngine.UI.Button _objButton)
    {
        UI_AbilityMainMenu uI_AbilityMainMenu = _objButton.GetComponent<UI_AbilityMainMenu>();

        if (uI_AbilityMainMenu)
        {
            m_iSelectIndex = uI_AbilityMainMenu.m_iKey;
            PrintScript();
        }
            
    }


    private void CopyAbility()
    {
        List<int> templist = GameObject.FindGameObjectWithTag("TotalController").GetComponent<AbilityController>().Get_AbilityList;
        
        if(templist.Count > m_listUIAbility.Count)
        {
            for(int i=m_listUIAbility.Count;i<templist.Count;++i)
            {
                //��ư����
                RectTransform rectTransform = m_AbilityButton.GetComponent<RectTransform>();

                int j = i/m_iWidth;
                int k = i%m_iWidth;

                GameObject objCreate = GameObject.Instantiate(m_AbilityButton);
                objCreate.transform.parent = this.transform;
                m_listUIAbility.Add(templist[i]);
                objCreate.name = "Ability_" + templist[i];
                objCreate.SetActive(true);

                objCreate.GetComponent<UI_AbilityMainMenu>().m_iKey = templist[i];
                objCreate.GetComponent<UnityEngine.UI.Image>().sprite = m_AbilityImages[templist[i]];
                
                //Vector3 pos = new Vector3();

                //pos.x = -150 + (100 * k);
                //pos.y = 350 - (75 * j);

                //objCreate.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                //objCreate.GetComponent<RectTransform>().localPosition = pos;
                m_ButtonList.Add(objCreate);
            }
        }
    }
}
