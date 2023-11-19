//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

//public class GameStartSystem : MonoBehaviour
//{
//    [Tooltip("���݂̉�ʂ��L�����N�^�[")]
//    public bool isCharSelect;
//    [Tooltip("�Q�[���V�[���ړ��t���O")]
//    [SerializeField] private bool isNextScene;
//    [Tooltip("�V�[���ړ����̑ҋ@����(���̊Ԃ�SE����)")]
//    [SerializeField] private float waitTime; //�V�[���ړ����Ɏg�p���鏈���ҋ@����(SE����I���܂�)

//    [Tooltip("����OK���ǂ���")]
//    [SerializeField] private bool isReady;
//    [Tooltip("�L�����N�^�[���I������Ă��邩�ǂ���")]
//    [SerializeField] private bool[] isSelected;

//    [Tooltip("ReceiveNotificationExample.cs���������I�u�W�F�N�g������")]
//    [SerializeField] private ReceiveNotificationExample receiveNotificationExample;
//    [Tooltip("ReadyUI�I�u�W�F�N�g������")]
//    [SerializeField] private GameObject readyUI;

//    private Gamepad[] gamePads; //�i�ǂ��ł��K�v�ȉ\���L��j

//    void Start()
//    {
//        waitTime = 2.0f;    //�R���[�`���̑ҋ@���Ԃ�2.0�b�ɐݒ肷��       

//        //flg�̏�����
//        isNextScene = false;
//        isReady = false;
//        isCharSelect = false;

//        //gamePads�̑傫����ڑ�����Ă���Q�[���p�b�h�̐��ɍ��킹��
//        Array.Resize(ref gamePads, Gamepad.all.Count);
//        for(int i = 0; i < gamePads.Length; i++)
//        {

//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Array.Resize(ref isSelected, receiveNotificationExample.playerNum);

//        CheckReady();

//        if (isNextScene == false)
//        {
//            //Ready�̏�Ԃ��Q�[���p�b�h��StartButton�������ꂽ��
//            if (isReady == true && Gamepad.current.startButton.isPressed)
//            {
//                isNextScene = true;
//                StartCoroutine("GotoGameScene");
//            }
//        }
//    }

//    private void CheckReady()
//    {
//        //for (int i = 0; i < receiveNotificationExample.playerNum; i++)
//        //{
//        //    //
//        //    if (receiveNotificationExample.count == 0)
//        //    {
//        //        //�e�L�����N�^�[���ƂɃL�����N�^�[��I���������ǂ����̒l���擾����
//        //        //isSelected[i] = GameObject.Find($"Player{i}").GetComponent<CharSelectManager>().isCharSelected;
//        //        isSelected[i] = GameObject.FindWithTag("SelectPlayer").GetComponent<CharSelectManager>().isCharSelected;
//        //    }

//        //}

//        //for (int i = 0; i < receiveNotificationExample.playerNum; i++)
//        //{
//        //    // �����ЂƂł��L�����N�^�[���I������Ă��Ȃ�������for���𔲂���
//        //    if (isSelected[i] == false)
//        //    {

//        //        readyUI.SetActive(false);   //Ready��UI���\���ɂ���
//        //        isReady = false;            //Ready��Ԃ���������
//        //        break;                      //for���𔲂���
//        //    }

//        //    //�����S�Ẵv���C���[���L�����N�^�[��I�����Ă�����
//        //    if (isSelected[receiveNotificationExample.playerNum - 1])
//        //    {
//        //        readyUI.SetActive(true);    //Ready��UI��\������
//        //        isReady = true;             //Ready��Ԃɂ���
//        //    }
//        //}

//        //�����v���C���[�̐��ɉ����Ĕz��̑傫����ύX����K�v������
//        for (int i = 0; i < receiveNotificationExample.playerNum; i++)
//        {
//            //
//            if (receiveNotificationExample.count == 0)
//            {
//                //�e�L�����N�^�[���ƂɃL�����N�^�[��I���������ǂ����̒l���擾����
//                //isSelected[i] = GameObject.Find($"Player{i}").GetComponent<CharSelectManager>().isCharSelected;
//                isSelected[i] = GameObject.FindWithTag("SelectPlayer").GetComponent<CharSelectManager>().isCharSelected;
//            }

//        }

//        for (int i = 0; i < receiveNotificationExample.playerNum; i++)
//        {
//            // �����ЂƂł��L�����N�^�[���I������Ă��Ȃ�������for���𔲂���
//            if (isSelected[i] == false)
//            {

//                readyUI.SetActive(false);   //Ready��UI���\���ɂ���
//                isReady = false;            //Ready��Ԃ���������
//                break;                      //for���𔲂���
//            }

//            //�����S�Ẵv���C���[���L�����N�^�[��I�����Ă�����
//            if (isSelected[receiveNotificationExample.playerNum - 1])
//            {
//                readyUI.SetActive(true);    //Ready��UI��\������
//                isReady = true;             //Ready��Ԃɂ���
//            }

//        }
//    }

//    private IEnumerator GotoGameScene()
//    {
//        yield return new WaitForSecondsRealtime(waitTime); //������ҋ@ �V�[�����̉���炷����
//        SceneManager.LoadScene("GameScene");
//    }
//}


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
    [Tooltip("�L�����N�^�[���I������Ă��邩�ǂ���")]
    [SerializeField] private bool[] isSelected;

    private bool flgCheck;  //1�l�ł��L�����N�^�[��I�����ĂȂ����̃t���O

    void Start()
    {
        waitTime = 2.0f;    //�R���[�`���̑ҋ@���Ԃ�2.0�b�ɐݒ肷��       

        //flg�̏�����
        isNextScene = false;
        isReady = false;
        isCharSelect = false;


        flgCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        Array.Resize(ref isSelected, Gamepad.all.Count);    //�ڑ�����Ă���Q�[���p�b�h�̐��ɔz��̑傫���𐏎��ύX����

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