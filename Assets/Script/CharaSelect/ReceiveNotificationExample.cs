using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ReceiveNotificationExample : MonoBehaviour
{

    public int playerNum;       //�v���C���[����
    public Transform[] playerSpawnPos; //�v���C���[�X�|�[���ʒu�z��


    public int count;

    private void Awake()
    {

    }

    private void Start()
    {
        count = 2;
    }

    private void Update()
    {
        if (--count < 0) { count = 0; }

        
    }

    //�v���C���[�������Ɏ󂯎��ʒm
    //�Q�[���p�b�h�܂��̓L�[�{�[�h��A�{�^�����������ƂŃv���C���[���쐬
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        count = 2;
        
        playerNum = playerInput.playerIndex + 1;    //�v���C���[������0�ł͂Ȃ��P����n�߂�悤�ɂ��Ă���

        //�v���C���[�ԍ��ɂ���Ĕz�u�ʒu��ύX���Ă���
        switch (playerNum)
        {
            //Player1
            case 1:
                playerInput.gameObject.transform.position = playerSpawnPos[0].position;
                break;
            //Player2
            case 2:
                playerInput.gameObject.transform.position = playerSpawnPos[1].position;
                break;
            //Player3
            case 3:
                playerInput.gameObject.transform.position = playerSpawnPos[2].position;
                break;
            //Player4
            case 4:
                playerInput.gameObject.transform.position = playerSpawnPos[3].position;
                break;
            //�����Ȃ�
            default:
                return;
        }

        //�ڑ����ꂽ���ǂ����̊m�F���O
        Debug.Log("�v���C���[" + playerNum + "���������܂���");
    }

    // �v���C���[�ގ����Ɏ󂯎��ʒm(�L��������Ă���ꍇ�̂ݔ���)
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        print($"�v���C���[#{playerInput.user.index + 1}���ގ��I");
    }
}