using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : TowerAI
{
    BaseAttack()
           : base() { }
    // Start is called before the first frame update
    protected override void Start()
    {
        m_tagStatus.eType = DataEnum.eTowerType.Atk;
        base.Start();
        //  m_tagStatus.strTowerName = gameObject.name;
        //EditorUtility
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
