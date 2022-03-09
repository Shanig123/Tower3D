using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerObject : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }
    private void OnEnable()
    {
      
    }
    private void OnDisable()
    {
        transform.position = new Vector3(100, 100, 100);
        GetComponent<AudioSource>().clip = null;
    }
    //void 
    // Update is called once per frame
    void Update()
    {
        if(GetComponent<AudioSource>().clip == null)
            this.gameObject.SetActive(false);

        GetComponent<AudioSource>().volume = Option_Manager.Instance.m_tOptiondata.fMasterVol * Option_Manager.Instance.m_tOptiondata.fSfxVol;

        if (!GetComponent<AudioSource>().isPlaying)
        {
            this.gameObject.SetActive(false);
        }
    }
}
