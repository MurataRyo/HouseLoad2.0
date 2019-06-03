using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectChoice : MonoBehaviour
{
    private ChoiceUi choiceUi;
    public PlayerTask playerTask;
    private GameObject choiceMark;
    private List<ChoiceClass> choiceObjects; //選択可能なオブジェクト
    private int choiceNum;                   //選択しているオブジェクト※choiceObjectsの配列番号

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

        choiceUi.Reset(choiceObjects[choiceNum].ThisObj().GetComponent<Gimmick>());
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
        NewChoiceObject(newPos);
        choiceNum = 0;

        ChoiceUiSet();
    }

    //choiceObjectの更新
    public void NewChoiceObject( Vector3Int newPos)
    {
        choiceObjects = new List<ChoiceClass>();
        AddChoicePos(newPos);
        AddChoicePos(Utility.PositionToData(playerTask.gameObject.transform.position + playerTask.gameObject.transform.forward));
        ChoicePosSort(newPos);
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

        ChoiceClass choiceObject = new ChoiceClass(pos, mapId);
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
                Vector3 centerPos = Utility.DataToPosition(newPos)  + transform.forward;
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
    private void ChoiceUiSet()
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