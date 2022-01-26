using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScreen_Effect : MonoBehaviour
{
    // Start is called before the first frame update
    string m_strNextSceneName = null;
    float m_fSceneStart_Time = 0;
    float m_fSceneEnd_Time = 0;

    bool m_bSceneStartEffect = false;
    bool m_bSceneEndEffect = false;

    public string Set_SceneEndWithEffect
    {
        set
        {
            m_strNextSceneName = value;
            m_bSceneEndEffect = true;
        }
    }
    public string Set_SceneEndWithNoEffect
    {
        set
        {
            m_strNextSceneName = value;
        }
    }

    void Start()
    {
        m_bSceneStartEffect = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bSceneStartEffect)
            SceneInEffect();
        if (m_bSceneEndEffect)
            SceneEndEffect();
        
    }

    void SceneInEffect()
    {
        m_fSceneStart_Time += Time.deltaTime;
    }

    void SceneEndEffect()
    {
        m_fSceneEnd_Time += Time.deltaTime;
    }
}
