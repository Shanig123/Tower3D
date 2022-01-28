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


    }
    private void OnEnable()
    {
        ChangeSprite();
        ChangeBanner();
        ChangeScript();
    }

    private void ChangeSprite()
    {
        if (Game_Manager.Instance)
        {
            bool[] arrUnlock = Game_Manager.Instance.m_tGameData.bArrUnlockAbility;
            if (arrUnlock.Length != m_listItem.Count)
                print("Size is not correct");
            for (int i = 0; i < m_listItem.Count; ++i)
            {
                if (arrUnlock[i])
                {
                    if (i == Game_Manager.Instance.m_tStageInfo.iStartAbility)
                        m_listItem[i].GetComponent<UnityEngine.UI.Image>().sprite = m_sprites[2];
                    else
                        m_listItem[i].GetComponent<UnityEngine.UI.Image>().sprite = m_sprites[1];
                }
                else
                    m_listItem[i].GetComponent<UnityEngine.UI.Image>().sprite = m_sprites[0];
            }

        }
    }
    private void ChangeBanner()
    {
        if (Option_Manager.Instance)
        {
            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                m_listText[0].GetComponent<UnityEngine.UI.Text>().text = "특 성 선 택"; 
            }
            else
            {
                m_listText[0].GetComponent<UnityEngine.UI.Text>().text = "ABILITY";
            }
        }
    }
    private void ChangeScript()
    {
        if(Option_Manager.Instance)
        {
            if(Option_Manager.Instance.m_tOptiondata.bKor)
            {
                m_listText[1].GetComponent<UnityEngine.UI.Text>().text =""+ Game_Manager.Instance.m_tStageInfo.iStartAbility;
            }
            else
            {
                m_listText[1].GetComponent<UnityEngine.UI.Text>().text = ""+Game_Manager.Instance.m_tStageInfo.iStartAbility;
            }
        }   
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickItem()
    {
        GameObject objClick = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if(Game_Manager.Instance)
        {
            if (objClick) 
            print(objClick.name);
            //int iKey = objClick.GetComponent<UI_AbilityMainMenu>().m_iKey;

            //Game_Manager.Instance.m_tStageInfo.iStartAbility = iKey;
            //ChangeSprite();
            //ChangeScript();
        }
  
    }
}
