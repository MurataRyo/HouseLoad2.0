using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseTask : MonoBehaviour
{
    private GameTask gameTask;
    private GameUiTask uiTask;

    private Text[] pauseTexts;
    private Image choiceUi;
    private Vector2 pauseUiPos = new Vector2(610f, -450f);
    private Vector2 choiceUiPos = new Vector2(500f, -470f);
    private float uiRange = 200f;
    private int nowChoice;
    private int logChoice;

    // Start is called before the first frame update
    void Start()
    {
        gameTask = Utility.GetGameTask();
        uiTask = gameTask.uiTask;

        pauseTexts = new Text[4];
        pauseTexts[0] = uiTask.NewTextUi("ポーズ中", new Vector2(550f, -100f), Color.black, 300);
        pauseTexts[1] = uiTask.NewTextUi("ゲームに戻る", pauseUiPos, Color.black, 150);
        pauseTexts[2] = uiTask.NewTextUi("リトライ", pauseUiPos - new Vector2(0f, uiRange), Color.black, 150);
        pauseTexts[3] = uiTask.NewTextUi("あきらめる", pauseUiPos - new Vector2(0f, uiRange * 2), Color.black, 150);
        choiceUi = uiTask.NewImageUi(Resources.Load<Sprite>(GetPath.Image + "/Choice"), choiceUiPos, Vector2.one * 80f);
        choiceUi.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 90f);

        nowChoice = logChoice = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //選択の変更
        if (gameTask.controllerTask.SerectKey(true))
            nowChoice = Utility.ChoiceChange(nowChoice, 3, true);
        else if (gameTask.controllerTask.SerectKey(false))
            nowChoice = Utility.ChoiceChange(nowChoice, 3, false);

        //選択が変更されたらUIの場所更新
        if (logChoice != nowChoice)
            ChoiceUiPosUpdate();

        //決定
        if (gameTask.controllerTask.EnterButton())
            Enter();

        logChoice = nowChoice;
    }

    private void ChoiceUiPosUpdate()
    {
        choiceUi.gameObject.transform.position = new Vector2(choiceUiPos.x, 1920 / 2 + choiceUiPos.y + 100f) - new Vector2(0f, (nowChoice * uiRange));
    }

    private void Enter()
    {
        switch (nowChoice)
        {
            //ゲームに戻る
            case 0:
                gameTask.gameMode = GameTask.GameMode.Main;
                break;

            //リトライ
            case 1:
                gameTask.sceneTask.LoadScene(SceneTask.SceneName.Main, true);
                break;

            //あきらめる
            case 2:
                gameTask.sceneTask.LoadScene(SceneTask.SceneName.Title, true);
                break;
        }
    }

    //消去時に使う関数※これをしないとUIが消えないため
    public void Destroy()
    {
        foreach (Text text in pauseTexts)
        {
            Destroy(text.gameObject);
        }
        Destroy(choiceUi.gameObject);

        Destroy(this);
    }
}
