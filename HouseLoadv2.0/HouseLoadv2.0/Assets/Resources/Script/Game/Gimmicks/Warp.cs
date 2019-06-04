using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : Gimmick
{
    [HideInInspector] public int number;
    [HideInInspector] public Warp warpPos;
    [HideInInspector] public Vector3[] lineVertex;
    public BesieData besieData;
    public WarpLine warpLine;
    private bool lineCreateFlag;    //どちらがラインを引いたほうか判断するため

    public override void Start()
    {
        base.Start();
        MapId = (int)Utility.MapId.Warp;
        lineCreateFlag = false;
        //ワープ先の指定
        foreach (MapObject mOb in gameTask.mapObjects)
        {
            if (mOb.go.GetComponent<Warp>() == null)
                continue;

            Warp warp = mOb.go.GetComponent<Warp>();

            if (warp.gameObject == gameObject ||
                warp.number != number)
                continue;

            warpPos = mOb.go.GetComponent<Warp>();

            if (warpPos.warpPos != null)
            {
                lineCreateFlag = true;
                CreateLine(transform.position, warpPos.transform.position);
            }
        }
    }

    public override IEnumerator Use(int itemNum)
    {
        gameTask.moveObjectTask.MoveObject(WarpMove(), true);
        yield break;
    }

    public MoveObject WarpMove()
    {
        Vector3[] poss;

        if(lineCreateFlag)
        {
            poss = besieData.Positions(20);
        }
        else
        {
            Vector3[] vec3s = warpPos.besieData.Positions(20);
            poss = new Vector3[vec3s.Length];
            for(int i = 0;i < vec3s.Length;i++)
            {
                poss[i] = vec3s[vec3s.Length - i　-1];
            }
        }

        GameObject player = gameTask.playerTask.gameObject;

        MoveObject moveObject = new MoveObject(player, poss, 5f);
        return moveObject;
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] { -1 }, new string[] { "移動する" }, new string[] { "えらーありえない" });
    }

    #region ワープ関連
    //ワープのパスを生成
    private void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject go = new GameObject();
        go.transform.position = transform.position;
        go.name = "WarpLine";
        go.tag = "WarpLine";
        warpLine = go.AddComponent<WarpLine>();

        gameObject.transform.parent = go.transform;
        warpPos.transform.parent = go.transform;


        warpLine.objectFloor[0] = Utility.PositionToData(transform.position).x;
        warpLine.objectFloor[1] = Utility.PositionToData(warpPos.transform.position).x;

        LineRenderer line =
        go.AddComponent<LineRenderer>();

        line.widthMultiplier = 0.15f;

        string Path = "Material/LineMaterial";
        line.material = Resources.Load<Material>(Path);

        line.alignment = LineAlignment.View;

        lineVertex = LinePath(start, end);

        line.positionCount = lineVertex.Length;
        line.SetPositions(lineVertex);

        Color lineColor = new Color(0.4f, 0.6f, 0.5f, 0.8f);

        line.startColor = lineColor;
        line.endColor = lineColor;
    }

    private Vector3[] LinePath(Vector3 start, Vector3 end)
    {
        const float MAX_HEIGHT = 2.5f;
        const float ONE_RANGE = 0.05f;
        float maxRange = (start - end).magnitude;
        Vector3 now = start - (end - start).normalized * ONE_RANGE;
        Vector3 center = (start + end) / 2; //中心座標を求める  

        center = new Vector3(center.x, center.y + MAX_HEIGHT, center.z);    //ベジエ曲線の制御点
        besieData = new BesieData(start, end, center);

        List<Vector3> vec3s = new List<Vector3>();

        do
        {
            now = Vector3.MoveTowards(now, end, ONE_RANGE);
            float t = (start - now).magnitude / maxRange;
            vec3s.Add(Utility.Besie(start, end, center, t));
        }
        while (now != end);

        return vec3s.ToArray();
    }
    #endregion
}
