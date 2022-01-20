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
            return null;

        return m_dictShader[_strKeyName];
    }
    // Start is called before the first frame update
    private void Awake()
    {
        m_dictShader.Add("Rimlight_Shader", Shader.Find("Custom/Rimlight_Shader"));
        m_dictShader.Add("Default_Shader", Shader.Find("Custom/Default_Shader"));
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //if (!m_bCheckLoad)
        //    StartCoroutine(LoadShader());
    }

    IEnumerator LoadShader()
    {
        m_dictShader.Add("Rimlight_Shader", Shader.Find("Custom/Rimlight_Shader"));
        print("Add Shader");
        yield return null;

        m_bCheckLoad = true;

        yield return null;

    }
}
