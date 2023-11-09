using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class DataRetation : MonoBehaviour
{
    public static DataRetation instance;

    public GameObject[] playerList; //�v���C���[�ԍ����Ɋi�[���邽�߂̔z��
    public int[] controllerID;          //�v���C���[�ԍ����i�[���邽�߂̔z��
    public int[] characterNum;      //�e�v���C���[���I�������L�����N�^�[�ԍ����i�[���邽�߂̔z��

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
        Array.Resize(ref playerList, 4);    //�ő�v���C�l�����̑傫���ɕύX
        Array.Resize(ref characterNum, 4);  //�ő�v���C�l�����̑傫���ɕύX
        Array.Resize(ref controllerID, 4);  //�ő�v���C�l�����̑傫���ɕύX

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
