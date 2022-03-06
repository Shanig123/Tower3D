using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipController : MonoBehaviour
{

    public List<AudioClip> m_clipAmbientSouds;
    public List<AudioClip> m_clipSfxs;
    public List<AudioClip> m_UI_Sounds;

    public AudioSource m_audioSource;
    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = this.gameObject.AddComponent<AudioSource>();
    }
    public void Play_AudioClip(DataEnum.eClip _clip, int _iIdx)
    {
        if (_clip == DataEnum.eClip.Ambi)
        {
            if(m_clipAmbientSouds.Count > _iIdx)
                m_audioSource.PlayOneShot(m_clipAmbientSouds[_iIdx]);
        }
        else if (_clip == DataEnum.eClip.Sfx)
        {
            if (m_clipSfxs.Count > _iIdx)
                m_audioSource.PlayOneShot(m_clipSfxs[_iIdx]);

        }
        else if (_clip == DataEnum.eClip.UI)
        {
            if (m_UI_Sounds.Count > _iIdx)
                m_audioSource.PlayOneShot(m_UI_Sounds[_iIdx]);
        }
        else if (_clip == DataEnum.eClip.Bgm)
        {
           // m_audioSource.PlayOneShot(m_clipAmbientSouds[iIdx]);
        }
        else
            return;
    }
    public void Play_AudioClip(DataEnum.eClip _clip, int _iIdx, AudioSource _audioSource)
    {
        if (_clip == DataEnum.eClip.Ambi)
        {
            if (m_clipAmbientSouds.Count > _iIdx)
                _audioSource.PlayOneShot(m_clipAmbientSouds[_iIdx]);
        }
        else if (_clip == DataEnum.eClip.Sfx)
        {
            if (m_clipSfxs.Count > _iIdx)
                _audioSource.PlayOneShot(m_clipSfxs[_iIdx]);

        }
        else if (_clip == DataEnum.eClip.UI)
        {
            if (m_UI_Sounds.Count > _iIdx)
                _audioSource.PlayOneShot(m_UI_Sounds[_iIdx]);
        }
        else if (_clip == DataEnum.eClip.Bgm)
        {
            // m_audioSource.PlayOneShot(m_clipAmbientSouds[iIdx]);
        }
        else
            return;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
