using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PauseMenu : MonoBehaviour
{
    #region インスペクターからの代入が必要or調整をするもの
    [Header("【事前に入れるもの】")]
    /*【事前の代入が必須】*/

    [Tooltip("GameStateをアタッチしてください")]
    [SerializeField] private GameState gameState;

    [Tooltip("UIという名前のオブジェクトをアタッチしてください")]
    [SerializeField] private GameObject ui;
    [Tooltip("PauseMenuという名前のオブジェクトをアタッチしてください")]
    [SerializeField] private RectTransform pauseMenu;
    [Tooltip("カーソルを入れてください")]
    [SerializeField] private GameObject cursor;
    [Tooltip("カーソルの子要素を入れてください")]
    [SerializeField] private GameObject inner; //カーソルの内側の色
    [Tooltip("メニューの項目の数を指定し、UIオブジェクトを入れてください")]
    [SerializeField] private GameObject[] menuItems;
    
    [Tooltip("ControlsのUIを入れてください")]
    [SerializeField] private GameObject controlsUI;

    //SE関連
    private AudioSource audioSource;
    [SerializeField] private AudioClip decisionSE;      // 決定音
    [SerializeField] private AudioClip cancelSE;        // キャンセル音
    [SerializeField] private AudioClip moveSE;          // カーソル移動音
    [SerializeField] private AudioClip openMenuSE;      // メニュー表示オン

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

    // Easing系
    [Tooltip("CreditかControlsを押したときの緩急の進み具合(0~1の点P)")]
    [SerializeField] private float easTime;
    [SerializeField] private float[] itemTime;
    [Tooltip("GameとGame以外を押したときの切り替わり時間(秒)")]
    [SerializeField] private float[] duration;

    #region 動作確認用に表示するもの
    [Header("【動作確認用】")]
    [Tooltip("UIのcanvas group Component")]
    [SerializeField] private CanvasGroup uiGroup;
    [Tooltip("カーソルのレンダートランスフォーム情報が入る")]
    [SerializeField] private RectTransform cursorRT;
    [Tooltip("現在選択されているメニュー番号")]
    [SerializeField] private int currentMenuNum;
    [SerializeField] private int oldMenuNum;
    [Tooltip("現在選択されているメニュー名")]
    [SerializeField] private string currentMenuName;
    [SerializeField] private string oldMenuName;
    [Tooltip("上下どちらかの入力がされているか")]
    [SerializeField] private bool ispush;
    [Tooltip("menu項目を選択したかどうか")]
    [SerializeField] private bool decision; // 決定フラグON
    [Tooltip("UIを戻すかどうか")]
    [SerializeField] private bool backUI;
    [Tooltip("UIを動かすかどうか")]
    [SerializeField] private bool moveUI;
    [Tooltip("QUITボタンが押されたか")]
    [SerializeField] private bool pushQuit;
    [Tooltip("入力継続時間")]
    [SerializeField] private float count;
    [Tooltip("ポーズの操作権限を持っているプレイヤー")]
    [SerializeField] private int controlNum;    // 操作番号
    #endregion

    private string[] items;     // 使用されているメニュー項目名を保存するstring型配列
    private Gamepad[] gamePad; // 接続されているゲームパッドを保存するstring型配列

    [SerializeField] private bool show;      // true:表示 false:非表示
    [SerializeField] private float waitTime; // シーン移動時に使用する処理待機時間(SEが鳴り終わるまで)

    //private float fps;

    void Start()
    {
        /*Titleと共通*/
        /*【オブジェクト情報の取得】*/
        uiGroup = ui.GetComponent<CanvasGroup>();
        ui.SetActive(false); // 最初は非表示に
        if (cursor == null) { Debug.LogError("カーソルオブジェクトがアタッチされていません"); }
        cursorRT = cursor.GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
        /*-------------------*/


        /*【事前の調整が必要な値が未調整だった場合】*/
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (menuItems[i] == null) { Debug.LogError($"menuObj[{i}]にオブジェクトがアタッチされていません"); }
        }
        if (interval == 0) { interval = 10; }                                                                                           // 長押しでの選択項目を移動する時間間隔の設定
        if (itemSpace == 0) { itemSpace = 120; }                                                                                        // メニューの項目同士のY座標間隔
        if (selectionItemColor.a == 0) { selectionItemColor.a = 1; }                                                                    // 透明度をマックスに設定
        if (notSelectionItemColor.a == 0) { notSelectionItemColor.a = 1; }                                                              // 透明度をマックスに設定
        if (selectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#008fd9", out selectionItemColor); }        // 水色に設定
        if (notSelectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#ffffff", out notSelectionItemColor); }  // 白色に設定

        /*------------------------------------------*/

        /*【自動調節系】*/
        Array.Resize(ref items, menuItems.Length);                                                          // 配列のサイズをメニュー項目と同じ数に設定
        Array.Resize(ref gamePad, Gamepad.all.Count);                                                    // 配列のサイズをゲームパッドの接続数と同じ数に設定
        Array.Resize(ref itemTime, menuItems.Length);  //menuItemsの数によってitemTimeの大きさを変更する


        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);  // メニュー項目同士のY座標間隔を指定した間隔に設定
            items[i] = menuItems[i].name;                                                             // 配列の中にメニュー項目の名前を代入
            itemTime[i] = 0;
        }
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            gamePad[i] = Gamepad.all[i];
        }
        /*--------------*/

        /*【その他変数の初期化】*/
        currentMenuName = items[0];
        currentMenuNum = 0;
        oldMenuNum = 0;
        ispush = false;
        decision = false;
        backUI = true;
        moveUI = false;
        pushQuit = false; // Quitとボタンが押されたか
        count = 0;
        controlNum = 0;

        uiGroup.alpha = 0.0f;
        /*----------------------*/

        /*Pause専用*/
        waitTime = 2.0f;
        Initialize();
        /*--------*/

        cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0f, true);                           // カーソルを徐々に戻す
    }

    void Update()
    {
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        /* 【表示状態】 */
        if (show)
        {
            easTime++;
            // (easTime / 60.0f)が指定した演出にかかる所要時間を超えた時
            if (easTime / 60.0f > duration[1])
            {
                easTime = duration[1] * 60.0f; // easTimeを(duration * 60.0f)に固定
            }

            if(uiGroup.alpha < 1.0f)
            {
                uiGroup.alpha = easing(duration[0], easTime, 0.5f);
            }

            if (pushQuit == false && decision == false)
            {
                // カーソル移動関数
                CursorMove();

                // 決定関数
                Decision();
            } 
            else if (decision)
            {
                //easTime++;

                //// (easTime / 60.0f)が指定した演出にかかる所要時間を超えた時
                //if (easTime / 60.0f > duration[1])
                //{
                //    easTime = duration[1] * 60.0f; // easTimeを(duration * 60.0f)に固定
                //}

                switch (currentMenuNum)
                {
                    case 0:
                        uiGroup.alpha = 1.0f - easing(duration[0],easTime,0.5f);
                        break;
                    case 1:
                        
                        // 戻る演出フラグが立っていない時
                        if (backUI == false)
                        {
                            if (moveUI)
                            {
                                if(Mathf.Abs(pauseMenu.anchoredPosition.x) < 1920.0f)
                                {
                                    pauseMenu.anchoredPosition = new Vector2(easing(duration[1], easTime, (1.0f / 2.0f)) * -1920.0f, 0);
                                }
                                if (easTime / 60.0f == duration[1]) {
                                    // Bボタンを押したとき
                                    if (Gamepad.current.bButton.wasPressedThisFrame)
                                    {
                                        backUI = true; // UIが戻る演出ON
                                        easTime = 0;   // easTime初期化
                                        Debug.Log("通りました");
                                    }
                                }
                            }
                            
                        }

                        // 戻る演出フラグが立っている時
                        else
                        {
                            if (currentMenuNum == 1)
                            {
                                pauseMenu.anchoredPosition = new Vector2(-1920.0f + (easing(duration[1], easTime, (1.0f / 2.0f)) * 1920.0f), 0.0f);
                            }

                            if (Mathf.Abs(pauseMenu.anchoredPosition.x) < 1.0f)
                            {
                                moveUI = false;
                                decision = false;
                                backUI = false;

                                cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // カーソルを徐々に戻す
                                inner.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // カーソルを徐々に戻す
                            }

                            StartCoroutine("BackMenu");

                        }
                        break;
                }
            }
        }

        /* 【非表示状態】 */
        else if(gameState.isGame && !gameState.isResult)
        {
            // 誰かしらがStartボタンを押した時
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                easTime = 0;
                /*【何PがStatボタンを押したのかを取得する】*/
                for (int i = 0; i< gamePad.Length; i++)
                {
                    if (gamePad[i].startButton.wasPressedThisFrame)
                    {
                        Debug.Log($"GamePad{i}が入力しました");
                        controlNum = i; // ポーズの操作権限をiPにする
                    }
                }
                show = true;        // ポーズメニューを表示状態のフラグにする
                Time.timeScale = 0; // ポーズ(時間を止める)
                ui.SetActive(true); // UIオブジェクトを有効化する

                //SE（Pauseを開く）
                audioSource.clip = openMenuSE;
                audioSource.PlayOneShot(openMenuSE);

                inner.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);   // カーソルを徐々に消す
                cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);
            }
        }
    }

    private void Initialize()
    {
        Time.timeScale = 1;             // ポーズ解除(時間を動かす)
        show = false;                   // メニューを 非表示状態 にする
        ui.SetActive(false);     // UIオブジェクトを無効化する
    }

    void CursorMove()
    {
        #region 選択項目の変更
        // 左スティック上入力時 or 十字キー上入力時
        if (gamePad[controlNum].leftStick.y.ReadValue() > 0 || gamePad[controlNum].dpad.up.isPressed)
        {
            if (ispush == false) // 押された時の処理
            {
                ispush = true;
                if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;
                audioSource.clip = moveSE;
                audioSource.PlayOneShot(moveSE);
                if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
            }
            else               // 長押し時の処理
            {
                count++;
                if (count % interval == 0)
                {
                    if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;
                    audioSource.clip = moveSE;
                    audioSource.PlayOneShot(moveSE);
                    if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
                }
            }

            if (currentMenuNum + 1 > (menuItems.Length - 1))
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
        // 左スティック下入力時 or 十字キー下入力時
        else if (gamePad[controlNum].leftStick.y.ReadValue() < 0 || gamePad[controlNum].dpad.down.isPressed)
        {
            if (ispush == false) // 押された時の処理
            {
                ispush = true;
                if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;
                audioSource.clip = moveSE;
                audioSource.PlayOneShot(moveSE);
                if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
            }
            else               // 長押し時の処理
            {
                count++;
                if (count % interval == 0)
                {
                    if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;
                    audioSource.clip = moveSE;
                    audioSource.PlayOneShot(moveSE);
                    if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
                }
            }

            if (currentMenuNum - 1 < 0)
            {
                oldMenuNum = menuItems.Length - 1;
            }
            else
            {
                oldMenuNum = currentMenuNum - 1;
            }

            if (itemTime[oldMenuNum] > 0.05f * 60.0f) itemTime[oldMenuNum] = 0;
            oldMenuName = items[oldMenuNum];
            currentMenuName = items[currentMenuNum];  //選択されているメニュー名をmenuNameに代入する（随時更新）

        }
        else
        {
            ispush = false;
            count = 0;
        }
        currentMenuName = items[currentMenuNum]; // 現在選択しているメニュー名を代入
        #endregion

        float scale = 0;

        #region カーソルの移動処理(項目の数によって自動でループ数が変更され、移動できるポジションも増減する)
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (currentMenuName == items[i])
            {
                cursorRT.position = menuItems[i].GetComponent<RectTransform>().position;
                menuItems[i].GetComponent<Text>().CrossFadeColor(selectionItemColor, 0.05f, true, true); // 文字を徐々に水色に変更

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
        #endregion
    }

    void Decision()
    {
        // 決定ボタン(aボタン)を押したとき
        if (gamePad[controlNum].aButton.wasPressedThisFrame)
        {

            CursorFade();
            easTime = 0;
            decision = true; // 決定フラグON
            backUI = false;

            audioSource.clip = decisionSE;
            audioSource.PlayOneShot(decisionSE);

            switch (currentMenuNum)
            {
                /*【ゲームに戻る】*/
                case 0:
                    StartCoroutine("PushContinue"); 
                    break;
                /*------------*/

                /*【操作説明を開く】*/
                case 1:
                    StartCoroutine("PushControls");
                    break;
                /*-------------*/

                /*【タイトルに戻る】*/
                case 2:
                    pushQuit = true;
                    StartCoroutine("BacktoTitleScene");
                    break;
                /*-------------*/

                default:
                    break;
            }
        }

        /* 【使いまわし(簡単にどのプロジェクトでも）出来るようにを意識したコード】 */
        //// 一番上の項目が選択されている状態で決定を押した時
        //if (menuName == item[0] && gamePad[controlNum].aButton.wasPressedThisFrame)
        //{
        //    /* 【ゲームに戻る】 */
        //    Time.timeScale = 1;         // ポーズ解除(時間を動かす)
        //    show = false;               // メニューを 非表示状態 にする
        //    pauseMenu.SetActive(false); // UIオブジェクトを有効化

        //}

        //// 二番目の項目が選択されている状態で決定を押した時
        //if (menuName == item[1] && gamePad[controlNum].aButton.wasPressedThisFrame)
        //{
        //    /* 【操作説明を開く】 */
        //}

        //// 三番目の項目が選択されている状態で決定を押した時
        //if (menuName == item[2] && gamePad[controlNum].aButton.wasPressedThisFrame)
        //{
        //    /* 【タイトルに戻る】 */
        //    pushQuit = true;
        //    StartCoroutine("BacktoTitleScene");
        //}
    }

    void CursorFade()
    {
        inner.GetComponent<RawImage>().CrossFadeAlpha(0, 0.2f, true);                                           // カーソルを徐々に消す
        cursor.GetComponent<RawImage>().CrossFadeAlpha(0, 0.2f, true);                                          // カーソルを徐々に消す
        menuItems[currentMenuNum].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.2f, true, true); // 文字を徐々に白色に戻す
    }

    private IEnumerator PushContinue()
    {
        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため

        Time.timeScale = 1;         // ポーズ解除(時間を動かす)
        show = false;               // メニューを 非表示状態 にする
        decision = false; // 決定フラグON
        ui.SetActive(false); // UIオブジェクトを有効化
    }

    private IEnumerator PushControls()
    {
        yield return new WaitForSecondsRealtime(0.25f);                                                        // 処理を待機 ボタンを押した演出のため

        moveUI = true;
        easTime = 0;
    }

    private IEnumerator BackMenu()
    {
        yield return new WaitForSecondsRealtime(duration[1]);
        controlsUI.GetComponentInChildren<RawImage>().CrossFadeAlpha(1.0f, 0.0f, true);
        //decision = false;
    }

    private IEnumerator BacktoTitleScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); // 処理を待機 シーン時の音を鳴らすため
        SceneManager.LoadScene("TitleScene");
        Initialize();
    }

    float easing(float duration, float time, float length)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easingの進行状況を示す値を算出
        //TPRate = t * length;

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero);
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
}
