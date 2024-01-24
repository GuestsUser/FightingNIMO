using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class DataRetation : MonoBehaviour
{
    public static DataRetation instance;

    public GameObject[] playerList; //プレイヤー番号順に格納するための配列
    public int[] controllerID;      //各プレイヤーのコントローラーIDを格納するための配列
    public int[] characterNum;      //各プレイヤーが選択したキャラクター番号を格納するための配列
    public int round;               //ラウンド回数（今は使用していない）

    private void Awake()
    {
        //シングルトン作成
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Array.Resize(ref playerList, 4);    //最大プレイ人数分の大きさに変更
        //Array.Resize(ref characterNum, 4);  //最大プレイ人数分の大きさに変更
        //Array.Resize(ref controllerID, 4);  //最大プレイ人数分の大きさに変更
    }

    //配列の初期化
    public void InitializeArray()
    {
        int maxPlayer = 4;  //最大プレイ人数分の大きさ
        playerList = new GameObject[maxPlayer];
        characterNum = new int[maxPlayer];
        controllerID = new int[maxPlayer];

        for (int i = 0; i < 4; i++)
        {
            playerList[i] = null;   //プレイヤーリストの初期化
            characterNum[i] = -1;  //各プレイヤーのキャラクター番号の初期化
            controllerID[i] = -1;  //各プレイヤーのコントローラーIDの初期化
        }
    }
}
