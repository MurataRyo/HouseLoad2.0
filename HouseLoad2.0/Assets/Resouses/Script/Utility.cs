using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    #region　ID一覧

    public enum ItemId
    {
        Ono,
        Hammer,
        Match,
        Baketu
    }

    public enum ExItemId
    {
        WaterBaketu,
    }


    //オブジェクトID[配列番号とのリンク]
    public enum ObjectId
    {
        Player,
        Ground,
        Wood,
        WoodBlock,
        Stone,
        Wall,
        House,
        Water,
        Fire,
        Warp
    }

    public enum MapId
    {
        None,
        Hole,
        Player,
        Ground,
        Wood,
        WoodBlock,
        Stone,
        Wall,
        House,
        Water,
        Fire,
        Warp
    }
    #endregion
}
