using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class DataRetation : MonoBehaviour
{
    public static DataRetation instance;

    public GameObject[] playerList; //�v���C���[�ԍ����Ɋi�[���邽�߂̔z��
    public int[] controllerID;      //�e�v���C���[�̃R���g���[���[ID���i�[���邽�߂̔z��
    public int[] characterNum;      //�e�v���C���[���I�������L�����N�^�[�ԍ����i�[���邽�߂̔z��
    public int round;               //���E���h�񐔁i���͎g�p���Ă��Ȃ��j

    private void Awake()
    {
        //�V���O���g���쐬
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
        //Array.Resize(ref playerList, 4);    //�ő�v���C�l�����̑傫���ɕύX
        //Array.Resize(ref characterNum, 4);  //�ő�v���C�l�����̑傫���ɕύX
        //Array.Resize(ref controllerID, 4);  //�ő�v���C�l�����̑傫���ɕύX
    }

    //�z��̏�����
    public void InitializeArray()
    {
        int maxPlayer = 4;  //�ő�v���C�l�����̑傫��
        playerList = new GameObject[maxPlayer];
        characterNum = new int[maxPlayer];
        controllerID = new int[maxPlayer];

        for (int i = 0; i < 4; i++)
        {
            playerList[i] = null;   //�v���C���[���X�g�̏�����
            characterNum[i] = -1;  //�e�v���C���[�̃L�����N�^�[�ԍ��̏�����
            controllerID[i] = -1;  //�e�v���C���[�̃R���g���[���[ID�̏�����
        }
    }
}
