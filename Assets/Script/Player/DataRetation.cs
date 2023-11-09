using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class DataRetation : MonoBehaviour
{
    public static DataRetation instance;

    public GameObject[] playerList; //プレイヤー番号順に格納するための配列
    public int[] controllerID;          //プレイヤー番号を格納するための配列
    public int[] characterNum;      //各プレイヤーが選択したキャラクター番号を格納するための配列

    private void Awake()
    {
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
        Array.Resize(ref playerList, 4);    //最大プレイ人数分の大きさに変更
        Array.Resize(ref characterNum, 4);  //最大プレイ人数分の大きさに変更
        Array.Resize(ref controllerID, 4);  //最大プレイ人数分の大きさに変更

        for (int i = 0; i < characterNum.Length; i++)
        {
            characterNum[i] = -1;
        }

        for (int i = 0; i < controllerID.Length; i++)
        {
            controllerID[i] = -1;
        }
    }
}
