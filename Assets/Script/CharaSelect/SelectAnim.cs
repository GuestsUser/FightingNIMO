using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SelectAnim : MonoBehaviour
{
    [SerializeField] private CharSelectManager CSM;
    [SerializeField] private GameStartSystem GSS;
    [SerializeField] private Text playerNumText;
    [SerializeField] private GameObject characterState;
    [SerializeField] private GameObject circle;
    [SerializeField] private GameObject[] circles;
    [SerializeField] private Image backImage;
    [SerializeField] private Color readyColor;
    [SerializeField] private GameObject check;
    [SerializeField] private float[] easTimes;
    [SerializeField] private float stopTime;
    [SerializeField] private float stopDuration;
    [SerializeField] private float maxY;
    [SerializeField] private float delay;
    [SerializeField] private float speed;

    [SerializeField] private float space;

    [SerializeField] private bool complete;

    [Tooltip("自身のPlayerInputを取得(自動)")]
    [SerializeField] private PlayerInput input;

    // Start is called before the first frame update
    void Start()
    {
        GSS = GameObject.Find("GameStartSystem").GetComponent<GameStartSystem>();
        input = this.GetComponent<PlayerInput>();			//このスクリプトを持つオブジェクトのPlayerInputを取得
        characterState.GetComponent<CanvasGroup>().alpha = 0.0f;
        Array.Resize(ref easTimes, circles.Length);   //circlesの数によってeasTimesの大きさを変更する
        //Array.Resize(ref stopTimes, circles.Length);  //circlesの数によってstopTimesの大きさを変更する

        for(int i = 0; i < circles.Length; i++)
        {
            easTimes[i] = 45.0f;
        }
        SetPlayerInfo();

        complete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GSS.isCharSelect)
        {
            //SetPlayerInfo();

            characterState.GetComponent<CanvasGroup>().alpha = 1.0f;
            if (CSM.isCharSelected)
            {
                backImage.color = readyColor;
                circle.GetComponent<CanvasGroup>().alpha = 0.0f;
                check.SetActive(true);
            }
            else
            {
                backImage.color = Color.white;
                circle.GetComponent<CanvasGroup>().alpha = 1.0f;
                check.SetActive(false);
                CircleAnim();
            }

        }
        else
        {
            characterState.GetComponent<CanvasGroup>().alpha = 0.0f;
        }
    }
        
    void SetPlayerInfo()
    {
        //space = (1920 - ((1920.0f / 2.0f) - initPos) * 2) / 4;
        //characterState.GetComponent<RectTransform>().anchoredPosition = new Vector2(-initPos + (input.playerIndex * space), 300.0f);

        //1P～4Pカーソルの初期位置設定
        switch (input.playerIndex)
        {
            //1P
            case 0:
                characterState.GetComponent<RectTransform>().anchoredPosition = new Vector2(-((space / 2.0f) + space), 300.0f);
                playerNumText.text = "<color=#ff6363>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorTextをP1/P2/P3/P4に設定
                /* ---------- */
                break;
            //2P
            case 1:
                characterState.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(space / 2.0f), 300.0f);
                playerNumText.text = "<color=#33b0ff>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorTextをP1/P2/P3/P4に設定
                /* ---------- */
                break;
            //3P
            case 2:
                characterState.GetComponent<RectTransform>().anchoredPosition = new Vector2((space / 2.0f), 300.0f);
                playerNumText.text = "<color=#f4f54b>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorTextをP1/P2/P3/P4に設定
                /* ---------- */
                break;
            //4P
            case 3:
                characterState.GetComponent<RectTransform>().anchoredPosition = new Vector2(((space / 2.0f) + space), 300.0f);
                playerNumText.text = "<color=#4cf54b>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorTextをP1/P2/P3/P4に設定
                /* ---------- */
                break;
        }
    }
    void CircleAnim()
    {
        for(int i = 0; i < circles.Length; i++)
        {
            circles[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(circles[i].GetComponent<RectTransform>().anchoredPosition.x, (maxY / 2.0f) * easing(speed, easTimes[i], 0.5f));

            // 点が初期位置に戻ってきたとき
            if (/*(maxY / 2.0f) * easing(speed, easTimes[2], 0.5f) == 0 && */easTimes[2] == 100.0f)
            {
                if (i == 2)
                {
                    //if(complete == false)
                    //{
                    //    complete = true;
                    //}
                    stopTime++;
                }

                
                if (stopTime > stopDuration)
                {
                    stopTime = 0;
                    //complete = false;

                    for (int j = 0; j < circles.Length; j++)
                    {
                        easTimes[j] = 45.0f;
                    }
                }
            }

            // stopTimeが0であれば
            if (stopTime == 0/* && complete == false*/)
            {
                if (i > 0)
                {
                    if (easTimes[i - 1] > delay && easTimes[i] < 100)
                    {
                        easTimes[i]++;
                    }

                }
                else if(easTimes[0] < 100)
                {
                    easTimes[0]++;
                }
            }
        }
    }

    float AddSpace(int playerIndex)
    {
        if (playerIndex > 0)
        {
            return 1.0f;
        }

        return 0.0f;
       
    }
    float easing(float duration, float time, float length)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easingの進行状況を示す値を算出

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) + 1;
    }
}
