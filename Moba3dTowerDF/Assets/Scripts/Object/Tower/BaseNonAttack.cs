using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNonAttack : TowerAI
{
    BaseNonAttack()
          : base() { }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
