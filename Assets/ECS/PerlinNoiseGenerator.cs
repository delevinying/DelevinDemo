using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{

    public static int BlockFace = 0;
    public static Texture2D NoiseHeightMap;

    int texture2DWidth = 200;
    int texture2DHeight = 200;

    float scale1 = 1f;
    float scale2 = 10f;
    float scale3 = 20f;
    float offsetX;
    float offsetY;

    void Awake()
    {
        offsetX = Random.Range(0, 99999);
        offsetY = Random.Range(0, 99999);
        GameSettings.HeightMap = GenerateHeightMap();
        SpawnNumberBlock.HeightMap = GenerateHeightMap();
    }

    public Texture2D GenerateHeightMap()
    {
        Texture2D heightMap = new Texture2D(texture2DWidth, texture2DHeight);
        for (int x = 0; x < texture2DWidth; x++)
        {
            for (int y = 0; y < texture2DHeight; y++)
            {
                Color color = CalculateColor(x, y);
                heightMap.SetPixel(x, y, color);
            }
        }
        heightMap.Apply();
        return heightMap;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord1 = (float)x / texture2DWidth * scale1 + offsetX;
        float yCoord1 = (float)y / texture2DHeight * scale1 + offsetY;
        float xCoord2 = (float)x / texture2DWidth * scale2 + offsetX;
        float yCoord2 = (float)y / texture2DHeight * scale2 + offsetY;
        float xCoord3 = (float)x / texture2DWidth * scale3 + offsetX;
        float yCoord3 = (float)y / texture2DHeight * scale3 + offsetY;

        float sample1 = Mathf.PerlinNoise(xCoord1, yCoord1) / 15;
		float sample2 = Mathf.PerlinNoise(xCoord2,yCoord2)/15;
		float sample3 = Mathf.PerlinNoise(xCoord3,yCoord3)/15;
		return new Color(sample1+sample2+sample3,sample1+sample2+sample3,sample1+sample2+sample3);

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
