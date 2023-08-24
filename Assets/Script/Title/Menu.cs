using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ↓コピー用
#region タイトル
#endregion



public class Menu : MonoBehaviour
{
    #region オブジェクト&コンポネント
    [Tooltip("このスクリプトがアタッチされているオブジェクトが入る")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject cursor;
    [SerializeField] private RectTransform cursorRT;
    #endregion

    [Tooltip("項目同士の縦の間隔")]
    [SerializeField] private float itemSpace;

    private enum menuItems // 扱うメニューの項目名
    {
        GAME = 0,
        CREDIT = 1,
        CONTROLS = 2,
        EXIT = 3
    }
    [Tooltip("現在選択されているメニュー番号")]
    [SerializeField] private menuItems menuName;
    [Tooltip("上下どちらかの入力がされているか")]
    [SerializeField] private bool push;
    [Tooltip("入力継続時間")]
    [SerializeField] private float count;
    [Tooltip("カーソル移動時の時間間隔")]
    [SerializeField] private float interval;


    #region シーン系
    [Tooltip("シーン移動フラグ")]
    [SerializeField] private bool pushScene;
    [Tooltip("シーン移動時の待機時間(この間にSEが鳴る)")]
    [SerializeField] private float waitTime; //シーン移動時に使用する処理待機時間(SEが鳴り終わるまで)
    #endregion

    private float fps;

    // Start is called before the first frame update
    void Start()
    {
        #region オブジェクト系
        menu = this.gameObject;
        cursor = transform.Find("Cursor").gameObject; //子オブジェクトのカーソルを取得
        cursorRT = transform.Find("Cursor").gameObject.GetComponent<RectTransform>();
        #endregion

        menuName = menuItems.GAME;
        if(interval == 0) { interval = 10; }
        //interval = 10;
        #region シーン系
        pushScene = false;
        waitTime = 2.0f;
        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        fps = 1f / Time.deltaTime;
        Debug.Log(fps);

        if (pushScene == false)
        {
            CursorMove();
        }
        
        if (menuName == menuItems.GAME && Gamepad.current.aButton.isPressed)
        {
            Debug.Log("シーン移動");
            pushScene = true;
            StartCoroutine("GotoGameScene");
        }
    }
    void CursorMove()
    {
        // 左スティック上入力時
        if (Gamepad.current.leftStick.y.ReadValue() > 0)
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
        // 左スティック下入力時
        else if (Gamepad.current.leftStick.y.ReadValue() < 0)
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

        #region (←の+で開く)カーソルのポジションを動かす処理
        switch (menuName)
        {
            case menuItems.GAME:
                cursorRT.position = transform.Find("GAME").gameObject.GetComponent<RectTransform>().position;
                break;

            case menuItems.CREDIT:
                cursorRT.position = transform.Find("CREDIT").gameObject.GetComponent<RectTransform>().position;
                break;

            case menuItems.CONTROLS:
                cursorRT.position = transform.Find("CONTROLS").gameObject.GetComponent<RectTransform>().position;
                break;

            case menuItems.EXIT:
                cursorRT.position = transform.Find("EXIT").gameObject.GetComponent<RectTransform>().position;
                break;

            default:
                break;
        }
        #endregion
    }

    private IEnumerator GotoGameScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); //処理を待機 シーン時の音を鳴らすため
        SceneManager.LoadScene("GameScene");
    } 
}
