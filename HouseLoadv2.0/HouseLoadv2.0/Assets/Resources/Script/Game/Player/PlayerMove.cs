﻿using System.Collections;
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
    public void PlayerRotation(Vector3 velocity, float speed)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity), (360f / RotationTime) * Time.deltaTime * speed);
    }

    public void Move()
    {
        if (playerTask.gameTask.eventCount != 0)
            return;

        Vector2 vec2 = playerTask.controllerTask.joyKey;   //Axisを取得

        //移動方向の取得
        Vector3 velocity =
            (new Vector3(playerTask.mainCamera.transform.forward.x, 0f, playerTask.mainCamera.transform.forward.z).normalized * vec2.y +
            playerTask.mainCamera.transform.right * vec2.x).normalized;

        //時間で管理する
        velocity *= Time.deltaTime;

        //移動後の地点を取得する
        Vector3 nextPos = transform.position + new Vector3(velocity.x, 0f, velocity.z) * MoveSpeed;

        //移動入力していたら回転する 回転は方向で決まるので計算前に大きさが変わっても問題ない
        if (vec2 != Vector2.zero)
        {
            UpdateData();
            PlayerRotation(velocity, 1f);
        }
        transform.position = ObjectOnNextPos(nextPos);
        //transform.position = nextPos;
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
    public void UpdateData()
    {
        Vector3Int pos = Utility.PositionToData(transform.position);
        playerTask.pos = pos;

        //変更時の処理を行う
        playerTask.objectChoice.UpdatePos(playerTask.pos);
    }

    //物を動かしたりするときに使う
    public IEnumerator NowPosMoveAndRotation(Vector3Int nextPos,Vector3Int fastMasu)
    {
        playerTask.gameTask.eventCount++;

        Vector3 movePos = Utility.DataToPosition(fastMasu);
        Vector3 velocity = movePos - transform.position;
        
        while (transform.position != movePos)
        {
            PlayerRotation(velocity, Time.fixedDeltaTime / Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, movePos, MoveSpeed * Time.deltaTime);
            yield return null;
        }

        if (nextPos == transform.position)
            yield break;

        velocity = Utility.DataToPosition(nextPos) - transform.position;
        while (transform.rotation != Quaternion.LookRotation(velocity))
        {
            PlayerRotation(velocity, Time.fixedDeltaTime / Time.deltaTime);
            yield return null;
        }

        playerTask.gameTask.eventCount--;
        yield break;
    }
}