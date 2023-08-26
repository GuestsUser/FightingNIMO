using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class SelectCharacter : MonoBehaviour
{
    [Header("【事前に入れるもの】")]
    [Tooltip("使用するキャラクターを入れてください")]
    [SerializeField] private List<GameObject> characters;
    [Tooltip("このプレイヤーのカーソルを入れてください")]
    [SerializeField] private GameObject cursor;
    [Tooltip("このプレイヤーのカーソルのテキストを入れてください")]
    [SerializeField] private RectTransform cursorText;
    [Tooltip("カーソル移動時の時間間隔")]
    [SerializeField] private Text PlayerNum;
    [Tooltip("このプレイヤーのカーソルを入れてください")]
    [SerializeField] private int maxChara;
    [Tooltip("カーソル移動時の時間間隔")]
    [SerializeField] private float interval;

    [Header("【動作確認用】")]
    [Tooltip("選べるキャラクター")]
    [SerializeField] private GameObject[] characterUI;
    [Tooltip("カーソルのレンダートランスフォーム情報が入る")]
    [SerializeField] private RectTransform cursorRT;
    [Tooltip("現在選択されているメニュー番号")]
    [SerializeField] private int menuNum;
    [Tooltip("現在選択されているメニュー名")]
    [SerializeField] private string menuName;
    //[Tooltip("項目同士の縦の間隔(正の値を入力してください)")]
    //[SerializeField] private float itemSpace;
    [Tooltip("入力継続時間")]
    [SerializeField] private float count;
    [Tooltip("左スティックのX軸を入力しているか")]
    [SerializeField] private bool push;


    [SerializeField] private PlayerInput input;

    private string[] item; // 使用されているキャラクター名を保存するstring型配列
    //private string parentParentName;
    void Awake()
    {
        input = this.GetComponent<PlayerInput>();        // PlayerInputを取得
        cursorRT = cursor.GetComponent<RectTransform>(); // カーソルのRectTransformを取得
        
        /*【キャラクターアイコンUI】*/
        Array.Resize(ref characterUI, maxChara); // 配列のサイズをキャラクターの数で初期化
        characterUI[0] = GameObject.Find("BaseButton");
        characterUI[1] = GameObject.Find("SharkButton");
        //characterObj[2] = GameObject.Find("");
        //characterObj[3] = GameObject.Find("");
        /*--------------------*/

        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (interval == 0) { interval = 10; }
        Array.Resize(ref item, maxChara);
        for (int i = 0; i < maxChara; i++)
        {
            item[i] = characterUI[i].name;
        }
        menuNum = 0;
        menuName = characterUI[0].name;
        //　プレイヤーカーソルテキストをP1/P2/P3/P4に設定
        PlayerNum.text = "P" + (input.user.index + 1);

        //switch (input.user.index)
        //{
        //    case 0:
                
        //        break;
        //    case 1:

        //        break;
        //    case 2:

        //        break;
        //    case 3:

        //        break;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        menuName = item[menuNum];
        switch (menuNum)
        {
            case 0: // ベース
                characters[menuNum].SetActive(true);
                for(int i = 0;i < maxChara; i++)
                {
                    if(i != menuNum)
                    {
                        characters[i].SetActive(false);
                    }
                }
                break;
            case 1: // サメ
                characters[menuNum].SetActive(true);
                for (int i = 0; i < maxChara; i++)
                {
                    if (i != menuNum)
                    {
                        characters[i].SetActive(false);
                    }
                }
                break;
            //case 2:
            //    break;
            //case 3:
            //    break;
        }
    }

    /*【On+Action名(InputValue value)←でアクションマップのコールバック関数を定義】*/

    public void OnMove(InputValue value)
    {
        //Debug.Log($"{this.name}:{value.Get<float>()}");
        //var value = context.ReadValue<Vector2>();
        if (value.Get<float>() > 0)
        {
            if (push == false) // 押された時の処理
            {
                push = true;
                if (--menuNum < 0) menuNum = maxChara - 1;
                //audioSouce.clip = moveSE;
                //audioSouce.PlayOneShot(moveSE);
            }
            else               // 長押し時の処理
            {
                count++;
                if (count % interval == 0)
                {
                    if (--menuNum < 0) menuNum = maxChara - 1;
                    //audioSouce.clip = moveSE;
                    //audioSouce.PlayOneShot(moveSE);
                }
            }
        }
        else if(value.Get<float>() < 0)
        {
            if (push == false)
            {
                push = true;
                if (++menuNum > maxChara - 1) menuNum = 0;
                //audioSouce.clip = moveSE;
                //audioSouce.PlayOneShot(moveSE);
            }
            else
            {
                count++;
                if (count % interval == 0)
                {
                    if (++menuNum > maxChara - 1) menuNum = 0;
                    //audioSouce.clip = moveSE;
                    //audioSouce.PlayOneShot(moveSE);
                }
            }
        }
        else
        {
            push = false;
            count = 0;
        }

        #region カーソルの移動処理(項目の数によって自動でループ数が変更され、移動できるポジションも増減する)
        for (int i = 0; i < maxChara; i++)
        {
            if (menuName == item[i])
            {
                // カーソル位置の変更
                cursorRT.position = characterUI[i].GetComponent<RectTransform>().position;

                // 文字の色の変更
                //characterObj[i].GetComponent<Text>().color = selectionItemColor;
                //for (int j = 0; j < menuObj.Length; j++)
                //{
                //    if (item[j] != menuName)
                //    {
                //        characterObj[j].GetComponent<Text>().color = notSelectionItemColor;
                //    }
                //}
            }
        }
        #endregion
    }

    public void OnSubmit(InputValue value)
    {
        Debug.Log("プレイキャラクターを確定");
        // プレイキャラクターを確定
        // 確定フラグ = true
    }

    public void OnCancel(InputValue value)
    {
        //if(確定フラグ == true) 確定フラグ = false
    }

    
}
