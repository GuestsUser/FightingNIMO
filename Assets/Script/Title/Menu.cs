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
        [Tooltip("メニューの項目の数を指定し、UIオブジェクトを入れてください")]
        [SerializeField] private GameObject[] menuObj;
        [Tooltip("メニューグループ(空の親オブジェクト)の子要素のカーソルを入れてください")]
        [SerializeField] private GameObject cursor;
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
    #endregion

    private string[] item; // 使用されているメニュー項目名を保存するstring型配列
    //private float fps;

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
        
        for (int i = 0; i < menuObj.Length; i++)
        {
            menuObj[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);  // メニュー項目同士のY座標間隔を指定した間隔に設定
            item[i] = menuObj[i].name;                                                                   // 配列の中にメニュー項目の名前を代入
        }
        /*--------------*/

        /*【その他変数の初期化】*/
        menuName = item[0];
        menuNum = 0;
        push = false;
        count = 0;
        /*----------------------*/
        
    }

    // Update is called once per frame
    void Update()
    {
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        // カーソル移動関数
        CursorMove();

        Decision();
        
    }
    void CursorMove()
    {
        #region 選択項目の変更
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
        for (int i = 0;i < menuObj.Length;i++)
        {
            if(menuName == item[i])
            {
                // カーソル位置の変更
                cursorRT.position = menuObj[i].GetComponent<RectTransform>().position;

                // 文字の色の変更
                menuObj[i].GetComponent<Text>().color = selectionItemColor;
                for (int j = 0; j < menuObj.Length; j++)
                {
                    if (item[j] != menuName)
                    {
                        menuObj[j].GetComponent<Text>().color = notSelectionItemColor;
                    }
                }
            }
        }
        #endregion
    }

    void Decision()
    {
        
        // 一番上の項目が選択されている状態で決定を押した時
        if (menuName == item[0] && Gamepad.current.aButton.wasPressedThisFrame)
        {
            menu.SetActive(false);
        }

    }

   
}
