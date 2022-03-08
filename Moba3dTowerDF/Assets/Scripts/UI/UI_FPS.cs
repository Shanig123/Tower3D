using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FPS : MonoBehaviour
{
    float m_fTime;
    // Start is called before the first frame update
    [SerializeField] UnityEngine.UI.Text m_text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_fTime += (Time.unscaledDeltaTime - m_fTime) * 0.1f;
    }
    private void OnGUI()
    {
        float fps = 1.0f / m_fTime;
        float fmsec = m_fTime * 1000f;
        string strtext = string.Format("{0:0.0} ms ({1:0.} fps)", fmsec, fps);
        m_text.text = strtext;
    }
}
