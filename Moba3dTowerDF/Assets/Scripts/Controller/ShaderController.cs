using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    ShaderController()
        :base()
    {
        m_dictShader = new Dictionary<string, Shader>();
    }
    Dictionary<string, Shader> m_dictShader;

    bool m_bCheckLoad = false;
    public Shader Get_Shader(string _strKeyName)
    {
        if (!m_dictShader.ContainsKey(_strKeyName))
        {
            return Shader.Find("Custom/" + _strKeyName);
        }
        Shader temp = null;
        temp = m_dictShader[_strKeyName];
        if( null == m_dictShader[_strKeyName])
        {
            return  Shader.Find("Custom/" + _strKeyName);
        }
        return temp;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        NotCorutineLoadShader("Rimlight_Shader");
        NotCorutineLoadShader("Default_Shader");
        NotCorutineLoadShader("RimlightNoAlpha_Shader");

        m_bCheckLoad = true;
    }
    void Start()
    {
        NotCorutineLoadShader("Rimlight_Shader");
        NotCorutineLoadShader("Default_Shader");
        NotCorutineLoadShader("RimlightNoAlpha_Shader");

        m_bCheckLoad = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!m_bCheckLoad)
        //    StartCoroutine(LoadShader());
    }

    IEnumerator LoadShader(string _strShaderName)
    {
        if (m_dictShader.ContainsKey(_strShaderName))
            yield return null;
        else
        { 
            m_dictShader.Add(_strShaderName, Shader.Find("Custom/"+ _strShaderName));


            yield return null;

            m_bCheckLoad = true;
        }
        yield return null;

    }

    void NotCorutineLoadShader(string _strShaderName)
    {
        if (m_dictShader.ContainsKey(_strShaderName))
            return;

        m_dictShader.Add(_strShaderName, Shader.Find("Custom/" + _strShaderName));

        return;
    }
}
