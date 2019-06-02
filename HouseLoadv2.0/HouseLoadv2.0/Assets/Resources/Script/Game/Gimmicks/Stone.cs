using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] {(int)Utility.ItemId.Hammer }, new string[] { "ハンマーを使用　壊す" }, new string[] { "ハンマーが必要" });
    }
}
