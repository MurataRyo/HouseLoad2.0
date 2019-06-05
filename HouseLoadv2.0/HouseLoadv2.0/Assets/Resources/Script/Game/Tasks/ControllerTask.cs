using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTask : MonoBehaviour
{
    public Vector2 joyKey = Vector2.zero;
    public Vector2 joyKeyLog;
    enum Controller
    {
        Keyboard,
    }
    Controller controller = Controller.Keyboard;

    // Update is called once per frame
    void Update()
    {
        JoyUpdate();
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
        Vector2 axis = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            axis.y++;

        if (Input.GetKey(KeyCode.S))
            axis.y--;

        if (Input.GetKey(KeyCode.D))
            axis.x++;

        if (Input.GetKey(KeyCode.A))
            axis.x--;

        joyKey = axis;
    }

    public bool EnterButton()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

    public bool BackButton()
    {
        return Input.GetKeyDown(KeyCode.Z);
    }

    public bool CameraModeChangeButton()
    {
        return Input.GetKeyDown(KeyCode.X);
    }

    public bool SerectKey(bool flag)
    {
        return Input.GetKeyDown(flag ? KeyCode.Q : KeyCode.E);
    }
}
