using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AbilityInfoBoard : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Text m_textScript;
    [SerializeField] private GameObject m_PlayerInfo;
    [SerializeField] private GameObject m_AbilityButton;
    [SerializeField] private List<Sprite> m_AbilityImages;
    [SerializeField] List<int> m_listUIAbility;
    int m_iSelectIndex;

    [SerializeField] int m_iHeight;
    [SerializeField] int m_iWidth;


    private bool m_bActive = false;
    // Start is called before the first frame update
    void Start()
    {
        m_listUIAbility = new List<int>();
    }

    private void OnEnable()
    {
        m_bActive = true;
        m_PlayerInfo.SetActive(false);
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
            return;

        int iAbiID = m_listUIAbility[m_iSelectIndex];

        if (Option_Manager.Instance.m_tOptiondata.bKor)
        {
            m_textScript.text = "모시깽이";
        }
        else
        {
            m_textScript.text = "mosiGGagi";
        }
    }

    void PrintSprite()
    {

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
                //버튼생성
                RectTransform rectTransform = m_AbilityButton.GetComponent<RectTransform>();

                //for (int j = 0; j < m_iHeight; ++j)
                //{
                //    for (int k= 0; k < m_iWidth; ++k)
                //    {
                        GameObject objCreate = GameObject.Instantiate(m_AbilityButton);
                        objCreate.transform.parent = this.transform;

                        objCreate.name = "Ability_" + templist[i];
                        objCreate.SetActive(true);
                        objCreate.GetComponent<UI_AbilityMainMenu>().m_iKey = templist[i];

                        objCreate.GetComponent<UnityEngine.UI.Image>().sprite = m_AbilityImages[templist[i]];

                        //int iIndex = j * m_iWidth + k;
                        Vector3 pos = new Vector3();
                        //if(i<m_iWidth)

                        //        pos.x = -220 + (100 * k);
                        //        pos.y = 240 - (75 * j);
                        objCreate.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                        objCreate.GetComponent<RectTransform>().localPosition = pos;
                //    }
                //}
            }
        }


    }
}
