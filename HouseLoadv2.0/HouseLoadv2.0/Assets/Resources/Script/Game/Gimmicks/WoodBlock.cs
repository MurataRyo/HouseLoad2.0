using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBlock : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] { -1, (int)Utility.ItemId.Match }, new string[] { "押す","マッチを使用　燃やす" }, new string[] { "えらーありえない","マッチが必要" });
    }
}
