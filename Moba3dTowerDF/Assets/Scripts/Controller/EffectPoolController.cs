using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectPoolController : MonoBehaviour
{
    EffectPoolController()
             : base()
    {
        m_strPreSceneName = "NoScene";
        m_bCheckLoad = false;
    }
    #region Value

    Dictionary<string, Queue<GameObject>> m_ObjEffectPool;
    public Queue<GameObject> m_queue;
    [SerializeField] private readonly Vector3 m_vecInitPoolPosition = new Vector3(50, 20, 50);

    [SerializeField] private string m_strPreSceneName;
    [SerializeField] private bool m_bCheckLoad;
    [SerializeField] private int m_iMaxPoolSize;
    #endregion

    private void OnDestroy()
    {
        // m_ObjBulletPool.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_ObjEffectPool = new Dictionary<string, Queue<GameObject>>();
        m_queue = new Queue<GameObject>();
        m_iMaxPoolSize = 10;
      //  MainGameDataLoad();
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
        {
            m_ObjEffectPool.Clear();
            m_bCheckLoad = false;
        }

        m_strPreSceneName = SceneManager.GetActiveScene().name;
    }


    #region PoolDataLoad

    void MainGameDataLoad()
    {
        StartCoroutine(EffectLoad("CornBust"));
        StartCoroutine(EffectDeadLoad("MagicDead"));
       // StartCoroutine(EffectLoad("Puff_"));
    }

    IEnumerator EffectLoad(string strName)
    {
        Queue<GameObject> queueObjs = new Queue<GameObject>();
        for (int i = 0; i < m_iMaxPoolSize; ++i)
        {
            GameObject createObject = Instantiate(Resource_Manager.Instance.m_dictPrefabs["Effect"][strName].objPrefabs, m_vecInitPoolPosition, Quaternion.identity);
            createObject.name = createObject.name + "_" + i;
            queueObjs.Enqueue(createObject);
            yield return null;
        }
        m_ObjEffectPool.Add(strName, queueObjs);
        m_bCheckLoad = true;
        yield return null;
    }

    IEnumerator EffectDeadLoad(string strName)
    {
        Queue<GameObject> queueObjs = new Queue<GameObject>();
        for (int i = 0; i < m_iMaxPoolSize; ++i)
        {
            GameObject createObject = Resource_Manager.Instance.InstanceObj("Effect", strName, m_vecInitPoolPosition);
            if (createObject == null)
                createObject = Resource_Manager.Instance.InstanceObj("Effect_Dead", strName, m_vecInitPoolPosition);

            //GameObject createObject = Instantiate(Resource_Manager.Instance.m_dictPrefabs["Effect_Dead"][strName].objPrefabs, m_vecInitPoolPosition, Quaternion.identity);

            createObject.GetComponent<Base_Effect>().m_strPrefabName = strName;

            createObject.name = createObject.name + "_" + i;
            queueObjs.Enqueue(createObject);
            yield return null;
        }
        m_ObjEffectPool.Add(strName, queueObjs);
        m_bCheckLoad = true;
        yield return null;
    }

    #endregion


    public GameObject Get_ObjPool(Vector3 _vCreatePos, in string _strKeyName)
    {
        if (m_ObjEffectPool.ContainsKey(_strKeyName))
        {
            GFunc.Function.Print_Log("GetEffect_"+ _strKeyName +"_"+ m_ObjEffectPool[_strKeyName].Count);
            if (m_ObjEffectPool[_strKeyName].Count > 0)
            {
                if (m_ObjEffectPool[_strKeyName].Peek())
                {
                    m_ObjEffectPool[_strKeyName].Peek().transform.position = _vCreatePos;
                    GFunc.Function.Print_Log("Peekpos");
                    GFunc.Function.Print_Log(m_ObjEffectPool[_strKeyName].Peek().transform.position.x.ToString());
                    GFunc.Function.Print_Log(m_ObjEffectPool[_strKeyName].Peek().transform.position.y.ToString());
                    GFunc.Function.Print_Log(m_ObjEffectPool[_strKeyName].Peek().transform.position.z.ToString());
                    GFunc.Function.Print_Log("createpos");
                    GFunc.Function.Print_Log(_vCreatePos.x.ToString());
                    GFunc.Function.Print_Log(_vCreatePos.y.ToString());
                    GFunc.Function.Print_Log(_vCreatePos.z.ToString());
                    m_ObjEffectPool[_strKeyName].Peek().SetActive(true);
                    //  m_ObjEffectPool[_strKeyName].Peek().GetComponent<BaseBullet>().Set_Data = _tagBulletStat;
                    m_ObjEffectPool[_strKeyName].Peek().GetComponent<Base_Effect>().m_bIsOn = true;
                    // m_ObjEffectPool[_strKeyName].Peek().GetComponent<ParticleSystem>().Play();

                    return m_ObjEffectPool[_strKeyName].Dequeue();
                }
                else
                {
                    GameObject createObject = Resource_Manager.Instance.InstanceObj("Effect", _strKeyName, _vCreatePos);
                    if (createObject == null)
                        createObject = Resource_Manager.Instance.InstanceObj("Effect_Dead", _strKeyName, _vCreatePos);

                    createObject.GetComponent<Base_Effect>().m_strPrefabName = _strKeyName;
                    createObject.name = createObject.name + "_" + m_iMaxPoolSize;
                    ++m_iMaxPoolSize;
                    createObject.SetActive(true);
                    return createObject;
                }

            }
            else
            {
                GameObject createObject = Resource_Manager.Instance.InstanceObj("Effect", _strKeyName, _vCreatePos);
                if (createObject == null)
                    createObject = Resource_Manager.Instance.InstanceObj("Effect_Dead", _strKeyName, _vCreatePos);

                createObject.name = createObject.name + "_" + m_iMaxPoolSize;
                ++m_iMaxPoolSize;
                createObject.SetActive(true);
                return createObject;
            }

        }
        else
        {           
            GFunc.Function.Print_Log(_strKeyName + "NotFound");
            Queue<GameObject> queueObjs = new Queue<GameObject>();

            //GameObject createObject = Instantiate(Resource_Manager.Instance.m_dictPrefabs["Effect_Dead"][_strKeyName].objPrefabs, m_vecInitPoolPosition, Quaternion.identity);
            GameObject createObject = Resource_Manager.Instance.InstanceObj("Effect", _strKeyName, _vCreatePos);
            if (createObject == null)
                createObject = Resource_Manager.Instance.InstanceObj("Effect_Dead", _strKeyName, _vCreatePos);

            createObject.GetComponent<Base_Effect>().m_strPrefabName = _strKeyName;

            createObject.name = createObject.name + "_" + 0;
            queueObjs.Enqueue(createObject);
            m_ObjEffectPool.Add(_strKeyName, queueObjs);

            if (m_ObjEffectPool[_strKeyName].Peek())
            {
                m_ObjEffectPool[_strKeyName].Peek().transform.position = _vCreatePos;
                m_ObjEffectPool[_strKeyName].Peek().SetActive(true);
                //  m_ObjEffectPool[_strKeyName].Peek().GetComponent<BaseBullet>().Set_Data = _tagBulletStat;
                m_ObjEffectPool[_strKeyName].Peek().GetComponent<Base_Effect>().m_bIsOn = true;
                // m_ObjEffectPool[_strKeyName].Peek().GetComponent<ParticleSystem>().Play();

                return m_ObjEffectPool[_strKeyName].Dequeue();
            }
        }
        return null;
    }



    public void ReturnPool(GameObject _objReturn, string _strTagName)
    {
        // _objReturn.GetComponent<BaseObj>();
        //_objReturn.

        if (m_ObjEffectPool.ContainsKey(_strTagName))
        {
            _objReturn.transform.position = m_vecInitPoolPosition;
            
            m_ObjEffectPool[_strTagName].Enqueue(_objReturn);
        }
        else
        {
            Queue<GameObject> temp = new Queue<GameObject>();
            _objReturn.transform.position = m_vecInitPoolPosition;
            temp.Enqueue(_objReturn);
            m_ObjEffectPool.Add(_strTagName, temp);
        }
    }
}
