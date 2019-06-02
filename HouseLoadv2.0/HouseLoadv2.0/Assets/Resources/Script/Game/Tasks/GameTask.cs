using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTask : MonoBehaviour
{
    public List<MapObject> mapObjects;  //マップのオブジェクト(動く可能性のある物のみ)
    public int[][][] stageData;         //ステージのデータを更新する左から y z x
    public SpecialObject Special;
    public int eventCount;              //動作が一部制限されるイベント
    public bool textEvent;              //テキストを読むことしかできないイベント
    public GameUiTask uiTask;
    public ControllerTask controllerTask;
    void Awake()
    {
        gameObject.AddComponent<StageCreateTask>();
        uiTask = gameObject.AddComponent<GameUiTask>();
        mapObjects = new List<MapObject>();
        GetComponent<StageCreateTask>().MapDataCreate(GetPath.Tutorial + "/Stage1", mapObjects, ref stageData);
        Special = new SpecialObject();
        textEvent = false;
        eventCount = 0;
        controllerTask = GetComponent<ControllerTask>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool InIfStageData(Vector3Int pos)
    {
        //プレイヤーが範囲外にいる場合
        if (pos.y < 0 || pos.z < 0 ||
            pos.y >= stageData[pos.x].Length ||
            pos.x >= stageData[pos.x][pos.y].Length)
            return false;

        return true;
    }
}