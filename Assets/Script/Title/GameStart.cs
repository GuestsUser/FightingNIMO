using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{

    #region シーン系
    [Tooltip("シーン移動フラグ")]
    [SerializeField] private bool pushScene;
    public bool _pushScene { get { return pushScene; } } //他のスクリプトでも操作を制御する用
    [Tooltip("シーン移動時の待機時間(この間にSEが鳴る)")]
    [SerializeField] private float waitTime; //シーン移動時に使用する処理待機時間(SEが鳴り終わるまで)
    #endregion

    [Tooltip("準備okフラグ(このフラグがオンの時スタートボタンを押すとゲームシーンに移動)")]
    [SerializeField] private bool ready;


    // Start is called before the first frame update
    void Start()
    {
        #region シーン系
        pushScene = false;
        waitTime = 2.0f;
        #endregion
        ready = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(pushScene == false)
        {
            if (ready == true && Gamepad.current.startButton.isPressed)
            {
                pushScene = true;
                //Debug.Log("start");
                StartCoroutine("GotoGameScene");
            }
        }
       
    }
    private IEnumerator GotoGameScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); //処理を待機 シーン時の音を鳴らすため
        SceneManager.LoadScene("GameScene");
    }
}
