using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ReceiveNotificationExample : MonoBehaviour
{

    public int playerNum;       //プレイヤー番号情報
    public int characterNum;    //キャラクター番号
    public Transform[] playerSpawnPos; //プレイヤースポーン位置配列

    public Button[] button;

    //public string[] controllerNames;
    //int controllerCount;
  

    private void Awake()
    {

    }

    private void Start()
    {
    }

    private void Update()
    {

    }

    //プレイヤー入室時に受け取る通知
    //ゲームパッドまたはキーボードのAボタンを押すことでプレイヤーを作成
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        //プレイヤー番号を0ではなく１から始めるようにしている
        playerNum = playerInput.playerIndex + 1;

        //OnClickボタンの項目にキャラクターセレクト関数を追加（ファイル内のPrefabからでもOK）
        //button[0].onClick.AddListener(() => playerInput.gameObject.GetComponent<CharacterCreation>().Select(0));
        //sbutton[1].onClick.AddListener(() => playerInput.gameObject.GetComponent<CharacterCreation>().Select(1));
        //button[2].onClick.AddListener(() => playerInput.gameObject.GetComponent<CharacterCreation>().Select(2));
        //button[3].onClick.AddListener(() => playerInput.gameObject.GetComponent<CharacterCreation>().Select(3));

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