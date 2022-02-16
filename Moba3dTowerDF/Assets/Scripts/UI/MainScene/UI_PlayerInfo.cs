using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerInfo : MonoBehaviour
{
    int m_iLife;
    int m_iGold;
    int m_iWave = -1;
    [SerializeField] List<UnityEngine.UI.Text> texts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController pContoller = GameObject.FindGameObjectWithTag("TotalController").GetComponent<PlayerController>();
        if (pContoller != null)
        {
            if (m_iGold != pContoller.Get_Gold)
            {
                m_iGold = pContoller.Get_Gold;
                texts[1].text = "G " + m_iGold.ToString();
            }
            if (m_iLife != pContoller.Get_Life)
            {
                m_iLife = pContoller.Get_Life;
                texts[0].text = "L " + m_iLife.ToString();
            }
            
        }

        StageController sContoller = GameObject.FindGameObjectWithTag("TotalController").GetComponent<StageController>();
        if (sContoller != null)
        {
            if (m_iWave != sContoller.Get_Wave)
            {
                m_iWave = sContoller.Get_Wave;
                if(Option_Manager.Instance.m_tOptiondata.bKor)
                    texts[2].text = "W " + m_iWave.ToString();
                else
                    texts[2].text = "W "+m_iWave.ToString();
            }
        }
    }
}
