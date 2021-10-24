using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BuildingPieces", order = 1)]
public class BuildingPieces : ScriptableObject
{
    public GameObject[] baseParts;
    public GameObject[] middleParts;
    public GameObject[] topParts;
}
