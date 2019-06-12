using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUiTask : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //ここでのPosは左上を0,0とする
    public Image NewImageUi(Sprite sprite,Vector2 pos,Vector2 size)
    {
        GameObject go = new GameObject();
        go.layer =
            LayerMask.NameToLayer("UI");

        Image image = go.AddComponent<Image>();
        image.sprite = sprite;

        go.transform.SetParent(Utility.GetCanvas().transform);
        go.transform.localPosition = pos + new Vector2(-Utility.GameSizeX / 2, Utility.GameSizeY / 2);
        go.GetComponent<RectTransform>().sizeDelta = size;

        return image;
    }

    //ここでのPosは左上を0,0とする
    public Text NewTextUi(string str, Vector2 pos,Color color)
    {
        GameObject go = new GameObject();
        go.layer =
            LayerMask.NameToLayer("UI");

        Text text = go.AddComponent<Text>();
        text.text = str;
        text.fontSize = 50;

        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        go.transform.SetParent(Utility.GetCanvas().transform);
        go.transform.localPosition = pos + new Vector2(-Utility.GameSizeX / 2, Utility.GameSizeY / 2);

        text.color = color;
        text.font = Resources.Load<Font>(GetPath.Font + "/Font");

        return text;
    }

    //ここでのPosは左上を0,0とする
    public Text NewTextUi(string str, Vector2 pos, Color color,int fontSize)
    {
        GameObject go = new GameObject();
        go.layer =
            LayerMask.NameToLayer("UI");

        Text text = go.AddComponent<Text>();
        text.text = str;
        text.fontSize = fontSize;

        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        go.transform.SetParent(Utility.GetCanvas().transform);
        go.transform.localPosition = pos + new Vector2(-Utility.GameSizeX / 2, Utility.GameSizeY / 2);

        text.color = color;
        text.font = Resources.Load<Font>(GetPath.Font + "/Font");

        return text;
    }
}
