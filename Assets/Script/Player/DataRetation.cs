using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class DataRetation : MonoBehaviour
{
    public GameObject[] playerList; //�v���C���[�ԍ����Ɋi�[���邽�߂̔z��
    public int[] playerID;          //
    public int[] characterNum;      //�e�v���C���[���I�������L�����N�^�[�ԍ����i�[���邽�߂̔z��

    private void Start()
    {
        Array.Resize(ref playerList, 4);    //�ő�v���C�l�����̑傫���ɕύX
        Array.Resize(ref characterNum, 4);  //�ő�v���C�l�����̑傫���ɕύX
        Array.Resize(ref playerID, 4);

        for (int i = 0; i < characterNum.Length; i++)
        {
            characterNum[i] = -1;
        }
    }
}
