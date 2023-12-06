using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStartSystem : MonoBehaviour
{
    [Tooltip("ReceiveNotificationExample.cs���������I�u�W�F�N�g������")]
    [SerializeField] private ReceiveNotificationExample receiveNotificationExample;
    [Tooltip("ReadyUI�I�u�W�F�N�g������")]
    [SerializeField] private GameObject readyUI;

    [Tooltip("�V�[���ړ����̑ҋ@����(���̊Ԃ�SE����)")]
    [SerializeField] private float waitTime; //�V�[���ړ����Ɏg�p���鏈���ҋ@����(SE����I���܂�)

    [Tooltip("���݂̉�ʂ��L�����N�^�[")]
    public bool isCharSelect;
    [Tooltip("�Q�[���V�[���ړ��t���O")]
    [SerializeField] private bool isNextScene;
    [Tooltip("����OK���ǂ���")]
    [SerializeField] public bool isReady;
    [Tooltip("�I�����ꂽ�L�����N�^�[�ԍ����i�[����")]
    public int[] selectCharacterNumber;

    private bool flgCheck;  //1�l�ł��L�����N�^�[��I�����ĂȂ����̃t���O

    void Start()
    {
        waitTime = 2.0f;    //�R���[�`���̑ҋ@���Ԃ�2.0�b�ɐݒ肷��       

        Array.Resize(ref selectCharacterNumber, 4);
        for (int i = 0; i < selectCharacterNumber.Length; i++)
        {
            selectCharacterNumber[i] = -1;
        }

        //flg�̏�����
        isNextScene = false;
        isReady = false;
        isCharSelect = false;
        flgCheck = false;
    }

    void Update()
    {
        CheckReady();

        //GameScene�֍s�����߂̏���
        if (isNextScene == false)
        {
            //Ready�̏�Ԃ��Q�[���p�b�h��StartButton�������ꂽ��
            if (isReady == true && Gamepad.current.startButton.isPressed)
            {
                isNextScene = true;
                StartCoroutine("GotoGameScene");
            }
        }
    }

    private void CheckReady()
    {
        //���o�[�W����
        //for (int i = 0; i < receiveNotificationExample.playerNum; i++)
        //{
        //    //
        //    if (receiveNotificationExample.count == 0)
        //    {
        //        //�e�L�����N�^�[���ƂɃL�����N�^�[��I���������ǂ����̒l���擾����
        //        //isSelected[i] = GameObject.Find($"Player{i}").GetComponent<CharSelectManager>().isCharSelected;
        //        isSelected[i] = GameObject.FindWithTag("SelectPlayer").GetComponent<CharSelectManager>().isCharSelected;
        //    }

        //}

        //for (int i = 0; i < receiveNotificationExample.playerNum; i++)
        //{
        //    // �����ЂƂł��L�����N�^�[���I������Ă��Ȃ�������for���𔲂���
        //    if (isSelected[i] == false)
        //    {

        //        readyUI.SetActive(false);   //Ready��UI���\���ɂ���
        //        isReady = false;            //Ready��Ԃ���������
        //        break;                      //for���𔲂���
        //    }

        //    //�����S�Ẵv���C���[���L�����N�^�[��I�����Ă�����
        //    if (isSelected[receiveNotificationExample.playerNum - 1])
        //    {
        //        readyUI.SetActive(true);    //Ready��UI��\������
        //        isReady = true;             //Ready��Ԃɂ���
        //    }
        //}

        //�V�o�[�W����
        //hierarchy�ɂ���CharSelectManager�����I�u�W�F�N�g�̔z����쐬
        CharSelectManager[] objectsWithScripts = FindObjectsOfType<CharSelectManager>();

        foreach (CharSelectManager obj in objectsWithScripts)
        {
            bool value = obj.isCharSelected;    //�L�����N�^�[���I������Ă��邩�ǂ����̒l����

            //1�ł�false������΃t���O�𗧂Ă�
            if (!value)
            {
                flgCheck = true;
                break;
            }
            else
            {
                flgCheck = false;
            }
        }

        //�L�����N�^�[�Z���N�g��ʂ���1�l�ł��L�����N�^�[��I�����Ă��Ȃ��ꍇ
        if (isCharSelect && flgCheck)
        {
            readyUI.SetActive(false);   //Ready��UI���\���ɂ���
            isReady = false;            //Ready��Ԃ���������
        }
        //�L�����N�^�[�Z���N�g��ʂ��S�����L�����N�^�[��I�����Ă���ꍇ
        else if (isCharSelect && !flgCheck)
        {
            readyUI.SetActive(true);    //Ready��UI��\������
            isReady = true;             //Ready��Ԃɂ���
        }
    }

    //�ҋ@���ԏ���
    private IEnumerator GotoGameScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); //������ҋ@ �V�[�����̉���炷����
        SceneManager.LoadScene("GameScene");
    }
}