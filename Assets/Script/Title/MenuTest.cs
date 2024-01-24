using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;


public class MenuTest : MonoBehaviour
{
    private string[] items; //タイトル画面のメニュー項目名を保持する配列

    // CS系
    [Tooltip("GameStartSystem.csを持っているオブジェクトを入れてください")]
    [SerializeField] private GameStartSystem gameStartSys;

    // UI系
    [Tooltip("UIという名前のオブジェクトをアタッチしてください")]
    [SerializeField] private RectTransform ui;
    private float uiScale;
    [Tooltip("menuオブジェクトが入る")]
    [SerializeField] private GameObject menu;
    [Tooltip("メニュー専用のカーソルを入れてください")]
    [SerializeField] private GameObject cursor;
    [Tooltip("カーソルの子要素を入れてください")]
    [SerializeField] private GameObject inner; //カーソルの内側の色
    [Tooltip("メニューカーソルのRectTransform取得用")]
    [SerializeField] private RectTransform cursorRT;
    [Tooltip("タイトルで表示する項目UI(4つ)を入れてください")]
    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject logo;
    [Tooltip("各キャラクターUIを入れてください")]
    [SerializeField] private GameObject characterUI;
    [Tooltip("キャラクターの背景UIを入れてください")]
    [SerializeField] private GameObject backGroundUI;
    [Tooltip("CreditのUIを入れてください")]
    [SerializeField] private GameObject creditUI;
    [Tooltip("ControlsのUIを入れてください")]
    [SerializeField] private GameObject controlsUI;

    // カメラ系
    [Tooltip("カメラオブジェクトを入れてください")]
    [SerializeField] private Camera cam;
    [Tooltip("『GAME』を押した時のカメラのターゲットポジションを入れてください")]
    [SerializeField] private Vector3 tPos;
    private float camPosY;
    private float camPosZ;

    // 音系
    [SerializeField] private AudioSource audioSource;   //自身のAudioSourceを入れる
    [SerializeField] private AudioClip decisionSE;      //決定音 decision
    [SerializeField] private AudioClip cancelSE;        //キャンセル音
    [SerializeField] private AudioClip moveSE;          //移動音
    [SerializeField] private AudioClip openMenuSE;      //
    [SerializeField] private AudioClip closeMenuSE;     //

    // 値調整系
    [Tooltip("項目同士の縦の間隔(正の値を入力してください)")]
    [SerializeField] private float itemSpace;
    [Tooltip("カーソル移動時の時間間隔")]
    [SerializeField] private float interval;
    [Tooltip("選択項目のカラー")]
    [SerializeField] private Color selectionItemColor;
    [Tooltip("非選択項目のカラー")]
    [SerializeField] private Color notSelectionItemColor;

    // Easing系
    [Tooltip("CreditかControlsを押したときの緩急の進み具合(0~1の点P)")]
    [SerializeField] private float easTime;
    [SerializeField] private float[] itemTime;
    [Tooltip("GameとGame以外を押したときの切り替わり時間(秒)")]
    [SerializeField] private float[] duration;

    [Header("動作確認用")]
    [Tooltip("現在選択されているメニュー番号")]
    [SerializeField] private int currentMenuNum;
    [Tooltip("ひとつ前に選択されていたメニュー番号")]
    [SerializeField] private int oldMenuNum;
    [Tooltip("現在選択されているメニュー名")]
    [SerializeField] private string currentMenuName;
    [Tooltip("ひとつ前に選択されていたメニュー名")]
    [SerializeField] private string oldMenuName;
    [Tooltip("上下どちらかの入力がされているか")]
    [SerializeField] private bool isPush;
    [Tooltip("入力継続時間")]
    [SerializeField] private float inputCount;
    [Tooltip("menu項目を選択したかどうか")]
    [SerializeField] private bool decision;
    [Tooltip("UIを戻すかどうか")]
    [SerializeField] private bool backUI;
    [Tooltip("UIを動かすかどうか")]
    [SerializeField] private bool moveUI;
    [Tooltip("『GAMEが押されたかどうか』")]
    [SerializeField] private bool game;

    private void Start()
    {
        //事前の調整が必要な値が未調整だった場合
        if (interval == 0) { interval = 12; }                                                                                           // 長押しでの選択項目を移動する時間間隔の設定
        if (itemSpace == 0) { itemSpace = 120; }                                                                                        // メニューの項目同士のY座標間隔
        if (selectionItemColor.a == 0) { selectionItemColor.a = 1; }                                                                    // 透明度をマックスに設定
        if (notSelectionItemColor.a == 0) { notSelectionItemColor.a = 1; }                                                              // 透明度をマックスに設定
        if (selectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#008fd9", out selectionItemColor); }        // 水色に設定
        if (notSelectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#ffffff", out notSelectionItemColor); }  // 白色に設定

        this.gameObject.SetActive(true);    //タイトルメニュー一覧を表示する
        logo.SetActive(true);               //ロゴを表示する

        Array.Resize(ref items, menuItems.Length);     //menuItemsの数によってitemsの大きさを変更する
        Array.Resize(ref itemTime, menuItems.Length);  //menuItemsの数によってitemTimeの大きさを変更する

        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);    //メニュー項目同士のY座標間隔を指定した間隔に設定
            items[i] = menuItems[i].name;   //配列の中にメニュー項目の名前を代入する
            itemTime[i] = 0;
        }

        //for(int i = 0; i < characterUI.Length; i++)
        //{
        //    characterUI[i].SetActive(false);    //各キャラクターUIを非表示にする
        //}
        characterUI.SetActive(false);    //各キャラクターUIを非表示にする
        backGroundUI.SetActive(false);

        currentMenuName = items[0];  //選択されているメニュ名を1番上に初期化
        oldMenuName = currentMenuName;
        currentMenuNum = 0;          //選択されているメニュー番号を1番上に初期化
        isPush = false;              //上下どちらか押されているかの確認用フラグの初期化
        inputCount = 0;              //ボタン入力継続時間の初期化

        decision = false;
        backUI = true;
        moveUI = false;
        game = false;

        camPosY = 0.0f;
        camPosZ = 0.0f;
        uiScale = 0.0f;

        easTime = 0.0f;
    }

    private void Update()
    {
        // まだ項目選択中の時
        if (decision == false && ui.anchoredPosition.x == 0.0f)
        {
            CursorMove();   //メニューカーソル移動処理

            Decision();     //各メニューボタンが押されたときの処理

            moveUI = false;
        }
        // 『CREDIT』と『CONTROLS』を選択した時
        else if (decision == true && (currentMenuNum > 0 && currentMenuNum < menuItems.Length - 1) && moveUI)
        {

            //Debug.Log(" 決定された ");

            easTime++;

            // (easTime / 60.0f)が指定した演出にかかる所要時間を超えた時
            if(easTime / 60.0f > duration[1])
            {
                easTime = duration[1] * 60.0f; // easTimeを(duration * 60.0f)に固定

                // Bボタンを押したとき
                if (Gamepad.all.Count != 0 && Gamepad.current.bButton.wasPressedThisFrame)
                {
                    backUI = true; // UIが戻る演出ON
                    easTime = 0;   // easTime初期化
                }
            }

            // 戻る演出フラグが立っていない時
            if(backUI == false)
            {
                switch (currentMenuNum)
                {
                    case 1: // 『CREDIT』への画面に移動する演出
                        ui.anchoredPosition = new Vector2(easing(duration[1], easTime, (1.0f / 2.0f)) * 1920.0f, 0);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, easing(duration[1], easTime, (1.0f / 2.0f)) * -90.0f, 0.0f);
                        break;

                    case 2: // 『CONTROLS』への画面に移動する演出
                        ui.anchoredPosition = new Vector2(easing(duration[1], easTime, (1.0f / 2.0f)) * -1920.0f, 0);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, easing(duration[1], easTime, (1.0f / 2.0f)) * 90.0f, 0.0f);
                        break;
                }
            }

            // 戻る演出フラグが立っている時
            else
            {
                switch (currentMenuNum)
                {
                    case 1:
                        ui.anchoredPosition = new Vector2(1920.0f - (easing(duration[1], easTime, (1.0f / 2.0f)) * 1920.0f), 0.0f);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, -90.0f + easing(duration[1], easTime, (1.0f / 2.0f)) * 90.0f, 0.0f);
                        break;
                    case 2:
                        ui.anchoredPosition = new Vector2(-1920.0f + (easing(duration[1], easTime, (1.0f / 2.0f)) * 1920.0f), 0.0f);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, 90.0f - easing(duration[1], easTime, (1.0f / 2.0f)) * 90, 0.0f);
                        break;
                }

                if (Mathf.Abs(ui.anchoredPosition.x) < 1.0f)
                {
                    moveUI = false;
                    decision = false;
                    backUI = false;

                    cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // カーソルを徐々に戻す
                    inner.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // カーソルを徐々に戻す
                }

                StartCoroutine("BackMenu");

                
            }
        }

        if(game == true)
        {
            /*【現状はこれで対応 creditは今後RawImageになる予定】*/
            creditUI.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, true);
            controlsUI.GetComponentInChildren<RawImage>().CrossFadeAlpha(0.0f, 0.0f, true);

            easTime++;
            if (easTime / 60.0f > duration[0])
            {
                easTime = duration[0] * 60.0f;
                cam.transform.position = tPos;
            }

            if (backUI == false)
            {
                // キャラクターセレクトの状態に進む
                Zoom(duration[0], easTime);

                // UIが動いてはいけないフラグが立っている時
                if (moveUI == false && !gameStartSys.isReady)
                {
                    // Bボタンを押したら
                    if (Gamepad.all.Count != 0 && Gamepad.current.bButton.wasPressedThisFrame && gameStartSys.submitCharCount <= 0)
                    {
                        // キャラクターUIを非表示にする
                        //for (int i = 0; i < characterUI.Length; i++)
                        //{
                        //    characterUI[i].SetActive(false); 
                        //}
                        characterUI.SetActive(false);
                        backGroundUI.SetActive(false);

                        gameStartSys.isCharSelect = false; // キャラクターセレクトを無効化

                        logo.SetActive(true);              // Logoを表示
                        menu.SetActive(true);              // メニューを表示
                        backUI = true;                     // メニュー画面に戻る演出 ON
                        easTime = 0;                       // easTime初期化
                    }
                }
                
            }
            else
            {
                ZoomOut(duration[0], easTime); // メニューの状態に戻る
            }
        }
    }

    //カーソルの移動関連処理
    private void CursorMove()
    {
        
        //GamePad（左スティック上入力時 or 十字キー上入力時）
        if (Gamepad.all.Count != 0 && Gamepad.current.leftStick.y.ReadValue() > 0 || Gamepad.current.dpad.up.isPressed)
        {
            if (isPush == false)    //押された時の処理
            {
                isPush = true;
                //currentMenuNumが上を押されることで減らされ、0より小さい値になったら
                if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;   //カーソルを一番下に移動させる
                audioSource.clip = moveSE;
                audioSource.PlayOneShot(moveSE);
                if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
            }
            else    //長押し時の処理
            {
                inputCount++;    //カウントさせる
                //カウントと移動インターバルを割った余りが0の場合
                if (inputCount % interval == 0)
                {
                    if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;   //カーソルを一番下に移動させる
                    audioSource.clip = moveSE;
                    audioSource.PlayOneShot(moveSE);
                    if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
                }
            }

            if(currentMenuNum + 1 > 3)
            {
                oldMenuNum = 0;
            }
            else
            {
                oldMenuNum = currentMenuNum + 1;
            }

            if (itemTime[oldMenuNum] > 0.05f * 60.0f) itemTime[oldMenuNum] = 0;
            oldMenuName = items[oldMenuNum];
            currentMenuName = items[currentMenuNum];  //選択されているメニュー名をmenuNameに代入する（随時更新）
            
        }
        //GamePad（左スティック下入力時 or 十字キー下入力時）
        else if (Gamepad.all.Count != 0 && Gamepad.current.leftStick.y.ReadValue() < 0 || Gamepad.current.dpad.down.isPressed)
        {
            if (isPush == false)    //押された時の処理
            {
                isPush = true;
                //currentMenuNumが下を押されることで増え、項目数より大きい値になったら
                if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;    //カーソルを一番上に移動させる
                audioSource.clip = moveSE;
                audioSource.PlayOneShot(moveSE);
                if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
            }
            else    //長押し時の処理
            {
                inputCount++;
                //カウントと移動インターバルを割った余りが0の場合
                if (inputCount % interval == 0)
                {
                    if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;    //カーソルを一番上に移動させる
                    audioSource.clip = moveSE;
                    audioSource.PlayOneShot(moveSE);
                    //if (itemTime[oldMenuNum] > 0.05f * 60.0f) itemTime[oldMenuNum] = 0;
                    if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
                }
            }
            if (currentMenuNum - 1 < 0)
            {
                oldMenuNum = 3;
            }
            else
            {
                oldMenuNum = currentMenuNum - 1;
            }
            //if (itemTime[oldMenuNum] > 0.05f * 60.0f) itemTime[oldMenuNum] = 0;
            //if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;

            if (itemTime[oldMenuNum] > 0.05f * 60.0f) itemTime[oldMenuNum] = 0;
            oldMenuName = items[oldMenuNum];
            currentMenuName = items[currentMenuNum];  //選択されているメニュー名をmenuNameに代入する（随時更新）
        }
        else      //何もしてない時
        {
            isPush = false;
            inputCount = 0;
        }


        float scale = 0;

        //色変更
        for (int i = 0; i < menuItems.Length; i++)
        {
            //現在選択されているメニュー名とメニュー項目内にある名前が一致した場合
            if (currentMenuName == items[i])
            {
                cursorRT.position = menuItems[i].GetComponent<RectTransform>().position;    //メニューカーソルの位置を選択されているメニューの位置に変更する
                menuItems[i].GetComponent<Text>().CrossFadeColor(selectionItemColor, 0.05f, true, true); // 文字を徐々に白色に戻す

                // 文字をだんだん大きくする
                scale = easing3(0.05f, itemTime[i], 0.5f, true, 1.0f, 1.25f, false);
                menuItems[i].GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
                itemTime[i]++;
            }
            else
            {
                menuItems[i].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.05f, true, true); // 文字を徐々に白色に戻す
                scale = easing3(0.05f, itemTime[i], 0.5f, false, menuItems[i].GetComponent<RectTransform>().localScale.x, 1.0f, false);
                menuItems[i].GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
                itemTime[i]++;
            }
        }
    }

    //各メニューボタンが押されたときの処理
    private void Decision()
    {
        
        // 決定ボタン(aボタン)を押したとき
        if (Gamepad.all.Count != 0 && Gamepad.current.aButton.wasPressedThisFrame/* && inner.GetComponent<RawImage>().canvasRenderer.GetAlpha() == 1*/)
        {
            CursorFade();
            easTime = 0;
            decision = true; // 決定フラグON
            backUI = false;

            audioSource.clip = decisionSE;
            audioSource.PlayOneShot(decisionSE);

            switch (currentMenuNum)
            {
                case 0:
                    StartCoroutine("PushGame");
                    break;

                case 1:
                    StartCoroutine("PushCredit");
                    break;

                case 2:
                    StartCoroutine("PushControls");
                    break;

                case 3:
                    #if UNITY_EDITOR

                        UnityEditor.EditorApplication.isPlaying = false;

                    #else

                        Application.Quit();

                    #endif
                    break;

                default:
                    break;
            }
        }
    }

    void CursorFade()
    {
        inner.GetComponent<RawImage>().CrossFadeAlpha(0, 0.2f, true);                                           // カーソルを徐々に消す
        cursor.GetComponent<RawImage>().CrossFadeAlpha(0, 0.2f, true);                                          // カーソルを徐々に消す
        menuItems[currentMenuNum].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.2f, true, true); // 文字を徐々に白色に戻す
    }

    private IEnumerator PushGame()
    {
        //CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため

        game = true;

        moveUI = true;
    }
    private IEnumerator PushCredit()
    {
        //CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため

        moveUI = true;
    }
    private IEnumerator PushControls()
    {
        //CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため

        moveUI = true;
    }
    private IEnumerator BackMenu()
    {
        yield return new WaitForSecondsRealtime(duration[1]);
        creditUI.GetComponentInChildren<Text>().CrossFadeAlpha(1.0f, 0.0f, true);
        controlsUI.GetComponentInChildren<RawImage>().CrossFadeAlpha(1.0f, 0.0f, true);
        //decision = false;
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
        //TPRate = t;                               // 進行率(%)
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

    void Zoom(float duration, float time)
    {
        /*【カメラとUIの引きと寄りの時に欲しい値】*/
        float[] targetPosY = new float[2] { 65.0f, 5.0f };
        float[] targetPosZ = new float[2] { -260.0f, -20.0f };

        float[] ui_zoom_scale = new float[2] { 0.7f, 40.0f };
        /*----------------------------------------*/

        float length = 1.5f; // 扱うSin(波の長さ)

        /*【UIとカメラのイージング】*/
        if (time < TurningTime(duration, length, 0.5f))
        {
            uiScale = easing3(duration, time, length, false, 1.0f, ui_zoom_scale[0], false);

            camPosY = easing3(duration, time, length, false, 50.0f, targetPosY[0], false);
            camPosZ = easing3(duration, time, length, false, -200.0f, targetPosZ[0], false);
        }
        else
        {
            float ui_old_pos = easing3(duration, TurningTime(duration, length, 0.5f), length, false, 1.0f, ui_zoom_scale[0], false);

            float old_cam_scaleY = easing3(duration, TurningTime(duration, length, 0.5f), length, false, 50.0f, targetPosY[0], false);
            float old_cam_scaleZ = easing3(duration, TurningTime(duration, length, 0.5f), length, true, -200.0f, targetPosZ[0], false);

            uiScale = easing3(duration, time, length, true, ui_old_pos, ui_zoom_scale[1], true);

            camPosY = easing3(duration, time, length, false, old_cam_scaleY, targetPosY[1], true);
            camPosZ = easing3(duration, time, length, true, old_cam_scaleZ, targetPosZ[1], true);

        }
        /*--------------------------*/

        cam.transform.position = new Vector3(0, camPosY, camPosZ);

        ui.transform.localScale = new Vector3(uiScale, uiScale, uiScale);

        /*【UIが通り過ぎる演出】*/
        if (ui.transform.localScale.x > 26.0f)  //10
        {
            // メニューUIを非表示にする
            for (int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i].GetComponent<Text>().CrossFadeAlpha(0, 0f, true);
            }
        }
        /*----------------------*/

        // カメラの目標地点を更新
        tPos = new Vector3(0, targetPosY[1], targetPosZ[1]);

        /*【キャラクターセレクト画面のUIの有効化】*/
        if (cam.transform.position == tPos)
        {
            moveUI = false;
            //for (int i = 0; i < characterUI.Length; i++)
            //{
            //    characterUI[i].SetActive(true); // キャラクターセレクトを表示する
            //}
            characterUI.SetActive(true); // キャラクターセレクトを表示する
            backGroundUI.SetActive(true);
            gameStartSys.isCharSelect = true;

            logo.SetActive(false);
            menu.SetActive(false);
        }
        /*-------------------------------*/
    }
    void ZoomOut(float duration, float time)
    {
        /*【カメラとUIの引きと寄りの時に欲しい値】*/
        float[] targetPosY = new float[2] { -15.0f, 50.0f };
        float[] targetPosZ = new float[2] { 40.0f, -200.0f };

        float[] ui_zoom_scale = new float[2] { 68.0f, 1.0f };
        /*----------------------------------------*/

        float length = 0.5f; // 扱うSin(波の長さ)

        uiScale = easing3(duration, time, length, false, 40.0f, ui_zoom_scale[1], false);

        camPosY = easing3(duration, time, length, true, 5.0f, targetPosY[1], false);
        camPosZ = easing3(duration, time, length, false, -20.0f, targetPosZ[1], false);

        cam.transform.position = new Vector3(0, camPosY, camPosZ);

        ui.transform.localScale = new Vector3(uiScale, uiScale, uiScale);

        /*【UIが出てくる演出】*/
        if (ui.transform.localScale.x < 26.0f)  //10
        {
            // メニューUIが表示される
            for (int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i].GetComponent<Text>().CrossFadeAlpha(1.0f, 0f, true);
            }
        }
        /*----------------------*/

        tPos = new Vector3(0, targetPosY[1], targetPosZ[1]);

        /*【キャラクターセレクト画面のUIの有効化】*/
        if (cam.transform.position == tPos)
        {
            game = false;
            backUI = false;
            decision = false;
            cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // カーソルを徐々に戻す
            inner.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // カーソルを徐々に戻す
            StartCoroutine("BackMenu");
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
    float TurningTime(float duration, float length, float turning_point)
    {
        float fps = 60.0f;
        float t = duration * fps;
        float divisor = length / turning_point;

        return t / divisor;
    }

    float easing(float duration, float time, float length)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easingの進行状況を示す値を算出
        //TPRate = t * length;

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero);
    }
}