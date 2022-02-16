using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DifficultyBoard : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Sprite> m_Arrsprite;
    public List<GameObject> m_listButton;
    void Start()
    {
        uint iUnlockLevl = Game_Manager.Instance.m_tGameData.iUnLockLevel;
        Game_Manager.Instance.m_tGameData.iUnLockLevel |= (uint)DataEnum.eDifficulty.Easy;
        Game_Manager.Instance.m_tGameData.iUnLockLevel |= (uint)DataEnum.eDifficulty.Normal;
        ChangeSprite();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        OnClick_Easy();
        ChangeSprite();
    }
    private void OnDisable()
    {
        if (Game_Manager.Instance)
            Game_Manager.Instance.m_tStageInfo.eDifficulty = DataEnum.eDifficulty.End;
    }

    private void ChangeSprite()
    {
        uint iUnlockLevl = Game_Manager.Instance.m_tGameData.iUnLockLevel;
        m_listButton[0].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[1];

        if ((iUnlockLevl & (int)DataEnum.eDifficulty.Normal) == (uint)DataEnum.eDifficulty.Normal)
            m_listButton[1].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[2];
        else
            m_listButton[1].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];

        if ((iUnlockLevl & (int)DataEnum.eDifficulty.Hard) == (uint)DataEnum.eDifficulty.Hard)
            m_listButton[2].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[3];
        else
            m_listButton[2].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];

        if ((iUnlockLevl & (int)DataEnum.eDifficulty.Infinite) == (uint)DataEnum.eDifficulty.Infinite)
            m_listButton[3].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[4];
        else
            m_listButton[3].GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
    }

    public void OnClick_Easy()
    {
        GameObject objDifficulty = gameObject.transform.Find("DifficultyText").gameObject;
        GameObject objScript = gameObject.transform.Find("DifficultyScriptTxt").gameObject;

        if (Option_Manager.Instance.m_tOptiondata.bKor)
        {
            objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "쉬움";
            objScript.GetComponent<UnityEngine.UI.Text>().text = 
                "웨이브제한 : 30 \n" +
                "목숨 : 30 \n" +
                "시작금액 : 30";
        }
        else
        {
            objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "EASY";
            objScript.GetComponent<UnityEngine.UI.Text>().text =
                "Last Wave : 30 \n" +
                "Life : 30 \n" +
                "Initial  \n" +
                "Amount : 30";
        }
        Game_Manager.Instance.m_tStageInfo.eDifficulty = DataEnum.eDifficulty.Easy;
    }
    public void OnClick_Normal()
    {
        uint iUnlockLevl = Game_Manager.Instance.m_tGameData.iUnLockLevel;
        if ((iUnlockLevl & (int)DataEnum.eDifficulty.Normal) == (uint)DataEnum.eDifficulty.Normal)
        {
            GameObject objDifficulty = gameObject.transform.Find("DifficultyText").gameObject;
            GameObject objScript = gameObject.transform.Find("DifficultyScriptTxt").gameObject;

            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "보통";
                objScript.GetComponent<UnityEngine.UI.Text>().text =
                    "웨이브제한 : 30 \n" +
                    "목숨 : 20 \n" +
                    "시작금액 : 20";
            }
            else
            {
                objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "NORMAL";
                objScript.GetComponent<UnityEngine.UI.Text>().text =
                    "Last Wave : 30 \n" +
                    "Life : 20 \n" +
                    "Initial\n" +
                    "Amount : 20";
            }
            Game_Manager.Instance.m_tStageInfo.eDifficulty = DataEnum.eDifficulty.Normal;
        }
    }
    public void OnClick_Hard()
    {
        uint iUnlockLevl = Game_Manager.Instance.m_tGameData.iUnLockLevel;
        if ((iUnlockLevl & (int)DataEnum.eDifficulty.Hard) == (uint)DataEnum.eDifficulty.Hard)
        {
            GameObject objDifficulty = gameObject.transform.Find("DifficultyText").gameObject;
            GameObject objScript = gameObject.transform.Find("DifficultyScriptTxt").gameObject;

            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "어려움";
                objScript.GetComponent<UnityEngine.UI.Text>().text =
                    "웨이브제한 : 40 \n" +
                    "목숨 : 20 \n" +
                    "시작금액 : 20 \n\n" +
                    "더 강해진 적 \n";
            }
            else
            {
                objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "HARD";
                objScript.GetComponent<UnityEngine.UI.Text>().text =
                    "Last Wave : 40 \n" +
                    "Life : 10 \n" +
                    "Initial \n" +
                    "Amount : 20 \n\n" +
                    "More hardest \n" +
                    "Enemy \n";
            }
            Game_Manager.Instance.m_tStageInfo.eDifficulty = DataEnum.eDifficulty.Hard;
        }
       

    }
    public void OnClick_Infinity()
    {
        uint iUnlockLevl = Game_Manager.Instance.m_tGameData.iUnLockLevel;
        if((iUnlockLevl & (int)DataEnum.eDifficulty.Infinite) == (uint)DataEnum.eDifficulty.Infinite)
        {
            GameObject objDifficulty = gameObject.transform.Find("DifficultyText").gameObject;
            GameObject objScript = gameObject.transform.Find("DifficultyScriptTxt").gameObject;

            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "무한";
                objScript.GetComponent<UnityEngine.UI.Text>().text =
                    "웨이브제한 : 없음 \n" +
                    "목숨 : 15 \n" +
                    "시작금액 : 20 \n";
            }
            else
            {
                objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "INFINITE";
                objScript.GetComponent<UnityEngine.UI.Text>().text =
                    "Last Wave : Nothing \n" +
                    "Life : 15 \n" +
                    "Initial\n" +
                    "Amount : 20";
            }
            Game_Manager.Instance.m_tStageInfo.eDifficulty = DataEnum.eDifficulty.Infinite;

        }

    }

}
