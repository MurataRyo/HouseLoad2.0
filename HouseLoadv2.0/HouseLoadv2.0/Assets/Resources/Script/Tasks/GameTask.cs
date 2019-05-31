using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTask : MonoBehaviour
{
    public List<MapObject> mapObjects;
    public int[][][] stageData;
    // Start is called before the first frame update
    void Start()
    {
        mapObjects = new List<MapObject>();
        GetComponent<StageCreateTask>().MapDataCreate(GetPath.Tutorial + "/Stage1",mapObjects,ref stageData);
    }
}