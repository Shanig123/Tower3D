using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Decoration : MonoBehaviour
{
    // Start is called before the first frame update
    #region Value
    public List<GameObject> m_listDecorations;

    [SerializeField] public float[,] m_arrGradientMap;
    [SerializeField] public float[] m_arrGradientPixel;
    [SerializeField] public float[,] m_arrNoiseMap;
    [SerializeField] public float[] m_arrNoisePixel;

    [SerializeField] private int m_iWidth;
    [SerializeField] private int m_iHeight;


    [SerializeField] private bool m_bCheckResource = false;

    //[SerializeField] private GameObject m_objOutGround;

    [SerializeField] private Tex_Gradient m_TexGradient;
    [SerializeField] private Tex_PerlinNoise m_TexNoise;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private StageController stageController;
    //[SerializeField] private GameObject Obj_stageControllerObject;

    [Range(0f, 1f)]
    public float[] fillPercents;
    public Color[] fillColors;
    #endregion
    private Color CalcColor(float noiseValue, float gradientValue, bool useColorMap)
    {
        float value = noiseValue + gradientValue;
        value = Mathf.InverseLerp(0, 2, value); //노이즈 맵과 그라디언트 맵을 더한 값을 0~1사이의 값으로 변환
        Color color = Color.Lerp(Color.black, Color.white, value); //변환된 값에 해당하는 색상을 그레이스케일로 저장
        if (useColorMap)
        {
            for (int i = 0; i < fillPercents.Length; i++)
            {
                if (color.grayscale < fillPercents[i])
                {
                    color = fillColors[i]; //미리 설정한 색상 범위에 따라 색상 변환
                    break;
                }
            }
        }
        return color;
    }
    void Start()
    {
        
        //m_arrGradientMap = new float[m_iWidth, m_iHeight];
        m_arrGradientMap = m_TexGradient.GenerateMap(m_iWidth, m_iHeight);
      //  m_arrNoiseMap = m_TexNoise.GenerateMap(m_iWidth, m_iHeight, 1.0f, 3, 0.5f, 2, Random.Range(0, stageController.Get_Seed%10), Random.Range(0, stageController.Get_Seed%5));

     //   stageController = Obj_stageControllerObject.GetComponent<StageController>;
        //int width = m_arrNoiseMap.GetLength(0);
        //int height = m_arrNoiseMap.GetLength(1);

        int width = m_arrGradientMap.GetLength(0);
        int height = m_arrGradientMap.GetLength(1);


        Texture2D noiseTex = new Texture2D(width, height);
        noiseTex.filterMode = FilterMode.Point;
        Color[] colorMap = new Color[width * height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //colorMap[x * height + y] = CalcColor(m_TexNoise.m_arrfPixel[x * height + y], m_TexGradient.m_arrfPixel[x * height + y], false);
                colorMap[x * height + y] = CalcColor(m_TexGradient.m_arrfPixel[x * height + y], m_TexGradient.m_arrfPixel[x * height + y], false);

            }
        }
        noiseTex.SetPixels(colorMap);
        noiseTex.Apply();

    //    spriteRenderer.sprite = Sprite.Create(noiseTex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));

    }

    // Update is called once per frame
    void Update()
    {
        if(!m_bCheckResource && Resource_Manager.Instance.Get_CheckLoad && Resource_Manager.Instance.m_dictPrefabs.ContainsKey("Nature"))
            Copy_NatureResources();



    }

    void Copy_NatureResources()
    {
        int iResourceCount = Resource_Manager.Instance.m_dictPrefabs["Nature"].Count;
        GameObject[] arrGameObjects = new GameObject[iResourceCount];

        foreach (KeyValuePair<string, DataStruct.tagPrefab> items in Resource_Manager.Instance.m_dictPrefabs["Nature"])
        {
            m_listDecorations.Add(items.Value.objPrefabs);
        }
       StartCoroutine(InstanceNature());
       
    }

    IEnumerator InstanceNature()
    {
      //  MeshCollider meshCollider = m_objOutGround.GetComponent<MeshCollider>();

        m_bCheckResource = true;

        yield return null;
    }

    //void Get_RandomPos(MeshCollider meshCollider)
    //{
      
    //   // Vector3 objPos = m_objOutGround.transform.position;
       
    //    //float fRandom = Random.RandomRange(0f,1f);
    //    //meshCollider.bounds.size.x
    //}
}
