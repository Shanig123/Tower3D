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
    void Start()
    {
        RectTransform rectTransform = m_objItemButtonOrigin.GetComponent<RectTransform>();
        m_listItem = new List<GameObject>();
        RectTransform createTransform;
        for(int i=0;i<m_iHeight;++i)
        {
            for(int j=0; j<m_iWidth;++j)
            {
                var pos =  rectTransform.position;
                pos.x += (75*j);
                pos.y += (75*i);
                createTransform = rectTransform;
                createTransform.position = pos;
                GameObject objCreate = GameObject.Instantiate(m_objItemButtonOrigin, createTransform);
                objCreate.transform.parent = this.transform;
                m_listItem.Add(objCreate);
            }
        }
    }
    private void OnEnable()
    {
        
    }

    private void ChangeAllText()
    {
        if(Option_Manager.Instance)
        {
            if(Option_Manager.Instance.m_tOptiondata.bKor)
            {

            }
            else
            {

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
    }
}
