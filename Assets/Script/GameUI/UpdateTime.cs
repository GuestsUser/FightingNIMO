using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTime : MonoBehaviour
{
    [Tooltip("分のテキスト")]
    [SerializeField] private Text[] minutesText;
    [Tooltip("秒のテキスト")]
    [SerializeField] private Text[] secondsText;

    [Tooltip("分")]
    [SerializeField] private int minutes;

    [Tooltip("秒")]
    [SerializeField] private int seconds;

    [Tooltip("残り時間")]
    [SerializeField] private float timeLimit;

    string tMinutes;
    string tSeconds;

    bool finished;

    // Start is called before the first frame update
    void Start()
    {
        minutes = 3;
        seconds = 0;

        timeLimit = (minutes * 60) + seconds;

        tMinutes = $"00";
        tSeconds = $"00";

        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームの進行状況を管理しているスクリプトから取得、試合進行状況を取得
        //if (isGame)
        //{
            // 制限時間が0秒以下なら何もしない
            if (timeLimit <= 0f)
            {
                timeLimit = 0;
                finished = true; // 終わりの合図

                Debug.Log("制限時間終了");
                return;
            }
            else
            {
                ChangeTime();
            }
        //}
    }

    void ChangeTime()
    {
        timeLimit -= Time.deltaTime;         // 経過時間を引いていく

        minutes = (int)timeLimit / 60;
        seconds = (int)timeLimit - minutes * 60;

        tMinutes = $"0{minutes.ToString()}";

        // secondsが2桁の場合
        if(seconds >= 10)
        {
            tSeconds = $"{seconds.ToString()}";
        }
        // secondsが1桁の場合
        else
        {
            tSeconds = $"0{seconds.ToString()}";
        }

        // Textをそれぞれ1文字取得
        minutesText[0].text = tMinutes.Substring(0, 1); // 分の0文字目から1文字取得
        minutesText[1].text = tMinutes.Substring(1, 1); // 分の1文字目から1文字取得
        secondsText[0].text = tSeconds.Substring(0, 1); // 秒の0文字目から1文字取得
        secondsText[1].text = tSeconds.Substring(1, 1); // 秒の1文字目から1文字取得

    }

}
