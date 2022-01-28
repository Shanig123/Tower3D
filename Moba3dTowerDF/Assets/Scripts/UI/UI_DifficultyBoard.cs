using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DifficultyBoard : MonoBehaviour
{
    // Start is called before the first frame update
  
    public List<Sprite> m_Arrsprite;
    void Start()
    {
        uint iUnlockLevl = Game_Manager.Instance.m_tGameData.iUnLockLevel;
        Game_Manager.Instance.m_tGameData.iUnLockLevel |= (uint)DataEnum.eDifficulty.Easy;
        Game_Manager.Instance.m_tGameData.iUnLockLevel |= (uint)DataEnum.eDifficulty.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        BoardInit();
    }
    private void OnDisable()
    {
        if (Game_Manager.Instance)
            Game_Manager.Instance.m_tStageInfo.eDifficulty = DataEnum.eDifficulty.End;
    }
    private void BoardInit()
    {
        OnClick_Easy();
        GameObject objEasy = gameObject.transform.Find("Easy").gameObject;
        GameObject objNormal = gameObject.transform.Find("Normal").gameObject;
        GameObject objHard = gameObject.transform.Find("Hard").gameObject;
        GameObject objInfifite = gameObject.transform.Find("Infinity").gameObject;

        uint iUnlockLevl = Game_Manager.Instance.m_tGameData.iUnLockLevel;


        objEasy.GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[1];

        if ((iUnlockLevl & (int)DataEnum.eDifficulty.Normal) == (uint)DataEnum.eDifficulty.Normal)
            objNormal.GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[2]; 
        else
            objNormal.GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];

        if ((iUnlockLevl & (int)DataEnum.eDifficulty.Hard) == (uint)DataEnum.eDifficulty.Hard)
            objHard.GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[3];
        else
            objHard.GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];

        if ((iUnlockLevl & (int)DataEnum.eDifficulty.Infinite) == (uint)DataEnum.eDifficulty.Infinite)
            objInfifite.GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[4];
        else
            objInfifite.GetComponent<UnityEngine.UI.Image>().sprite = m_Arrsprite[0];
    }

    public void OnClick_Easy()
    {
        GameObject objDifficulty = gameObject.transform.Find("DifficultyText").gameObject;
        GameObject objScript = gameObject.transform.Find("ScriptTxt").gameObject;

        if (Option_Manager.Instance.m_tOptiondata.bKor)
        {
            objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "����";
            objScript.GetComponent<UnityEngine.UI.Text>().text = 
                "���̺����� : 30 \n" +
                "��� : 30 \n" +
                "���۱ݾ� : 30";
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
            GameObject objScript = gameObject.transform.Find("ScriptTxt").gameObject;

            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "����";
                objScript.GetComponent<UnityEngine.UI.Text>().text =
                    "���̺����� : 30 \n" +
                    "��� : 20 \n" +
                    "���۱ݾ� : 20";
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
            GameObject objScript = gameObject.transform.Find("ScriptTxt").gameObject;

            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "�����";
                objScript.GetComponent<UnityEngine.UI.Text>().text =
                    "���̺����� : 40 \n" +
                    "��� : 20 \n" +
                    "���۱ݾ� : 20 \n\n" +
                    "�� ������ �� \n";
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
            GameObject objScript = gameObject.transform.Find("ScriptTxt").gameObject;

            if (Option_Manager.Instance.m_tOptiondata.bKor)
            {
                objDifficulty.GetComponent<UnityEngine.UI.Text>().text = "����";
                objScript.GetComponent<UnityEngine.UI.Text>().text =
                    "���̺����� : ���� \n" +
                    "��� : 15 \n" +
                    "���۱ݾ� : 20 \n";
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
