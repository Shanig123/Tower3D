using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sound_Manager : MonoBehaviour
{

    #region Value
    private static Sound_Manager m_cInstance = null;

    public List<AudioClip> m_clipAmbientSouds;
    public List<AudioClip> m_clipSfxs;
    public List<AudioClip> m_UI_Sounds;

    public List<AudioClip> m_clipLoopBGMs;
    public List<AudioClip> m_clipShortBGMs;



    public AudioSource m_audioSource;
    [SerializeField]
    private AudioSource m_audioSource_BGM;
    [SerializeField]
    private AudioSource m_audioSource_Ambi;
    private int m_iBgmTrackNumber;
    [SerializeField] private string m_strPreSceneName;
    // private AudioSource m_audioSource_BGM;

    #endregion
    #region Property
    public static Sound_Manager Instance
    {
        get
        {
            if (null == m_cInstance)
            {
                return null;
            }
            return m_cInstance;
        }
    }

    #endregion
    private void OnDestroy()
    {

    }
    #region Awake
    void Awake()
    {
        if (null == m_cInstance)
        {
            m_cInstance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            /*
             * 씬 이동 후 이미 인스턴스가 되어 있다면 파괴한다.
             */
            Destroy(this.gameObject);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public void Play_AudioClip(DataEnum.eClip _clip, int _iIdx)
    {
        if (_clip == DataEnum.eClip.Ambi)
        {
            if (m_clipAmbientSouds.Count > _iIdx)
                m_audioSource.PlayOneShot(m_clipAmbientSouds[_iIdx]);
        }
        else if (_clip == DataEnum.eClip.Sfx)
        {
            if (m_clipSfxs.Count > _iIdx)
            {
                m_audioSource.volume = Option_Manager.Instance.m_tOptiondata.fMasterVol * Option_Manager.Instance.m_tOptiondata.fSfxVol;
                m_audioSource.PlayOneShot(m_clipSfxs[_iIdx]);
            }

        }
        else if (_clip == DataEnum.eClip.UI)
        {
            if (m_UI_Sounds.Count > _iIdx)
            {
                m_audioSource.volume = Option_Manager.Instance.m_tOptiondata.fMasterVol * (Option_Manager.Instance.m_tOptiondata.fSfxVol * 0.8f);
                m_audioSource.PlayOneShot(m_UI_Sounds[_iIdx]);
            }
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

    public void Play_Sfx(AudioSource _audioSource)
    {
        _audioSource.volume = Option_Manager.Instance.m_tOptiondata.fMasterVol * Option_Manager.Instance.m_tOptiondata.fSfxVol;
        _audioSource.PlayOneShot(_audioSource.clip);
    }
    public void Play_Sfx(AudioSource _audioSource, AudioClip _audioClip)
    {
        _audioSource.volume = Option_Manager.Instance.m_tOptiondata.fMasterVol * Option_Manager.Instance.m_tOptiondata.fSfxVol;
        _audioSource.PlayOneShot(_audioClip);
    }
  
    public void Play_UISfx(AudioSource _audioSource)
    {
        _audioSource.volume = Option_Manager.Instance.m_tOptiondata.fMasterVol * (Option_Manager.Instance.m_tOptiondata.fSfxVol * 0.8f);
        _audioSource.PlayOneShot(_audioSource.clip);
    }
    public void Play_UISfx(AudioSource _audioSource, AudioClip _audioClip)
    {
        _audioSource.volume = Option_Manager.Instance.m_tOptiondata.fMasterVol * (Option_Manager.Instance.m_tOptiondata.fSfxVol*0.8f);
        _audioSource.PlayOneShot(_audioClip);
    }
   
    public void Set_BgmVol()
    {
        m_audioSource_BGM.volume = Option_Manager.Instance.m_tOptiondata.fMasterVol * (Option_Manager.Instance.m_tOptiondata.fBgmVol);
        m_audioSource_Ambi.volume = Option_Manager.Instance.m_tOptiondata.fMasterVol * (Option_Manager.Instance.m_tOptiondata.fSfxVol*0.8f);
        //m_audioSource_Ambi.volume = Option_Manager.Instance.m_tOptiondata.fMasterVol * Option_Manager.Instance.m_tOptiondata.fBgmVol;
    }
    // Update is called once per frame
    void Update()
    {
        SceneCheange();
    }

    private void SceneCheange()
    {
        Set_BgmVol();

        if (SceneManager.GetActiveScene().name != m_strPreSceneName)
        {
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                MainScene_Bgm();
            }
            else
            {
                if (SceneManager.GetActiveScene().name == "MainMenuScene")
                {
                    MainMenuScene_Bgm();
                }
                else
                {
                    LoadScene_Bgm();                  
                }
            }
            m_strPreSceneName = SceneManager.GetActiveScene().name;
        }
       
    }


    private void MainScene_Bgm()
    {   //인게임 브금 재생
        m_iBgmTrackNumber = GameObject.FindGameObjectWithTag("TotalController").GetComponent<DataController>().ExtractRandomNumberFromSeed_NoCount(0, 6);
        m_audioSource_BGM.clip = m_clipLoopBGMs[m_iBgmTrackNumber];
        m_audioSource_BGM.Play();

        if (GameObject.FindGameObjectWithTag("TotalController").GetComponent<StageCreateController>().Get_DayNight)
        {
            m_audioSource_Ambi.clip = Sound_Manager.Instance.m_clipAmbientSouds[0];
            m_audioSource_Ambi.Play();
        }
        else
        {
            m_audioSource_Ambi.clip = Sound_Manager.Instance.m_clipAmbientSouds[2];
            m_audioSource_Ambi.Play();
        }

    }
    private void MainMenuScene_Bgm()
    {
        if (m_strPreSceneName == "MainScene")
        {
            m_iBgmTrackNumber = 7;
            m_audioSource_BGM.clip = m_clipLoopBGMs[m_iBgmTrackNumber];
            m_audioSource_BGM.Play();
        }
    }
    private void LoadScene_Bgm()
    {
        m_iBgmTrackNumber = 7;
        m_audioSource_BGM.clip = m_clipLoopBGMs[m_iBgmTrackNumber];
        m_audioSource_BGM.Play();
    }
}
