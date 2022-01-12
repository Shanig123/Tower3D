using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Obj_Torche : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Light m_Light;
    [SerializeField] private float m_fTime;
    [SerializeField] private float m_fRange;
    private bool m_bInit = false;
    public float m_fRangeRatio;
    public float m_fRandorm;
    public float m_fLightChangeSpeed = 0.5f;
    public bool m_bOnOff = true;


    void Start()
    {

        m_Light = GetComponent<Light>();
        m_fRange = m_Light.range;
    
        if (m_fRangeRatio == 0)
        {
            m_fRangeRatio = 1;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_bInit)
            UpdateInit();

        if(m_bOnOff)
            Update_LightRange();
    }

    private void Update_LightRange()
    {
        m_fTime += Time.deltaTime * m_fRandorm* m_fLightChangeSpeed;
        m_Light.range = m_fRange + (Mathf.Sin(m_fTime) * m_fRangeRatio);
    }

    private void UpdateInit()
    {
        CheckDayNight();

        if (!m_bOnOff) // false�϶� ����.
        {
            //m_Light.isActiveAndEnabled = false;
            gameObject.SetActive(false);
            //m_Light.
            return;
        }
        System.Random random = new System.Random(Time.deltaTime.GetHashCode());
       
        m_fRandorm = GameObject.FindGameObjectWithTag("TotalController").GetComponent<DataController>().ExtractRandomNumberFromSeed();
        m_fRandorm *= m_fRangeRatio;
        //m_fRangeRatio += m_fRandorm;
        m_bInit = true;
    }
    private void CheckDayNight()
    {
        m_bOnOff = GameObject.FindGameObjectWithTag("TotalController").GetComponent<StageController>().Get_DayNight ?  false: true;
    }
}
