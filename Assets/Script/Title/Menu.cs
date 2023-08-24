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
    private enum menuItems // 扱うメニューの項目名
    {
        GAME = 0,
        CREDIT = 1,
        CONTROLS = 2,
        EXIT = 3
    }

    #region インスペクターからの代入が必要or調整をするもの
    [Header("【事前に入れるの必須】")]
        [Tooltip("メニューの項目のUIオブジェクトを入れてください")]
        [SerializeField] private GameObject[] menuItem;

        /*【調整するところ】*/
        [Tooltip("項目同士の縦の間隔(正の値を入力してください)")]
        [SerializeField] private float itemSpace;
        [Tooltip("カーソル移動時の時間間隔")]
        [SerializeField] private float interval;
        [Tooltip("選択項目のカラー")]
        [SerializeField] private Color selectionItemColor;
        [Tooltip("非選択項目のカラー")]
        [SerializeField] private Color notSelectionItemColor;
        /*-------------*/
    #endregion

    #region 動作確認用に表示するもの
    [Header("【動作確認用】")]
        [Tooltip("このスクリプトがアタッチされているオブジェクトが入る")]
        [SerializeField] private GameObject menu;
        [Tooltip("このスクリプトがアタッチされているオブジェクトの子要素のカーソルオブジェクトが入る")]
        [SerializeField] private GameObject cursor;
        [Tooltip("カーソルのレンダートランスフォーム情報が入る")]
        [SerializeField] private RectTransform cursorRT;
        [Tooltip("現在選択されているメニュー番号")]
        [SerializeField] private menuItems menuName;
        [Tooltip("上下どちらかの入力がされているか")]
        [SerializeField] private bool push;
        [Tooltip("入力継続時間")]
        [SerializeField] private float count;
    #endregion

    private string[] item;
    //private float fps;

    // Start is called before the first frame update
    void Start()
    {
        /*【オブジェクト情報の取得】*/
        menu = this.gameObject;
        menu.SetActive(true);
        cursor = transform.Find("Cursor").gameObject; //子オブジェクトのカーソルを取得
        cursorRT = cursor.GetComponent<RectTransform>();
        /*-------------------*/

        /*【その他変数の初期化】*/
        menuName = menuItems.GAME;
        push = false;
        count = 0;
        /*----------------*/

        /*【事前の調整が必要な値が未調整だった場合のデフォルトの値】*/
        if (interval == 0) { interval = 10; }                                                                                           // 長押しでの選択項目を移動する時間間隔の設定
        if (itemSpace == 0) { itemSpace = 120; }                                                                                        // メニューの項目同士のY座標間隔
        if (selectionItemColor.a == 0) { selectionItemColor.a = 1; }                                                                    // 透明度をマックスに設定
        if (notSelectionItemColor.a == 0) { notSelectionItemColor.a = 1; }                                                              // 透明度をマックスに設定
        if (selectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#008fd9", out selectionItemColor); }        // 水色に設定
        if (notSelectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#ffffff", out notSelectionItemColor); }  // 白色に設定
        /*----------------------------------------------*/

        /*【自動調節系】*/
        Array.Resize(ref item, menuItem.Length);                                                         // 配列のサイズをメニュー項目と同じ数に設定
        for (int i = 0;i < menuItem.Length; i++)
        {
            menuItem[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0,i * -itemSpace);  // メニュー項目同士のY座標間隔を指定した間隔に設定
            item[i] = menuItem[i].name;                                                                  // 配列の中にメニュー項目の名前を代入
        }
        /*----------------------------------------------*/
    }

    // Update is called once per frame
    void Update()
    {
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        // カーソル移動関数
        CursorMove();

        // GAME項目が選択されている状態で決定を押した時
        if (menuName == menuItems.GAME && Gamepad.current.aButton.wasPressedThisFrame)
        {
            menu.SetActive(false);
        }
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
                if (--menuName < menuItems.GAME) menuName = menuItems.EXIT;
            }
            else               // 長押し時の処理
            {
                count++;
                if (count % interval == 0)
                {
                    if (--menuName < menuItems.GAME) menuName = menuItems.EXIT;
                }
            }

        }
        // 左スティック下入力時 or 十字キー下入力時
        else if (Gamepad.current.leftStick.y.ReadValue() < 0 || Gamepad.current.dpad.down.isPressed)
        {
            if (push == false)
            {
                push = true;
                if (++menuName > menuItems.EXIT) menuName = menuItems.GAME;
            }
            else
            {
                count++;
                if (count % interval == 0)
                {
                    if (++menuName > menuItems.EXIT) menuName = menuItems.GAME;
                }
            }
        }
        else
        {
            push = false;
            count = 0;
        }
        #endregion

        #region カーソルのポジションの変更とメニュー項目の色の変更
        switch (menuName)
        {
            case (menuItems)0:
                // カーソル位置の変更
                cursorRT.position = menuItem[(int)menuName].GetComponent<RectTransform>().position;

                // 文字の色の変更
                menuItem[(int)menuName].GetComponent<Text>().color = selectionItemColor;
                for(menuItems i = menuItems.GAME; i < menuItems.EXIT + 1; i++)
                {
                    if(i != menuName)
                    {
                        menuItem[(int)i].GetComponent<Text>().color = notSelectionItemColor;
                    }
                }
                break;

            case (menuItems)1:
                // カーソル位置の変更
                cursorRT.position = menuItem[(int)menuName].gameObject.GetComponent<RectTransform>().position;

                // 文字の色の変更
                menuItem[(int)menuName].GetComponent<Text>().color = selectionItemColor;
                for (menuItems i = menuItems.GAME; i < menuItems.EXIT + 1; i++)
                {
                    if (i != menuName)
                    {
                        menuItem[(int)i].GetComponent<Text>().color = notSelectionItemColor;
                    }
                }
                break;

            case (menuItems)2:
                // カーソル位置の変更
                cursorRT.position = menuItem[(int)menuName].gameObject.GetComponent<RectTransform>().position;

                // 文字の色の変更
                menuItem[(int)menuName].GetComponent<Text>().color = selectionItemColor;
                for (menuItems i = menuItems.GAME; i < menuItems.EXIT + 1; i++)
                {
                    if (i != menuName)
                    {
                        menuItem[(int)i].GetComponent<Text>().color = notSelectionItemColor;
                    }
                }
                break;

            case (menuItems)3:
                // カーソル位置の変更
                cursorRT.position = menuItem[(int)menuName].gameObject.GetComponent<RectTransform>().position;

                // 文字の色の変更
                menuItem[(int)menuName].GetComponent<Text>().color = selectionItemColor;
                for (menuItems i = menuItems.GAME; i < menuItems.EXIT + 1; i++)
                {
                    if (i != menuName)
                    {

                        menuItem[(int)i].GetComponent<Text>().color = notSelectionItemColor;
                    }
                }
                break;

            default:
                break;
        }
        #endregion
    }

   
}
