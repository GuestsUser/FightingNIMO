using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private Result result;
    [SerializeField] private PlayerInstantiate playerIns;
    [SerializeField] private DataRetation dataRetation;
    [SerializeField] private UpdateTime updateTime;

    private void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        result = GameObject.Find("Result").GetComponent<Result>();
        playerIns = GameObject.Find("CreatePlayer").GetComponent<PlayerInstantiate>();
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();
        //updateTime = GameObject.Find("TimeLimit").GetComponent<UpdateTime>();
    }

    private void Update()
    {
        IsWin();
        IsDraw();
    }

    //勝利条件
    private void IsWin()
    {
        //まだ制限時間以内かつ、試合中かつ、プレイヤー（キャラクター番号）リストが1以下の場合
        if (!updateTime.finished && gameState.isGame && playerIns.playerNum.Count <= 1)
        {
            for (int i = 0; i < dataRetation.characterNum.Length; i++)
            {
                if (dataRetation.characterNum[i] == playerIns.playerNum[0])
                {
                    result.winner = i;
                    gameState.isResult = true;
                    break;
                }
            }
        }
    }

    //引き分け処理
    private void IsDraw()
    {
        //ゲーム終了の合図をされた時かつ、残りのプレイヤーが2人以上残っている場合は
        if (updateTime.finished && playerIns.playerNum.Count >= 2)
        {
            //引き分けにする
            result.winner = -1;
            gameState.isResult = true;
        }
    }
}
