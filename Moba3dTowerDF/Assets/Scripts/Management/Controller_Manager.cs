using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Controller_Manager : MonoBehaviour, UnityEngine.EventSystems.IDragHandler, UnityEngine.EventSystems.IEndDragHandler
{
    // Start is called before the first frame update


    #region Value
    private static Controller_Manager m_cInstance = null;
    private bool m_bIsClick = false;
    private bool m_bIsDrag = false;

    [SerializeField] private float m_fClickTime = 0;   
    [SerializeField] private int m_iClickState = -1;

#if UNITY_EDITOR
    public bool m_bDebugmode = false;
#endif

    #endregion
    #region Property
    public static Controller_Manager Instance
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

    public float Get_ClickTime { get { return m_fClickTime; } }
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
 
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        m_bDebugmode = true;
        DebugConsoleMode();
#endif
        CheckInputMouse();



        if (Application.platform == RuntimePlatform.Android)
            AndroidKeyInput();


    }

    #region DragFunc

    public void OnDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
        m_bIsDrag = true;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
        m_bIsDrag = false;
    }

    #endregion



    private void DebugConsoleMode()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Game_Manager.Instance.AppQuit();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    private void AndroidKeyInput()
    {
        if(Input.GetKey(KeyCode.Home))
        {
            //Android HomeKey
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            //Android Escape
            Game_Manager.Instance.AppQuit();
        }
        else if (Input.GetKey(KeyCode.Menu))
        {
            //Android Menu
        }
    }

    public bool LButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_bIsClick = true;
            m_iClickState = 1;
            return true;
        }
        return false;

    }
    public bool LButtonUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            m_bIsClick = false;
            m_fClickTime = 0;
            m_iClickState = 2;
            return true;
        }
        return false;
    }

    public bool IsPointerOnUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public int CheckInputMouse()
    {
        // using  UnityEngine.EventSystems;
        LButtonUp();
        IsLongClick();

        return -1;
    }

    private void IsLongClick()
    {
        if (m_bIsClick)
        {
            m_fClickTime += Time.deltaTime;
        }
        else
        {
            m_fClickTime = 0;
        }
    }
}
