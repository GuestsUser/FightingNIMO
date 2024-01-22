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
    public bool isNextScene;
    [Tooltip("準備OKかどうか")]
    public bool isReady;
    [Tooltip("選択されたキャラクター番号を格納する")]
    public int[] selectCharacterNumber;

    public int conectCount;     //接続しているキャラ数
    public int submitCharCount; //選択が確定されたキャラ数を記録

    void Start()
    {
        waitTime = 2.0f;    //コルーチンの待機時間を2.0秒に設定する       

        Array.Resize(ref selectCharacterNumber, 4);
        for (int i = 0; i < selectCharacterNumber.Length; i++)
        {
            selectCharacterNumber[i] = -1;
        }

        //flgの初期化
        isNextScene = false;
        isReady = false;
        isCharSelect = false;
    }

    void Update()
    {
        CheckReady();   //Ready状態ONとOFFの条件関数

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

    //Ready状態ONとOFFの条件関数
    private void CheckReady()
    {
        //hierarchyにあるCharSelectManagerを持つオブジェクトの配列を作成
        CharSelectManager[] objectsWithScripts = FindObjectsOfType<CharSelectManager>();

        conectCount = objectsWithScripts.Length; //選択数初期化
        submitCharCount = 0;

        foreach (CharSelectManager obj in objectsWithScripts)
        {
            if (obj.isCharSelected) { submitCharCount++; } //確定されていればカウント加算
            //Debug.Log(submitCharCount);
        }

        //キャラクターセレクト画面かつ1人でもキャラクターを選択していない,または接続数が1以下の場合
        if (isCharSelect && submitCharCount < conectCount || conectCount <= 1)
        {
            readyUI.SetActive(false);   //ReadyのUIを非表示にする
            isReady = false;            //Ready状態を解除する
        }
        //キャラクターセレクト画面かつ全員がキャラクターを選択している、接続数が1以上の場合
        else if (isCharSelect && submitCharCount >= conectCount && conectCount > 1)
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