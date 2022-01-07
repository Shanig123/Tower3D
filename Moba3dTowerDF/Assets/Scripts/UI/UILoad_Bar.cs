using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoad_Bar : MonoBehaviour
{
    public RectTransform m_transform = null;

    public const float m_fAnchorX_max = 0.95f;
    public const float m_fAnchorX_min = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float fLoadRatio = (float)Resource_Manager.Instance.m_iLoadCount / Resource_Manager.Instance.m_iMaxLoadCount;

        m_transform.anchorMax = new Vector2((m_fAnchorX_max - m_fAnchorX_min)* fLoadRatio+ 0.05f, m_transform.anchorMax.y);
    }
}
