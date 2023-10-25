using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;


public class MenuTest : MonoBehaviour
{
    private string[] items; //タイトル画面をメニュー項目名を保持する配列

    // CS系
    [Tooltip("GameStartSystem.csを持っているオブジェクトを入れてください")]
    [SerializeField] private GameStartSystem gameStartSys;
    //[Tooltip("PlayerInputManager.csを持つオブジェクトを入れてください")]
    //[SerializeField] private PlayerInputManager playerInputManager;

    //UI系
    [Tooltip("メニュー専用のカーソルを入れてください")]
    [SerializeField] private GameObject cursor;
    [Tooltip("メニューカーソルのRectTransform取得用")]
    [SerializeField] private RectTransform cursorRT;
    [Tooltip("タイトルで表示する項目UI(4つ)を入れてください")]
    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject logo;
    [Tooltip("各キャラクターUIを入れてください")]
    [SerializeField] private GameObject[] characterUI;

    // 音系
    [SerializeField] private AudioSource audioSource;   //自身のAudioSourceを入れる
    [SerializeField] private AudioClip desisionSE;      //決定音
    [SerializeField] private AudioClip cancelSE;        //キャンセル音
    [SerializeField] private AudioClip moveSE;          //移動音
    [SerializeField] private AudioClip openMenuSE;      //
    [SerializeField] private AudioClip closeMenuSE;     //

    //値調整系
    [Tooltip("項目同士の縦の間隔(正の値を入力してください)")]
    [SerializeField] private float itemSpace;
    [Tooltip("カーソル移動時の時間間隔")]
    [SerializeField] private float interval;
    [Tooltip("選択項目のカラー")]
    [SerializeField] private Color selectionItemColor;
    [Tooltip("非選択項目のカラー")]
    [SerializeField] private Color notSelectionItemColor;

    [Header("動作確認用")]
    [Tooltip("現在選択されているメニュー番号")]
    [SerializeField] private int currentMenuNum;
    [Tooltip("現在選択されているメニュー名")]
    [SerializeField] private string currentMenuName;
    [Tooltip("上下どちらかの入力がされているか")]
    [SerializeField] private bool isPush;
    [Tooltip("入力継続時間")]
    [SerializeField] private float inputCount;

    private void Awake()
    {
        //事前の調整が必要な値が未調整だった場合
        if (interval == 0) { interval = 12; }                                                                                           // 長押しでの選択項目を移動する時間間隔の設定
        if (itemSpace == 0) { itemSpace = 120; }                                                                                        // メニューの項目同士のY座標間隔
        if (selectionItemColor.a == 0) { selectionItemColor.a = 1; }                                                                    // 透明度をマックスに設定
        if (notSelectionItemColor.a == 0) { notSelectionItemColor.a = 1; }                                                              // 透明度をマックスに設定
        if (selectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#008fd9", out selectionItemColor); }        // 水色に設定
        if (notSelectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#ffffff", out notSelectionItemColor); }  // 白色に設定
    }

    private void Start()
    {
        this.gameObject.SetActive(true);    //タイトルメニュー一覧を表示する
        logo.SetActive(true);               //ロゴを表示する

        Array.Resize(ref items, menuItems.Length);  //menuItemsの数によってitemsの大きさを変更する

        for(int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);    //メニュー項目同士のY座標間隔を指定した間隔に設定
            items[i] = menuItems[i].name;   //配列の中にメニュー項目の名前を代入する
        }

        for(int i = 0; i < characterUI.Length; i++)
        {
            characterUI[i].SetActive(false);    //各キャラクターUIを非表示にする
        }

        currentMenuName = items[0];  //選択されているメニュ名を1番上に初期化
        currentMenuNum = 0;          //選択されているメニュー番号を1番上に初期化
        isPush = false;              //上下どちらか押されているかの確認用フラグの初期化
        inputCount = 0;              //ボタン入力継続時間の初期化
    }

    private void Update()
    {
        //Gamepadが1台も接続されていないとエラーを起こすため、1台でも接続されているときだけ処理させる
        if (Gamepad.all.Count != 0)
        {
            CursorMove();   //メニューカーソル移動処理
        }
        Decision();     //各メニューボタンが押されたときの処理
    }

    //カーソルの移動関連処理
    private void CursorMove()
    {
        //GamePad（左スティック上入力時 or 十字キー上入力時）
        if (Gamepad.current.leftStick.y.ReadValue() > 0 || Gamepad.current.dpad.up.isPressed)
        {
            if (isPush == false)    //押された時の処理
            {
                isPush = true;
                //currentMenuNumが上を押されることで減らされ、0より小さい値になったら
                if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;   //カーソルを一番下に移動させる
                //audioSource.clip = moveSE;
                //audioSource.PlayOneShot(moveSE);
            }
            else    //長押し時の処理
            {
                inputCount++;    //カウントさせる
                //カウントと移動インターバルを割った余りが0の場合
                if (inputCount % interval == 0)
                {
                    if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;   //カーソルを一番下に移動させる
                    //audioSource.clip = moveSE;
                    //audioSource.PlayOneShot(moveSE);
                }
            }
        }
        //GamePad（左スティック下入力時 or 十字キー下入力時）
        else if (Gamepad.current.leftStick.y.ReadValue() < 0 || Gamepad.current.dpad.down.isPressed)
        {
            if (isPush == false)    //押された時の処理
            {
                isPush = true;
                //currentMenuNumが下を押されることで増え、項目数より大きい値になったら
                if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;    //カーソルを一番上に移動させる
                //audioSource.clip = moveSE;
                //audioSource.PlayOneShot(moveSE);
            }
            else    //長押し時の処理
            {
                inputCount++;
                //カウントと移動インターバルを割った余りが0の場合
                if (inputCount % interval == 0)
                {
                    if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;    //カーソルを一番上に移動させる
                    //audioSource.clip = moveSE;
                    //audioSource.PlayOneShot(moveSE);
                }
            }
        }
        else      //何もしてない時
        {
            isPush = false;
            inputCount = 0;
        }

        currentMenuName = items[currentMenuNum];  //選択されているメニュー名をmenuNameに代入する（随時更新）

        //色変更
        for (int i = 0; i < menuItems.Length; i++)
        {
            //現在選択されているメニュー名とメニュー項目内にある名前が一致した場合
            if (currentMenuName == items[i])
            {
                cursorRT.position = menuItems[i].GetComponent<RectTransform>().position;    //メニューカーソルの位置を選択されているメニューの位置に変更する
                menuItems[i].GetComponent<Text>().color = selectionItemColor;               //メニュー名の色を選択されているときの色に変更する
            }
            //現在選択されているメニュー名とメニュー項目内にある名前が一致しなかった場合
            else if (currentMenuName != items[i])
            {
                menuItems[i].GetComponent<Text>().color = notSelectionItemColor;    //メニュー名の色を選択されていない時の色に変更する
            }
        }
    }

    //各メニューボタンが押されたときの処理
    private void Decision()
    {
        //[GAME]項目が選択されている状態かつプレイヤーが決定を押した時
        if (currentMenuName == items[0] && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            gameStartSys.isCharSelect = true; //trueにすることでキャラクターセレクトになる
            for (int i = 0; i < characterUI.Length; i++)
            {
                gameStartSys.isCharSelect = true;   //現在がキャラクターセレクト画面であることを示す
                characterUI[i].SetActive(true);     //各キャラクターUI(ボタン)を表示する
                logo.SetActive(false);              //タイトルロゴを非表示にする
                this.gameObject.SetActive(false);   //タイトルメニューを非表示にする
            }
        }

        ////[CREDIT]項目が選択されている状態かつプレイヤーが決定を押した時
        //if (menuName == items[1] && gamePad.buttonSouth.wasPressedThisFrame)
        //{

        //}

        ////[CONTROLS]項目が選択されている状態かつプレイヤーが決定を押した時
        //if (menuName == items[2] && gamePad.buttonSouth.wasPressedThisFrame)
        //{

        //}

        //[EXIT]が選択されている状態かつプレイヤーが決定を押した時
        if (currentMenuName == items[3] && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            //ゲームを終了させる
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}