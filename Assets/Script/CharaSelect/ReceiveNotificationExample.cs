using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ReceiveNotificationExample : MonoBehaviour
{

    public int playerNum;       //プレイヤー総数
    public Transform[] playerSpawnPos; //プレイヤースポーン位置配列


    public int count;

    private void Awake()
    {

    }

    private void Start()
    {
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

        //接続されたかどうかの確認ログ
        Debug.Log("プレイヤー" + playerNum + "が入室しました");
    }

    // プレイヤー退室時に受け取る通知(有効化されている場合のみ反応)
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        print($"プレイヤー#{playerInput.user.index + 1}が退室！");
    }
}