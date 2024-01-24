using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;


public class MenuTest : MonoBehaviour
{
    private string[] items; //�^�C�g����ʂ̃��j���[���ږ���ێ�����z��

    // CS�n
    [Tooltip("GameStartSystem.cs�������Ă���I�u�W�F�N�g�����Ă�������")]
    [SerializeField] private GameStartSystem gameStartSys;

    // UI�n
    [Tooltip("UI�Ƃ������O�̃I�u�W�F�N�g���A�^�b�`���Ă�������")]
    [SerializeField] private RectTransform ui;
    private float uiScale;
    [Tooltip("menu�I�u�W�F�N�g������")]
    [SerializeField] private GameObject menu;
    [Tooltip("���j���[��p�̃J�[�\�������Ă�������")]
    [SerializeField] private GameObject cursor;
    [Tooltip("�J�[�\���̎q�v�f�����Ă�������")]
    [SerializeField] private GameObject inner; //�J�[�\���̓����̐F
    [Tooltip("���j���[�J�[�\����RectTransform�擾�p")]
    [SerializeField] private RectTransform cursorRT;
    [Tooltip("�^�C�g���ŕ\�����鍀��UI(4��)�����Ă�������")]
    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject logo;
    [Tooltip("�e�L�����N�^�[UI�����Ă�������")]
    [SerializeField] private GameObject characterUI;
    [Tooltip("�L�����N�^�[�̔w�iUI�����Ă�������")]
    [SerializeField] private GameObject backGroundUI;
    [Tooltip("Credit��UI�����Ă�������")]
    [SerializeField] private GameObject creditUI;
    [Tooltip("Controls��UI�����Ă�������")]
    [SerializeField] private GameObject controlsUI;

    // �J�����n
    [Tooltip("�J�����I�u�W�F�N�g�����Ă�������")]
    [SerializeField] private Camera cam;
    [Tooltip("�wGAME�x�����������̃J�����̃^�[�Q�b�g�|�W�V���������Ă�������")]
    [SerializeField] private Vector3 tPos;
    private float camPosY;
    private float camPosZ;

    // ���n
    [SerializeField] private AudioSource audioSource;   //���g��AudioSource������
    [SerializeField] private AudioClip decisionSE;      //���艹 decision
    [SerializeField] private AudioClip cancelSE;        //�L�����Z����
    [SerializeField] private AudioClip moveSE;          //�ړ���
    [SerializeField] private AudioClip openMenuSE;      //
    [SerializeField] private AudioClip closeMenuSE;     //

    // �l�����n
    [Tooltip("���ړ��m�̏c�̊Ԋu(���̒l����͂��Ă�������)")]
    [SerializeField] private float itemSpace;
    [Tooltip("�J�[�\���ړ����̎��ԊԊu")]
    [SerializeField] private float interval;
    [Tooltip("�I�����ڂ̃J���[")]
    [SerializeField] private Color selectionItemColor;
    [Tooltip("��I�����ڂ̃J���[")]
    [SerializeField] private Color notSelectionItemColor;

    // Easing�n
    [Tooltip("Credit��Controls���������Ƃ��̊ɋ}�̐i�݋(0~1�̓_P)")]
    [SerializeField] private float easTime;
    [SerializeField] private float[] itemTime;
    [Tooltip("Game��Game�ȊO���������Ƃ��̐؂�ւ�莞��(�b)")]
    [SerializeField] private float[] duration;

    [Header("����m�F�p")]
    [Tooltip("���ݑI������Ă��郁�j���[�ԍ�")]
    [SerializeField] private int currentMenuNum;
    [Tooltip("�ЂƂO�ɑI������Ă������j���[�ԍ�")]
    [SerializeField] private int oldMenuNum;
    [Tooltip("���ݑI������Ă��郁�j���[��")]
    [SerializeField] private string currentMenuName;
    [Tooltip("�ЂƂO�ɑI������Ă������j���[��")]
    [SerializeField] private string oldMenuName;
    [Tooltip("�㉺�ǂ��炩�̓��͂�����Ă��邩")]
    [SerializeField] private bool isPush;
    [Tooltip("���͌p������")]
    [SerializeField] private float inputCount;
    [Tooltip("menu���ڂ�I���������ǂ���")]
    [SerializeField] private bool decision;
    [Tooltip("UI��߂����ǂ���")]
    [SerializeField] private bool backUI;
    [Tooltip("UI�𓮂������ǂ���")]
    [SerializeField] private bool moveUI;
    [Tooltip("�wGAME�������ꂽ���ǂ����x")]
    [SerializeField] private bool game;

    private void Start()
    {
        //���O�̒������K�v�Ȓl���������������ꍇ
        if (interval == 0) { interval = 12; }                                                                                           // �������ł̑I�����ڂ��ړ����鎞�ԊԊu�̐ݒ�
        if (itemSpace == 0) { itemSpace = 120; }                                                                                        // ���j���[�̍��ړ��m��Y���W�Ԋu
        if (selectionItemColor.a == 0) { selectionItemColor.a = 1; }                                                                    // �����x���}�b�N�X�ɐݒ�
        if (notSelectionItemColor.a == 0) { notSelectionItemColor.a = 1; }                                                              // �����x���}�b�N�X�ɐݒ�
        if (selectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#008fd9", out selectionItemColor); }        // ���F�ɐݒ�
        if (notSelectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#ffffff", out notSelectionItemColor); }  // ���F�ɐݒ�

        this.gameObject.SetActive(true);    //�^�C�g�����j���[�ꗗ��\������
        logo.SetActive(true);               //���S��\������

        Array.Resize(ref items, menuItems.Length);     //menuItems�̐��ɂ����items�̑傫����ύX����
        Array.Resize(ref itemTime, menuItems.Length);  //menuItems�̐��ɂ����itemTime�̑傫����ύX����

        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);    //���j���[���ړ��m��Y���W�Ԋu���w�肵���Ԋu�ɐݒ�
            items[i] = menuItems[i].name;   //�z��̒��Ƀ��j���[���ڂ̖��O��������
            itemTime[i] = 0;
        }

        //for(int i = 0; i < characterUI.Length; i++)
        //{
        //    characterUI[i].SetActive(false);    //�e�L�����N�^�[UI���\���ɂ���
        //}
        characterUI.SetActive(false);    //�e�L�����N�^�[UI���\���ɂ���
        backGroundUI.SetActive(false);

        currentMenuName = items[0];  //�I������Ă��郁�j������1�ԏ�ɏ�����
        oldMenuName = currentMenuName;
        currentMenuNum = 0;          //�I������Ă��郁�j���[�ԍ���1�ԏ�ɏ�����
        isPush = false;              //�㉺�ǂ��炩������Ă��邩�̊m�F�p�t���O�̏�����
        inputCount = 0;              //�{�^�����͌p�����Ԃ̏�����

        decision = false;
        backUI = true;
        moveUI = false;
        game = false;

        camPosY = 0.0f;
        camPosZ = 0.0f;
        uiScale = 0.0f;

        easTime = 0.0f;
    }

    private void Update()
    {
        // �܂����ڑI�𒆂̎�
        if (decision == false && ui.anchoredPosition.x == 0.0f)
        {
            CursorMove();   //���j���[�J�[�\���ړ�����

            Decision();     //�e���j���[�{�^���������ꂽ�Ƃ��̏���

            moveUI = false;
        }
        // �wCREDIT�x�ƁwCONTROLS�x��I��������
        else if (decision == true && (currentMenuNum > 0 && currentMenuNum < menuItems.Length - 1) && moveUI)
        {

            //Debug.Log(" ���肳�ꂽ ");

            easTime++;

            // (easTime / 60.0f)���w�肵�����o�ɂ����鏊�v���Ԃ𒴂�����
            if(easTime / 60.0f > duration[1])
            {
                easTime = duration[1] * 60.0f; // easTime��(duration * 60.0f)�ɌŒ�

                // B�{�^�����������Ƃ�
                if (Gamepad.all.Count != 0 && Gamepad.current.bButton.wasPressedThisFrame)
                {
                    backUI = true; // UI���߂鉉�oON
                    easTime = 0;   // easTime������
                }
            }

            // �߂鉉�o�t���O�������Ă��Ȃ���
            if(backUI == false)
            {
                switch (currentMenuNum)
                {
                    case 1: // �wCREDIT�x�ւ̉�ʂɈړ����鉉�o
                        ui.anchoredPosition = new Vector2(easing(duration[1], easTime, (1.0f / 2.0f)) * 1920.0f, 0);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, easing(duration[1], easTime, (1.0f / 2.0f)) * -90.0f, 0.0f);
                        break;

                    case 2: // �wCONTROLS�x�ւ̉�ʂɈړ����鉉�o
                        ui.anchoredPosition = new Vector2(easing(duration[1], easTime, (1.0f / 2.0f)) * -1920.0f, 0);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, easing(duration[1], easTime, (1.0f / 2.0f)) * 90.0f, 0.0f);
                        break;
                }
            }

            // �߂鉉�o�t���O�������Ă��鎞
            else
            {
                switch (currentMenuNum)
                {
                    case 1:
                        ui.anchoredPosition = new Vector2(1920.0f - (easing(duration[1], easTime, (1.0f / 2.0f)) * 1920.0f), 0.0f);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, -90.0f + easing(duration[1], easTime, (1.0f / 2.0f)) * 90.0f, 0.0f);
                        break;
                    case 2:
                        ui.anchoredPosition = new Vector2(-1920.0f + (easing(duration[1], easTime, (1.0f / 2.0f)) * 1920.0f), 0.0f);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, 90.0f - easing(duration[1], easTime, (1.0f / 2.0f)) * 90, 0.0f);
                        break;
                }

                if (Mathf.Abs(ui.anchoredPosition.x) < 1.0f)
                {
                    moveUI = false;
                    decision = false;
                    backUI = false;

                    cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // �J�[�\�������X�ɖ߂�
                    inner.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // �J�[�\�������X�ɖ߂�
                }

                StartCoroutine("BackMenu");

                
            }
        }

        if(game == true)
        {
            /*�y����͂���őΉ� credit�͍���RawImage�ɂȂ�\��z*/
            creditUI.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, true);
            controlsUI.GetComponentInChildren<RawImage>().CrossFadeAlpha(0.0f, 0.0f, true);

            easTime++;
            if (easTime / 60.0f > duration[0])
            {
                easTime = duration[0] * 60.0f;
                cam.transform.position = tPos;
            }

            if (backUI == false)
            {
                // �L�����N�^�[�Z���N�g�̏�Ԃɐi��
                Zoom(duration[0], easTime);

                // UI�������Ă͂����Ȃ��t���O�������Ă��鎞
                if (moveUI == false && !gameStartSys.isReady)
                {
                    // B�{�^������������
                    if (Gamepad.all.Count != 0 && Gamepad.current.bButton.wasPressedThisFrame && gameStartSys.submitCharCount <= 0)
                    {
                        // �L�����N�^�[UI���\���ɂ���
                        //for (int i = 0; i < characterUI.Length; i++)
                        //{
                        //    characterUI[i].SetActive(false); 
                        //}
                        characterUI.SetActive(false);
                        backGroundUI.SetActive(false);

                        gameStartSys.isCharSelect = false; // �L�����N�^�[�Z���N�g�𖳌���

                        logo.SetActive(true);              // Logo��\��
                        menu.SetActive(true);              // ���j���[��\��
                        backUI = true;                     // ���j���[��ʂɖ߂鉉�o ON
                        easTime = 0;                       // easTime������
                    }
                }
                
            }
            else
            {
                ZoomOut(duration[0], easTime); // ���j���[�̏�Ԃɖ߂�
            }
        }
    }

    //�J�[�\���̈ړ��֘A����
    private void CursorMove()
    {
        
        //GamePad�i���X�e�B�b�N����͎� or �\���L�[����͎��j
        if (Gamepad.all.Count != 0 && Gamepad.current.leftStick.y.ReadValue() > 0 || Gamepad.current.dpad.up.isPressed)
        {
            if (isPush == false)    //�����ꂽ���̏���
            {
                isPush = true;
                //currentMenuNum�����������邱�ƂŌ��炳��A0��菬�����l�ɂȂ�����
                if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;   //�J�[�\������ԉ��Ɉړ�������
                audioSource.clip = moveSE;
                audioSource.PlayOneShot(moveSE);
                if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
            }
            else    //���������̏���
            {
                inputCount++;    //�J�E���g������
                //�J�E���g�ƈړ��C���^�[�o�����������]�肪0�̏ꍇ
                if (inputCount % interval == 0)
                {
                    if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;   //�J�[�\������ԉ��Ɉړ�������
                    audioSource.clip = moveSE;
                    audioSource.PlayOneShot(moveSE);
                    if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
                }
            }

            if(currentMenuNum + 1 > 3)
            {
                oldMenuNum = 0;
            }
            else
            {
                oldMenuNum = currentMenuNum + 1;
            }

            if (itemTime[oldMenuNum] > 0.05f * 60.0f) itemTime[oldMenuNum] = 0;
            oldMenuName = items[oldMenuNum];
            currentMenuName = items[currentMenuNum];  //�I������Ă��郁�j���[����menuName�ɑ������i�����X�V�j
            
        }
        //GamePad�i���X�e�B�b�N�����͎� or �\���L�[�����͎��j
        else if (Gamepad.all.Count != 0 && Gamepad.current.leftStick.y.ReadValue() < 0 || Gamepad.current.dpad.down.isPressed)
        {
            if (isPush == false)    //�����ꂽ���̏���
            {
                isPush = true;
                //currentMenuNum������������邱�Ƃő����A���ڐ����傫���l�ɂȂ�����
                if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;    //�J�[�\������ԏ�Ɉړ�������
                audioSource.clip = moveSE;
                audioSource.PlayOneShot(moveSE);
                if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
            }
            else    //���������̏���
            {
                inputCount++;
                //�J�E���g�ƈړ��C���^�[�o�����������]�肪0�̏ꍇ
                if (inputCount % interval == 0)
                {
                    if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;    //�J�[�\������ԏ�Ɉړ�������
                    audioSource.clip = moveSE;
                    audioSource.PlayOneShot(moveSE);
                    //if (itemTime[oldMenuNum] > 0.05f * 60.0f) itemTime[oldMenuNum] = 0;
                    if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
                }
            }
            if (currentMenuNum - 1 < 0)
            {
                oldMenuNum = 3;
            }
            else
            {
                oldMenuNum = currentMenuNum - 1;
            }
            //if (itemTime[oldMenuNum] > 0.05f * 60.0f) itemTime[oldMenuNum] = 0;
            //if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;

            if (itemTime[oldMenuNum] > 0.05f * 60.0f) itemTime[oldMenuNum] = 0;
            oldMenuName = items[oldMenuNum];
            currentMenuName = items[currentMenuNum];  //�I������Ă��郁�j���[����menuName�ɑ������i�����X�V�j
        }
        else      //�������ĂȂ���
        {
            isPush = false;
            inputCount = 0;
        }


        float scale = 0;

        //�F�ύX
        for (int i = 0; i < menuItems.Length; i++)
        {
            //���ݑI������Ă��郁�j���[���ƃ��j���[���ړ��ɂ��閼�O����v�����ꍇ
            if (currentMenuName == items[i])
            {
                cursorRT.position = menuItems[i].GetComponent<RectTransform>().position;    //���j���[�J�[�\���̈ʒu��I������Ă��郁�j���[�̈ʒu�ɕύX����
                menuItems[i].GetComponent<Text>().CrossFadeColor(selectionItemColor, 0.05f, true, true); // ���������X�ɔ��F�ɖ߂�

                // ���������񂾂�傫������
                scale = easing3(0.05f, itemTime[i], 0.5f, true, 1.0f, 1.25f, false);
                menuItems[i].GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
                itemTime[i]++;
            }
            else
            {
                menuItems[i].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.05f, true, true); // ���������X�ɔ��F�ɖ߂�
                scale = easing3(0.05f, itemTime[i], 0.5f, false, menuItems[i].GetComponent<RectTransform>().localScale.x, 1.0f, false);
                menuItems[i].GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
                itemTime[i]++;
            }
        }
    }

    //�e���j���[�{�^���������ꂽ�Ƃ��̏���
    private void Decision()
    {
        
        // ����{�^��(a�{�^��)���������Ƃ�
        if (Gamepad.all.Count != 0 && Gamepad.current.aButton.wasPressedThisFrame/* && inner.GetComponent<RawImage>().canvasRenderer.GetAlpha() == 1*/)
        {
            CursorFade();
            easTime = 0;
            decision = true; // ����t���OON
            backUI = false;

            audioSource.clip = decisionSE;
            audioSource.PlayOneShot(decisionSE);

            switch (currentMenuNum)
            {
                case 0:
                    StartCoroutine("PushGame");
                    break;

                case 1:
                    StartCoroutine("PushCredit");
                    break;

                case 2:
                    StartCoroutine("PushControls");
                    break;

                case 3:
                    #if UNITY_EDITOR

                        UnityEditor.EditorApplication.isPlaying = false;

                    #else

                        Application.Quit();

                    #endif
                    break;

                default:
                    break;
            }
        }
    }

    void CursorFade()
    {
        inner.GetComponent<RawImage>().CrossFadeAlpha(0, 0.2f, true);                                           // �J�[�\�������X�ɏ���
        cursor.GetComponent<RawImage>().CrossFadeAlpha(0, 0.2f, true);                                          // �J�[�\�������X�ɏ���
        menuItems[currentMenuNum].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.2f, true, true); // ���������X�ɔ��F�ɖ߂�
    }

    private IEnumerator PushGame()
    {
        //CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���

        game = true;

        moveUI = true;
    }
    private IEnumerator PushCredit()
    {
        //CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���

        moveUI = true;
    }
    private IEnumerator PushControls()
    {
        //CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���

        moveUI = true;
    }
    private IEnumerator BackMenu()
    {
        yield return new WaitForSecondsRealtime(duration[1]);
        creditUI.GetComponentInChildren<Text>().CrossFadeAlpha(1.0f, 0.0f, true);
        controlsUI.GetComponentInChildren<RawImage>().CrossFadeAlpha(1.0f, 0.0f, true);
        //decision = false;
    }

    /// <summary>
    /// Sin�g���g����Easing�֐� ����(���v����,���ݎ���,Sin�J�[�u�̒���,�Ԃ�l�̕��� true:+ false:-,�n�܂�̒l,�ŏI�I�ɗ~�����l)
    /// </summary>
    /// <param name="duration">���v����</param>
    /// <param name="time">���ݎ���</param>
    /// <param name="length">�T�C���J�[�u�̒���</param>
    /// <param name="symbol">�T�C���J�[�u�̎n�܂�̕��� true:+ false:-</param>
    /// <param name="source">�n�܂�̒l</param>
    /// <param name="max">�ŏI�I�ɗ~�����l</param>
    /// <returns></returns>
    float easing3(float duration, float time, float length, bool symbol, float source, float max, bool turn)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easing�̐i�s�󋵂������l���Z�o
        //TPRate = t;                               // �i�s��(%)
        //present_length = t * length;              // ���ݒn�_(Sin�J�[�u���猩��)

        // Sin�J�[�u�̐i�ޕ������w�� true:+ false:- (�ŏ��Ɍ��̒l����l�� ���₵���� : +,���炵���� : -)
        float symbol_num = symbol ? 1.0f : -1.0f;

        // �^�[�������ǂ����ŕς��@�^�[������Sin�J�[�u��Ō����Ɓ@-1 ���� 1 �܂ł܂苗���� 2 
        if (turn)
        {
            // (time / frame) �� duration ���߂��Ă���Ȃ�
            if ((time / frame) >= duration)
            {
                t = 1; // t�͐i�s���Ȃ̂� 1=100%��100%�ɌŒ肵�A�֐��̕Ԃ�l��max�Ŏw�肵���l����ς��Ȃ��悤�ɂ��Ă���
                       //Debug.Log("time is over");
                return source + (((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) - 1.0f) * (symbol_num * -1)) * Mathf.Abs(max - source);
            }
            //Debug.Log("�^�[��");
            return source + (((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) - 1.0f) / 2.0f * (symbol_num * -1)) * Mathf.Abs(max - source);
        }
        // ����ȊO�� 0 ���� 1 �ŋ����� 1
        else
        {
            // (time / frame) �� duration ���߂��Ă���Ȃ�
            if ((time / frame) >= duration)
            {
                t = 1; // t�͐i�s���Ȃ̂� 1=100%��100%�ɌŒ肵�A�֐��̕Ԃ�l��max�Ŏw�肵���l����ς��Ȃ��悤�ɂ��Ă���
                       //Debug.Log("time is over");
                return source - ((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) * (symbol_num * -1)) * Mathf.Abs(max - source);
            }
            //Debug.Log("�ʏ�");
            return source - ((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) * (symbol_num * -1)) * Mathf.Abs(max - source);
        }
    }

    void Zoom(float duration, float time)
    {
        /*�y�J������UI�̈����Ɗ��̎��ɗ~�����l�z*/
        float[] targetPosY = new float[2] { 65.0f, 5.0f };
        float[] targetPosZ = new float[2] { -260.0f, -20.0f };

        float[] ui_zoom_scale = new float[2] { 0.7f, 40.0f };
        /*----------------------------------------*/

        float length = 1.5f; // ����Sin(�g�̒���)

        /*�yUI�ƃJ�����̃C�[�W���O�z*/
        if (time < TurningTime(duration, length, 0.5f))
        {
            uiScale = easing3(duration, time, length, false, 1.0f, ui_zoom_scale[0], false);

            camPosY = easing3(duration, time, length, false, 50.0f, targetPosY[0], false);
            camPosZ = easing3(duration, time, length, false, -200.0f, targetPosZ[0], false);
        }
        else
        {
            float ui_old_pos = easing3(duration, TurningTime(duration, length, 0.5f), length, false, 1.0f, ui_zoom_scale[0], false);

            float old_cam_scaleY = easing3(duration, TurningTime(duration, length, 0.5f), length, false, 50.0f, targetPosY[0], false);
            float old_cam_scaleZ = easing3(duration, TurningTime(duration, length, 0.5f), length, true, -200.0f, targetPosZ[0], false);

            uiScale = easing3(duration, time, length, true, ui_old_pos, ui_zoom_scale[1], true);

            camPosY = easing3(duration, time, length, false, old_cam_scaleY, targetPosY[1], true);
            camPosZ = easing3(duration, time, length, true, old_cam_scaleZ, targetPosZ[1], true);

        }
        /*--------------------------*/

        cam.transform.position = new Vector3(0, camPosY, camPosZ);

        ui.transform.localScale = new Vector3(uiScale, uiScale, uiScale);

        /*�yUI���ʂ�߂��鉉�o�z*/
        if (ui.transform.localScale.x > 26.0f)  //10
        {
            // ���j���[UI���\���ɂ���
            for (int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i].GetComponent<Text>().CrossFadeAlpha(0, 0f, true);
            }
        }
        /*----------------------*/

        // �J�����̖ڕW�n�_���X�V
        tPos = new Vector3(0, targetPosY[1], targetPosZ[1]);

        /*�y�L�����N�^�[�Z���N�g��ʂ�UI�̗L�����z*/
        if (cam.transform.position == tPos)
        {
            moveUI = false;
            //for (int i = 0; i < characterUI.Length; i++)
            //{
            //    characterUI[i].SetActive(true); // �L�����N�^�[�Z���N�g��\������
            //}
            characterUI.SetActive(true); // �L�����N�^�[�Z���N�g��\������
            backGroundUI.SetActive(true);
            gameStartSys.isCharSelect = true;

            logo.SetActive(false);
            menu.SetActive(false);
        }
        /*-------------------------------*/
    }
    void ZoomOut(float duration, float time)
    {
        /*�y�J������UI�̈����Ɗ��̎��ɗ~�����l�z*/
        float[] targetPosY = new float[2] { -15.0f, 50.0f };
        float[] targetPosZ = new float[2] { 40.0f, -200.0f };

        float[] ui_zoom_scale = new float[2] { 68.0f, 1.0f };
        /*----------------------------------------*/

        float length = 0.5f; // ����Sin(�g�̒���)

        uiScale = easing3(duration, time, length, false, 40.0f, ui_zoom_scale[1], false);

        camPosY = easing3(duration, time, length, true, 5.0f, targetPosY[1], false);
        camPosZ = easing3(duration, time, length, false, -20.0f, targetPosZ[1], false);

        cam.transform.position = new Vector3(0, camPosY, camPosZ);

        ui.transform.localScale = new Vector3(uiScale, uiScale, uiScale);

        /*�yUI���o�Ă��鉉�o�z*/
        if (ui.transform.localScale.x < 26.0f)  //10
        {
            // ���j���[UI���\�������
            for (int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i].GetComponent<Text>().CrossFadeAlpha(1.0f, 0f, true);
            }
        }
        /*----------------------*/

        tPos = new Vector3(0, targetPosY[1], targetPosZ[1]);

        /*�y�L�����N�^�[�Z���N�g��ʂ�UI�̗L�����z*/
        if (cam.transform.position == tPos)
        {
            game = false;
            backUI = false;
            decision = false;
            cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // �J�[�\�������X�ɖ߂�
            inner.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // �J�[�\�������X�ɖ߂�
            StartCoroutine("BackMenu");
        }
        /*-----------------------------------------*/
    }

    /// <summary>
    /// Sin�g��Easing�֐��̓���̃^�C�~���O�̎��Ԃ��擾����֐� ����(���v����,Sin�J�[�u�̒���,�擾��������(Sin�J�[�u��̒n�_))
    /// </summary>
    /// <param name="duration">���v����</param>
    /// <param name="length">�T�C���J�[�u�̒���</param>
    /// <param name="turning_point">�擾��������(Sin�J�[�u��̒n�_)</param>
    /// <returns></returns>
    float TurningTime(float duration, float length, float turning_point)
    {
        float fps = 60.0f;
        float t = duration * fps;
        float divisor = length / turning_point;

        return t / divisor;
    }

    float easing(float duration, float time, float length)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easing�̐i�s�󋵂������l���Z�o
        //TPRate = t * length;

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero);
    }
}