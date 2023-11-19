//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

//public class GameStartSystem : MonoBehaviour
//{
//    [Tooltip("現在の画面がキャラクター")]
//    public bool isCharSelect;
//    [Tooltip("ゲームシーン移動フラグ")]
//    [SerializeField] private bool isNextScene;
//    [Tooltip("シーン移動時の待機時間(この間にSEが鳴る)")]
//    [SerializeField] private float waitTime; //シーン移動時に使用する処理待機時間(SEが鳴り終わるまで)

//    [Tooltip("準備OKかどうか")]
//    [SerializeField] private bool isReady;
//    [Tooltip("キャラクターが選択されているかどうか")]
//    [SerializeField] private bool[] isSelected;

//    [Tooltip("ReceiveNotificationExample.csを持ったオブジェクトを入れる")]
//    [SerializeField] private ReceiveNotificationExample receiveNotificationExample;
//    [Tooltip("ReadyUIオブジェクトを入れる")]
//    [SerializeField] private GameObject readyUI;

//    private Gamepad[] gamePads; //（どこでも必要な可能性有り）

//    void Start()
//    {
//        waitTime = 2.0f;    //コルーチンの待機時間を2.0秒に設定する       

//        //flgの初期化
//        isNextScene = false;
//        isReady = false;
//        isCharSelect = false;

//        //gamePadsの大きさを接続されているゲームパッドの数に合わせる
//        Array.Resize(ref gamePads, Gamepad.all.Count);
//        for(int i = 0; i < gamePads.Length; i++)
//        {

//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Array.Resize(ref isSelected, receiveNotificationExample.playerNum);

//        CheckReady();

//        if (isNextScene == false)
//        {
//            //Readyの状態かつゲームパッドのStartButtonが押されたら
//            if (isReady == true && Gamepad.current.startButton.isPressed)
//            {
//                isNextScene = true;
//                StartCoroutine("GotoGameScene");
//            }
//        }
//    }

//    private void CheckReady()
//    {
//        //for (int i = 0; i < receiveNotificationExample.playerNum; i++)
//        //{
//        //    //
//        //    if (receiveNotificationExample.count == 0)
//        //    {
//        //        //各キャラクターごとにキャラクターを選択したかどうかの値を取得する
//        //        //isSelected[i] = GameObject.Find($"Player{i}").GetComponent<CharSelectManager>().isCharSelected;
//        //        isSelected[i] = GameObject.FindWithTag("SelectPlayer").GetComponent<CharSelectManager>().isCharSelected;
//        //    }

//        //}

//        //for (int i = 0; i < receiveNotificationExample.playerNum; i++)
//        //{
//        //    // もしひとつでもキャラクターが選択されていなかったらfor文を抜ける
//        //    if (isSelected[i] == false)
//        //    {

//        //        readyUI.SetActive(false);   //ReadyのUIを非表示にする
//        //        isReady = false;            //Ready状態を解除する
//        //        break;                      //for文を抜ける
//        //    }

//        //    //もし全てのプレイヤーがキャラクターを選択していたら
//        //    if (isSelected[receiveNotificationExample.playerNum - 1])
//        //    {
//        //        readyUI.SetActive(true);    //ReadyのUIを表示する
//        //        isReady = true;             //Ready状態にする
//        //    }
//        //}

//        //随時プレイヤーの数に応じて配列の大きさを変更する必要がある
//        for (int i = 0; i < receiveNotificationExample.playerNum; i++)
//        {
//            //
//            if (receiveNotificationExample.count == 0)
//            {
//                //各キャラクターごとにキャラクターを選択したかどうかの値を取得する
//                //isSelected[i] = GameObject.Find($"Player{i}").GetComponent<CharSelectManager>().isCharSelected;
//                isSelected[i] = GameObject.FindWithTag("SelectPlayer").GetComponent<CharSelectManager>().isCharSelected;
//            }

//        }

//        for (int i = 0; i < receiveNotificationExample.playerNum; i++)
//        {
//            // もしひとつでもキャラクターが選択されていなかったらfor文を抜ける
//            if (isSelected[i] == false)
//            {

//                readyUI.SetActive(false);   //ReadyのUIを非表示にする
//                isReady = false;            //Ready状態を解除する
//                break;                      //for文を抜ける
//            }

//            //もし全てのプレイヤーがキャラクターを選択していたら
//            if (isSelected[receiveNotificationExample.playerNum - 1])
//            {
//                readyUI.SetActive(true);    //ReadyのUIを表示する
//                isReady = true;             //Ready状態にする
//            }

//        }
//    }

//    private IEnumerator GotoGameScene()
//    {
//        yield return new WaitForSecondsRealtime(waitTime); //処理を待機 シーン時の音を鳴らすため
//        SceneManager.LoadScene("GameScene");
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStartSystem : MonoBehaviour
{
    [Tooltip("ReceiveNotificationExample.csを持ったオブジェクトを入れる")]
    [SerializeField] private ReceiveNotificationExample receiveNotificationExample;
    [Tooltip("ReadyUIオブジェクトを入れる")]
    [SerializeField] private GameObject readyUI;

    [Tooltip("シーン移動時の待機時間(この間にSEが鳴る)")]
    [SerializeField] private float waitTime; //シーン移動時に使用する処理待機時間(SEが鳴り終わるまで)

    [Tooltip("現在の画面がキャラクター")]
    public bool isCharSelect;
    [Tooltip("ゲームシーン移動フラグ")]
    [SerializeField] private bool isNextScene;
    [Tooltip("準備OKかどうか")]
    [SerializeField] public bool isReady;
    [Tooltip("キャラクターが選択されているかどうか")]
    [SerializeField] private bool[] isSelected;

    private bool flgCheck;  //1人でもキャラクターを選択してない時のフラグ

    void Start()
    {
        waitTime = 2.0f;    //コルーチンの待機時間を2.0秒に設定する       

        //flgの初期化
        isNextScene = false;
        isReady = false;
        isCharSelect = false;


        flgCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        Array.Resize(ref isSelected, Gamepad.all.Count);    //接続されているゲームパッドの数に配列の大きさを随時変更する

        CheckReady();

        //GameSceneへ行くための処理
        if (isNextScene == false)
        {
            //Readyの状態かつゲームパッドのStartButtonが押されたら
            if (isReady == true && Gamepad.current.startButton.isPressed)
            {
                isNextScene = true;
                StartCoroutine("GotoGameScene");
            }
        }
    }

    private void CheckReady()
    {
        //旧バージョン
        //for (int i = 0; i < receiveNotificationExample.playerNum; i++)
        //{
        //    //
        //    if (receiveNotificationExample.count == 0)
        //    {
        //        //各キャラクターごとにキャラクターを選択したかどうかの値を取得する
        //        //isSelected[i] = GameObject.Find($"Player{i}").GetComponent<CharSelectManager>().isCharSelected;
        //        isSelected[i] = GameObject.FindWithTag("SelectPlayer").GetComponent<CharSelectManager>().isCharSelected;
        //    }

        //}

        //for (int i = 0; i < receiveNotificationExample.playerNum; i++)
        //{
        //    // もしひとつでもキャラクターが選択されていなかったらfor文を抜ける
        //    if (isSelected[i] == false)
        //    {

        //        readyUI.SetActive(false);   //ReadyのUIを非表示にする
        //        isReady = false;            //Ready状態を解除する
        //        break;                      //for文を抜ける
        //    }

        //    //もし全てのプレイヤーがキャラクターを選択していたら
        //    if (isSelected[receiveNotificationExample.playerNum - 1])
        //    {
        //        readyUI.SetActive(true);    //ReadyのUIを表示する
        //        isReady = true;             //Ready状態にする
        //    }
        //}

        //新バージョン
        //hierarchyにあるCharSelectManagerを持つオブジェクトの配列を作成
        CharSelectManager[] objectsWithScripts = FindObjectsOfType<CharSelectManager>();

        foreach (CharSelectManager obj in objectsWithScripts)
        {
            bool value = obj.isCharSelected;    //キャラクターが選択されているかどうかの値を代入

            //1つでもfalseがあればフラグを立てる
            if (!value)
            {
                flgCheck = true;
                break;
            }
            else
            {
                flgCheck = false;
            }
        }

        //キャラクターセレクト画面かつ1人でもキャラクターを選択していない場合
        if (isCharSelect && flgCheck)
        {
            readyUI.SetActive(false);   //ReadyのUIを非表示にする
            isReady = false;            //Ready状態を解除する
        }
        //キャラクターセレクト画面かつ全員がキャラクターを選択している場合
        else if (isCharSelect && !flgCheck)
        {
            readyUI.SetActive(true);    //ReadyのUIを表示する
            isReady = true;             //Ready状態にする
        }
    }

    //待機時間処理
    private IEnumerator GotoGameScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); //処理を待機 シーン時の音を鳴らすため
        SceneManager.LoadScene("GameScene");
    }
}