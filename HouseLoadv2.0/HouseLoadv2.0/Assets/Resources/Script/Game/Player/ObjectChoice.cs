using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectChoice : MonoBehaviour
{
    private ChoiceUi choiceUi;
    public PlayerTask playerTask;
    public GameObject choiceMark;
    public List<ChoiceClass> choiceObjects; //選択可能なオブジェクト
    public int choiceNum;                   //選択しているオブジェクト※choiceObjectsの配列番号

    private void Awake()
    {
        playerTask = GetComponent<PlayerTask>();
        choiceMark = Instantiate(Resources.Load<GameObject>(GetPath.Ui + "/ChoiceMark"));
        choiceMark.AddComponent<ChoiceMark>().mainCamera = playerTask.mainCamera;
        choiceMark.SetActive(false);
    }

    private void Start()
    {
        choiceUi = playerTask.gameTask.gameObject.AddComponent<ChoiceUi>();
    }

    //選択の決定
    public void Enter()
    {
        //そもそもボタンを押していないとき
        if (!playerTask.gameTask.controllerTask.EnterButton())
            return;

        //ボタンを押しても意味がないとき
        if (choiceObjects.Count == 0 || playerTask.gameTask.eventCount != 0)
            return;

        //ワープの場合選択時の移動する場所が違うための式
        Gimmick gimmick = choiceObjects[choiceNum].ThisObj().GetComponent<Gimmick>();
        Vector3Int pos = gimmick.MapId == (int)Utility.MapId.Warp ? choiceObjects[choiceNum].pos : playerTask.pos; 

        choiceUi.Reset(gimmick);
        playerTask.playerMove.StartCoroutine(playerTask.playerMove.NowPosMoveAndRotation(choiceObjects[choiceNum].pos,pos));
    }

    //選択の変更
    public void ChoiceChange()
    {
        if (choiceObjects == null || choiceObjects.Count == 0 || playerTask.gameTask.eventCount != 0)
            return;

        //切り替えボタンを押したら
        if (playerTask.controllerTask.SerectKey(true) || playerTask.controllerTask.SerectKey(false))
        {
            //どっちのボタンか判別
            bool flag = playerTask.controllerTask.SerectKey(true);
            //変更
            choiceNum = Utility.ChoiceChange(choiceNum, choiceObjects.Count, flag);
            //UIの位置を更新
            ChoicePosUpdate();
        }
        Enter();
    }

    //マスが変更されたとき
    public void UpdatePos(Vector3Int newPos)
    {
        if (playerTask.gameTask.eventCount != 0)
            return;

        NewChoiceObject(newPos);
        choiceNum = 0;

        ChoiceUiSet();
    }

    //choiceObjectの更新
    public void NewChoiceObject(Vector3Int newPos)
    {
        choiceObjects = new List<ChoiceClass>();
        AddChoicePos(newPos);
        AddChoicePos(FowardPos(newPos, playerTask.gameObject.transform.position + playerTask.gameObject.transform.forward));
        ChoicePosSort(newPos);
    }

    //正面の座標を取る※今いる地点からの4方向のみ
    public Vector3Int FowardPos(Vector3Int nowPos, Vector3 pos)
    {
        Vector3 returnVec3 = Vector3.zero;
        Vector3[] poss = new Vector3[4];
        Vector3 nowPos1 = Utility.DataToPosition(nowPos);
        poss[0] = nowPos1 + Vector3.right;
        poss[1] = nowPos1 + Vector3.left;
        poss[2] = nowPos1 + Vector3.forward;
        poss[3] = nowPos1 + Vector3.back;

        float minRange = Mathf.Infinity;

        for (int i = 0; i < 4; i++)
        {
            float range = (pos - poss[i]).magnitude;
            if (minRange > range)
            {
                minRange = range;
                returnVec3 = poss[i];
            }
        }
        return Utility.PositionToData(returnVec3);
    }

    //選択可能マスを追加
    void AddChoicePos(Vector3Int pos)
    {
        //存在しない場所なら
        if (!playerTask.gameTask.InIfStageData(pos))
            return;

        //選択不可な場所なら
        int mapId = Array.IndexOf(playerTask.gameTask.Special.ChoiceObject, playerTask.gameTask.stageData[pos.x][pos.y][pos.z]);
        if (mapId == -1)
            return;

        ChoiceClass choiceObject = new ChoiceClass(pos, playerTask.gameTask.stageData[pos.x][pos.y][pos.z]);
        choiceObjects.Add(choiceObject);
    }

    //並び替え
    void ChoicePosSort(Vector3Int newPos)
    {
        int count = 1;

        //forrachとforで総あたりになる
        foreach (ChoiceClass choiceObject in choiceObjects)
        {
            for (int i = count; i < choiceObjects.Count; i++)
            {
                //centerに近いほど先に選択される※キャラの向いているマス
                Vector3 centerPos = Utility.DataToPosition(newPos) + transform.forward;
                if ((Utility.DataToPosition(choiceObject.pos) - centerPos).magnitude < (Utility.DataToPosition(choiceObjects[i].pos) - centerPos).magnitude)
                {
                    choiceObject.number++;
                }
                else
                {
                    choiceObjects[i].number++;
                }
            }
            count++;
        }

        choiceObjects.Sort();
    }

    //UIの更新
    public void ChoiceUiSet()
    {
        if (choiceObjects.Count == 0)
        {
            if (choiceMark.activeInHierarchy)
                choiceMark.SetActive(false);

            return;
        }

        if (!choiceMark.activeInHierarchy)
            choiceMark.SetActive(true);

        ChoicePosUpdate();
    }

    private void ChoicePosUpdate()
    {
        //選択中の座標に場所を変更                                                          微調整
        choiceMark.transform.position = Utility.DataToPosition(choiceObjects[choiceNum].pos) + Vector3.up * 1.25f;
    }
}