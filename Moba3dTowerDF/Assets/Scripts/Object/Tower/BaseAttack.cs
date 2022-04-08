using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : TowerAI
{
    BaseAttack()
           : base() { }
    // Start is called before the first frame update

    [SerializeField] private bool m_bHitScan;
    [SerializeField] private int m_iAtkCount;
    [SerializeField] private int m_iAtkMaxCount;

    [SerializeField] private int m_iBulletCount;
    [SerializeField] private int m_iBulletMaxCount;

    protected override void Start()
    {
        if(DataEnum.eTowerType.End == m_tTowerInfo.eType)
            m_tTowerInfo.eType = DataEnum.eTowerType.Atk;
        base.Start();
        if((m_strBulletName == null) || (m_strBulletName == ""))
        {
            m_strBulletName = "Magic_Bullet_0";
        }
        //  m_tagStatus.strTowerName = gameObject.name;
        //EditorUtility
    }


    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void DoNoActiveState()
    {

    }
    protected override void DoActiveState()
    {
        CheckTarget();
    }
    protected override void DoDeadState()
    {

    }
    protected override void CheckDead()
    {

    }
}
