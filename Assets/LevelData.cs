using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelData : ScriptableObject
{
    public Vector3 Size;
    public List<Vector3> Position = new List<Vector3>();
    public List<Vector3> Rotation= new List<Vector3>();
}
