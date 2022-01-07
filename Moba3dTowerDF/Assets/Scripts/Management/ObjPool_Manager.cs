using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjPool_Manager : MonoBehaviour
{
    ObjPool_Manager()
        : base()
    {
        m_strPreSceneName = "NoScene";
        m_bCheckLoad = false;
    }
    #region Value
    private static ObjPool_Manager m_cInstance = null;


    Dictionary<string, Queue<GameObject>> m_ObjBulletPool;

    [SerializeField] private List<GameObject> m_ObjUpdate;
    [SerializeField] private readonly Vector3 m_vecInitPoolPosition = new Vector3(50, 20, 50);

    [SerializeField] private string m_strPreSceneName;
    [SerializeField] private bool m_bCheckLoad;
    [SerializeField] private int m_iMaxPoolSize;
    #endregion

    #region Property
    public static ObjPool_Manager Instance
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
        m_ObjBulletPool = new Dictionary<string, Queue<GameObject>>();
        m_ObjUpdate = new List<GameObject>();

        m_iMaxPoolSize = 1;
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
            if (!m_bCheckLoad)
                MainGameDataLoad();
        }
        else
            m_bCheckLoad = false;

        m_strPreSceneName = SceneManager.GetActiveScene().name;
    }

    #region PoolDataLoad
    void MainGameDataLoad()
    {
        StartCoroutine( BulletLoad());
    }

    IEnumerator BulletLoad()
    {
        Queue<GameObject> queueObjs = new Queue<GameObject>();
        for(int i=0; i< m_iMaxPoolSize; ++i)
        {
            GameObject createObject = Instantiate(Resource_Manager.Instance.m_dictPrefabs["Bullets"]["Empty_Bullet"].objPrefabs, m_vecInitPoolPosition, Quaternion.identity);
            createObject.name = createObject.name + "_" + i;
            queueObjs.Enqueue(createObject);
            yield return null;
        }
        m_ObjBulletPool.Add("Empty_Bullet", queueObjs);
        m_bCheckLoad = true;
        yield return null;
    }
    #endregion


    public GameObject Get_ObjPool(Vector3 _vCreatePos, in DataStruct.tagBulletStatus _tagBulletStat)
    {
       
        if(m_ObjBulletPool.ContainsKey(_tagBulletStat.strObjTagName))
        {
            if(m_ObjBulletPool[_tagBulletStat.strObjTagName].Count > 0)
            {
                m_ObjBulletPool[_tagBulletStat.strObjTagName].Peek().transform.position = _vCreatePos;
                m_ObjBulletPool[_tagBulletStat.strObjTagName].Peek().SetActive(true);
                m_ObjBulletPool[_tagBulletStat.strObjTagName].Peek().GetComponent<BaseBullet>().Set_Data = _tagBulletStat;               
                return m_ObjBulletPool[_tagBulletStat.strObjTagName].Dequeue();
               
            }
            else
            {
                GameObject createObject = Resource_Manager.Instance.InstanceObj("Bullets", _tagBulletStat.strObjTagName, _vCreatePos);

                createObject.GetComponent<BaseBullet>().Set_Data = _tagBulletStat;
                createObject.name = createObject.name + "_" + m_iMaxPoolSize;
                ++m_iMaxPoolSize;
                createObject.SetActive(true);
                return createObject;
            }

        }
        return null;    
    }

    public void ReturnPool(ref GameObject _objReturn, string _strTagName)
    {
        // _objReturn.GetComponent<BaseObj>();
        //_objReturn.

        if(m_ObjBulletPool.ContainsKey(_strTagName))
        {
            _objReturn.transform.position = m_vecInitPoolPosition;
            m_ObjBulletPool[_strTagName].Enqueue(_objReturn);
        }
        else
        {
            Queue<GameObject> temp = new Queue<GameObject>();
            _objReturn.transform.position = m_vecInitPoolPosition;
            temp.Enqueue(_objReturn);
            m_ObjBulletPool.Add(_strTagName, temp);
        }
    }
}
