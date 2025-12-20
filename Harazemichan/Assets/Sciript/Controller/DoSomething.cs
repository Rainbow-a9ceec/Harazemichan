// Unityでシリアル通信で送られてくるデータをデコードする雛形
// 2025_8月Ver.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSomething : MonoBehaviour
{
    // 制御対象のオブジェクト用に宣言しておいて、Start関数内で名前で検索
    [SerializeField]GameObject targetObject;

    // 制御対象にアタッチされたスクリプト
    [SerializeField]LoadValue targetScript;

    // シリアル通信のクラス、クラス名は正しく書くこと
    public SerialHandler serialHandler;

    GameObject harazemichanObject;

 

    void Start()
    {
        // 制御対象のオブジェクトを取得、このオブジェクトにMain.csが関連付けられている
        //targetObject = GameObject.Find("PlayerObject"); // この記述ではUnityのヒエラルキーにGameMasterオブジェクトがいる必要がある。

        // 制御対象にアタッチされたスクリプトを取得。
        // 大文字、小文字を区別するので、player.csを作ったのなら「p」layer。

        // 信号受信時に呼ばれる関数としてOnDataReceived関数を登録
        serialHandler.OnDataReceived += OnDataReceived;

        ResetFlag();
    }

    void Update()
    {

        if(targetScript.isChecking)
        {
            SendValueStart();
        }

        //if(targetScript.CheckValue())
        //{
        //    PlayShakeSound();
        //}

        if(targetScript.endShake)
        {
            ChangeSendValue();  
        }
        if (targetScript.decideAngle)
        {
            DecideAngle();
        }
        // UnityからArduinoに送る場合はココに記述
        //string command = "hogehoge";
        //serialHandler.Write(command);

        // UnityからArduinoに送る場合はココに記述
        //if (targetScript.jklPress[0])
        //{
        //    targetScript.jklPress[0] = false;
        //    if (targetScript.jklToggle[0])
        //    {
        //        // LED ON
        //        serialHandler.Write("a");
        //    }
        //    else
        //    {
        //        // LED OFF
        //        serialHandler.Write("b");
        //    }
        //}

        //if (targetScript.jklPress[1])
        //{
        //    targetScript.jklPress[1] = false;
        //    if (targetScript.jklToggle[1])
        //    {
        //        // LED ON
        //        serialHandler.Write("c");
        //    }
        //    else
        //    {
        //        // LED OFF
        //        serialHandler.Write("d");
        //    }
        //}

        //if (targetScript.jklPress[2])
        //{
        //    targetScript.jklPress[2] = false;
        //    if (targetScript.jklToggle[2])
        //    {
        //        // LED ON
        //        serialHandler.Write("e");
        //    }
        //    else
        //    {
        //        // LED OFF
        //        serialHandler.Write("f");
        //    }
        //}
    }

    void ResetFlag()
    {
        serialHandler.Write("0");
    }

    void SendValueStart()
    {
        serialHandler.Write("a");
    }

    void ChangeSendValue()
    {
        serialHandler.Write("b");
    }

    void DecideAngle()
    {
        serialHandler.Write("c");
    }

    void PlayShakeSound()
    {
        serialHandler.Write("1");
    }

    //受信した信号(message)に対する処理
    void OnDataReceived(string message)
    {
        if (message == null)
            return;

        // ここでデコード処理等を記述
        string receivedData;
        int t;

        receivedData=message.Substring(1,6);
        
        int.TryParse(receivedData, out t);
        targetScript.value = t;

        if (targetScript.endShake)
        {
            receivedData = message.Substring(7,6);

            int.TryParse (receivedData, out t);
            targetScript.value2 = t;


            receivedData = message.Substring(13, 1);

            Debug.Log(receivedData);
            
            if(receivedData!="E")
            {
                int.TryParse(receivedData, out t);
                targetScript.swValue = t;
            }
            
        }
    }
}
