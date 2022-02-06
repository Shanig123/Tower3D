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
        m_listScripts = new List<List<string>>();

        m_bCheckLoadResource = false;
        m_fCheckLoadTime = 0;
        m_iLoadCount = 0;
    }

    #region Value
    private static Resource_Manager m_cInstance = null;
    public GameObject[] m_arrPrefabs;
    public Dictionary<string, Dictionary<string, DataStruct.tagPrefab>> m_dictPrefabs;
    public Dictionary<string, Dictionary<int, int>> m_dictTest;
    //public Dictionary<string, List<string>> m_dictScripts;
    private List<List<string>> m_listScripts;

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

    public List<List<string>> Get_Scripts { get{ return m_listScripts; } }
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
        // m_dictPrefabs.Clear();
        // m_dictTest.Clear();
        
        Resources.UnloadUnusedAssets();
        
        //cInstance = null;

    }
    // Start is called before the first frame update
    void Start()
    {
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Object/").Length;
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Tower/").Length;
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Wave_Monster/").Length;
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Nature/").Length;
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Default/").Length;
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Bullerts/").Length;
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Board/").Length;
        m_iMaxLoadCount += Resources.LoadAll<GameObject>("Prefabs/Effect/").Length;

        

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
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    /*IEnumerator*/
    void Loading()
    {
        //StartCoroutine(LoadFileData());

        LoadScriptsFildData("KorUnLockScripts");
        LoadScriptsFildData("KorLockScripts");
        LoadScriptsFildData("EngUnLockScripts");
        LoadScriptsFildData("EngLockScripts");

        LoadFileData();

        StartCoroutine(LoadPrefabs("Object"));
        StartCoroutine(LoadPrefabs("Tower"));
        StartCoroutine(LoadPrefabs("Wave_Monster"));
        StartCoroutine(LoadPrefabs("Nature"));
        StartCoroutine(LoadPrefabs("Default"));
        StartCoroutine(LoadPrefabs("Bullets"));
        StartCoroutine(LoadPrefabs("Board"));
        StartCoroutine(LoadPrefabs("Effect"));
    }

    public void SaveData()
    {
        //StartCoroutine(SaveFileData());
        SaveFileData();
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

        //Debug.Log("dictPrefabs     /   " + m_dictPrefabs.Count);
        //Debug.Log("dictPrefabs   /  " + strPathName + " / " + m_dictPrefabs[strPathName].Count);

        yield return null;
    }

    /*IEnumerator*/void LoadFileData()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GFunc.Function.Print_Log("Load Android.");

            Load_AndroidFileData("wave.dat");
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            GFunc.Function.Print_Log("Load WindowsEditor.");

            Load_EditorFileData("wave.dat");
        }
        else if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            GFunc.Function.Print_Log("Load WindowsPlayer.");

            Load_WinFileData("wave.dat");
        }
        //yield return null;
    }

    private void Load_AndroidFileData(string _strFileName)
    {
        string strPath = Application.persistentDataPath + "/" +_strFileName;
        if(File.Exists(strPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(strPath,FileMode.Open);
            DataStruct.tagGameData tData = GFunc.Function.InitGameData();
            try
            {
                tData = (DataStruct.tagGameData)(bf.Deserialize(fs)); // 여기서문제 발생
                if (System.Runtime.InteropServices.Marshal.SizeOf(tData)
                    != System.Runtime.InteropServices.Marshal.SizeOf(Game_Manager.Instance.Get_DefaultGameData))
                {
                    tData = Game_Manager.Instance.Get_DefaultGameData;
                }
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                GFunc.Function.Print_Log("catch \n " + "Err : " + e.Message);
                tData = Game_Manager.Instance.Get_DefaultGameData;
                throw;
            }
            finally
            {
                fs.Close();
                Save_AndroidFileData(_strFileName);
            }
            print(tData.bArrUnlockAbility[0]);
            Game_Manager.Instance.m_tGameData = tData;

            GFunc.Function.Print_Log("Load Complete.");
        }
        else
        {
            GFunc.Function.Print_Log("Err_File is Not Found.");
            Game_Manager.Instance.m_tGameData = Game_Manager.Instance.Get_DefaultGameData;
            Save_AndroidFileData(_strFileName);
        }
    }
    private void Load_EditorFileData(string _strFileName)
    {
        string strPath = Application.persistentDataPath + "/" + _strFileName;
        GFunc.Function.Print_Log(strPath);
        if (File.Exists(strPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(strPath, FileMode.Open);
            DataStruct.tagGameData tData = GFunc.Function.InitGameData();
            try
            {
              
                tData =(DataStruct.tagGameData)(bf.Deserialize(fs)); // 여기서문제 발생
                if(System.Runtime.InteropServices.Marshal.SizeOf(tData) != System.Runtime.InteropServices.Marshal.SizeOf(Game_Manager.Instance.Get_DefaultGameData))
                {
                    tData = Game_Manager.Instance.Get_DefaultGameData;
                }
     
            }
            catch(System.Runtime.Serialization.SerializationException e)
            {
                GFunc.Function.Print_Log("catch \n " + "Err : " + e.Message);
                tData = Game_Manager.Instance.Get_DefaultGameData;
                throw;
            }
            finally
            {
                fs.Close();
                Save_EditorFileData(_strFileName);
            }

            Game_Manager.Instance.m_tGameData = tData;
        
            GFunc.Function.Print_Log("Load Complete.");
        }
        else
        {
            GFunc.Function.Print_Log("Err_File is Not Found.");
            Game_Manager.Instance.m_tGameData = Game_Manager.Instance.Get_DefaultGameData;
            Save_EditorFileData(_strFileName);
        }

    }
    private void Load_WinFileData(string _strFileName)
    {

    }

    private void LoadScriptsFildData(string _strFileName)
    { 
        TextAsset textAsset = Resources.Load<TextAsset>("TextScripts/" + _strFileName);
        StringReader stringReader = new StringReader(textAsset.text);
        if (stringReader == null)
            return;

        List<string> listScripts = new List<string>();
        string strRL =null;

        while((strRL=stringReader.ReadLine()) != null)
        {
            strRL = strRL.Replace("_", System.Environment.NewLine);
            listScripts.Add(strRL);
        }  

        m_listScripts.Add(listScripts);
   
    }

    void /*IEnumerator*/ SaveFileData()
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
        //yield return null;
    }

    private void Save_AndroidFileData(string _strFileName)
    {
        string strPath = Application.persistentDataPath + "/" + _strFileName;
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(strPath);

        DataStruct.tagGameData tData= GFunc.Function.InitGameData();
        tData = Game_Manager.Instance.m_tGameData;
        //tData.iBestWave = Game_Manager.Instance.m_tGameData.iBestWave;
        //tData.iUnLockLevel = Game_Manager.Instance.m_tGameData.iUnLockLevel;

        bf.Serialize(fs, tData);
        fs.Close();
    }
    private void Save_EditorFileData(string _strFileName)
    {
        GFunc.Function.Print_Log("Save.");

        string strPath = Application.persistentDataPath + "/" + _strFileName;
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(strPath);

        DataStruct.tagGameData tData = GFunc.Function.InitGameData();

        tData = Game_Manager.Instance.m_tGameData;

        try
        {
            bf.Serialize(fs, tData);
        }
        catch (System.Runtime.Serialization.SerializationException e)
        {
            GFunc.Function.Print_Log("catch \n " + "Err : " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }


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
