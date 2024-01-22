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
    public bool isNextScene;
    [Tooltip("����OK���ǂ���")]
    public bool isReady;
    [Tooltip("�I�����ꂽ�L�����N�^�[�ԍ����i�[����")]
    public int[] selectCharacterNumber;

    public int conectCount;     //�ڑ����Ă���L������
    public int submitCharCount; //�I�����m�肳�ꂽ�L���������L�^

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
    }

    void Update()
    {
        CheckReady();   //Ready���ON��OFF�̏����֐�

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

    //Ready���ON��OFF�̏����֐�
    private void CheckReady()
    {
        //hierarchy�ɂ���CharSelectManager�����I�u�W�F�N�g�̔z����쐬
        CharSelectManager[] objectsWithScripts = FindObjectsOfType<CharSelectManager>();

        conectCount = objectsWithScripts.Length; //�I�𐔏�����
        submitCharCount = 0;

        foreach (CharSelectManager obj in objectsWithScripts)
        {
            if (obj.isCharSelected) { submitCharCount++; } //�m�肳��Ă���΃J�E���g���Z
            //Debug.Log(submitCharCount);
        }

        //�L�����N�^�[�Z���N�g��ʂ���1�l�ł��L�����N�^�[��I�����Ă��Ȃ�,�܂��͐ڑ�����1�ȉ��̏ꍇ
        if (isCharSelect && submitCharCount < conectCount || conectCount <= 1)
        {
            readyUI.SetActive(false);   //Ready��UI���\���ɂ���
            isReady = false;            //Ready��Ԃ���������
        }
        //�L�����N�^�[�Z���N�g��ʂ��S�����L�����N�^�[��I�����Ă���A�ڑ�����1�ȏ�̏ꍇ
        else if (isCharSelect && submitCharCount >= conectCount && conectCount > 1)
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