using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateParkTerrain : MonoBehaviour
{
    //Uses Brackey's Terrain Generation Tutorial

    public int mapWidth = 20;
    public int mapHeight = 20;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public bool useRandomSeed;
    public Vector2 offset;

    //public int xSize = 20;
    //public int zSize = 20;
    public float meshHeightMultipler = 1;
    public Mesh mesh;
    public Material material;
    public Vector3[] vertices;
    public int[] triangles;
    public Mesh surfaceMesh;
    public MeshCollider meshCollider;
    public float zoom = .3f;

    Matrix4x4[] matrices;
    public GenerateTrees trees;

    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        surfaceMesh = GetComponent<MeshFilter>().mesh;
        meshCollider = gameObject.GetComponent<MeshCollider>();
        matrices = new Matrix4x4[(mapWidth+ 1) * (mapHeight + 1)];

        CreateShape();
        UpdateMesh();
        trees.Generate();
    }

    /*private void Update()
    {
        Graphics.DrawMeshInstanced(surfaceMesh, 0, material, matrices);
    }*/

    void CreateShape()
    {
        //vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        vertices = new Vector3[(mapWidth + 1) * (mapHeight + 1)];
        if (useRandomSeed)
        {
            seed = Random.Range(-1000000, 1000000);
        }
        float[,] noiseMap = NewPerlinNoise.GenerateNoiseMap(mapWidth+1, mapHeight+1, seed, noiseScale, octaves, persistance, lacunarity, offset);

        int i = 0;
        for (int z = 0; z <= mapHeight; z++)
        {
            for(int x = 0; x <= mapWidth; x++)
            {
                //zoom = Random.Range(.1f, .2f);
                //float y = Mathf.PerlinNoise(x * zoom, z * zoom) * 2f;
                //float y = 0;
                float y = noiseMap[x, z] * meshHeightMultipler;


                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        /*for(int j = 0; j < vertices.Length; j++)
        {
            matrices[j] = Matrix4x4.TRS(vertices[j], this.transform.rotation, Vector3.one);
        }*/

        int vert = 0;
        int tris = 0;
        triangles = new int[mapWidth * mapHeight * 6];
        for (int z = 0; z < mapHeight; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + mapWidth + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + mapWidth + 1;
                triangles[tris + 5] = vert + mapWidth + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mesh.RecalculateBounds();
        meshCollider.sharedMesh = mesh;

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



}
