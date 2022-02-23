using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_DirLight : MonoBehaviour
{
    // Start is called before the first frame update
    private bool m_bUpdateInit = false;
    [SerializeField] private Light m_Light;

    void Start()
    {
        m_Light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_bUpdateInit)
        {
            if(!GameObject.FindGameObjectWithTag("TotalController").GetComponent<StageCreateController>().Get_DayNight) //밤이라면
            {
                Quaternion rot = transform.rotation;
                rot.x = 0;
                transform.rotation = rot;
                m_Light.shadows = LightShadows.None;
            }
            int iShadow = Option_Manager.Instance.m_tOptiondata.iShadow;
            
            //if(iShadow < 0)
            //{
            //    m_Light.shadows = LightShadows.Hard;
            //}

            if (iShadow > 0)
            {
                m_Light.shadows = LightShadows.Soft;
            }
            else
            {
                m_Light.shadows = LightShadows.Hard;
            }


            m_bUpdateInit = true;
        }
    }
}
