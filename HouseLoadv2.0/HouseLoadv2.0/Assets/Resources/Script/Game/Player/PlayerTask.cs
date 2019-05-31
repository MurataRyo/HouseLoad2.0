using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTask : MonoBehaviour
{
    PlayerMove playerMove;
    // Start is called before the first frame update
    void Awake()
    {
        playerMove = gameObject.AddComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3Int NowPos()
    {
        int y = Mathf.RoundToInt(transform.position.y) - 1;
        y /= StageCreateTask.Y_Scale;

        int z = -Mathf.RoundToInt(transform.position.z);

        int x = Mathf.RoundToInt(transform.position.x);

        return new Vector3Int(y, z, x);
    }

}