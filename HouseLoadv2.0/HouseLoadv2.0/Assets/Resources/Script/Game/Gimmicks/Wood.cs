using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] {(int)Utility.ItemId.Ono, (int)Utility.ItemId.Match }, new string[] { "斧を使用　切り倒す", "マッチを使用　燃やす" }, new string[] { "斧が必要", "マッチが必要" });
    }
}
