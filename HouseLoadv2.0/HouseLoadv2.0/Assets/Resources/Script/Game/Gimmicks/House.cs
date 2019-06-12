using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    GameTask gameTask;
    private void Start()
    {
        gameTask = Utility.GetGameTask();
    }

    //クリア時の処理
    public void StageCrear()
    {
        MoveObject moveObject = Move(gameTask.playerTask.gameObject);
        MoveObject moveObject2 = new MoveObject();
        moveObject2.ie = EndMove();
        moveObject.endEvent = moveObject2;
        gameTask.moveObjectTask.MoveObject(moveObject, true);
    }

    IEnumerator EndMove()
    {
        gameTask.uiTask.NewTextUi("STAGE CREAR",new Vector2(310f,-300f),Color.black,200);
        gameTask.gameMode = GameTask.GameMode.Crear;
        yield break;
    }

    MoveObject Move(GameObject player)
    {
        Vector3[] movePoint = new Vector3[] { player.transform.position, transform.position };
        return new MoveObject(player, movePoint, 3f);
    }
}
