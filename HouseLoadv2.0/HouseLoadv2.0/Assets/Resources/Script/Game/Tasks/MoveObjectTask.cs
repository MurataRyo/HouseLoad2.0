using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectTask : MonoBehaviour
{
    GameTask gameTask;
    int uiCount;        //0ならUiをつけていい
    GameObject choiceUi;
    void Start()
    {
        gameTask = Utility.GetGameTask();
    }

    public void MoveObject(MoveObject moveObjects, bool EventIf)
    {
        StartCoroutine(Move(moveObjects, EventIf));
    }

    public IEnumerator Move(MoveObject moveObject, bool EventIf)
    {
        if (choiceUi == null)
        {
            choiceUi = gameTask.playerTask.objectChoice.choiceMark;
        }

        if (EventIf)
        {
            uiCount++;
            gameTask.eventCount++;
            choiceUi.SetActive(false);
        }

        if (moveObject.startEvent != null)
            yield return Move(moveObject.startEvent,EventIf);

        yield return moveObject.ie;

        if (moveObject.endEvent != null)
            yield return Move(moveObject.endEvent, EventIf);

        if (EventIf)
        {
            uiCount--;
            gameTask.eventCount--;
            if (uiCount == 0)
            {
                gameTask.playerTask.objectChoice.ChoiceUiSet();
            }
        }

        gameTask.playerTask.playerMove.UpdateData();
        yield break;
    }

    public void MoveObject(MoveObject[] moveObjects, bool EventIf)
    {
        StartCoroutine(Move(moveObjects, EventIf));
    }

    public IEnumerator Move(MoveObject[] moveObjects, bool EventIf)
    {
        if (choiceUi == null)
        {
            choiceUi = gameTask.playerTask.objectChoice.choiceMark;
        }

        if (EventIf)
        {
            uiCount++;
            gameTask.eventCount++;
            choiceUi.SetActive(false);
        }

        foreach (MoveObject moveObject in moveObjects)
        {
            if (moveObject.startEvent != null)
                yield return Move(moveObject.startEvent, EventIf);
        }

        foreach (MoveObject moveObject in moveObjects)
        {
            yield return moveObject.ie;
        }


        foreach (MoveObject moveObject in moveObjects)
        {
            if (moveObject.endEvent != null)
                yield return Move(moveObject.endEvent, EventIf);
        }

        if (EventIf)
        {
            uiCount--;
            gameTask.eventCount--;
            if (uiCount == 0)
            {
                choiceUi.SetActive(true);
            }
        }

        gameTask.playerTask.playerMove.UpdateData();
        yield break;
    }
}

public class MoveObject
{
    public IEnumerator ie;
    public MoveObject endEvent;
    public MoveObject startEvent;
    //          移動するオブジェクト          通過点         1秒で進む距離
    public MoveObject(GameObject go, Vector3[] movePoints, float moveSpeed)
    {
        ie = enumerator(go, movePoints, moveSpeed);
    }

    public MoveObject()
    {

    }

    //移動式のコルーチン
    public IEnumerator enumerator(GameObject go, Vector3[] movePoints, float moveSpeed)
    {
        int count = 0;
        float now = 0f;
        float NextRange = (movePoints[count] - movePoints[count + 1]).magnitude;
        //移動が完了するまで抜け出さない
        while (movePoints[movePoints.Length - 1] != go.transform.position)
        {
            now += Time.deltaTime * moveSpeed;
            while (now > NextRange)
            {
                if (count == movePoints.Length - 2)
                {
                    go.transform.position = movePoints[movePoints.Length - 1];
                    yield break;
                }

                now -= NextRange;
                count++;
                NextRange = (movePoints[count] - movePoints[count + 1]).magnitude;
            }
            go.transform.position = Vector3.MoveTowards(movePoints[count], movePoints[count + 1], now);

            yield return null;
        }
    }
}