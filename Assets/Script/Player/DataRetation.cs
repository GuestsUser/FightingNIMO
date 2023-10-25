using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class DataRetation : MonoBehaviour
{
    public GameObject[] playerList; //プレイヤー番号順に格納するための配列
    public int[] playerID;          //
    public int[] characterNum;      //各プレイヤーが選択したキャラクター番号を格納するための配列

    private void Start()
    {
        Array.Resize(ref playerList, 4);    //最大プレイ人数分の大きさに変更
        Array.Resize(ref characterNum, 4);  //最大プレイ人数分の大きさに変更
        Array.Resize(ref playerID, 4);

        for (int i = 0; i < characterNum.Length; i++)
        {
            characterNum[i] = -1;
        }
    }
}
