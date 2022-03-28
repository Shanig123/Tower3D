using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tex_PerlinNoise : MonoBehaviour
{
    #region Value

    [SerializeField] public float[,]    m_arrNoiseMap;
    [SerializeField] public float[]     m_arrfPixel;

    [SerializeField] private float m_fAmplitude = 1;
    [SerializeField] private float m_fFrequency = 1;

    [SerializeField] private DataController dataController;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private int XSeed = 1;
    [SerializeField] private int YSeed = 1;
    [SerializeField] private int iSeed = 1;

    private Color[] pix;

    //[SerializeField] private int m_iWidth = 1;
    //[SerializeField] private int m_iHeight = 1;

    #endregion
    public void Start()
    {
        Texture2D noiseTex = new Texture2D(64, 64);
        spriteRenderer.material.mainTexture = noiseTex;
        iSeed = dataController.Get_Seed;
        System.Random test = new System.Random(iSeed);
        XSeed = test.Next(0, 99999);
        YSeed = test.Next(0, 99999);
        m_arrNoiseMap = GenerateMap(64, 64, 1.0f, 3, 0.5f, 2, XSeed, YSeed);

       
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                //  float fGray = gradientTex.GetPixel(xCoord, yCoord).grayscale;//텍스처에서 색상을 가져와 그레이 스케일로 배열에 저장
                Color color = new Color();
                color = Color.Lerp(Color.black, Color.white, m_arrNoiseMap[x, y]);
                noiseTex.SetPixel(x,y,color);
            }
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        //spriteRenderer.sprite = Sprite.Create(noiseTex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
    }
    public float[,] GenerateMap(int width, int height, float scale, float octaves, float persistance, float lacunarity, float xOrg, float yOrg)
    {
        float[,] noiseMap = new float[width, height];
        m_arrfPixel = new float[width * height];
        scale = Mathf.Max(0.0001f, scale);
        float maxNoiseHeight = float.MinValue; //최대 값을 담기 위한 변수
        float minNoiseHeight = float.MaxValue; //최소 값을 담기 위한 변수
        pix = new Color[width * height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float amplitude = m_fAmplitude; //진폭. 노이즈의 폭과 관련된 값.
                float frequency = m_fFrequency; //주파수. 노이즈의 간격과 관련된 값. 주파수가 커질수록 노이즈가 세밀해짐
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) //옥타브가 증가할수록 높은 주파수와 낮은 진폭의 노이즈가 중첩됨.
                {
                    float xCoord = xOrg + x / scale * frequency;
                    float yCoord = yOrg + y / scale * frequency;
                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1; //0~1 사이의 값을 반환하는 함수. 2를 곱하고 1을 빼서 -1~1 사이의 값으로 변환
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                   // Debug.Log(noiseHeight);
                }

                if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;

                float xCoord2 = xOrg + x / width * scale;
                float yCoord2 = yOrg + y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord2, yCoord2);
                noiseHeight = sample;

                pix[(int)y * width + (int)x] = new Color(sample, sample, sample);
                //Debug.Log(noiseMap[x, y]+" / "+ octaves);
                //Debug.Log(noiseHeight);
                noiseMap[x, y] = noiseHeight;
                m_arrfPixel[x * width + y] = noiseHeight;
             
            }
        }
        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < height; y++)
        //    {

        //        float fValue = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);

        //        Debug.Log(fValue);
        //        noiseMap[x, y] = fValue; //lerp의 역함수로 최솟값과 최댓값의 사잇값을 3번째 인자로 넣으면 0~1사이의 값을 반
        //         m_arrfPixel[x * width + y] = fValue;
        //    }
        //}
        return noiseMap;
    }

}
