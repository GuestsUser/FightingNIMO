using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControll : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private Result result;
    [SerializeField] private PlayerInstantiate playerIns;
    [SerializeField] private DataRetation dataRetation;
    [SerializeField] private UpdateTime updateTime;

    //SE関連
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip roundWinSE;      //1ラウンド勝った時の音


    private void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        result = GameObject.Find("Result").GetComponent<Result>();
        playerIns = GameObject.Find("CreatePlayer").GetComponent<PlayerInstantiate>();
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();
    }

    private void Update()
    {
        IsWin();
        IsDraw();
    }

    //勝利条件
    private void IsWin()
    {
        //制限時間以内かつ、固法のプレイヤーが1人だけの場合
        if (!updateTime.finished && gameState.isGame && playerIns.playerNum.Count <= 1)
        {
            for (int i = 0; i < dataRetation.characterNum.Length; i++)
            {
                if (dataRetation.characterNum[i] == playerIns.playerNum[0])
                {
                    //SE（1ラウンド勝った時の音）
                    audioSource.clip = roundWinSE;
                    audioSource.PlayOneShot(roundWinSE);

                    //iPの勝ちにする
                    result.winner = i;
                    gameState.isResult = true;
                    gameState.isGame = false;
                }
            }
        }
    }

    //引き分け処理
    private void IsDraw()
    {
        //ゲーム終了の合図をされた時かつ、残りのプレイヤーが2人以上残っている場合
        if (updateTime.finished && playerIns.playerNum.Count >= 2)
        {
            //引き分けにする
            result.winner = -1;
            gameState.isResult = true;
        }
    }
}
