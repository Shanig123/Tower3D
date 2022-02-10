using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreateController : MonoBehaviour
{
    StageCreateController()
       : base()
    {
        m_bDayNight = false;
    }
    #region Value

    [SerializeField] private bool m_bDayNight;

    #endregion
    #region Property
    //true = day / false = night
    public bool Get_DayNight
    {
        get
        {
            return m_bDayNight;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //m_Object_Manager = GameObject.FindGameObjectWithTag("Management").GetComponent<Object_Manager>();

        int iDayNight = Option_Manager.Instance.m_tOptiondata.iDayNight;

        if (iDayNight < 0)
            m_bDayNight = false;
        else if (iDayNight > 0)
            m_bDayNight = true;
        else
        {
            int iSeed = GameObject.FindGameObjectWithTag("TotalController").GetComponent<DataController>().Get_Seed;
            System.Random rd = new System.Random(iSeed);
            m_bDayNight = rd.Next(0, 2) > 0 ? true : false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitInfo()
    {
        Init_PlayerInfo();
        Init_AbilityInfo();
    }

    void Init_PlayerInfo()
    {
      //  PlayerController playerController = GetComponent<PlayerController>();
        if (GetComponent<PlayerController>() == null)
            return;
       

        if (Game_Manager.Instance.m_tStageInfo.eDifficulty == DataEnum.eDifficulty.Easy)
        {
            GetComponent<PlayerController>().Add_Life(30);
            GetComponent<PlayerController>().Add_Gold(30);
        }
        else if (Game_Manager.Instance.m_tStageInfo.eDifficulty == DataEnum.eDifficulty.Normal)
        {
            GetComponent<PlayerController>().Add_Life(20);
            GetComponent<PlayerController>().Add_Gold(20);
        }
        else if (Game_Manager.Instance.m_tStageInfo.eDifficulty == DataEnum.eDifficulty.Hard)
        {
            GetComponent<PlayerController>().Add_Life(20);
            GetComponent<PlayerController>().Add_Gold(20);

        }
        else if (Game_Manager.Instance.m_tStageInfo.eDifficulty == DataEnum.eDifficulty.Infinite)
        {
            GetComponent<PlayerController>().Add_Life(15);
            GetComponent<PlayerController>().Add_Gold(20);
        }
        else
        {
            GetComponent<PlayerController>().Add_Life(30);
            GetComponent<PlayerController>().Add_Gold(30);
        }
    }

    void Init_AbilityInfo()
    {
        int istart = Game_Manager.Instance.m_tStageInfo.iStartAbility;
        Game_Manager.Instance.m_tStageInfo.iStartAbility = 0;
        if (istart > 0)
        {
            //AbilityController abilityController = GetComponent<AbilityController>();
            //if (abilityController != null)
            //{
            //    abilityController.
            //}
        }
    }
}
