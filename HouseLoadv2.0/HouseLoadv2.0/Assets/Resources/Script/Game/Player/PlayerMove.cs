using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    PlayerTask playerTask;
    private const float MoveSpeed = 3f;         //移動速度
    private float playerRadius = 0.25f;         //プレイヤーの半径
    private const float RotationTime = 0.3f;    //回転速度※360°回転する時間
    // Start is called before the first frame update
    void Awake()
    {
        playerTask = GetComponent<PlayerTask>();

        GameObject go = new GameObject();
    }

    //プレイヤーの回転　　　　　　　　移動方向
    public void PlayerRotation(Vector3 rotation)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rotation), (360f / RotationTime) * Time.fixedDeltaTime);
    }

    public void Move()
    {
        Vector2 vec2 = playerTask.controllerTask.joyKey;   //Axisを取得

        //移動方向の取得
        Vector3 velocity =
            (new Vector3(playerTask.mainCamera.transform.forward.x, 0f, playerTask.mainCamera.transform.forward.z).normalized * vec2.y +
            playerTask.mainCamera.transform.right * vec2.x).normalized;

        //時間で管理する
        velocity *= Time.fixedDeltaTime;

        //移動後の地点を取得する
        Vector3 nextPos = transform.position + new Vector3(velocity.x, 0f, velocity.z) * MoveSpeed;

        //移動入力していたら回転する 回転は方向で決まるので計算前に大きさが変わっても問題ない
        if (vec2 != Vector2.zero)
            PlayerRotation(velocity);

        transform.position = ObjectOnNextPos(nextPos);

        UpdateData();
    }

    //物の計算をした次の座標
    public Vector3 ObjectOnNextPos(Vector3 nextPos)
    {
        Vector3Int[] nextPoss = Utility.PositionToData(nextPos, playerRadius);
        List<Vector3Int> dontPoss = new List<Vector3Int>();
        //プレイヤーが踏んでいるマス全て検索
        foreach (Vector3Int vec3s in nextPoss)
        {
            //全てのオブジェクト(当たり判定の可能性のあるもの)を検索
            foreach (MapObject mapOb in playerTask.gameTask.mapObjects)
            {
                //同じ場所かどうか       //移動できない場所かどうか
                if (vec3s == mapOb.pos && Array.IndexOf(playerTask.gameTask.Special.Block, mapOb.objectId) != -1)
                {
                    nextPos = FixPos(nextPos, mapOb.go.transform.position);
                }
            }

            //プレイヤーが範囲外にいる場合
            if (!playerTask.gameTask.InIfStageData(vec3s))
            {
                nextPos = FixPos(nextPos, Utility.DataToPosition(vec3s));
            }
        }

        return nextPos;
    }

    private Vector3 FixPos(Vector3 nextPos, Vector3 objPos)
    {
        //X軸を直すかX軸を直すか決める(めり込み具合が少ないほうを直す)
        if (Mathf.Abs(nextPos.x - objPos.x) > Mathf.Abs(nextPos.z - objPos.z))
        {
            nextPos.x = objPos.x + (StageCreateTask.ZX_Scale * 0.5f + playerRadius) * Utility.BoolToInt(nextPos.x - objPos.x > 0);
        }
        else
        {
            nextPos.z = objPos.z + (StageCreateTask.ZX_Scale * 0.5f + playerRadius) * Utility.BoolToInt(nextPos.z - objPos.z > 0);
        }

        return nextPos;
    }

    //データ上の場所変更
    private void UpdateData()
    {
        Vector3Int pos = Utility.PositionToData(transform.position);
        playerTask.positionLog = playerTask.position;
        playerTask.position = pos;

        //場所が変更されたら変更時の処理を行う
        if (playerTask.position != playerTask.positionLog)
            playerTask.objectChoice.UpdatePos(playerTask.positionLog, playerTask.position);
    }
}