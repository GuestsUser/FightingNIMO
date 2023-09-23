using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
// ↓コピー用
#region (←の+で閉じる/開く)タイトル
/*ただのregionの説明用*/
#endregion



public class Menu : MonoBehaviour
{

    #region インスペクターからの代入が必要or調整をするもの
    [Header("【事前に入れるもの】")]
        /*【事前の代入が必須】*/
        [Tooltip("UIという名前のオブジェクトをアタッチしてください")]
        [SerializeField] private RectTransform ui;
        [Tooltip("GameStartスクリプトをアタッチしてください")]
        [SerializeField] private GameStart gameStart;
        [Tooltip("メニューグループ(空の親オブジェクト)の子要素のカーソルを入れてください")]
        [SerializeField] private GameObject cursor;
        [Tooltip("メニューの項目の数を指定し、UIオブジェクトを入れてください")]
        [SerializeField] private GameObject[] menuObj;
        [Tooltip("ロゴを入れてください")]
        [SerializeField] private GameObject logo;
        [Tooltip("選べるキャラクター")]
        [SerializeField] private GameObject[] characterUI;
        [Tooltip("プレイヤーインプットマネージャーを入れてください")]
        [SerializeField] private PlayerInputManager PIManeger;
    // 音
    private AudioSource audioSouce;
        [SerializeField] private AudioClip desisionSE;
        [SerializeField] private AudioClip cancelSE;
        [SerializeField] private AudioClip moveSE;
        [SerializeField] private AudioClip openMenuSE;
        [SerializeField] private AudioClip closeMenuSE;

        /*--------------------*/
    /*【調整するところ】*/
        [Tooltip("項目同士の縦の間隔(正の値を入力してください)")]
        [SerializeField] private float itemSpace;
        [Tooltip("カーソル移動時の時間間隔")]
        [SerializeField] private float interval;
        [Tooltip("選択項目のカラー")]
        [SerializeField] private Color selectionItemColor;
        [Tooltip("非選択項目のカラー")]
        [SerializeField] private Color notSelectionItemColor;
    /*------------------*/
    #endregion

    #region 動作確認用に表示するもの
    [Header("【動作確認用】")]
        [Tooltip("このスクリプトがアタッチされているオブジェクトが入る")]
        [SerializeField] private GameObject menu;
        [Tooltip("カーソルのレンダートランスフォーム情報が入る")]
        [SerializeField] private RectTransform cursorRT;
        [Tooltip("現在選択されているメニュー番号")]
        [SerializeField] private int menuNum;
        [Tooltip("現在選択されているメニュー名")]
        [SerializeField] private string menuName;
        [Tooltip("上下どちらかの入力がされているか")]
        [SerializeField] private bool push;
        [Tooltip("入力継続時間")]
        [SerializeField] private float count;
        [Tooltip("CreditかControlsを押したときの緩急の進み具合")]
        [SerializeField] private bool easTime;
        [Tooltip("CreditかControlsを押したときの切り替わり時間")]
        [SerializeField] private float duration;
    #endregion

    private string[] item; // 使用されているメニュー項目名を保存するstring型配列
    private Gamepad[] gamePad; // 接続されているゲームパッドを保存するstring型配列
    //private float fps;

    //private void Awake()
    //{
    //    gameObject.GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(0f,0f,true);
    //}

    // Start is called before the first frame update
    void Start()
    {
        /*【オブジェクト情報の取得】*/
        menu = this.gameObject;
        menu.SetActive(true);
        if(cursor == null) { Debug.LogError("カーソルオブジェクトがアタッチされていません"); }
        cursorRT = cursor.GetComponent<RectTransform>();
        audioSouce = GetComponent<AudioSource>();
        /*-------------------*/


        /*【事前の調整が必要な値が未調整だった場合】*/
        for (int i = 0; i < menuObj.Length; i++)
        {
            if (menuObj[i] == null) { Debug.LogError($"menuObj[{i}]にオブジェクトがアタッチされていません"); }
        }
        if (interval == 0) { interval = 10; }                                                                                           // 長押しでの選択項目を移動する時間間隔の設定
        if (itemSpace == 0) { itemSpace = 120; }                                                                                        // メニューの項目同士のY座標間隔
        if (selectionItemColor.a == 0) { selectionItemColor.a = 1; }                                                                    // 透明度をマックスに設定
        if (notSelectionItemColor.a == 0) { notSelectionItemColor.a = 1; }                                                              // 透明度をマックスに設定
        if (selectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#008fd9", out selectionItemColor); }        // 水色に設定
        if (notSelectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#ffffff", out notSelectionItemColor); }  // 白色に設定
        
        /*------------------------------------------*/

        /*【自動調節系】*/
        Array.Resize(ref item, menuObj.Length);                                                          // 配列のサイズをメニュー項目と同じ数に設定
        Array.Resize(ref gamePad, Gamepad.all.Count);
        
        for (int i = 0; i < menuObj.Length; i++)
        {
            menuObj[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);  // メニュー項目同士のY座標間隔を指定した間隔に設定
            menuObj[i].GetComponent<Text>().CrossFadeColor(notSelectionItemColor,0f,true,true);
            item[i] = menuObj[i].name;                                                                   // 配列の中にメニュー項目の名前を代入
        }
        
        for (int i = 0;i < characterUI.Length; i++)
        {
            characterUI[i].SetActive(false);
        }
        /*--------------*/

        /*【その他変数の初期化】*/
        menuName = item[0];
        menuNum = 0;
        push = false;
        count = 0;
        decision = false;
        /*----------------------*/

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            gamePad[i] = Gamepad.all[i];
        }
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        // カーソル移動関数
        if (decision == false)
        {
            CursorMove();
            Decision();
        }
        
        
    }
    void CursorMove()
    {
        #region 選択項目の変更
        // 左スティック上入力時 or 十字キー上入力時
        if (gamePad[0].leftStick.y.ReadValue() > 0 || gamePad[0].dpad.up.isPressed)
        {
            if (push == false) // 押された時の処理
            {
                push = true;
                if (--menuNum < 0) menuNum = menuObj.Length - 1;
                audioSouce.clip = moveSE;
                audioSouce.PlayOneShot(moveSE);
            }
            else               // 長押し時の処理
            {
                count++;
                if (count % interval == 0)
                {
                    if (--menuNum < 0) menuNum = menuObj.Length - 1;
                    audioSouce.clip = moveSE;
                    audioSouce.PlayOneShot(moveSE);
                }
            }

        }
        // 左スティック下入力時 or 十字キー下入力時
        else if (gamePad[0].leftStick.y.ReadValue() < 0 || gamePad[0].dpad.down.isPressed)
        {
            if (push == false)
            {
                push = true;
                if (++menuNum > menuObj.Length - 1) menuNum = 0;
                audioSouce.clip = moveSE;
                audioSouce.PlayOneShot(moveSE);
            }
            else
            {
                count++;
                if (count % interval == 0)
                {
                    if (++menuNum > menuObj.Length - 1) menuNum = 0;
                    audioSouce.clip = moveSE;
                    audioSouce.PlayOneShot(moveSE);
                }
            }
        }
        else
        {
            push = false;
            count = 0;
        }
        menuName = item[menuNum];
        #endregion

        #region カーソルの移動処理(項目の数によって自動でループ数が変更され、移動できるポジションも増減する)
        for (int i = 0; i < menuObj.Length; i++)
        {
            if (menuName == item[i])
            {
                // カーソル位置の変更
                cursorRT.position = menuObj[i].GetComponent<RectTransform>().position;

                // 文字の色の変更
                //menuObj[i].GetComponent<Text>().color = selectionItemColor;
                menuObj[i].GetComponent<Text>().CrossFadeColor(selectionItemColor, 0.1f, true, true);
                for (int j = 0; j < menuObj.Length; j++)
                {
                    if (item[j] != menuName)
                    {
                        //menuObj[j].GetComponent<Text>().color = notSelectionItemColor;
                        menuObj[j].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.1f, true, true);
                    }
                }
            }
        }
        #endregion
    }
    

    void Decision()
    {

        // 決定ボタン(aボタン)を押したとき
        if (gamePad[0].aButton.wasPressedThisFrame)
        {
            decision = true; // 決定フラグON

            switch (menuNum)
            {
                /*【GAME】*/
                case 0:
                    StartCoroutine("PushGame");
                    break;
                /*-------*/

                /*【CREDIT】*/
                case 1:
                    StartCoroutine("PushCredit");
                    break;
                /*---------*/

                /*【CONTROLS】*/
                case 2:
                    StartCoroutine("PushControls");
                    break;
                /*-----------*/

                /*【EXIT】*/
                case 3:
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                    break;
                /*-------*/

                default:
                    break;
            }
        }


        //// 一番上の項目が選択されている状態で決定を押した時
        //if (menuName == item[0] && gamePad[0].buttonSouth.wasPressedThisFrame)
        //{
        //    decision = true;
        //    StartCoroutine("PushGame");
            
        //}

        //// 四番目の項目が選択されている状態で決定を押した時（switchでまとめれるかも）
        //if(menuName == item[3] && gamePad[0].buttonSouth.wasPressedThisFrame)
        //{
        //    #if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //    #else
        //        Application.Quit();
        //    #endif
        //}
    }

    void CursorFade()
    {
        cursor.GetComponent<RawImage>().CrossFadeAlpha(0, 0.25f, true);                           // カーソルを徐々に消す
        menuObj[0].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.25f, true, true); // 文字を徐々に白色に戻す
    }
    private IEnumerator PushGame()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため

        gameStart.onCharaSelect = true;
        for (int i = 0; i < characterUI.Length; i++)
        {
            characterUI[i].SetActive(true);
        }
        logo.SetActive(false);
        menu.SetActive(false);
    }
    private IEnumerator PushCredit()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため

        /*【CREDITを実装したときの処理】*/
        ui.anchoredPosition = new Vector2(1920, 0);
        /*--------------------------*/

        //logo.SetActive(false);
        //menu.SetActive(false);
    }

    private IEnumerator PushControls()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため

        /*【CONTROLSを選択したときの処理】*/
       
        ui.anchoredPosition = new Vector2(-1920, 0);
        /*--------------------------*/

        //logo.SetActive(false);
        //menu.SetActive(false);
    }
}
