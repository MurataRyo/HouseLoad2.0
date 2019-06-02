using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] { -1 }, new string[] { "飛び降りる" }, new string[] { "ここを飛び降りるのはあぶない" });
    }
}
