using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleTask : MonoBehaviour
{
    ControllerTask controllerTask;
    int stageNum;   //ステージの数
    int nowSelect;  //現在調べてる数
    string[] stageText;
    Text text;
    TextAsset[] texts;
    // Start is called before the first frame update
    void Start()
    {
        GameObject go = new GameObject();
        go.transform.parent = gameObject.transform;
        text = go.AddComponent<Text>();
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.fontSize = 150;
        go.transform.localPosition = Vector3.zero;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.sizeDelta = Vector2.one * 2000;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        nowSelect = 0;
        controllerTask = GetComponent<ControllerTask>();
        texts = Resources.LoadAll<TextAsset>(GetPath.Txt);
        stageNum = texts.Length;
        stageText = new string[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            stageText[i] = texts[i].name;
        }
    }

    void Update()
    {
        TextUpdate();
        Select();
    }

    void TextUpdate()
    {
        text.text = texts[nowSelect].name;
    }

    void Select()
    {
        if (controllerTask.SerectKey(true))
        {
            nowSelect--;
            if(nowSelect < 0)
            {
                nowSelect = stageNum - 1;
            }
        }
        else if(controllerTask.SerectKey(false))
        {
            nowSelect++;
            if(nowSelect >= stageNum)
            {
                nowSelect = 0;
            }
        }

        if(controllerTask.EnterButton())
        {
            GameTask.stageName = texts[nowSelect].name;
            SceneManager.LoadScene("Main");
        }
    }
}
