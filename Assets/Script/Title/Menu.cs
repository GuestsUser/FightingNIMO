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

            Zoom(0.8f, easTime);

            if (Gamepad.current.bButton.wasReleasedThisFrame)
            {
                for (int i = 0; i < characterUI.Length; i++)
                {
                    characterUI[i].SetActive(false);
                }

                gameStart.onCharaSelect = false;

                logo.SetActive(true);
                menu.SetActive(true);
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
        

        //yield return new WaitForSecondsRealtime(duration[1] - 0.25f);

        //gameStart.onCharaSelect = true;

        //logo.SetActive(false);
        //menu.SetActive(false);
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
        float t = ((easTime / frame) / duration); // easingの進行状況を示す値を算出
        TPRate = t * length;
        //Debug.Log($"easingの進捗{t * length}");
        //Debug.Log($"Sinの値{ (float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) }");

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length),4, MidpointRounding.AwayFromZero);
    }

    float easing2(float duration, float time, float min, float max,float length)
    {
        float frame = 60.0f;                    // fps
        float t = ((easTime / frame) / duration); // easingの進行状況を示す値を算出

        float easedValue = Mathf.Sin(t * Mathf.PI * length) - 1;

        // スケーリングとオフセット適用
    
        float result = min + (max - min) * easedValue;

        return Mathf.Round(result * 10000.0f) / 10000.0f;
    }

    // duration:max値にするまでにかかる時間 max:最終的な値 delay:遅らせたい時間 time:特定条件下でカウントアップしている変数
    float Lerp(float duration,float min,float max,float length, float time)
    {
        float delay = (TPRate / length) * duration; // 経過時間
        float Remain_time = duration - delay;　　　 // 最終地点までの残り時間を計算

        float velocity = (max - min) / duration;    // 最終地点にたどり着くために必要な速度の計算
        float acceleration = velocity / duration;　 // 最終地点にたどり着くために

        add_value = velocity + acceleration;　　　　// 

        if(TPRate <= length)
        {
            velocity = 0;
            add_value = velocity;

            return min;
        }
        //min += add_value;
        //min = max;
        return min;
    }

    void Zoom(float duration,float time)
    {
        Debug.Log("zoom中");

        /*【カメラとUIのminとmaxのポジション、スケールの設定】*/
        //float[] cam_posX = { (125.0f / 6.0f) * 0.3f, cSPos.x - 0 };  // { min, max }
        cam_posY = new float[2] { (50.0f / 6.0f) * 0.3f, cSPos.y - 5.0f - 2.5f };
        cam_posZ = new float[2] { (-200.0f / 6.0f) * 0.3f, cSPos.z + 20.0f + 10.0f };

        ui_zoom_scale = new float[2] { 1.0f - 0.7f, 40.0f - (1.0f - 0.7f) }; // { min, max }
        //float ui_out_scale = 1 - 0.7f;              // UIのmin
        //float ui_in_scale = 40.0f - zoom_out_scale; // UIのmax


        float length = 1.5f; // 扱うSin(波の長さ)

        //float cam_scaleX = cSPos.x + easing(duration, easTime, (length)) * cam_posX[0] - Mathf.Abs((easing(duration, easTime, (length)) - 1.0f) / 2.0f) * cam_posX[1] * Convert.ToInt32(TPRate >= 0.5);
        cam_scaleY = cSPos.y + easing(duration, time, (length)) * cam_posY[0] - Mathf.Abs((easing(duration, time, (length)) - 1.0f) / 2.0f) * cam_posY[1] * Convert.ToInt32(TPRate >= 0.5f);
        cam_scaleZ = cSPos.z + easing(duration, time, (length)) * cam_posZ[0] - Mathf.Abs((easing(duration, time, (length)) - 1.0f) / 2.0f) * cam_posZ[1] * Convert.ToInt32(TPRate >= 0.5f);
        
        ui_scale = 1.0f - easing(duration, time, (length)) * ui_zoom_scale[0] + Mathf.Abs((easing(duration, time, (length)) - 1.0f) / 2.0f) * ui_zoom_scale[1] * Convert.ToInt32(TPRate >= 0.5f);

        //Debug.Log($"cam_scaleY{cam_scaleY},cam_scaleZ{cam_scaleZ},ui_scale{ui_scale}");


        cam.transform.position = new Vector3(0, cam_scaleY, cam_scaleZ);
        
        ui.transform.localScale = new Vector3(ui_scale, ui_scale, ui_scale);


        /*【UIが通り過ぎる演出】*/
        if (ui.transform.localScale.x > 26.0f)  //10
        {
            //logo.SetActive(false);
            //menu.SetActive(false);
            for (int i = 0; i < menuObj.Length; i++)
            {
                menuObj[i].GetComponent<Text>().CrossFadeAlpha(0, 0f, true);
            }

        }
        /*----------------------*/

        /*【キャラクターセレクト画面のUIの有効化】*/
        if (cam.transform.position == TPos)
        {
            //Debug.Log("キャラクターセレクトオン");
            for (int i = 0; i < characterUI.Length; i++)
            {
                characterUI[i].SetActive(true);
            }

            gameStart.onCharaSelect = true;

            logo.SetActive(false);
            menu.SetActive(false);
        }
        else
        {
            //Debug.Log($"{cam.transform.position}");
        }
        /*-----------------------------------------*/
    }

}
