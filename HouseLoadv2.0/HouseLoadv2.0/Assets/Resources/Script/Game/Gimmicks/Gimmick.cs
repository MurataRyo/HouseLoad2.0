using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick : MonoBehaviour
{
    public UseBase useBase;
    public GameTask gameTask;

    // Start is called before the first frame update
    public virtual void Start()
    {
        gameTask = Utility.GetGameTask();
        UseSet();
    }

    public virtual void UseSet()
    {

    }
}


//使用条件のクラス
public class UseBase
{
    public int[] itemNum;      //使用できるアイテム番号※何も必要ない場合は-1を入れる
    public string[] message;   //説明欄※itemNumと配列番号は同期
    public string[] noMessage; //持ち物がなかった時の説明欄
    public UseBase(int[] itemNum, string[] message, string[] noMessage)
    {
        this.itemNum = itemNum;
        this.message = message;
        this.noMessage = noMessage;
    }

    //特殊な使用条件
    public bool UseIf()
    {
        return true;
    }
}

