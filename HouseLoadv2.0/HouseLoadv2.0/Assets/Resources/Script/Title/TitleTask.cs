using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleTask : MonoBehaviour
{
    private int nowChoice;
    private int logChoice;
    private TextAsset[] mapData;
    private Text stageName;
    private GameUiTask uiTask;
    private ControllerTask controllerTask;
    private SceneTask sceneTask;

    void Awake()
    {
        uiTask = gameObject.AddComponent<GameUiTask>();
        controllerTask = gameObject.AddComponent<ControllerTask>();
        sceneTask = gameObject.AddComponent<SceneTask>();
    }
    // Start is called before the first frame update
    void Start()
    {
        nowChoice = logChoice = 0;
        mapData = Resources.LoadAll<TextAsset>(GetPath.Tutorial);
        stageName = uiTask.NewTextUi(mapData[nowChoice].name, new Vector2(650f, -720f), Color.white, 200);
    }

    // Update is called once per frame
    void Update()
    {
        //選択の変更
        if (controllerTask.SerectKey(true))
            nowChoice = Utility.ChoiceChange(nowChoice, mapData.Length, true);
        else if (controllerTask.SerectKey(false))
            nowChoice = Utility.ChoiceChange(nowChoice, mapData.Length, false);

        //選択が変更されたら名前変更
        if (logChoice != nowChoice)
            stageName.text = mapData[nowChoice].name;

        //決定
        if (controllerTask.EnterButton())
            Enter();

        logChoice = nowChoice;
    }

    void Enter()
    {
        GameTask.mapData = mapData[nowChoice].text;
        sceneTask.LoadScene(SceneTask.SceneName.Main, true);
    }
}
