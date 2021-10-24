using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerlinGenerator : MonoBehaviour
{
    public static PerlinGenerator instance = null;

    /*public int perlinTextureSizeX;
    public int perlinTextureSizeY;
    public bool randomizeNoiseOffset;
    public Vector2 perlinOffset;
    public float noiseScale = 1f;*/
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;


    public int perlinGridStepSizeX = 4;
    public int perlinGridStepSizeY = 4;

    public bool visualizeGrid = false;
    public GameObject visualizationCube;
    public float visualizationHeightScale = 5f;
    public RawImage visualizationUI;

    private Texture2D perlinTexture;
    Color[] colorMap;
    //private List<List<Color>> colorsList;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    public void Generate()
    {
        GenerateNoise();
        if (visualizeGrid)
        {
            VisualizeGrid();
        }
    }

    /*void GenerateNoise()
    {
        //Creates the Perlin Texture

        
        colorsList = new List<List<Color>>();

        if (randomizeNoiseOffset)
        {
            perlinOffset = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));
        }

        perlinTexture = new Texture2D(perlinTextureSizeX, perlinTextureSizeY);

        for (int y= 0; y < perlinTextureSizeY; y++)
        {
            List<Color> colorRow = new List<Color>();
            for (int x = 0; x < perlinTextureSizeX; x++)
            {
                //perlinTexture.SetPixel(x, y, SampleNoise(x, y));
                colorRow.Add(SampleNoise(x, y));
                
            }
            colorsList.Add(colorRow);
        }
        //Color[] colorArray = colorsList.ToArray();
        perlinTexture.SetPixels(FlattenColors(colorsList));

        perlinTexture.Apply();
        visualizationUI.texture = perlinTexture;
    }

    Color[] FlattenColors(List<List<Color>> colorsList)
    {
        List<Color> temp = new List<Color>();
        foreach(List<Color> colorRow in colorsList)
        {
            foreach(Color col in colorRow)
            {
                temp.Add(col);
            }
        }
        return temp.ToArray();
    }*/

    public void GenerateNoise()
    {
        float[,] noiseMap = NewPerlinNoise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        //PerlinMapDisplay display = FindObjectOfType<PerlinMapDisplay>();
        DrawNoiseMap(noiseMap);


    }

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        //Texture2D texture = new Texture2D(width, height);

        colorMap = new Color[width * height];
        perlinTexture = new Texture2D(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
                colorMap[y * width + x] = new Color(noiseMap[x,y], noiseMap[x, y], noiseMap[x, y]);
            }
        }

        //perlinTexture.SetPixels(FlattenColors(colorsList));

        //perlinTexture.Apply();
        //visualizationUI.texture = perlinTexture;

        perlinTexture.SetPixels(colorMap);
        perlinTexture.Apply();
        visualizationUI.texture = perlinTexture;
        //perlinTexture.Apply();

        
    }

    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }

        if (mapHeight < 1)
        {
            mapHeight = 1;
        }

        if (lacunarity < 1)
        {
            lacunarity = 1;
        }

        if (octaves < 0)
        {
            octaves = 0;
        }


    }

    /*Color SampleNoise(int x, int y)
    {
        //Gets the color of the perlin noise at the x and y coordinates

        float xCoord = (float)x / perlinTextureSizeX * noiseScale + perlinOffset.x;
        float yCoord = (float)y / perlinTextureSizeY * noiseScale + perlinOffset.y;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        Color perlinColor = new Color(sample, sample, sample);

        return perlinColor;
    }*/

    public float SampleStepped(int x, int y)
    {
        //Gets the value 

        //int gridStepSizeX = perlinTextureSizeX / perlinGridStepSizeX;
        //int gridStepSizeY = perlinTextureSizeY / perlinGridStepSizeY;
        int gridStepSizeX = mapWidth / perlinGridStepSizeX;
        int gridStepSizeY = mapHeight / perlinGridStepSizeY;

        int index = Mathf.FloorToInt(y * gridStepSizeY) * mapWidth + Mathf.FloorToInt(x * gridStepSizeX);

        //float sampledFloat = perlinTexture.GetPixel((Mathf.FloorToInt(x * gridStepSizeX)), (Mathf.FloorToInt(y * gridStepSizeX))).grayscale;
        //float sampledFloat = colorsList[Mathf.FloorToInt(y * gridStepSizeY)][Mathf.FloorToInt(x * gridStepSizeX)].grayscale;
        float sampledFloat = colorMap[Mathf.FloorToInt(y * gridStepSizeY) * mapWidth + Mathf.FloorToInt(x * gridStepSizeX)].grayscale;

        return sampledFloat;
    }

    public float PerlinSteppedPosition(Vector3 worldPosition)
    { 

        int xToSample = Mathf.FloorToInt(worldPosition.x + perlinGridStepSizeX * .5f);
        int yToSample = Mathf.FloorToInt(worldPosition.z + perlinGridStepSizeY * .5f);

        xToSample = Mathf.Abs(xToSample % perlinGridStepSizeX);
        yToSample = Mathf.Abs(yToSample % perlinGridStepSizeY);

        float sampledValue = SampleStepped(xToSample, yToSample);

        return sampledValue;
    }

    void VisualizeGrid()
    {
        GameObject visualizationParent = new GameObject("VisualizationParent");
        visualizationParent.transform.SetParent(this.transform);

        for (int x = 0; x < perlinGridStepSizeX; x++)
        {
            for (int y = 0; y < perlinGridStepSizeY; y++)
            {
                GameObject clone = Instantiate(visualizationCube, new Vector3(x, SampleStepped(x, y) * visualizationHeightScale, y)
                     + transform.position, transform.rotation);

                clone.transform.SetParent(visualizationParent.transform);
                GeneratedObjectControl.instance.AddObject(clone);
            }
        }

        visualizationParent.transform.position = new Vector3(-perlinGridStepSizeX * .5f,
            -visualizationHeightScale * .5f, -perlinGridStepSizeY * .5f);

    }

    public void SetNoiseScaleFromSlider(Slider slider)
    {
        noiseScale = slider.value;
    }
}

