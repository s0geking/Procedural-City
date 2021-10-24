using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ParkPieces", order = 1)]
public class ParkPieces : ScriptableObject
{
    public GameObject[] parkDecor;
}
