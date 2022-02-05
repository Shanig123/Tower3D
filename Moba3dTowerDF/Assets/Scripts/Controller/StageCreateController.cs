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
}
