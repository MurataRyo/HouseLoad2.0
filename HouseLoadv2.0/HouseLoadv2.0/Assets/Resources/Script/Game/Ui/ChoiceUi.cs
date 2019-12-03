using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceUi : MonoBehaviour
{
    private Gimmick choiceGimmick;
    private bool activeFlag;
    private GameTask gameTask;
    private GameUiTask uiTask;
    private Vector2 basePos;
    private Text[] text;
    public GameObject choiceMark;
    private const float uiInterval = 75f;
    private int choiceNum;
    private bool[] flag;
    private ObjectChoice objectChoice;

    private bool okFlag;    //これがないと決定と選択の1回で処理を行ってしまう

    private void Start()
    {
        basePos = new Vector2(150f, -100f);
        gameTask = Utility.GetGameTask();
        uiTask = Utility.GetTaskObject().GetComponent<GameUiTask>();

        Sprite sprite = Resources.Load<Sprite>(GetPath.Image + "/choice");
        choiceMark = uiTask.NewImageUi(sprite, Vector2.zero, Vector2.one * 50f).gameObject;
        choiceMark.transform.eulerAngles = Vector3.forward * 90f;
        choiceMark.SetActive(false);
        objectChoice = GameObject.FindGameObjectWithTag("Player").GetComponent<ObjectChoice>();
    }

    //初期化
    public void Reset(Gimmick choiceGimmick)
    {
        okFlag = false;
        choiceNum = 0;
        activeFlag = true;
        gameTask.eventCount++;
        this.choiceGimmick = choiceGimmick;
        choiceMark.SetActive(true);
        choiceMark.transform.localScale = Vector3.one;
        MarkPos(0);
        UiAdd();
    }

    //終了時の処理
    private void End()
    {
        activeFlag = false;
        gameTask.eventCount--;
        choiceMark.SetActive(false);
        TextDelete();
    }

    private void Enter()
    {
        StartCoroutine(choiceGimmick.Use(choiceGimmick.useBase.itemNum[choiceNum]));
    }

    private void UiAdd()
    {
        int textNum = choiceGimmick.useBase.itemNum.Length;
        text = new Text[textNum];
        flag = new bool[textNum];
        for (int i = 0; i < textNum; i++)
        {
            flag[i] = choiceGimmick.UseIf(choiceGimmick.useBase.itemNum[i]);   //実行できるかどうか
            Color color = flag[i] ? new Color(0f, 0f, 0f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
            string message = flag[i] ? choiceGimmick.useBase.message[i] : choiceGimmick.useBase.noMessage[i];
            Vector2 pos = basePos - new Vector2(0f, uiInterval * i);
            text[i] = uiTask.NewTextUi(message, pos, color);
            text[i].gameObject.transform.localScale = Vector3.one;
        }
    }

    //Markの座標指定     配列番号
    private void MarkPos(int num)
    {
        choiceMark.transform.localPosition = new Vector3(-915f, 465f - uiInterval * num, 0f);
    }

    //テキストの消去
    private void TextDelete()
    {
        if (text == null)
            return;

        //古いテキストを削除
        for (int i = text.Length - 1; i >= 0; i--)
        {
            Destroy(text[i].gameObject);
        }
    }

    private void Update()
    {   
        //                  このUIで1カウントは進んでいるから
        if (!activeFlag || gameTask.eventCount > 1)
            return;

        if (gameTask.controllerTask.BackButton())
        {
            End();
            return;
        }

        //決定
        if (gameTask.controllerTask.EnterButton() && flag[choiceNum] && okFlag)
        {
            End();
            Enter();
            Vector3Int pos = Utility.PositionToData(objectChoice.playerTask.transform.position);
            objectChoice.UpdatePos(pos);
            return;
        }
        okFlag = true;

        //切り替えボタンを押したら
        if (gameTask.controllerTask.SerectKey(true) || gameTask.controllerTask.SerectKey(false))
        {
            //どっちのボタンか判別
            bool flag = gameTask.controllerTask.SerectKey(true);
            int maxNum = choiceGimmick.useBase.itemNum.Length;
            //変更
            choiceNum = Utility.ChoiceChange(choiceNum, maxNum, flag);
            //UIの位置を更新
            MarkPos(choiceNum);
        }
    }
}
