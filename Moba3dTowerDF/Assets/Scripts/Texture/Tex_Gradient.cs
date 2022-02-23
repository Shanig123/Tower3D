using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tex_Gradient : MonoBehaviour
{
    [SerializeField] private Texture2D gradientTex;

    [SerializeField] private bool m_bReverse;

    [SerializeField] private float m_fWeight;
  // [SerializeField] public float[,] m_arrGradientMap;
    [SerializeField] public float[] m_arrfPixel;

    public float[,] GenerateMap(int _Width , int _Height)
    {
        float[,] gradientMap = new float[_Width, _Height];
        m_arrfPixel = new float[_Width * _Height];

        if (m_bReverse)
        {
            for (int x = 0; x < _Width; x++)
            {
                for (int y = 0; y < _Height; y++)
                {
                    int xCoord = Mathf.RoundToInt(x * (float)gradientTex.width / _Width); //텍스처 값과 크기 값에 맞춰 좌표 저장
                    int yCoord = Mathf.RoundToInt(y * (float)gradientTex.height / _Height);
                    float fGray = (1.0f - (gradientTex.GetPixel(xCoord, yCoord).grayscale * m_fWeight)) ; //텍스처에서 색상을 가져와 그레이 스케일로 배열에 저장

                    gradientMap[x, y] = fGray;
                    m_arrfPixel[x * _Width + y] = fGray;
                }
            }

        }
        else
        {
            for (int x = 0; x < _Width; x++)
            {
                for (int y = 0; y < _Height; y++)
                {
                    int xCoord = Mathf.RoundToInt(x * (float)gradientTex.width / _Width); //텍스처 값과 크기 값에 맞춰 좌표 저장
                    int yCoord = Mathf.RoundToInt(y * (float)gradientTex.height / _Height);
                    float fGray = gradientTex.GetPixel(xCoord, yCoord).grayscale * m_fWeight;//텍스처에서 색상을 가져와 그레이 스케일로 배열에 저장

                    gradientMap[x, y] = fGray;
                    m_arrfPixel[x * _Width + y] = fGray;
                }
            }

        }

        return gradientMap;
    }
    // Start is called before the first frame update
    void Start()
    {
       // m_arrGradientMap = new float[m_iWidth, m_iHeight];
      //  m_arrGradientMap = GenerateMap();

    }

}
//https://velog.io/@1217pgy/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EC%A0%88%EC%B0%A8%EC%A0%81-%EC%83%9D%EC%84%B1%EC%9D%84-%EC%9C%84%ED%95%9C-%EC%84%AC-%EC%83%9D%EC%84%B1-%EA%B8%B0%EB%B2%95
