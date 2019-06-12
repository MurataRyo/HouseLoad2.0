﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameTask : MonoBehaviour
{
    public List<MapObject> mapObjects;  //マップのオブジェクト(動く可能性のある物のみ)
    public int[][][] stageData;         //ステージのデータを更新する左から y z x
    public SpecialObject Special;
    public int eventCount;              //動作が一部制限されるイベント
    public bool textEvent;              //テキストを読むことしかできないイベント
    public PlayerTask playerTask;
    public GameUiTask uiTask;
    public ControllerTask controllerTask;
    public MoveObjectTask moveObjectTask;
    private StageCreateTask stageCreateTask;
    public DrawingFloorTask drawFloorTask;
    public SceneTask sceneTask;
    private PauseTask pauseTask;

    public static string mapData;
    public enum GameMode
    {
        Main,
        Pause,
        Crear
    }
    public GameMode gameMode;
    public GameMode gameModeLog;

    void Awake()
    {
        gameMode = gameModeLog = GameMode.Main;
        moveObjectTask = gameObject.AddComponent<MoveObjectTask>();
        stageCreateTask = gameObject.AddComponent<StageCreateTask>();
        uiTask = gameObject.AddComponent<GameUiTask>();
        mapObjects = new List<MapObject>();
        stageCreateTask.MapDataCreate(mapData, mapObjects, ref stageData);
        Special = new SpecialObject();
        textEvent = false;
        eventCount = 0;
        controllerTask = GetComponent<ControllerTask>();
        drawFloorTask = GetComponent<DrawingFloorTask>();
        sceneTask = GetComponent<SceneTask>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTask = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTask>();
    }

    private void Update()
    {
        IfGameModeChange();
        GameModeUpdate();
    }

    //オブジェクトの移動※元々の場所は地面になる
    public void MoveObject(Vector3Int pos, Vector3Int nextPos, int mapId)
    {
        stageData[pos.x][pos.y][pos.z] = (int)Utility.MapId.Ground;
        stageData[nextPos.x][nextPos.y][nextPos.z] = mapId;

        MapObject mobj = GetMapObj(pos, (int)Utility.GetObjectId((Utility.MapId)mapId));
        mobj.pos = nextPos;
    }

    //オブジェクトの移動※元々の場所は地面になる
    public void MoveObject(Vector3Int pos, Vector3Int nextPos, int mapId, int nextMapid, int deleteMapId)
    {
        stageData[pos.x][pos.y][pos.z] = (int)Utility.MapId.Ground;
        stageData[nextPos.x][nextPos.y][nextPos.z] = nextMapid;

        DeleteObject(nextPos, deleteMapId);

        MapObject mobj = GetMapObj(pos, (int)Utility.GetObjectId((Utility.MapId)mapId));
        mobj.pos = nextPos;
        mobj.objectId = (int)Utility.GetObjectId((Utility.MapId)nextMapid);
    }

    //下のDeleteObject用
    public enum CreateData
    {
        noCreate,       //生成しない
        groundCreate,   //地面を生成する
        upCreate        //地上に生成する
    }

    public void DeleteObject(Vector3Int pos, int objectId)
    {
        MapObject mobj = GetMapObj(pos, objectId);

        mapObjects.Remove(mobj);
        Destroy(mobj.go);
    }

    public void DeleteObject(Vector3Int pos, int nextData, int objectId, CreateData createData)
    {
        MapObject mobj = GetMapObj(pos, objectId);

        mapObjects.Remove(mobj);
        Destroy(mobj.go);

        stageData[pos.x][pos.y][pos.z] = nextData;
        if (createData == CreateData.noCreate)
            return;

        //下にブロックを置くもの
        SpecialObject Special = new SpecialObject();
        Vector3 position = Utility.DataToPosition(new Vector3Int(pos.x, pos.y, pos.z));
        stageCreateTask.CreateObject(position, nextData, 0, Special, mapObjects, drawFloorTask.floorObjects[pos.x], createData == CreateData.groundCreate);
    }

    public MapObject GetMapObj(Vector3Int pos, int objectId)
    {
        foreach (MapObject mobj in mapObjects)
        {
            if (mobj.pos == pos && objectId == mobj.objectId)
            {
                return mobj;
            }
        }

        Debug.Log("検索しましたがありませんでした" + (Utility.ObjectId)objectId);
        return null;
    }

    //IDのオブジェクトを全取得する
    public Gimmick[] GetGimmcks(Utility.ObjectId[] objctId)
    {
        List<Gimmick> retrunGimmick = new List<Gimmick>();
        foreach (MapObject mapObject in mapObjects)
        {
            if (-1 != Array.IndexOf(objctId, (Utility.ObjectId)mapObject.objectId))
            {
                retrunGimmick.Add(mapObject.go.GetComponent<Gimmick>());
            }
        }
        return retrunGimmick.ToArray();
    }

    //ステージの中かどうか
    public bool InIfStageData(Vector3Int pos)
    {
        //範囲外にいる場合
        if (pos.x < 0 || pos.y < 0 || pos.z < 0 ||
            pos.x >= stageData.Length ||
            pos.y >= stageData[pos.x].Length ||
            pos.z >= stageData[pos.x][pos.y].Length)
            return false;

        return true;
    }

    //ゲームモードの変更関連
    private void IfGameModeChange()
    {
        if (gameMode == GameMode.Main && controllerTask.PauseKey())
        {
            gameMode = GameMode.Pause;
            pauseTask = gameObject.AddComponent<PauseTask>();
        }
        else if (gameMode == GameMode.Pause && controllerTask.PauseKey())
            gameMode = GameMode.Main;

        if (gameMode != gameModeLog)
            ChangeGameMode();

        gameModeLog = gameMode;
    }

    private void ChangeGameMode()
    {
        switch (gameMode)
        {
            case GameMode.Main:
                if (pauseTask != null)
                    pauseTask.Destroy();
                Time.timeScale = 1;
                eventCount--;
                break;

            case GameMode.Pause:
                Time.timeScale = 0;
                eventCount++;
                break;

            case GameMode.Crear:
                eventCount++;
                break;
        }
    }

    private void GameModeUpdate()
    {
        switch (gameMode)
        {
            case GameMode.Main:
                break;

            case GameMode.Crear:

                if (controllerTask.EnterButton())
                {
                    sceneTask.LoadScene(SceneTask.SceneName.Title, true);
                }

                break;
        }
    }
}