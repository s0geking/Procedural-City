using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGeneratorNoiseInput : MonoBehaviour
{

    public int maxPieces = 20;
    public float perlineScaleFactor = 2f;

    public int randomVariationMin = -5;
    public int randomVariationMax = 10;
    public BuildingPieces parts;
    public GameObject park;
    public ParkPieces parkPieces;
    //MeshFilter[] meshFilters;
    CombineInstance[] combine;
    //public GameObject[] baseParts;
    //public GameObject[] middleParts;
    //public GameObject[] topParts;

    // Start is called before the first frame update
    void Start()
    {
        Build();
    }

    public void Build()
    {
        //Creates each building

        float sampledValue = PerlinGenerator.instance.PerlinSteppedPosition(transform.position);

        int targetPieces = Mathf.FloorToInt(maxPieces * (sampledValue));
        targetPieces += Random.Range(randomVariationMin, randomVariationMax);

        if(targetPieces <= 0)
        {
            GameObject clone = Instantiate(park.gameObject,
            this.transform.position + new Vector3(-.36f, .04f, -.36f), transform.rotation) as GameObject;
           // Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().mesh;
           // Bounds bounds = cloneMesh.bounds;
            clone.transform.SetParent(this.transform);
            GeneratedObjectControl.instance.AddObject(clone);
            if(Random.Range(0,10) >= 5)
            {
                GameObject piece = parkPieces.parkDecor[Random.Range(0, parkPieces.parkDecor.Length)];
                GameObject clonePiece = Instantiate(piece.gameObject,
                    this.transform.position + new Vector3(-.36f, .04f, -.36f), transform.rotation);
                clonePiece.transform.SetParent(this.transform);
                GeneratedObjectControl.instance.AddObject(clonePiece);
            }
            return;
        }

        combine = new CombineInstance[targetPieces];
        float heightOffset = 0;
        heightOffset += SpawnPieceLayer(parts.baseParts, heightOffset, 1);

        for (int i = 2; i < targetPieces; i++)
        {
            heightOffset += SpawnPieceLayer(parts.middleParts, heightOffset, i);

        }

        SpawnPieceLayer(parts.topParts, heightOffset, targetPieces);

        this.GetComponent<MeshFilter>().mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh.CombineMeshes(combine,true);
        this.gameObject.SetActive(true);
    }

    float SpawnPieceLayer(GameObject[] pieceArray, float inputHeight, int pieceNumber)
    {
        //Spawns each piece of the building

        Transform randomTransform = pieceArray[Random.Range(0, pieceArray.Length)].transform;
        GameObject clone = Instantiate(randomTransform.gameObject,
            this.transform.position + new Vector3(0, inputHeight, 0), transform.rotation) as GameObject;
        MeshFilter cloneMeshFilter = clone.GetComponentInChildren<MeshFilter>();
        Mesh cloneMesh = cloneMeshFilter.mesh;
        Bounds bounds = cloneMesh.bounds;
        float heightOffset = bounds.size.y;

        clone.transform.SetParent(this.transform);

        GeneratedObjectControl.instance.AddObject(clone);

        combine[pieceNumber - 1].mesh = cloneMeshFilter.sharedMesh;
        combine[pieceNumber - 1].transform = cloneMeshFilter.transform.localToWorldMatrix;
        //cloneMeshFilter.gameObject.SetActive(false);

        return heightOffset;
    }
}
