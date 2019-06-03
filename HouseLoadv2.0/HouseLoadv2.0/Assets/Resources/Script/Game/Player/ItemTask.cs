using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ItemTask : MonoBehaviour
{
    Item[] items;
    Image whiteBack;
    GameTask gameTask;
    public void Awake()
    {
        gameTask = Utility.GetGameTask();
    }

    //初期化               //ItemIdの順で渡す
    public void ItemCreate(int[] itemNum)
    {
        whiteBack = gameTask.uiTask.NewImageUi(null, new Vector2(960f, -990f), new Vector2(1920f, 180f));
        whiteBack.color = new Color(1f, 1f, 1f, 0.8f);
        int num = Enum.GetValues(typeof(Utility.ItemId)).Length;
        items = new Item[num];
        for (int i = 0; i < num; i++)
        {
            Sprite sprite = Resources.Load<Sprite>(GetPath.Item + "/" + Enum.GetName(typeof(Utility.ItemId), i));

            if (num == (int)Utility.ItemId.Baketu)
                items[i] = new ItemWater(itemNum[i], i, sprite);

            items[i] = new Item(itemNum[i], i, sprite);
        }
    }
}

class Item
{
    public int num;        //数
    public int itemId;
    public Sprite sprite;  //画像
    public GameObject go;
    public Image image;
    public Text text;
    public Vector3 pos;

    public Item(int num, int itemId, Sprite sprite)
    {
        this.num = num;
        this.itemId = itemId;
        this.sprite = sprite;
        pos = new Vector3(-550f + 300f * itemId, -450f, 0f);
        go = new GameObject();

        go.name = Enum.GetName(typeof(Utility.ItemId), itemId);
        go.layer = LayerMask.NameToLayer("UI");
        image = go.AddComponent<Image>();
        image.sprite = this.sprite;
        go.transform.SetParent(Utility.GetCanvas().transform);

        RectTransform rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = Vector2.one * 135f;
        rect.localPosition = pos;

        if (itemId != (int)Utility.ItemId.Baketu)
            text = Utility.GetTaskObject().GetComponent<GameUiTask>().NewTextUi(num.ToString(), pos + Vector3.right * 135f - new Vector3(-Utility.GameSizeX / 2, Utility.GameSizeY / 2, 0f), Color.black);
        UseUpdate();
    }

    public void TextUpdate()
    {
        if (text == null)
            return;

        bool flag = num != 0;
        if(text.gameObject.activeInHierarchy != flag)
        {
            text.gameObject.SetActive(flag);
        }
    }

    //数の更新
    public virtual void UseUpdate()
    {
        Color color = num <= 0 ? new Color(0.05f, 0.05f, 0.05f, 1f) : Color.white;
        image.color = color;
        TextUpdate();
    }
}

class ItemWater : Item
{
    Sprite waterSprite;
    public ItemWater(int num, int itemId, Sprite sprite) : base(num, itemId, sprite)
    {
        waterSprite = Resources.Load<Sprite>(GetPath.ExItem + "/WaterBaketu");
    }

    public override void UseUpdate()
    {
        Sprite next = num <= 1 ? sprite : waterSprite;
        sprite = next;
    }
}
