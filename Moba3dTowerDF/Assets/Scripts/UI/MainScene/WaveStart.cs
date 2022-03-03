using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStart : MonoBehaviour
{
    public StageController m_stageCtrl;
    public ConstructionController m_constructionCtrl;
    public void OnClickStart()
    {
        if (m_stageCtrl.m_bWaveStart || m_stageCtrl.Get_WaveOnOff)
            return;
        m_stageCtrl.m_bWaveStart = true;
        m_constructionCtrl.AutoInBoard();

    }
}
