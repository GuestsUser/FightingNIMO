using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PauseMenu : MonoBehaviour
{
    #region �C���X�y�N�^�[����̑�����K�vor�������������
    [Header("�y���O�ɓ������́z")]
    /*�y���O�̑�����K�{�z*/

    [Tooltip("GameState���A�^�b�`���Ă�������")]
    [SerializeField] private GameState gameState;

    [Tooltip("UI�Ƃ������O�̃I�u�W�F�N�g���A�^�b�`���Ă�������")]
    [SerializeField] private GameObject ui;
    [Tooltip("PauseMenu�Ƃ������O�̃I�u�W�F�N�g���A�^�b�`���Ă�������")]
    [SerializeField] private RectTransform pauseMenu;
    [Tooltip("�J�[�\�������Ă�������")]
    [SerializeField] private GameObject cursor;
    [Tooltip("�J�[�\���̎q�v�f�����Ă�������")]
    [SerializeField] private GameObject inner; //�J�[�\���̓����̐F
    [Tooltip("���j���[�̍��ڂ̐����w�肵�AUI�I�u�W�F�N�g�����Ă�������")]
    [SerializeField] private GameObject[] menuItems;
    
    [Tooltip("Controls��UI�����Ă�������")]
    [SerializeField] private GameObject controlsUI;

    //SE�֘A
    private AudioSource audioSource;
    [SerializeField] private AudioClip decisionSE;      // ���艹
    [SerializeField] private AudioClip cancelSE;        // �L�����Z����
    [SerializeField] private AudioClip moveSE;          // �J�[�\���ړ���
    [SerializeField] private AudioClip openMenuSE;      // ���j���[�\���I��

    /*--------------------*/
    /*�y��������Ƃ���z*/
    [Tooltip("���ړ��m�̏c�̊Ԋu(���̒l����͂��Ă�������)")]
    [SerializeField] private float itemSpace;
    [Tooltip("�J�[�\���ړ����̎��ԊԊu")]
    [SerializeField] private float interval;
    [Tooltip("�I�����ڂ̃J���[")]
    [SerializeField] private Color selectionItemColor;
    [Tooltip("��I�����ڂ̃J���[")]
    [SerializeField] private Color notSelectionItemColor;
    /*------------------*/
    #endregion

    // Easing�n
    [Tooltip("Credit��Controls���������Ƃ��̊ɋ}�̐i�݋(0~1�̓_P)")]
    [SerializeField] private float easTime;
    [SerializeField] private float[] itemTime;
    [Tooltip("Game��Game�ȊO���������Ƃ��̐؂�ւ�莞��(�b)")]
    [SerializeField] private float[] duration;

    #region ����m�F�p�ɕ\���������
    [Header("�y����m�F�p�z")]
    [Tooltip("UI��canvas group Component")]
    [SerializeField] private CanvasGroup uiGroup;
    [Tooltip("�J�[�\���̃����_�[�g�����X�t�H�[����񂪓���")]
    [SerializeField] private RectTransform cursorRT;
    [Tooltip("���ݑI������Ă��郁�j���[�ԍ�")]
    [SerializeField] private int currentMenuNum;
    [SerializeField] private int oldMenuNum;
    [Tooltip("���ݑI������Ă��郁�j���[��")]
    [SerializeField] private string currentMenuName;
    [SerializeField] private string oldMenuName;
    [Tooltip("�㉺�ǂ��炩�̓��͂�����Ă��邩")]
    [SerializeField] private bool ispush;
    [Tooltip("menu���ڂ�I���������ǂ���")]
    [SerializeField] private bool decision; // ����t���OON
    [Tooltip("UI��߂����ǂ���")]
    [SerializeField] private bool backUI;
    [Tooltip("UI�𓮂������ǂ���")]
    [SerializeField] private bool moveUI;
    [Tooltip("QUIT�{�^���������ꂽ��")]
    [SerializeField] private bool pushQuit;
    [Tooltip("���͌p������")]
    [SerializeField] private float count;
    [Tooltip("�|�[�Y�̑��쌠���������Ă���v���C���[")]
    [SerializeField] private int controlNum;    // ����ԍ�
    #endregion

    private string[] items;     // �g�p����Ă��郁�j���[���ږ���ۑ�����string�^�z��
    private Gamepad[] gamePad; // �ڑ�����Ă���Q�[���p�b�h��ۑ�����string�^�z��

    [SerializeField] private bool show;      // true:�\�� false:��\��
    [SerializeField] private float waitTime; // �V�[���ړ����Ɏg�p���鏈���ҋ@����(SE����I���܂�)

    //private float fps;

    void Start()
    {
        /*Title�Ƌ���*/
        /*�y�I�u�W�F�N�g���̎擾�z*/
        uiGroup = ui.GetComponent<CanvasGroup>();
        ui.SetActive(false); // �ŏ��͔�\����
        if (cursor == null) { Debug.LogError("�J�[�\���I�u�W�F�N�g���A�^�b�`����Ă��܂���"); }
        cursorRT = cursor.GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
        /*-------------------*/


        /*�y���O�̒������K�v�Ȓl���������������ꍇ�z*/
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (menuItems[i] == null) { Debug.LogError($"menuObj[{i}]�ɃI�u�W�F�N�g���A�^�b�`����Ă��܂���"); }
        }
        if (interval == 0) { interval = 10; }                                                                                           // �������ł̑I�����ڂ��ړ����鎞�ԊԊu�̐ݒ�
        if (itemSpace == 0) { itemSpace = 120; }                                                                                        // ���j���[�̍��ړ��m��Y���W�Ԋu
        if (selectionItemColor.a == 0) { selectionItemColor.a = 1; }                                                                    // �����x���}�b�N�X�ɐݒ�
        if (notSelectionItemColor.a == 0) { notSelectionItemColor.a = 1; }                                                              // �����x���}�b�N�X�ɐݒ�
        if (selectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#008fd9", out selectionItemColor); }        // ���F�ɐݒ�
        if (notSelectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#ffffff", out notSelectionItemColor); }  // ���F�ɐݒ�

        /*------------------------------------------*/

        /*�y�������ߌn�z*/
        Array.Resize(ref items, menuItems.Length);                                                          // �z��̃T�C�Y�����j���[���ڂƓ������ɐݒ�
        Array.Resize(ref gamePad, Gamepad.all.Count);                                                    // �z��̃T�C�Y���Q�[���p�b�h�̐ڑ����Ɠ������ɐݒ�
        Array.Resize(ref itemTime, menuItems.Length);  //menuItems�̐��ɂ����itemTime�̑傫����ύX����


        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);  // ���j���[���ړ��m��Y���W�Ԋu���w�肵���Ԋu�ɐݒ�
            items[i] = menuItems[i].name;                                                             // �z��̒��Ƀ��j���[���ڂ̖��O����
            itemTime[i] = 0;
        }
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            gamePad[i] = Gamepad.all[i];
        }
        /*--------------*/

        /*�y���̑��ϐ��̏������z*/
        currentMenuName = items[0];
        currentMenuNum = 0;
        oldMenuNum = 0;
        ispush = false;
        decision = false;
        backUI = true;
        moveUI = false;
        pushQuit = false; // Quit�ƃ{�^���������ꂽ��
        count = 0;
        controlNum = 0;

        uiGroup.alpha = 0.0f;
        /*----------------------*/

        /*Pause��p*/
        waitTime = 2.0f;
        Initialize();
        /*--------*/

        cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0f, true);                           // �J�[�\�������X�ɖ߂�
    }

    void Update()
    {
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        /* �y�\����ԁz */
        if (show)
        {
            easTime++;
            // (easTime / 60.0f)���w�肵�����o�ɂ����鏊�v���Ԃ𒴂�����
            if (easTime / 60.0f > duration[1])
            {
                easTime = duration[1] * 60.0f; // easTime��(duration * 60.0f)�ɌŒ�
            }

            if(uiGroup.alpha < 1.0f)
            {
                uiGroup.alpha = easing(duration[0], easTime, 0.5f);
            }

            if (pushQuit == false && decision == false)
            {
                // �J�[�\���ړ��֐�
                CursorMove();

                // ����֐�
                Decision();
            } 
            else if (decision)
            {
                //easTime++;

                //// (easTime / 60.0f)���w�肵�����o�ɂ����鏊�v���Ԃ𒴂�����
                //if (easTime / 60.0f > duration[1])
                //{
                //    easTime = duration[1] * 60.0f; // easTime��(duration * 60.0f)�ɌŒ�
                //}

                switch (currentMenuNum)
                {
                    case 0:
                        uiGroup.alpha = 1.0f - easing(duration[0],easTime,0.5f);
                        break;
                    case 1:
                        
                        // �߂鉉�o�t���O�������Ă��Ȃ���
                        if (backUI == false)
                        {
                            if (moveUI)
                            {
                                if(Mathf.Abs(pauseMenu.anchoredPosition.x) < 1920.0f)
                                {
                                    pauseMenu.anchoredPosition = new Vector2(easing(duration[1], easTime, (1.0f / 2.0f)) * -1920.0f, 0);
                                }
                                if (easTime / 60.0f == duration[1]) {
                                    // B�{�^�����������Ƃ�
                                    if (Gamepad.current.bButton.wasPressedThisFrame)
                                    {
                                        backUI = true; // UI���߂鉉�oON
                                        easTime = 0;   // easTime������
                                        Debug.Log("�ʂ�܂���");
                                    }
                                }
                            }
                            
                        }

                        // �߂鉉�o�t���O�������Ă��鎞
                        else
                        {
                            if (currentMenuNum == 1)
                            {
                                pauseMenu.anchoredPosition = new Vector2(-1920.0f + (easing(duration[1], easTime, (1.0f / 2.0f)) * 1920.0f), 0.0f);
                            }

                            if (Mathf.Abs(pauseMenu.anchoredPosition.x) < 1.0f)
                            {
                                moveUI = false;
                                decision = false;
                                backUI = false;

                                cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // �J�[�\�������X�ɖ߂�
                                inner.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);                           // �J�[�\�������X�ɖ߂�
                            }

                            StartCoroutine("BackMenu");

                        }
                        break;
                }
            }
        }

        /* �y��\����ԁz */
        else if(gameState.isGame && !gameState.isResult)
        {
            // �N�����炪Start�{�^������������
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                easTime = 0;
                /*�y��P��Stat�{�^�����������̂����擾����z*/
                for (int i = 0; i< gamePad.Length; i++)
                {
                    if (gamePad[i].startButton.wasPressedThisFrame)
                    {
                        Debug.Log($"GamePad{i}�����͂��܂���");
                        controlNum = i; // �|�[�Y�̑��쌠����iP�ɂ���
                    }
                }
                show = true;        // �|�[�Y���j���[��\����Ԃ̃t���O�ɂ���
                Time.timeScale = 0; // �|�[�Y(���Ԃ��~�߂�)
                ui.SetActive(true); // UI�I�u�W�F�N�g��L��������

                //SE�iPause���J���j
                audioSource.clip = openMenuSE;
                audioSource.PlayOneShot(openMenuSE);

                inner.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);   // �J�[�\�������X�ɏ���
                cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0.2f, true);
            }
        }
    }

    private void Initialize()
    {
        Time.timeScale = 1;             // �|�[�Y����(���Ԃ𓮂���)
        show = false;                   // ���j���[�� ��\����� �ɂ���
        ui.SetActive(false);     // UI�I�u�W�F�N�g�𖳌�������
    }

    void CursorMove()
    {
        #region �I�����ڂ̕ύX
        // ���X�e�B�b�N����͎� or �\���L�[����͎�
        if (gamePad[controlNum].leftStick.y.ReadValue() > 0 || gamePad[controlNum].dpad.up.isPressed)
        {
            if (ispush == false) // �����ꂽ���̏���
            {
                ispush = true;
                if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;
                audioSource.clip = moveSE;
                audioSource.PlayOneShot(moveSE);
                if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
            }
            else               // ���������̏���
            {
                count++;
                if (count % interval == 0)
                {
                    if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;
                    audioSource.clip = moveSE;
                    audioSource.PlayOneShot(moveSE);
                    if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
                }
            }

            if (currentMenuNum + 1 > (menuItems.Length - 1))
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
        // ���X�e�B�b�N�����͎� or �\���L�[�����͎�
        else if (gamePad[controlNum].leftStick.y.ReadValue() < 0 || gamePad[controlNum].dpad.down.isPressed)
        {
            if (ispush == false) // �����ꂽ���̏���
            {
                ispush = true;
                if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;
                audioSource.clip = moveSE;
                audioSource.PlayOneShot(moveSE);
                if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
            }
            else               // ���������̏���
            {
                count++;
                if (count % interval == 0)
                {
                    if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;
                    audioSource.clip = moveSE;
                    audioSource.PlayOneShot(moveSE);
                    if (itemTime[currentMenuNum] > 0.05f * 60.0f) itemTime[currentMenuNum] = 0;
                }
            }

            if (currentMenuNum - 1 < 0)
            {
                oldMenuNum = menuItems.Length - 1;
            }
            else
            {
                oldMenuNum = currentMenuNum - 1;
            }

            if (itemTime[oldMenuNum] > 0.05f * 60.0f) itemTime[oldMenuNum] = 0;
            oldMenuName = items[oldMenuNum];
            currentMenuName = items[currentMenuNum];  //�I������Ă��郁�j���[����menuName�ɑ������i�����X�V�j

        }
        else
        {
            ispush = false;
            count = 0;
        }
        currentMenuName = items[currentMenuNum]; // ���ݑI�����Ă��郁�j���[������
        #endregion

        float scale = 0;

        #region �J�[�\���̈ړ�����(���ڂ̐��ɂ���Ď����Ń��[�v�����ύX����A�ړ��ł���|�W�V��������������)
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (currentMenuName == items[i])
            {
                cursorRT.position = menuItems[i].GetComponent<RectTransform>().position;
                menuItems[i].GetComponent<Text>().CrossFadeColor(selectionItemColor, 0.05f, true, true); // ���������X�ɐ��F�ɕύX

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
        #endregion
    }

    void Decision()
    {
        // ����{�^��(a�{�^��)���������Ƃ�
        if (gamePad[controlNum].aButton.wasPressedThisFrame)
        {

            CursorFade();
            easTime = 0;
            decision = true; // ����t���OON
            backUI = false;

            audioSource.clip = decisionSE;
            audioSource.PlayOneShot(decisionSE);

            switch (currentMenuNum)
            {
                /*�y�Q�[���ɖ߂�z*/
                case 0:
                    StartCoroutine("PushContinue"); 
                    break;
                /*------------*/

                /*�y����������J���z*/
                case 1:
                    StartCoroutine("PushControls");
                    break;
                /*-------------*/

                /*�y�^�C�g���ɖ߂�z*/
                case 2:
                    pushQuit = true;
                    StartCoroutine("BacktoTitleScene");
                    break;
                /*-------------*/

                default:
                    break;
            }
        }

        /* �y�g���܂킵(�ȒP�ɂǂ̃v���W�F�N�g�ł��j�o����悤�ɂ��ӎ������R�[�h�z */
        //// ��ԏ�̍��ڂ��I������Ă����ԂŌ������������
        //if (menuName == item[0] && gamePad[controlNum].aButton.wasPressedThisFrame)
        //{
        //    /* �y�Q�[���ɖ߂�z */
        //    Time.timeScale = 1;         // �|�[�Y����(���Ԃ𓮂���)
        //    show = false;               // ���j���[�� ��\����� �ɂ���
        //    pauseMenu.SetActive(false); // UI�I�u�W�F�N�g��L����

        //}

        //// ��Ԗڂ̍��ڂ��I������Ă����ԂŌ������������
        //if (menuName == item[1] && gamePad[controlNum].aButton.wasPressedThisFrame)
        //{
        //    /* �y����������J���z */
        //}

        //// �O�Ԗڂ̍��ڂ��I������Ă����ԂŌ������������
        //if (menuName == item[2] && gamePad[controlNum].aButton.wasPressedThisFrame)
        //{
        //    /* �y�^�C�g���ɖ߂�z */
        //    pushQuit = true;
        //    StartCoroutine("BacktoTitleScene");
        //}
    }

    void CursorFade()
    {
        inner.GetComponent<RawImage>().CrossFadeAlpha(0, 0.2f, true);                                           // �J�[�\�������X�ɏ���
        cursor.GetComponent<RawImage>().CrossFadeAlpha(0, 0.2f, true);                                          // �J�[�\�������X�ɏ���
        menuItems[currentMenuNum].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.2f, true, true); // ���������X�ɔ��F�ɖ߂�
    }

    private IEnumerator PushContinue()
    {
        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���

        Time.timeScale = 1;         // �|�[�Y����(���Ԃ𓮂���)
        show = false;               // ���j���[�� ��\����� �ɂ���
        decision = false; // ����t���OON
        ui.SetActive(false); // UI�I�u�W�F�N�g��L����
    }

    private IEnumerator PushControls()
    {
        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���

        moveUI = true;
        easTime = 0;
    }

    private IEnumerator BackMenu()
    {
        yield return new WaitForSecondsRealtime(duration[1]);
        controlsUI.GetComponentInChildren<RawImage>().CrossFadeAlpha(1.0f, 0.0f, true);
        //decision = false;
    }

    private IEnumerator BacktoTitleScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); // ������ҋ@ �V�[�����̉���炷����
        SceneManager.LoadScene("TitleScene");
        Initialize();
    }

    float easing(float duration, float time, float length)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easing�̐i�s�󋵂������l���Z�o
        //TPRate = t * length;

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero);
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
}
