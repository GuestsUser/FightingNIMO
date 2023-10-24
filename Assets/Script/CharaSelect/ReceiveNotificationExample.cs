using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ReceiveNotificationExample : MonoBehaviour
{

    public int playerNum;       //プレイヤー総数

    //追加
    public int[] testNum;

    public Transform[] playerSpawnPos; //プレイヤースポーン位置配列


    public int count;

    private void Start()
    {
        //追加
        Array.Resize(ref testNum, 4);
        //追加
        for (int i = 0; i < testNum.Length; i++)
        {
            testNum[i] = -1;
        }
        count = 2;
    }

    private void Update()
    {
        if (--count < 0) { count = 0; }
    }

    //プレイヤー入室時に受け取る通知
    //ゲームパッドまたはキーボードのAボタンを押すことでプレイヤーを作成
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        count = 2;

        playerNum = playerInput.playerIndex + 1;    //プレイヤー総数を0ではなく１から始めるようにしている

        for (int i = 0; i < testNum.Length; i++)
        {
            if(testNum[i] == -1)
            {
                testNum[i] = i + 1;
                break;
            }
            else if (testNum[i] != i + 1 && playerInput.gameObject != null)
            {
                testNum[i] = i + 1;
                break;
            }
            else {
                continue;
            }
        }

        //プレイヤー番号によって配置位置を変更している
        switch (playerNum)
        {
            //Player1
            case 1:
                playerInput.gameObject.transform.position = playerSpawnPos[0].position;
                break;
            //Player2
            case 2:
                playerInput.gameObject.transform.position = playerSpawnPos[1].position;
                break;
            //Player3
            case 3:
                playerInput.gameObject.transform.position = playerSpawnPos[2].position;
                break;
            //Player4
            case 4:
                playerInput.gameObject.transform.position = playerSpawnPos[3].position;
                break;
            //何もない
            default:
                return;
        }

        //for (int i = 0; i < testNum.Length; i++) {
        //    switch (testNum[i])
        //    {
        //        //Player1
        //        case 1:
        //            playerInput.gameObject.transform.position = playerSpawnPos[0].position;
        //            break;
        //        //Player2
        //        case 2:
        //            playerInput.gameObject.transform.position = playerSpawnPos[1].position;
        //            break;
        //        //Player3
        //        case 3:
        //            playerInput.gameObject.transform.position = playerSpawnPos[2].position;
        //            break;
        //        //Player4
        //        case 4:
        //            playerInput.gameObject.transform.position = playerSpawnPos[3].position;
        //            break;
        //        //何もない
        //        default:
        //            return;
        //    }
        //}

        //接続されたかどうかの確認ログ
        //Debug.Log("プレイヤー" + playerNum + "が入室しました");
        //Debug.Log("playerIndex : " + playerInput.playerIndex);
    }

    // プレイヤー退室時に受け取る通知(有効化されている場合のみ反応)
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        for (int i = 0; i < testNum.Length; i++)
        {
            if (testNum[i] != -1)
            {
                testNum[i] = -1;
                break;
            }
        }


        //print($"プレイヤー#{playerInput.user.index + 1}が退室！");
    }
}