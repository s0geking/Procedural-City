using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public int minPieces = 5;
    public int maxPieces = 20;
    public BuildingPieces parts;
    //public GameObject[] baseParts;
    //public GameObject[] middleParts;
    //public GameObject[] topParts;

    private void Start()
    {
        Build();
    }

    void Build()
    {
        int targetPieces = Random.Range(minPieces, maxPieces);
        float heightOffset = 0;
        heightOffset += SpawnPieceLayer(parts.baseParts, heightOffset);

        for(int i = 2; i < targetPieces; i++)
        {
            heightOffset += SpawnPieceLayer(parts.middleParts, heightOffset);

        }

        SpawnPieceLayer(parts.topParts, heightOffset);
    }

    float SpawnPieceLayer(GameObject[] pieceArray, float inputHeight)
    {
        Transform randomTransform = pieceArray[Random.Range(0, pieceArray.Length)].transform;
        GameObject clone = Instantiate(randomTransform.gameObject,
            this.transform.position + new Vector3(0, inputHeight, 0), transform.rotation);
        Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().mesh;
        Bounds bounds = cloneMesh.bounds;
        float heightOffset = bounds.size.y;

        clone.transform.SetParent(this.transform);

        return heightOffset;
    }
}
