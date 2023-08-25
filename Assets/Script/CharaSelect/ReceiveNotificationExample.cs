using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ReceiveNotificationExample : MonoBehaviour
{

    public int playerNum;       //�v���C���[�ԍ����
    public int characterNum;    //�L�����N�^�[�ԍ�
    //private Transform[] playerSpawnPos; //�v���C���[�X�|�[���ʒu�z��

    public Button[] button;

    public string[] controllerNames;
    int controllerCount;
  

    private void Awake()
    {

    }

    private void Start()
    {
    }

    private void Update()
    {

    }

    //�v���C���[�������Ɏ󂯎��ʒm
    //�Q�[���p�b�h�܂��̓L�[�{�[�h��A�{�^�����������ƂŃv���C���[���쐬
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        //�v���C���[�ԍ���0�ł͂Ȃ��P����n�߂�悤�ɂ��Ă���
        playerNum = playerInput.playerIndex + 1;

        //OnClick�{�^���̍��ڂɃL�����N�^�[�Z���N�g�֐���ǉ��i�t�@�C������Prefab����ł�OK�j
        button[0].onClick.AddListener(() => playerInput.gameObject.GetComponent<CharacterCreation>().Select(0));
        button[1].onClick.AddListener(() => playerInput.gameObject.GetComponent<CharacterCreation>().Select(1));
        button[2].onClick.AddListener(() => playerInput.gameObject.GetComponent<CharacterCreation>().Select(2));
        button[3].onClick.AddListener(() => playerInput.gameObject.GetComponent<CharacterCreation>().Select(3));

        //�ڑ����ꂽ���ǂ����̊m�F���O
        Debug.Log("�v���C���[" + playerNum + "���������܂���");
    }

    // �v���C���[�ގ����Ɏ󂯎��ʒm(�L��������Ă���ꍇ�̂ݔ���)
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        print($"�v���C���[#{playerInput.user.index + 1}���ގ��I");
    }
}