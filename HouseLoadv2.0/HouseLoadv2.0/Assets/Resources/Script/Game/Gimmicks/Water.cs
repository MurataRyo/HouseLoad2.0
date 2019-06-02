using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] {(int)Utility.ItemId.Baketu }, new string[] { "水を汲む" }, new string[] { "空バケツが必要" });
    }
}
