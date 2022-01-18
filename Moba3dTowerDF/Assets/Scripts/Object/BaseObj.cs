using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObj : MonoBehaviour
{
    public bool m_bCheckDead = true;
    public bool m_bObjActiveOnOff = false;

    [SerializeField] protected bool m_bFirstInit = false;

    [SerializeField] protected float m_fReadyTimer;
    [SerializeField] protected float m_fReadyTimerMax;


    [SerializeField] protected Transform m_Transform;
    [SerializeField] protected Animator m_Ani;

    public string m_strPrefabName;

    protected virtual  void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Ani = GetComponent<Animator>();
    }
}
