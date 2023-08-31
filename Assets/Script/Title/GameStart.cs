using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [Tooltip("現在の画面がキャラクターセレクト画面かどうか")]
    [SerializeField] public bool onCharaSelect;

    #region シーン系
    [Tooltip("シーン移動フラグ")]
    [SerializeField] private bool pushScene;
    public bool _pushScene { get { return pushScene; } } //他のスクリプトでも操作を制御する用
    [Tooltip("シーン移動時の待機時間(この間にSEが鳴る)")]
    [SerializeField] private float waitTime; //シーン移動時に使用する処理待機時間(SEが鳴り終わるまで)
    #endregion

    [Tooltip("準備okフラグ(このフラグがオンの時スタートボタンを押すとゲームシーンに移動)")]
    [SerializeField] private bool ready;

    [SerializeField] private ReceiveNotificationExample receiveNotificationExample;
    private bool allSubmit;
    [SerializeField] private bool[] isSubmit;

    [SerializeField] private GameObject readyUI;


    // Start is called before the first frame update
    void Start()
    {
        #region シーン系
        pushScene = false;
        waitTime = 2.0f;
        #endregion
        ready = false;
        onCharaSelect = false;
    }

    // Update is called once per frame
    void Update()
    {
        Array.Resize(ref isSubmit, receiveNotificationExample.playerNum);

        CheckReady();

        if (pushScene == false)
        {
            if (ready == true && Gamepad.current.startButton.isPressed)
            {
                pushScene = true;
                //Debug.Log("start");
                StartCoroutine("GotoGameScene");
            }
        }
       
    }

    private void CheckReady()
    {
        for(int i = 0; i < receiveNotificationExample.playerNum; i++)
        {
            Debug.Log($"Player{i}");
            if(receiveNotificationExample.count == 0)
            {
                isSubmit[i] = GameObject.Find($"Player{i}").GetComponent<SelectCharacter>().charSubmitFlg;
            }
            
        }

        for (int i = 0; i < receiveNotificationExample.playerNum; i++)
        {

            if (isSubmit[i] == false)
            {
                // ひとつでもfalseであればfor文を抜ける
                Debug.Log("キャラクター選択中です");
                readyUI.SetActive(false);
                ready = false;
                break;
            }

            // 最後のフラグにたどり着いたとき
            if (isSubmit[receiveNotificationExample.playerNum-1])
            {
                Debug.Log("準備ok");
                readyUI.SetActive(true);
                ready = true;
            }
        }
    }

    private IEnumerator GotoGameScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); //処理を待機 シーン時の音を鳴らすため
        SceneManager.LoadScene("GameScene");
    }
}
