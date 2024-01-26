using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    [Tooltip("開始前のカウントダウンテキスト")]
    [SerializeField] public Image fadePanel;
    [Tooltip("開始前のカウントダウンテキスト")]
    [SerializeField] public Text coundDownText;

    // Gameの進行状況
    [Tooltip("試合が開始されたかどうかかどうか")]
    [SerializeField] public bool isStart;
    [Tooltip("試合続行中かどうか")]
    [SerializeField] public bool isGame;
    [Tooltip("Result表示かどうか")]
    [SerializeField] public bool isResult;
    [Tooltip("決着がついたかどうか")]
    [SerializeField] public bool isGameSet;

    //SE関連
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameStartSE;      //FIGHTのタイミングの音

    // 時間系
    [Tooltip("試合開始までの待機時間")]
    [SerializeField] private float waitTime; // インスペクターから調整

    void Start()
    {
        isStart = false;
        isGame = false;
        isResult = false;
        isGameSet = false;

        coundDownText.text = "3";
        coundDownText.CrossFadeAlpha(0.0f, 0.0f, true);
        fadePanel.CrossFadeAlpha(0.0f, 0.5f, true);
    }

    void Update()
    {
        // 試合開始前の場合
        if(isStart == false)
        {
            // 待機時間が0.0fよりも大きかった場合、waitTimeから経過した時間を引いていく (どのタイミングでもwaitTimeに0.0f以上の値が代入されれば演出がストップする)
            if (waitTime > 0.0f)
            {
                waitTime -= Time.deltaTime;
                if(waitTime < 3.0f)
                {
                    coundDownText.CrossFadeAlpha(1.0f, 0.0f, true);
                    coundDownText.text = Mathf.Ceil(waitTime).ToString("0");
                }
                
                
            }
            else // 試合開始
            {
                coundDownText.text = "FIGHT!";
                coundDownText.CrossFadeAlpha(0.0f, 1.0f, true);

                //SE（FIGHTのタイミングの音）
                audioSource.clip = gameStartSE;
                audioSource.PlayOneShot(gameStartSE);
                isStart = true;
                isGame = true;
            }
        }
        else if (isGame == false) // ゲーム終了時 Resultを表示させる
        {
            isResult = true;
        }
    }
}
