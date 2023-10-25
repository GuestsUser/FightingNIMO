using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;


public class MenuTest : MonoBehaviour
{
    private string[] items; //�^�C�g����ʂ����j���[���ږ���ێ�����z��

    // CS�n
    [Tooltip("GameStartSystem.cs�������Ă���I�u�W�F�N�g�����Ă�������")]
    [SerializeField] private GameStartSystem gameStartSys;
    //[Tooltip("PlayerInputManager.cs�����I�u�W�F�N�g�����Ă�������")]
    //[SerializeField] private PlayerInputManager playerInputManager;

    //UI�n
    [Tooltip("���j���[��p�̃J�[�\�������Ă�������")]
    [SerializeField] private GameObject cursor;
    [Tooltip("���j���[�J�[�\����RectTransform�擾�p")]
    [SerializeField] private RectTransform cursorRT;
    [Tooltip("�^�C�g���ŕ\�����鍀��UI(4��)�����Ă�������")]
    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject logo;
    [Tooltip("�e�L�����N�^�[UI�����Ă�������")]
    [SerializeField] private GameObject[] characterUI;

    // ���n
    [SerializeField] private AudioSource audioSource;   //���g��AudioSource������
    [SerializeField] private AudioClip desisionSE;      //���艹
    [SerializeField] private AudioClip cancelSE;        //�L�����Z����
    [SerializeField] private AudioClip moveSE;          //�ړ���
    [SerializeField] private AudioClip openMenuSE;      //
    [SerializeField] private AudioClip closeMenuSE;     //

    //�l�����n
    [Tooltip("���ړ��m�̏c�̊Ԋu(���̒l����͂��Ă�������)")]
    [SerializeField] private float itemSpace;
    [Tooltip("�J�[�\���ړ����̎��ԊԊu")]
    [SerializeField] private float interval;
    [Tooltip("�I�����ڂ̃J���[")]
    [SerializeField] private Color selectionItemColor;
    [Tooltip("��I�����ڂ̃J���[")]
    [SerializeField] private Color notSelectionItemColor;

    [Header("����m�F�p")]
    [Tooltip("���ݑI������Ă��郁�j���[�ԍ�")]
    [SerializeField] private int currentMenuNum;
    [Tooltip("���ݑI������Ă��郁�j���[��")]
    [SerializeField] private string currentMenuName;
    [Tooltip("�㉺�ǂ��炩�̓��͂�����Ă��邩")]
    [SerializeField] private bool isPush;
    [Tooltip("���͌p������")]
    [SerializeField] private float inputCount;

    private void Awake()
    {
        //���O�̒������K�v�Ȓl���������������ꍇ
        if (interval == 0) { interval = 12; }                                                                                           // �������ł̑I�����ڂ��ړ����鎞�ԊԊu�̐ݒ�
        if (itemSpace == 0) { itemSpace = 120; }                                                                                        // ���j���[�̍��ړ��m��Y���W�Ԋu
        if (selectionItemColor.a == 0) { selectionItemColor.a = 1; }                                                                    // �����x���}�b�N�X�ɐݒ�
        if (notSelectionItemColor.a == 0) { notSelectionItemColor.a = 1; }                                                              // �����x���}�b�N�X�ɐݒ�
        if (selectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#008fd9", out selectionItemColor); }        // ���F�ɐݒ�
        if (notSelectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#ffffff", out notSelectionItemColor); }  // ���F�ɐݒ�
    }

    private void Start()
    {
        this.gameObject.SetActive(true);    //�^�C�g�����j���[�ꗗ��\������
        logo.SetActive(true);               //���S��\������

        Array.Resize(ref items, menuItems.Length);  //menuItems�̐��ɂ����items�̑傫����ύX����

        for(int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);    //���j���[���ړ��m��Y���W�Ԋu���w�肵���Ԋu�ɐݒ�
            items[i] = menuItems[i].name;   //�z��̒��Ƀ��j���[���ڂ̖��O��������
        }

        for(int i = 0; i < characterUI.Length; i++)
        {
            characterUI[i].SetActive(false);    //�e�L�����N�^�[UI���\���ɂ���
        }

        currentMenuName = items[0];  //�I������Ă��郁�j������1�ԏ�ɏ�����
        currentMenuNum = 0;          //�I������Ă��郁�j���[�ԍ���1�ԏ�ɏ�����
        isPush = false;              //�㉺�ǂ��炩������Ă��邩�̊m�F�p�t���O�̏�����
        inputCount = 0;              //�{�^�����͌p�����Ԃ̏�����
    }

    private void Update()
    {
        //Gamepad��1����ڑ�����Ă��Ȃ��ƃG���[���N�������߁A1��ł��ڑ�����Ă���Ƃ���������������
        if (Gamepad.all.Count != 0)
        {
            CursorMove();   //���j���[�J�[�\���ړ�����
        }
        Decision();     //�e���j���[�{�^���������ꂽ�Ƃ��̏���
    }

    //�J�[�\���̈ړ��֘A����
    private void CursorMove()
    {
        //GamePad�i���X�e�B�b�N����͎� or �\���L�[����͎��j
        if (Gamepad.current.leftStick.y.ReadValue() > 0 || Gamepad.current.dpad.up.isPressed)
        {
            if (isPush == false)    //�����ꂽ���̏���
            {
                isPush = true;
                //currentMenuNum�����������邱�ƂŌ��炳��A0��菬�����l�ɂȂ�����
                if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;   //�J�[�\������ԉ��Ɉړ�������
                //audioSource.clip = moveSE;
                //audioSource.PlayOneShot(moveSE);
            }
            else    //���������̏���
            {
                inputCount++;    //�J�E���g������
                //�J�E���g�ƈړ��C���^�[�o�����������]�肪0�̏ꍇ
                if (inputCount % interval == 0)
                {
                    if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;   //�J�[�\������ԉ��Ɉړ�������
                    //audioSource.clip = moveSE;
                    //audioSource.PlayOneShot(moveSE);
                }
            }
        }
        //GamePad�i���X�e�B�b�N�����͎� or �\���L�[�����͎��j
        else if (Gamepad.current.leftStick.y.ReadValue() < 0 || Gamepad.current.dpad.down.isPressed)
        {
            if (isPush == false)    //�����ꂽ���̏���
            {
                isPush = true;
                //currentMenuNum������������邱�Ƃő����A���ڐ����傫���l�ɂȂ�����
                if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;    //�J�[�\������ԏ�Ɉړ�������
                //audioSource.clip = moveSE;
                //audioSource.PlayOneShot(moveSE);
            }
            else    //���������̏���
            {
                inputCount++;
                //�J�E���g�ƈړ��C���^�[�o�����������]�肪0�̏ꍇ
                if (inputCount % interval == 0)
                {
                    if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;    //�J�[�\������ԏ�Ɉړ�������
                    //audioSource.clip = moveSE;
                    //audioSource.PlayOneShot(moveSE);
                }
            }
        }
        else      //�������ĂȂ���
        {
            isPush = false;
            inputCount = 0;
        }

        currentMenuName = items[currentMenuNum];  //�I������Ă��郁�j���[����menuName�ɑ������i�����X�V�j

        //�F�ύX
        for (int i = 0; i < menuItems.Length; i++)
        {
            //���ݑI������Ă��郁�j���[���ƃ��j���[���ړ��ɂ��閼�O����v�����ꍇ
            if (currentMenuName == items[i])
            {
                cursorRT.position = menuItems[i].GetComponent<RectTransform>().position;    //���j���[�J�[�\���̈ʒu��I������Ă��郁�j���[�̈ʒu�ɕύX����
                menuItems[i].GetComponent<Text>().color = selectionItemColor;               //���j���[���̐F��I������Ă���Ƃ��̐F�ɕύX����
            }
            //���ݑI������Ă��郁�j���[���ƃ��j���[���ړ��ɂ��閼�O����v���Ȃ������ꍇ
            else if (currentMenuName != items[i])
            {
                menuItems[i].GetComponent<Text>().color = notSelectionItemColor;    //���j���[���̐F��I������Ă��Ȃ����̐F�ɕύX����
            }
        }
    }

    //�e���j���[�{�^���������ꂽ�Ƃ��̏���
    private void Decision()
    {
        //[GAME]���ڂ��I������Ă����Ԃ��v���C���[���������������
        if (currentMenuName == items[0] && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            gameStartSys.isCharSelect = true; //true�ɂ��邱�ƂŃL�����N�^�[�Z���N�g�ɂȂ�
            for (int i = 0; i < characterUI.Length; i++)
            {
                gameStartSys.isCharSelect = true;   //���݂��L�����N�^�[�Z���N�g��ʂł��邱�Ƃ�����
                characterUI[i].SetActive(true);     //�e�L�����N�^�[UI(�{�^��)��\������
                logo.SetActive(false);              //�^�C�g�����S���\���ɂ���
                this.gameObject.SetActive(false);   //�^�C�g�����j���[���\���ɂ���
            }
        }

        ////[CREDIT]���ڂ��I������Ă����Ԃ��v���C���[���������������
        //if (menuName == items[1] && gamePad.buttonSouth.wasPressedThisFrame)
        //{

        //}

        ////[CONTROLS]���ڂ��I������Ă����Ԃ��v���C���[���������������
        //if (menuName == items[2] && gamePad.buttonSouth.wasPressedThisFrame)
        //{

        //}

        //[EXIT]���I������Ă����Ԃ��v���C���[���������������
        if (currentMenuName == items[3] && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            //�Q�[�����I��������
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}