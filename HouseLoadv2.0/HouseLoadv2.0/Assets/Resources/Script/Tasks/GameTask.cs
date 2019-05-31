using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTask : MonoBehaviour
{
    public List<MapObject> mapObjects;  //マップのオブジェクト(動く可能性のある物のみ)
    public int[][][] stageData;         //ステージのデータを更新する左から y z x
    // Start is called before the first frame update
    void Start()
    {
        mapObjects = new List<MapObject>();
        GetComponent<StageCreateTask>().MapDataCreate(GetPath.Tutorial + "/Stage1",mapObjects,ref stageData);
    }
}