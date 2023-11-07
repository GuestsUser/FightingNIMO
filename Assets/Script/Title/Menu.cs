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
        [SerializeField] private GameStartSystem gameStart;
        [Tooltip("メニューグループ(空の親オブジェクト)の子要素のカーソルを入れてください")]
        [SerializeField] private GameObject cursor;
        [Tooltip("メニューの項目の数を指定し、UIオブジェクトを入れてください")]
        [SerializeField] private GameObject[] menuObj;
        [Tooltip("ロゴを入れてください")]
        [SerializeField] private GameObject logo;
        [Tooltip("選べるキャラクター")]
        [SerializeField] private GameObject[] characterUI;
        [Tooltip("CreditのUIを入れてください")]
        [SerializeField] private GameObject creditUI;
        [Tooltip("ControlsのUIを入れてください")]
        [SerializeField] private GameObject controlsUI;
        [Tooltip("プレイヤーインプットマネージャーを入れてください")]
        [SerializeField] private PlayerInputManager PIManeger;
        [Tooltip("カメラオブジェクトを入れてください")]
        [SerializeField] private Camera cam;
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
        [Tooltip("『GAME』を押した時のカメラのターゲットポジションを入れてください")]
        [SerializeField] private Vector3 TPos;
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
        [Tooltip("CreditかControlsを押したときの緩急の進み具合(0~1の点P)")]
        [SerializeField] private bool decision;
        [Tooltip("CreditかControlsを押したときの緩急の進み具合(0~1の点P)")]
        [SerializeField] private float easTime;
        [Tooltip("GameとGame以外を押したときの切り替わり時間(秒)")]
        [SerializeField] private float[] duration;
        [Tooltip("UIを戻すかどうか")]
        [SerializeField] private bool backUI;
        [Tooltip("UIを動かすかどうか")]
        [SerializeField] private bool moveUI;
        [Tooltip("『GAMEが押されたかどうか』")]
        [SerializeField] private bool game;
    #endregion

    private string[] item; // 使用されているメニュー項目名を保存するstring型配列
    //private Gamepad[] gamePad; // 接続されているゲームパッドを保存するstring型配列
    private Vector3 cSPos;
    private float TPRate; //進捗率
    private float add_value;

    private float cam_scaleY;
    private float cam_scaleZ;
    private float ui_scale;
    float[] cam_posY = new float[2] {0f, 0f};
    float[] cam_posZ = new float[2] {0f, 0f};

    float[] ui_zoom_scale = new float[2] { 0f, 0f };
    //float acceleration = velocity / duration;
    //private float fps;

    //private void Awake()
    //{
    //    gameObject.GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(0f,0f,true);
    //}

    // Start is called before the first frame update
    void Start()
    {
        /*【オブジェクト情報の取得】*/
        //menu = this.gameObject;
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
        if (TPos == Vector3.zero) { TPos = new Vector3(0, 5, -20); }
        if (duration[0] == 0.0f) { duration[0] = 0.5f; }
        if (duration[1] == 0.0f) { duration[1] = 0.8f; }
        /*------------------------------------------*/

        /*【自動調節系】*/
        Array.Resize(ref item, menuObj.Length);                                                          // 配列のサイズをメニュー項目と同じ数に設定
        //Array.Resize(ref gamePad, Gamepad.all.Count);
        
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
        backUI = true;
        moveUI = false;
        cSPos = cam.transform.position;
        game = false;
        TPRate = 0;
        add_value = 0;
        easTime = 0f;

        cam_scaleY = 0.0f;
        cam_scaleZ = 0.0f;
        ui_scale = 0.0f;
        //duration = new float[2];
        //duration[0] = 0.5f;
        //duration[1] = 0.8f;
        /*----------------------*/

    }

    // Update is called once per frame
    void Update()
    {
        //if (Gamepad.current.xButton.wasPressedThisFrame)
        //{
        //    easTime--;
        //}
        //if (Gamepad.current.yButton.wasPressedThisFrame)
        //{
        //    easTime++;
        //}
        //for (int i = 0; i < Gamepad.all.Count; i++)
        //{
        //    gamePad[i] = Gamepad.all[i];
        //}
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        // カーソル移動関数
        if (decision == false)
        {
            CursorMove();
            Decision();
            //easTime = 0;
            moveUI = false; // 戻りながら連打をするとなぜかdecisionはfalseなのにmoveUIはtrueになるという問題があったためここでもfalseにする
        }

        /*【『CREDIT』と『CONTROLS』決定時の演出】*/
        else if (decision == true && (menuNum > 0 && menuNum < menuObj.Length - 1) && moveUI)
        {

            easTime++;
            if (easTime / 60.0f > duration[0])
            {
                easTime = duration[0] * 60.0f;

                if (Gamepad.current.bButton.wasPressedThisFrame)
                {
                    backUI = true;
                    easTime = 0;
                    Debug.Log("easTime初期化");
                }
            }

            if(backUI == false)
            {
                Debug.Log("UI移動");
                switch (menuNum)
                {
                    case 1:
                        ui.anchoredPosition = new Vector2(easing(duration[0], easTime, (1.0f / 2.0f)) * 1920.0f, 0);
                        //Debug.Log($"duration[0]{duration[0]},easTime{easTime}");
                        //Debug.Log($"easing(duration[0], easTime, (1.0f / 2.0f)){easing(duration[0], easTime, (1.0f / 2.0f))} * 1920 = {easing(duration[0], easTime, (1 / 2)) * 1920}");
                        cam.transform.localRotation = Quaternion.Euler(0.0f, easing(duration[0], easTime, (1.0f / 2.0f)) * -90.0f, 0.0f);
                        break;
                    case 2:
                        ui.anchoredPosition = new Vector2(easing(duration[0], easTime, (1.0f / 2.0f)) * -1920.0f, 0);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, easing(duration[0], easTime, (1.0f / 2.0f)) * 90.0f, 0.0f);
                        break;
                }
                
            }
            else
            {
                /*【MENUに戻る処理】*/
                Debug.Log("戻る");
                switch (menuNum)
                {
                    case 1:

                        ui.anchoredPosition = new Vector2(1920.0f - (easing(duration[0], easTime, (1.0f / 2.0f)) * 1920.0f), 0.0f);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, -90.0f + easing(duration[0], easTime, (1.0f / 2.0f)) * 90.0f, 0.0f);
                        break;
                    case 2:
                        ui.anchoredPosition = new Vector2(-1920.0f + (easing(duration[0], easTime, (1.0f / 2.0f)) * 1920.0f), 0.0f);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, 90.0f - easing(duration[0], easTime,(1.0f / 2.0f)) * 90, 0.0f);
                        break;
                }
                
               
                StartCoroutine("BackMenu");
                
            }
           
        }
        /*------------------------------------------*/
        /*【『GAME』決定時の演出】*/
        if (game == true)
        {
            /*【現状はこれで対応 creditは今後RawImageになる予定】*/
            creditUI.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, true);
            controlsUI.GetComponentInChildren<RawImage>().CrossFadeAlpha(0.0f,0.0f,true);
            easTime++;
            if (easTime / 60.0f > 0.8f)
            {
                easTime = 0.8f * 60.0f;
                cam.transform.position = TPos;
            }

            //Debug.Log($"easTime{easTime}");

            
           

            if (backUI == false)
            {
                Zoom(0.8f, easTime);

                if(moveUI == false)
                {
                    if (Gamepad.current.bButton.wasReleasedThisFrame)
                    {
                        for (int i = 0; i < characterUI.Length; i++)
                        {
                            characterUI[i].SetActive(false);
                            Debug.Log("キャラクターUIが非表示になりました");
                        }

                        gameStart.isCharSelect = false;

                        logo.SetActive(true);
                        menu.SetActive(true);
                        backUI = true;
                        easTime = 0;
                    }
                }
                
            }
            else
            {
                Debug.Log("メニューに戻る");
                ZoomOut(0.8f, easTime);
            }


           
        }
        /*------------------------*/

    }
    void CursorMove()
    {
        #region 選択項目の変更
        //Gamepad.current.
        // 左スティック上入力時 or 十字キー上入力時
        if (Gamepad.current.leftStick.y.ReadValue() > 0 || Gamepad.current.dpad.up.isPressed)
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
        else if (Gamepad.current.leftStick.y.ReadValue() < 0 || Gamepad.current.dpad.down.isPressed)
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
        if (Gamepad.current.aButton.wasPressedThisFrame)
        {
            easTime = 0;
            decision = true; // 決定フラグON
            backUI = false;
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
        menuObj[menuNum].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.25f, true, true); // 文字を徐々に白色に戻す
    }
    private IEnumerator PushGame()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため
        
        game = true;

        moveUI = true;

       
    }
    private IEnumerator PushCredit()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため

        moveUI = true;

        //yield return new WaitForSecondsRealtime(duration[0] - 0.25f);                                                     // 処理を待機 UIが移動する演出のため

        /*【CREDITを実装したときの処理】*/

        //ui.anchoredPosition = new Vector2(easing(duration,easTime) * 1920, 0);
        /*--------------------------*/

        //logo.SetActive(false);
        //menu.SetActive(false);
    }

    private IEnumerator PushControls()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため

        moveUI = true;

        //yield return new WaitForSecondsRealtime(duration[0] - 0.25f);                                                     // 処理を待機 UIが移動する演出のため

        /*【CONTROLSを選択したときの処理】*/

        //ui.anchoredPosition = new Vector2(-1920, 0);
        /*--------------------------*/

        //logo.SetActive(false);
        //menu.SetActive(false);
    }
    private IEnumerator BackMenu()
    {
        yield return new WaitForSecondsRealtime(duration[0]);
        cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0.1f, true);                           // カーソルを徐々に戻す
        decision = false;
        backUI = false;
        moveUI = false;
    }
    float easing(float duration,float time,float length)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration); // easingの進行状況を示す値を算出
        TPRate = t * length;

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length),4, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Sin波を使ったEasing関数 引数(所要時間,現在時間,Sinカーブの長さ,返り値の符号 true:+ false:-,始まりの値,最終的に欲しい値)
    /// </summary>
    /// <param name="duration">所要時間</param>
    /// <param name="time">現在時間</param>
    /// <param name="length">サインカーブの長さ</param>
    /// <param name="symbol">サインカーブの始まりの符号 true:+ false:-</param>
    /// <param name="source">始まりの値</param>
    /// <param name="max">最終的に欲しい値</param>
    /// <returns></returns>
    float easing3(float duration, float time, float length, bool symbol, float source, float max, bool turn)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easingの進行状況を示す値を算出
        TPRate = t;                               // 進行率(%)
        //present_length = t * length;              // 現在地点(Sinカーブから見た)

        // Sinカーブの進む方向を指定 true:+ false:- (最初に元の値から値を 増やしたい : +,減らしたい : -)
        float symbol_num = symbol ? 1.0f : -1.0f;

        // ターン時かどうかで変わる　ターン時はSinカーブ上で言うと　-1 から 1 までつまり距離は 2 
        if (turn)
        {
            // (time / frame) が duration を過ぎているなら
            if ((time / frame) >= duration)
            {
                t = 1; // tは進行率なので 1=100%で100%に固定し、関数の返り値がmaxで指定した値から変わらないようにしている
                       //Debug.Log("time is over");
                return source + (((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) - 1.0f) * (symbol_num * -1)) * Mathf.Abs(max - source);
            }
            //Debug.Log("ターン");
            return source + (((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) - 1.0f) / 2.0f * (symbol_num * -1)) * Mathf.Abs(max - source);
        }
        // それ以外は 0 から 1 で距離は 1
        else
        {
            // (time / frame) が duration を過ぎているなら
            if ((time / frame) >= duration)
            {
                t = 1; // tは進行率なので 1=100%で100%に固定し、関数の返り値がmaxで指定した値から変わらないようにしている
                       //Debug.Log("time is over");
                return source - ((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) * (symbol_num * -1)) * Mathf.Abs(max - source);
            }
            //Debug.Log("通常");
            return source - ((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) * (symbol_num * -1)) * Mathf.Abs(max - source);
        }
    }

    
    void Zoom(float duration,float time)
    {
        /*【カメラとUIの引きと寄りの時に欲しい値】*/
        cam_posY = new float[2] { 65.0f, 5.0f};
        cam_posZ = new float[2] { -260.0f, -20.0f};

        ui_zoom_scale = new float[2] { 0.7f, 40.0f};
        /*----------------------------------------*/

        float length = 1.5f; // 扱うSin(波の長さ)

        /*【UIとカメラのイージング】*/
        if (time < TurningTime(duration,length,0.5f))
        {
            ui_scale = easing3(duration, time, length, false, 1.0f, ui_zoom_scale[0], false);

            cam_scaleY = easing3(duration, time, length, false, 50.0f, cam_posY[0], false);
            cam_scaleZ = easing3(duration, time, length, false, -200.0f, cam_posZ[0], false);
        }
        else
        {
            float ui_old_pos = easing3(duration, TurningTime(duration, length, 0.5f), length, false, 1.0f, ui_zoom_scale[0], false);

            float old_cam_scaleY = easing3(duration, TurningTime(duration, length, 0.5f), length, false, 50.0f, cam_posY[0], false);
            float old_cam_scaleZ = easing3(duration, TurningTime(duration, length, 0.5f), length, true, -200.0f, cam_posZ[0], false);

            ui_scale = easing3(duration, time, length, true, ui_old_pos, ui_zoom_scale[1], true);

            cam_scaleY = easing3(duration, time, length, false, old_cam_scaleY, cam_posY[1], true);
            cam_scaleZ = easing3(duration, time, length, true, old_cam_scaleZ, cam_posZ[1], true);

        }
        /*--------------------------*/

        //Debug.Log($"cam_scaleY{cam_scaleY},cam_scaleZ{cam_scaleZ},ui_scale{ui_scale}");


        cam.transform.position = new Vector3(0, cam_scaleY, cam_scaleZ);
        
        ui.transform.localScale = new Vector3(ui_scale, ui_scale, ui_scale);


        /*【UIが通り過ぎる演出】*/
        if (ui.transform.localScale.x > 26.0f)  //10
        {
            for (int i = 0; i < menuObj.Length; i++)
            {
                menuObj[i].GetComponent<Text>().CrossFadeAlpha(0, 0f, true);
            }
            Debug.Log("メニューUIが非表示になりました");
        }
        /*----------------------*/

        TPos = new Vector3(0, cam_posY[1], cam_posZ[1]);

        /*【キャラクターセレクト画面のUIの有効化】*/
        if (cam.transform.position == TPos)
        {
            moveUI = false;
            //Debug.Log("キャラクターセレクトオン");
            for (int i = 0; i < characterUI.Length; i++)
            {
                characterUI[i].SetActive(true);
                Debug.Log("キャラクターUIを表示しました");
            }

            gameStart.isCharSelect = true;

            logo.SetActive(false);
            menu.SetActive(false);
        }
        else
        {
            //Debug.Log($"{cam.transform.position}");
        }
        /*-----------------------------------------*/
    }
    void ZoomOut(float duration, float time)
    {
        /*【カメラとUIの引きと寄りの時に欲しい値】*/
        ui_zoom_scale = new float[2] { 68.0f, 1.0f };

        cam_posY = new float[2] { -15.0f, 50.0f };
        cam_posZ = new float[2] { 40.0f, -200.0f };
        /*----------------------------------------*/

        float length = 0.5f; // 扱うSin(波の長さ)

        ui_scale = easing3(duration, time, length, false, 40.0f, ui_zoom_scale[1], false);

        cam_scaleY = easing3(duration, time, length, true, 5.0f, cam_posY[1], false);
        cam_scaleZ = easing3(duration, time, length, false, -20.0f, cam_posZ[1], false);

        /*【UIとカメラのイージング】*/
        //if (time < TurningTime(duration, length, 0.5f))
        //{
        //    ui_scale = easing3(duration, time, length, true, 40.0f, ui_zoom_scale[0], false);

        //    cam_scaleY = easing3(duration, time, length, false, 5.0f, cam_posY[0], false);
        //    cam_scaleZ = easing3(duration, time, length, true, -20.0f, cam_posZ[0], false);
        //}
        //else
        //{
        //    float ui_old_pos = easing3(duration, TurningTime(duration, length, 0.5f), length, true, 40.0f, ui_zoom_scale[0], false);

        //    float old_cam_scaleY = easing3(duration, TurningTime(duration, length, 0.5f), length, false, 5.0f, cam_posY[0], false);
        //    float old_cam_scaleZ = easing3(duration, TurningTime(duration, length, 0.5f), length, true, -20.0f, cam_posZ[0], false);
        //    // Debug.Log($"old_cam_scaleZ{-old_cam_scaleZ}");

        //    ui_scale = easing3(duration, time, length, false, ui_old_pos, ui_zoom_scale[1], true);

        //    cam_scaleY = easing3(duration, time, length, true, old_cam_scaleY, cam_posY[1], true);
        //    cam_scaleZ = easing3(duration, time, length, false, old_cam_scaleZ, cam_posZ[1], true);

        //}
        /*--------------------------*/


        cam.transform.position = new Vector3(0, cam_scaleY, cam_scaleZ);

        ui.transform.localScale = new Vector3(ui_scale, ui_scale, ui_scale);


        /*【UIが通り過ぎる演出】*/
        if (ui.transform.localScale.x < 26.0f)  //10
        {
            //logo.SetActive(false);
            //menu.SetActive(false);
            for (int i = 0; i < menuObj.Length; i++)
            {
                menuObj[i].GetComponent<Text>().CrossFadeAlpha(1.0f, 0f, true);
            }
            Debug.Log("メニューUIが表示されています");
        }
        /*----------------------*/

        TPos = new Vector3(0, cam_posY[1], cam_posZ[1]);

        /*【キャラクターセレクト画面のUIの有効化】*/
        if (cam.transform.position == TPos)
        {
            game = false;
            backUI = false;
            StartCoroutine("BackMenu");
        }
        else
        {
            //Debug.Log($"{cam.transform.position}");
        }
        /*-----------------------------------------*/
    }


    /// <summary>
    /// Sin波のEasing関数の特定のタイミングの時間を取得する関数 引数(所要時間,Sinカーブの長さ,取得したい所(Sinカーブ上の地点))
    /// </summary>
    /// <param name="duration">所要時間</param>
    /// <param name="length">サインカーブの長さ</param>
    /// <param name="turning_point">取得したい所(Sinカーブ上の地点)</param>
    /// <returns></returns>
    float TurningTime(float duration,float length,float turning_point)
    {
        float fps = 60.0f;
        float t = duration * fps;
        float divisor = length / turning_point;

        return t / divisor;
    }
}