using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTrees : MonoBehaviour
{
    public GameObject[] trees;
    public int gridX = 4;
    public int gridZ = 4;
    public Vector3 gridOrigin = Vector3.zero;
    public float gridOffset = 2f;
    public bool generateOnEnable;
    public GameObject[,] landscape;

    public MapGenerator map;
    public GenerateParkTerrain ground;

    private void OnEnable()
    {
        if (generateOnEnable)
        {
            Generate();
        }
    }

    public void Generate()
    {
        //map = new MapGenerator();
        gridX = map.width;
        gridZ = map.height;
        landscape = new GameObject[gridX, gridZ];
        map.GenerateMap();
        SpawnGrid();
        AdjustHeight();
    }

    void SpawnGrid()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                if (map.GetMap()[x, z] == 0)
                {
                    GameObject clone = Instantiate(trees[Random.Range(0, 9)],
                        transform.position + gridOrigin + new Vector3(gridOffset * x, 0, gridOffset * z), transform.rotation);

                    //clone.transform.SetParent(this.transform);
                    landscape[x, z] = clone;
                }
                else
                {
                    landscape[x, z] = null;
                }
            }
        }
    }

    void AdjustHeight()
    {
        //Loop thought the landscape array and when there is a tree chnage its y position to the ground vertice at the same indice height
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                if(landscape[x,z] != null)
                {
                    GameObject tree = landscape[x, z];

                    //tree.transform.position = new Vector3(tree.transform.position.x, Mathf.Abs(ground.vertices[(gridZ * z) + x].y - Mathf.Abs(1 - ground.vertices[(gridZ*z)+x].y)), tree.transform.position.z);
                    //Vector3.Distance(tree.transform.position, ground.vertices[(gridZ * z) + x]).y
                    int index = (((gridX + 1) * z) + x);
                    float height = ground.vertices[(((gridX + 1) * z) + x)].y;
                    if (height >= .9f)
                    {
                        tree.transform.position = new Vector3(tree.transform.position.x,
                            height, tree.transform.position.z);
                    }
                    else
                    {
                        landscape[x, z] = null;
                        Destroy(tree);
                        
                    }
                }
            }
        }

    }
}
