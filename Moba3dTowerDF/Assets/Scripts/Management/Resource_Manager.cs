using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class Resource_Manager : MonoBehaviour
{
    Resource_Manager()
        : base()
    {
        m_iMaxLoadCount = 0;
        m_dictPrefabs = new Dictionary<string, Dictionary<string, DataStruct.tagPrefab>>();
        m_dictTest = new Dictionary<string, Dictionary<int, int>>();
        m_bCheckLoadResource = false;
        m_fCheckLoadTime = 0;
        m_iLoadCount = 0;
    }

    #region Value
    private static Resource_Manager m_cInstance = null;
    public GameObject[] m_arrPrefabs;
    public Dictionary<string, Dictionary<string, DataStruct.tagPrefab>> m_dictPrefabs;
    public Dictionary<string, Dictionary<int, int>> m_dictTest;
    public List<string> m_listKey;

    [SerializeField] public int m_iMaxLoadCount;
    [SerializeField] public int m_iLoadCount;

    [SerializeField] private bool m_bCheckLoadResource;
    [SerializeField] private float m_fCheckLoadTime;
   
    #endregion
    #region Property
    public static Resource_Manager Instance
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
    public bool Get_CheckLoad
    {
        get
        {
            return m_bCheckLoadResource;
        }
    }
    #endregion
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

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
      //  cInstance = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Object/").Length;
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Nature/").Length;
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Default/").Length;
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Bullerts/").Length;
       
        Loading();
    }

    // Update is called once per frame
    void Update()
    {
        if ((m_iMaxLoadCount < m_iLoadCount) && (!m_bCheckLoadResource))
        {
            //UnityEditor.EditorApplication.isPaused = true;
            m_bCheckLoadResource = true;
        }
        if ((SceneManager.GetActiveScene().name == "FirstLoadScene") && m_bCheckLoadResource)
            EndLoad();
    }

    void EndLoad()
    {
        if (m_bCheckLoadResource)
            m_fCheckLoadTime += Time.deltaTime;

        if (m_bCheckLoadResource && (m_fCheckLoadTime > 5.0f) && (SceneManager.GetActiveScene().name == "FirstLoadScene"))
        {
            m_fCheckLoadTime = 0;
            SceneManager.LoadScene("MainUIScene");
        }
    }

    /*IEnumerator*/
    void Loading()
    {
        //StartCoroutine(LoadFileData());

        LoadFileData();

        StartCoroutine(LoadPrefabs("Object"));
        StartCoroutine(LoadPrefabs("Nature"));
        StartCoroutine(LoadPrefabs("Default"));
        StartCoroutine(LoadPrefabs("Bullets"));
    }

    public void SaveData()
    {
        StartCoroutine(SaveFileData());
    }

    IEnumerator LoadPrefabs(string strPathName)
    {
        Dictionary<string, DataStruct.tagPrefab> dictLoadPrefabs = new Dictionary<string, DataStruct.tagPrefab>();
  
        GameObject[] arrTempPrefabs;
        //= new GameObject[Resources.LoadAll<GameObject>("Prefabs/" + strPathName + "/").Length];
        arrTempPrefabs = Resources.LoadAll<GameObject>("Prefabs/"+ strPathName + "/");
        int iLoadCount = 0;

        if (arrTempPrefabs == null)
            Debug.Log("arrTempPrefabs is NULL.");
        for (int i = 0; i < arrTempPrefabs.Length; ++i)
        {
            DataStruct.tagPrefab temp = new DataStruct.tagPrefab();
            temp.objPrefabs = arrTempPrefabs[i];
            temp.strKeys = arrTempPrefabs[i].name;
            dictLoadPrefabs.Add(temp.strKeys, temp);

           // Debug.Log(temp.strKeys+"  /  " + strPathName);

            ++iLoadCount;
            yield return null;
        }
        m_dictPrefabs.Add(strPathName, dictLoadPrefabs);
        m_listKey.Add(strPathName);
        m_iLoadCount+=iLoadCount;

        Debug.Log("dictPrefabs     /   " + m_dictPrefabs.Count);
        Debug.Log("dictPrefabs   /  " + strPathName + " / " + m_dictPrefabs[strPathName].Count);

        yield return null;
    }

    /*IEnumerator*/void LoadFileData()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityEngine.UI.Text tLog = GameObject.Find("Log_File").GetComponent<UnityEngine.UI.Text>();
            tLog.text = "Load Android.";
            print("Load Android.");
            Load_AndroidFileData("wave.dat");
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            UnityEngine.UI.Text tLog = GameObject.Find("Log_File").GetComponent<UnityEngine.UI.Text>();
            tLog.text = "Load WindowsEditor.";
            print("Load WindowsEditor.");
            Load_EditorFileData("wave.dat");
        }
        else if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            UnityEngine.UI.Text tLog = GameObject.Find("Log_File").GetComponent<UnityEngine.UI.Text>();
            tLog.text = "Load WindowsPlayer.";
            print("Load WindowsPlayer.");
            Load_WinFileData("wave.dat");
        }
        //yield return null;
    }

    IEnumerator SaveFileData()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Save_AndroidFileData("wave.dat");
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Save_AndroidFileData("wave.dat");
            //Save_EditorFileData("wave.dat");
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Save_WinFileData("wave.dat");
        }
        yield return null;
    }

    private void Load_AndroidFileData(string _strFileName)
    {
        string strPath = Application.persistentDataPath + "/" +_strFileName;
        if(File.Exists(strPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(strPath,FileMode.Open);
            DataStruct.tagGameData tData;
            try
            {
                print("try");
                tData = (DataStruct.tagGameData)(bf.Deserialize(fs)); // 여기서문제 발생
                print("Deserialize");
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                print("catch");
                print("Err : " + e.Message);
                throw;
            }
            finally
            {
                print("finally");
                fs.Close();
                print("Stram close");
                Save_EditorFileData(_strFileName);
            }
            Game_Manager.Instance.m_tGameData = tData;
            UnityEngine.UI.Text tLog = GameObject.Find("Log_File").GetComponent<UnityEngine.UI.Text>();
            tLog.text = "Load Complete.";
            print("Load Complete.");
        }
        else
        {
            UnityEngine.UI.Text tLog = GameObject.Find("Log_File").GetComponent<UnityEngine.UI.Text>();
            tLog.text = "Err_File is Not Found.";
            print("Err_File is Not Found.");
        }
    }
    private void Save_AndroidFileData(string _strFileName)
    {
        string strPath = Application.persistentDataPath + "/" + _strFileName;
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(strPath);

        DataStruct.tagGameData tData= new DataStruct.tagGameData();

        tData.iBestWave = Game_Manager.Instance.m_tGameData.iBestWave;
        tData.iUnLockLevel = Game_Manager.Instance.m_tGameData.iUnLockLevel;

        bf.Serialize(fs, tData);
        fs.Close();
    }

    private void Load_EditorFileData(string _strFileName)
    {
         string strPath = Application.persistentDataPath + "/" + _strFileName;
        if (File.Exists(strPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(strPath, FileMode.Open);
            DataStruct.tagGameData tData;
            try
            {
                print("try");
                tData=(DataStruct.tagGameData)(bf.Deserialize(fs)); // 여기서문제 발생
                print("Deserialize");
            }
            catch(System.Runtime.Serialization.SerializationException e)
            {
                print("catch");
                print("Err : " + e.Message);
                throw;
            }
            finally
            {
                print("finally");
                fs.Close();
                print("Stram close");
                Save_EditorFileData(_strFileName);
            }
     
            //fs.Close();
            //print("Stram close");

            Game_Manager.Instance.m_tGameData = tData;
            UnityEngine.UI.Text tLog = GameObject.Find("Log_File").GetComponent<UnityEngine.UI.Text>();
            tLog.text = "Load Complete.";
            print("Load Complete.");
        }
        else
        {
            UnityEngine.UI.Text tLog = GameObject.Find("Log_File").GetComponent<UnityEngine.UI.Text>();
            tLog.text = "Err_File is Not Found.";
            print("Err_File is Not Found.");
        }

    }

    private void Save_EditorFileData(string _strFileName)
    {
        print("Save");

        string strPath = Application.persistentDataPath + "/" + _strFileName;
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(strPath);
        print("fs open");
        DataStruct.tagGameData tData = new DataStruct.tagGameData();

        tData.iBestWave = Game_Manager.Instance.m_tGameData.iBestWave;
        tData.iUnLockLevel = Game_Manager.Instance.m_tGameData.iUnLockLevel;
        print("write");

        try
        {
            print("try");
            bf.Serialize(fs, tData);
            print("Serialize");
        }
        catch (System.Runtime.Serialization.SerializationException e)
        {
            print("catch");
            print("Err : " + e.Message);
            throw;
        }
        finally
        {
            print("finally");
            fs.Close();
            print("write fs close");

        }


    }

    private void Load_WinFileData(string _strFileName)
    {

    }

    private void Save_WinFileData(string _strFileName)
    {

    }

    public GameObject InstanceObj(string _strCategory, string _strTagName, Vector3 _vCreatePos)
    {
        if (m_dictPrefabs.ContainsKey(_strCategory))
        {
            if (m_dictPrefabs[_strCategory].ContainsKey(_strTagName))
            {
                return Instantiate(m_dictPrefabs[_strCategory][_strTagName].objPrefabs, _vCreatePos, Quaternion.identity);
            }
        }
        return null;
    }
}
