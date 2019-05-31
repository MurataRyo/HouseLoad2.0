using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTask : MonoBehaviour
{
    public Vector2 joyKey;
    public Vector2 joyKeyLog;
    enum Controller
    {
        Keyboard,
    }
    Controller controller = Controller.Keyboard;

    // Update is called once per frame
    void Update()
    {

    }

    void JoyUpdate()
    {
        joyKeyLog = joyKey;
        switch (controller)
        {
            case Controller.Keyboard:
                KyeboardUpdate();
                break;
        }
    }

    void KyeboardUpdate()
    {
        float x = 0;
        float y = 0;

        if (Input.GetKey(KeyCode.W))
            y++;

        if (Input.GetKey(KeyCode.S))
            y--;

        if (Input.GetKey(KeyCode.D))
            x++;

        if (Input.GetKey(KeyCode.A))
            x--;
    }
}
