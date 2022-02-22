using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : BaseBullet
{
    MagicBullet()
        : base(){}
    // Start is called before the first frame update 
    public Color m_colorEffect;
    public Color m_colorGlow;
     protected override void Start()
    {
        base.Start();
        GetComponentInChildren<Renderer>().material.SetColor("_RimCol", new Color(0, 0, 1));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
