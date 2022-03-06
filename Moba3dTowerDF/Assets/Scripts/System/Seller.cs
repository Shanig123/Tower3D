using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour
{
    [SerializeField]
    private ConstructionController m_construction;
    [SerializeField]
    private StageController m_stageController;
    [SerializeField]
    private PlayerController m_playerController;

    public AudioClip m_audioClip;

    // Start is called before the first frame update
    void Start()
    {
        m_construction = GameObject.FindWithTag("TotalController").GetComponent<ConstructionController>();
        m_stageController = GameObject.FindWithTag("TotalController").GetComponent<StageController>();
        m_playerController = GameObject.FindWithTag("TotalController").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Sell_Tower()
    {
        if (!m_stageController.Get_WaveOnOff)
            return m_playerController.Sell_Tower();
        return false;
    }
    public void SellTower2()
    {
        //클릭시 레이피킹 후 ui 이벤트 처리로 이벤트 처리 순서에 대한 우선적 작업이 필요함.
        if (!m_stageController.Get_WaveOnOff)
        {
            bool bCheckSell =  m_playerController.Sell_Tower();
            GFunc.Function.Print_Log("Sell : " + bCheckSell);
            //if ()
            //    return;
            //else
            AudioSource audioSource = GameObject.FindGameObjectWithTag("TotalController").GetComponent<AudioSource>();
            // audioSource.clip = 
            audioSource.PlayOneShot(m_audioClip);
        }
    }

}
