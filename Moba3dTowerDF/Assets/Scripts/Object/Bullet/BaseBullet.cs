using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : BaseObj
{
    #region Values

    public DataStruct.tagBulletStatus m_tagStatus;

    [SerializeField] protected GameObject m_objTargetMob;
    [SerializeField] protected int m_iTargetID;

    [SerializeField] protected DataEnum.eState m_eNextState = DataEnum.eState.End;
    [SerializeField] protected DataEnum.eState m_eCurState = DataEnum.eState.End;

    #endregion

    #region Property
    public DataEnum.eState GetState
    {
        get
        {
            return m_eCurState;
        }
    }

    public string GetTagName
    {
        get
        {
            return m_tagStatus.strObjTagName;
        }

    }
    public DataStruct.tagBulletStatus Set_Data
    {
        set
        {
            m_tagStatus = value;
        }
    }

    public DataEnum.eState SetState
    {
        set
        {
            m_eNextState = value;
        }
    }

    #endregion
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Shader RimShader = GameObject.FindWithTag("TotalController").GetComponent<ShaderController>().Get_Shader("Rimlight_Shader");
        //Shader defaultShader = Shader.Find("Custom/Default_Shader");
        if (RimShader == null)
        {
            print("defaultShader null");
            return;
        }
        GetComponentInChildren<Renderer>().material.shader = RimShader;
        GetComponentInChildren<Renderer>().material.SetFloat("_Pow", 1.0f);
        GetComponentInChildren<Renderer>().material.SetColor("_RimCol", new Color(0, 1, 0));
        GetComponentInChildren<Renderer>().material.SetFloat("_Holo", 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
