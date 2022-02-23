using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Manager : MonoBehaviour
{
    Object_Manager()
        :base()
    {
        m_strPreSceneName = "NoScene";
    }
    #region Value
    private static Object_Manager m_cInstance = null;

    
    public Dictionary<string, List<GameObject>> m_dictClone_Object;
    public List<GameObject> m_listObject;
    //public DataStruct.tagStageInfo m_tStageInfo;

    [SerializeField] private GameObject m_objAlphaBlock;
    [SerializeField] private int width = 6;
    [SerializeField] private int height = 6;

    [SerializeField] private string m_strPreSceneName;

    [SerializeField] private StageController m_StageController;
    #endregion
    #region Property
    public static Object_Manager Instance
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
        //m_dictClone_Object.Clear();
        //m_listObject.Clear();
    }

    #region Awake
    void Awake()
    {
        if (null == m_cInstance)
        {
            m_cInstance = this;
           // Debug.Log("Instance ObjMgr");
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
      //  m_strPreSceneName = SceneManager.GetActiveScene().name;

        m_dictClone_Object = new Dictionary<string, List<GameObject>>();
//#if UNITY_EDITOR
//        SceneCheange();
//#endif


    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != m_strPreSceneName && Resource_Manager.Instance.Get_CheckLoad)
        {
         //   Debug.Log(Resource_Manager.Instance.Get_CheckLoad);
            SceneCheange();
        }
    }

    private void SceneCheange()
    {
        Debug.Log("Resource_Manager.Instance.Get_CheckLoad      /     " + Resource_Manager.Instance.Get_CheckLoad);
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            Debug.Log("SceneChange");
           
            m_StageController = GameObject.FindWithTag("TotalController").GetComponent<StageController>();
           if(DataEnum.eDifficulty.End == m_StageController.Get_Difficulty)
            {
               // pauseMenuCanvas.SetActive(true);

                m_StageController.Set_Difficulty = DataEnum.eDifficulty.Easy;
            }

            InstanceObjects();

        }
        else
        {
            if(Game_Manager.Instance)
                Game_Manager.Instance.m_tStageInfo.eDifficulty = DataEnum.eDifficulty.End;
            ClearInstance();
        }

        m_strPreSceneName = SceneManager.GetActiveScene().name;
    }

    #region InstanceFunc

    private void InstanceObjects()
    {
        InstanceAlphaBlock();
        if (DataEnum.eDifficulty.Easy == m_StageController.Get_Difficulty)
            InstanceWaypointsType1();

        InstanceAwaitBox();
    }

    private List<GameObject> InstanceAlphaBlock()
    {
        List<GameObject> gameObjects = new List<GameObject>();

        int i = 0;

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Vector3 vCreatePos = new Vector3(x - 5.0f, -0.25f, y - 5.0f);
                GameObject createObject = Instantiate(Resource_Manager.Instance.m_dictPrefabs["Default"]["TriggerCube"].objPrefabs, vCreatePos, Quaternion.identity);
                createObject.name = createObject.name + "_" + i;
                gameObjects.Add(createObject);

                //테스트 위해 색상 설정
                float Color = ((float)i / 121.0f);
                gameObjects[i].GetComponent<Renderer>().material.SetVector("_Color", new Vector4(Color, 1, 1.0f - Color, 0));


                if (i == 113)
                    InstanceCreateZone(vCreatePos);

                ++i;
            }
        }

        m_dictClone_Object.Add("AlphaBlock", gameObjects);
        return gameObjects;
    }

    private List<GameObject> InstanceMoster()
    {
        List<GameObject> gameObjects = new List<GameObject>();

        for (int y = 0; y < 5; ++y)
        {
            for (int x = 0; x < 5; ++x)
            {
                Quaternion temp = Quaternion.LookRotation(new Vector3(0,0,-1));
                GameObject createObj = Instantiate(Resource_Manager.Instance.m_dictPrefabs["Wave_Monster"]["TestMob00"].objPrefabs, new Vector3(x + 50, 0.5f, y), temp);

                createObj.GetComponent<BaseObj>().m_strPrefabName = "TestMob00";
                gameObjects.Add(createObj);
            }
        }
        m_dictClone_Object.Add("Monster", gameObjects);
        return gameObjects;
    }

    public GameObject InstanceObject(Vector3 _vCreatePos, string _strCloneKeyName, string _strPrefabsKeyName0, string _strPrefabsKeyName1)
    {
        if (m_dictClone_Object.ContainsKey(_strCloneKeyName))
        {

            Quaternion temp = Quaternion.LookRotation(new Vector3(0, 0, -1));
            GameObject createObj = Instantiate(Resource_Manager.Instance.m_dictPrefabs[_strPrefabsKeyName0][_strPrefabsKeyName1].objPrefabs, _vCreatePos, temp);

            createObj.GetComponent<BaseObj>().m_strPrefabName = _strPrefabsKeyName1;

            m_dictClone_Object[_strCloneKeyName].Add(createObj);

            //float Color = ((float)10 / 121.0f);
            //m_dictClone_Object["Waypoints"][m_dictClone_Object["Waypoints"].Count].GetComponent<Renderer>().material.SetVector("_Color", new Vector4(Color, 0, 0, 0));
            return createObj;
        }
        else
        {
            List<GameObject> gameObjects = new List<GameObject>();
            Quaternion temp = Quaternion.LookRotation(new Vector3(0, 0, -1));
            GameObject createObj = Instantiate(Resource_Manager.Instance.m_dictPrefabs[_strPrefabsKeyName0][_strPrefabsKeyName1].objPrefabs, _vCreatePos, temp);

            createObj.GetComponent<BaseObj>().m_strPrefabName = _strPrefabsKeyName1;
            gameObjects.Add(createObj);

            m_dictClone_Object.Add(_strCloneKeyName, gameObjects);
            return createObj;
        }
    }
    public GameObject InstanceObject(Vector3 _vCreatePos, string _strCloneKeyName, string _strPrefabsKeyName0, string _strPrefabsKeyName1,  int _iIndex)
    {
        if (m_dictClone_Object.ContainsKey(_strCloneKeyName))
        {

            Quaternion temp = Quaternion.LookRotation(new Vector3(0, 0, -1));
            GameObject createObj = Instantiate(Resource_Manager.Instance.m_dictPrefabs[_strPrefabsKeyName0][_strPrefabsKeyName1].objPrefabs, _vCreatePos, temp);
            m_dictClone_Object[_strCloneKeyName].Add(createObj);

            //float Color = ((float)10 / 121.0f);
            //m_dictClone_Object["Waypoints"][m_dictClone_Object["Waypoints"].Count].GetComponent<Renderer>().material.SetVector("_Color", new Vector4(Color, 0, 0, 0));
            return createObj;
        }
        else
        {
            if (!(m_dictClone_Object[_strCloneKeyName].Count > _iIndex))
                return null;
            
            Quaternion temp = Quaternion.LookRotation(new Vector3(0, 0, -1));
     
            GameObject createObj = Instantiate(Resource_Manager.Instance.m_dictPrefabs[_strPrefabsKeyName0][_strPrefabsKeyName1].objPrefabs, _vCreatePos, temp);

            m_dictClone_Object[_strCloneKeyName][_iIndex] = createObj;
   

            return createObj;
        }
    }
    public GameObject InstanceObject(Vector3 _vCreatePos, string _strPrefabsKeyName0, string _strPrefabsKeyName1)
    {
        Quaternion temp = Quaternion.LookRotation(new Vector3(0, 0, -1));
        GameObject createObj = Instantiate(
            Resource_Manager.
            Instance.m_dictPrefabs[_strPrefabsKeyName0][_strPrefabsKeyName1].objPrefabs, _vCreatePos, temp
            );
        return createObj;
    }
    private List<GameObject> InstanceCreateZone(Vector3 vPos)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        gameObjects.Add(Instantiate(Resource_Manager.Instance.m_dictPrefabs["Default"]["TriggerCube"].objPrefabs, vPos, Quaternion.identity));

        //테스트 위해 색상 설정
        gameObjects[0].GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0, 1, 0, 0.5f));

        m_dictClone_Object.Add("CreateZone", gameObjects);
        InstanceWaypointZone(vPos);
        return gameObjects;
    }

    private void InstanceWaypointZone(Vector3 vPos)
    {
        if(m_dictClone_Object.ContainsKey("Waypoints"))
        {
            GameObject objCreate = Instantiate(Resource_Manager.Instance.m_dictPrefabs["Board"]["WayPoint"].objPrefabs, vPos, Quaternion.identity);
            objCreate.layer = 0;
            m_dictClone_Object["Waypoints"].Add(objCreate);

            //float Color = ((float)10 / 121.0f);
            //m_dictClone_Object["Waypoints"][m_dictClone_Object["Waypoints"].Count].GetComponent<Renderer>().material.SetVector("_Color", new Vector4(Color, 0, 0, 0));
        }
        else
        {
            List<GameObject> gameObjects = new List<GameObject>();

            gameObjects.Add(Instantiate(Resource_Manager.Instance.m_dictPrefabs["Board"]["WayPoint"].objPrefabs, vPos, Quaternion.identity));

            //테스트 위해 색상 설정
            float Color = ((float)10 / 121.0f);
            gameObjects[0].GetComponentInChildren<Renderer>().material.SetVector("_Color", new Vector4(Color, 1, 1.0f - Color, 0));

            m_dictClone_Object.Add("Waypoints", gameObjects);
        }
    }

    private void InstanceWaypointsType1()
    {
        //113 , 3, 0 ,  33 , (36) , 43 , 10 , 7 ,117 , 120 , 87 , 77 ,110
        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][3].GetComponent<Transform>().position);
        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][0].GetComponent<Transform>().position);
        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][33].GetComponent<Transform>().position);

        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][43].GetComponent<Transform>().position);
        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][10].GetComponent<Transform>().position);
        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][7].GetComponent<Transform>().position);

        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][117].GetComponent<Transform>().position);
        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][120].GetComponent<Transform>().position);
        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][87].GetComponent<Transform>().position);

        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][77].GetComponent<Transform>().position);
        InstanceWaypointZone(m_dictClone_Object["AlphaBlock"][110].GetComponent<Transform>().position);



        //WayPointsRenderOnOff(false);
        WayPointsColor(new Vector4(0, 1, 0, 0.1f));
        ModifyWayPointsLook();

        //foreach (GameObject iter in m_dictClone_Object["AlphaBlock"])
        //{
        //    iter.GetComponent<Renderer>().enabled = false;
        //}
        m_dictClone_Object["Waypoints"][m_dictClone_Object["Waypoints"].Count - 1].GetComponentInChildren<Renderer>().material.SetVector("_Color", new Vector4(1, 0, 0, 0.5f));
        m_dictClone_Object["Waypoints"][m_dictClone_Object["Waypoints"].Count - 1].GetComponentInChildren<Renderer>().enabled = true;
        AlphaBlockRenderOnOff(false);

        NodeSetType1();
    }
    private void NodeSetType1()
    {
        int[] iArr = { -4, 3, -4 ,
             -1, 2, -1, 3, -1, 2, -1,
             -1, 2, -1, 3, -1, 2, -1,
             -11,
             3, -1, 3, -1 , 3,
             3, -1, 3, -1 , 3,
             3, -1, 3, -1 , 3,
             -11,
             -1, 2, -1, 3, -1, 2, -1,
             -1, 2, -1, 3, -1, 2, -1,
             -1, 2, -1, 3, -4
        };

        int iter = 0;

        for(int i =0; i< iArr.Length; ++i)
        {
            if(iArr[i]>0)
            {
                for (int j = 0; j< iArr[i];++j)
                {
                    ++iter;
                }
            }
            else
            {
                for (int j = 0; j < (-iArr[i]); ++j)
                {
                    //m_dictClone_Object["AlphaBlock"][iter].layer = (1 << LayerMask.NameToLayer("NoCollision"));
                    m_dictClone_Object["AlphaBlock"][iter].layer = 0;
                    ++iter;
                }
            }
        }
        
    }

    private void InstanceWaypointsType2()
    {

        NodeSetType2();
    }
    private void NodeSetType2()
    {

    }
    private void InstanceAwaitBox()
    {
        List<GameObject> gameObjects = new List<GameObject>();
        const int iBoxCreateMax = GConst.BaseValue.iAwaitBoxMax;

        for (int x = 0; x < iBoxCreateMax; ++x)
        {
            float fXPos= -9f;
            float fZPos = x-5f;
            if (x >= (iBoxCreateMax >> 1))
            {
                fXPos += 1.0f;
                fZPos -= (iBoxCreateMax >> 1);
            }
            Vector3 vCreatePos = new Vector3(fXPos, 0, fZPos);
            GameObject createObject = Instantiate(Resource_Manager.Instance.m_dictPrefabs["Object"]["AwaitListBox"].objPrefabs, vCreatePos, Quaternion.identity);
            createObject.name = createObject.name + "_" + x;
            gameObjects.Add(createObject);
        }

        m_dictClone_Object.Add("Box", gameObjects);
    }


    private void ModifyWayPointsLook()
    {
        int iWayPointCount = m_dictClone_Object["Waypoints"].Count;

        for(int i=0; i<iWayPointCount;++i)
        {
            if (iWayPointCount - 1 == i)
                continue;
            m_dictClone_Object["Waypoints"][i].transform.LookAt(m_dictClone_Object["Waypoints"][i + 1].transform);
        }
    }
    #endregion

    private void ClearInstance()
    {
        m_dictClone_Object.Clear();
        //m_dictClone_Object.
    }

    public void AlphaBlockRenderOnOff(bool _bOnOff)
    {
        foreach (GameObject iter in m_dictClone_Object["AlphaBlock"])
        {
            iter.GetComponent<Renderer>().enabled = _bOnOff;
        }
    }

    public void WayPointsRenderOnOff(bool _bOnOff)
    {
        if (!m_dictClone_Object.ContainsKey("Waypoints"))
            return;

        foreach (GameObject iter in m_dictClone_Object["Waypoints"])
        {
            iter.GetComponentInChildren<Renderer>().enabled = _bOnOff;
        }
        m_dictClone_Object["Waypoints"][m_dictClone_Object["Waypoints"].Count - 1].
            GetComponentInChildren<Renderer>().enabled = true;
    }
    public void WayPointsColor(Vector4 _vColor)
    {
        if (!m_dictClone_Object.ContainsKey("Waypoints"))
            return;
        foreach (GameObject iter in m_dictClone_Object["Waypoints"])
        {
            iter.GetComponentInChildren<Renderer>().material.SetVector("_Color", _vColor);
        }
        m_dictClone_Object["Waypoints"][m_dictClone_Object["Waypoints"].Count - 1].
            GetComponentInChildren<Renderer>().material.SetVector("_Color", new Vector4(1, 0, 0, 0.5f));
    }
 }
