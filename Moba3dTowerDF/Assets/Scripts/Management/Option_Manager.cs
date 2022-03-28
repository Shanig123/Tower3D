using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Option_Manager : MonoBehaviour
{
    Option_Manager()
        :base()
    {
        m_tDefaultData.bKor = true;
        m_tDefaultData.iDayNight = 1;
        m_tDefaultData.iShadow = 0;
        m_tDefaultData.fMasterVol = 0.5f;
        m_tDefaultData.fSfxVol = 1;
        m_tDefaultData.fBgmVol = 1;
        m_tOptiondata = m_tDefaultData;
    }
    #region Value
    private static Option_Manager m_cInstance = null;

    public DataStruct.tagOptionData m_tOptiondata;
    private DataStruct.tagOptionData m_tDefaultData;

    #endregion
    #region Property
    public static Option_Manager Instance
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

    public DataStruct.tagOptionData Get_DefaultData
    { get {return m_tDefaultData; } }
    #endregion

    private void OnDestroy()
    {
        SaveFileData();
    }

    #region Awake
    void Awake()
    {
        if (null == m_cInstance)
        {
            m_cInstance = this;
            LoadOptionData();
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
    
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LoadOptionData()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Load_AndroidOptionData("Option.dat");
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Load_EditorOptionData("Option.dat");
        }
         //yield return null;
    }
    private void Load_AndroidOptionData(string _strFileName)
    {
        string strPath = Application.persistentDataPath + "/" + _strFileName;
        if (File.Exists(strPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(strPath, FileMode.Open);
            DataStruct.tagOptionData tData;
            try
            {
                tData = (DataStruct.tagOptionData)(bf.Deserialize(fs)); // 여기서문제 발생
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                GFunc.Function.Print_Log("catch \n " + "Err : " + e.Message);
                tData = m_tDefaultData;
                throw;
            }
            finally
            {
                fs.Close();
                Save_EditorFileData(_strFileName);
            }
            m_tOptiondata = tData;

            GFunc.Function.Print_Log("Load Complete.");
        }
        else
        {
            m_tOptiondata = m_tDefaultData;
            GFunc.Function.Print_Log("Err_File is Not Found.");
        }
    }
    private void Load_EditorOptionData(string _strFileName)
    {
        string strPath = Application.persistentDataPath + "/" + _strFileName;
        if (File.Exists(strPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(strPath, FileMode.Open);
            DataStruct.tagOptionData tData;
            try
            {
                tData = (DataStruct.tagOptionData)(bf.Deserialize(fs)); // 여기서문제 발생
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                GFunc.Function.Print_Log("catch \n " + "Err : " + e.Message);
                tData = m_tDefaultData;
                throw;
            }
            finally
            {
                fs.Close();
                Save_EditorFileData(_strFileName);
            }

            m_tOptiondata = tData;

            GFunc.Function.Print_Log("Load Complete.");
        }
        else
        {
            m_tOptiondata = m_tDefaultData;
            GFunc.Function.Print_Log("Err_File is Not Found.");
        }

    }

    public void  SaveFileData()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Save_AndroidFileData("Option.dat");
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Save_AndroidFileData("Option.dat");
            //Save_EditorFileData("wave.dat");
        }
        return;
    }

    private void Save_AndroidFileData(string _strFileName)
    {
        string strPath = Application.persistentDataPath + "/" + _strFileName;
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(strPath);

        DataStruct.tagOptionData tData = new DataStruct.tagOptionData();

        tData = m_tOptiondata;

        bf.Serialize(fs, tData);
        fs.Close();
    }

    private void Save_EditorFileData(string _strFileName)
    {
        GFunc.Function.Print_Log("Save.");

        string strPath = Application.persistentDataPath + "/" + _strFileName;
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(strPath);

        DataStruct.tagOptionData tData = new DataStruct.tagOptionData();

        tData = m_tOptiondata;


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
}
