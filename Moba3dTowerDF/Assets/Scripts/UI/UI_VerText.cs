using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_VerText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text ="Ver : " + Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
