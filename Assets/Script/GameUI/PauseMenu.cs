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
    [Tooltip("PauseMenu���\�����Ă���e�v�f�����Ă�������")]
    [SerializeField] private GameObject pauseMenu;
    [Tooltip("�J�[�\�������Ă�������")]
    [SerializeField] private GameObject cursor;
    [Tooltip("�J�[�\���̎q�v�f�����Ă�������")]
    [SerializeField] private GameObject inner; //�J�[�\���̓����̐F
    [Tooltip("���j���[�̍��ڂ̐����w�肵�AUI�I�u�W�F�N�g�����Ă�������")]
    [SerializeField] private GameObject[] menuItems;

    // ��
    private AudioSource audioSouce;
    [SerializeField] private AudioClip desisionSE;      // ���艹
    [SerializeField] private AudioClip cancelSE;        // �L�����Z����
    [SerializeField] private AudioClip moveSE;          // �J�[�\���ړ���
    [SerializeField] private AudioClip openMenuSE;      // ���j���[�\���I��
    [SerializeField] private AudioClip closeMenuSE;     // ���j���[��\���I��

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
    [Tooltip("�J�[�\���̃����_�[�g�����X�t�H�[����񂪓���")]
    [SerializeField] private RectTransform cursorRT;
    [Tooltip("���ݑI������Ă��郁�j���[�ԍ�")]
    [SerializeField] private int currentMenuNum;
    [Tooltip("���ݑI������Ă��郁�j���[��")]
    [SerializeField] private string menuName;
    [Tooltip("�㉺�ǂ��炩�̓��͂�����Ă��邩")]
    [SerializeField] private bool ispush;
    [Tooltip("QUIT�{�^���������ꂽ��")]
    [SerializeField] private bool pushQuit;
    [Tooltip("���͌p������")]
    [SerializeField] private float count;
    [Tooltip("�|�[�Y�̑��쌠���������Ă���v���C���[")]
    [SerializeField] private int controlNum;    // ����ԍ�
    #endregion

    private string[] item;     // �g�p����Ă��郁�j���[���ږ���ۑ�����string�^�z��
    private Gamepad[] gamePad; // �ڑ�����Ă���Q�[���p�b�h��ۑ�����string�^�z��

    [SerializeField] private bool show;      // true:�\�� false:��\��
    [SerializeField] private float waitTime; // �V�[���ړ����Ɏg�p���鏈���ҋ@����(SE����I���܂�)

    //private float fps;

    // Start is called before the first frame update
    void Start()
    {
        /*Title�Ƌ���*/
        /*�y�I�u�W�F�N�g���̎擾�z*/
        pauseMenu.SetActive(false); // �ŏ��͔�\����
        if (cursor == null) { Debug.LogError("�J�[�\���I�u�W�F�N�g���A�^�b�`����Ă��܂���"); }
        cursorRT = cursor.GetComponent<RectTransform>();
        audioSouce = GetComponent<AudioSource>();
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
        Array.Resize(ref item, menuItems.Length);                                                          // �z��̃T�C�Y�����j���[���ڂƓ������ɐݒ�
        Array.Resize(ref gamePad, Gamepad.all.Count);                                                    // �z��̃T�C�Y���Q�[���p�b�h�̐ڑ����Ɠ������ɐݒ�
        Array.Resize(ref itemTime, menuItems.Length);  //menuItems�̐��ɂ����itemTime�̑傫����ύX����


        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);  // ���j���[���ړ��m��Y���W�Ԋu���w�肵���Ԋu�ɐݒ�
            item[i] = menuItems[i].name;                                                             // �z��̒��Ƀ��j���[���ڂ̖��O����
            itemTime[i] = 0;
        }
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            gamePad[i] = Gamepad.all[i];
        }
        /*--------------*/

        /*�y���̑��ϐ��̏������z*/
        menuName = item[0];
        currentMenuNum = 0;
        ispush = false;
        pushQuit = false; // Quit�ƃ{�^���������ꂽ��
        count = 0;
        controlNum = 0;
        /*----------------------*/

        /*Pause��p*/
        waitTime = 2.0f;
        Initialize();
        /*--------*/

        cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0f, true);                           // �J�[�\�������X�ɖ߂�
    }

    // Update is called once per frame
    void Update()
    {
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        /* �y�\����ԁz */
        if (show)
        {
            if(pushQuit == false)
            {
                // �J�[�\���ړ��֐�
                CursorMove();

                // ����֐�
                Decision();
            }
        }

        /* �y��\����ԁz */
        else
        {
            // �N�����炪Start�{�^������������
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                /*�y��P��Stat�{�^�����������̂����擾����z*/
                for(int i = 0; i< gamePad.Length; i++)
                {
                    if (gamePad[i].startButton.wasPressedThisFrame)
                    {
                        Debug.Log($"GamePad{i}�����͂��܂���");
                        controlNum = i;                        // �|�[�Y�̑��쌠����iP�ɂ���
                    }
                }
                show = true;                                   // �|�[�Y���j���[��\����Ԃ̃t���O�ɂ���
                Time.timeScale = 0;                            // �|�[�Y(���Ԃ��~�߂�)
                pauseMenu.SetActive(true);                     // UI�I�u�W�F�N�g��L��������
            }
        }
    }

    private void Initialize()
    {
        Time.timeScale = 1;             // �|�[�Y����(���Ԃ𓮂���)
        show = false;                   // ���j���[�� ��\����� �ɂ���
        pauseMenu.SetActive(false);     // UI�I�u�W�F�N�g�𖳌�������
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
                audioSouce.clip = moveSE;
                audioSouce.PlayOneShot(moveSE);
            }
            else               // ���������̏���
            {
                count++;
                if (count % interval == 0)
                {
                    if (--currentMenuNum < 0) currentMenuNum = menuItems.Length - 1;
                    audioSouce.clip = moveSE;
                    audioSouce.PlayOneShot(moveSE);
                }
            }

        }
        // ���X�e�B�b�N�����͎� or �\���L�[�����͎�
        else if (gamePad[controlNum].leftStick.y.ReadValue() < 0 || gamePad[controlNum].dpad.down.isPressed)
        {
            if (ispush == false) // �����ꂽ���̏���
            {
                ispush = true;
                if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;
                audioSouce.clip = moveSE;
                audioSouce.PlayOneShot(moveSE);
            }
            else               // ���������̏���
            {
                count++;
                if (count % interval == 0)
                {
                    if (++currentMenuNum > menuItems.Length - 1) currentMenuNum = 0;
                    audioSouce.clip = moveSE;
                    audioSouce.PlayOneShot(moveSE);
                }
            }
        }
        else
        {
            ispush = false;
            count = 0;
        }
        menuName = item[currentMenuNum]; // ���ݑI�����Ă��郁�j���[������
        #endregion

        float scale = 0;

        #region �J�[�\���̈ړ�����(���ڂ̐��ɂ���Ď����Ń��[�v�����ύX����A�ړ��ł���|�W�V��������������)
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (menuName == item[i])
            {
                cursorRT.position = menuItems[i].GetComponent<RectTransform>().position;
                menuItems[i].GetComponent<Text>().CrossFadeColor(selectionItemColor, 0.05f, true, true); // ���������X�ɔ��F�ɖ߂�

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
            switch (currentMenuNum)
            {
                /*�y�Q�[���ɖ߂�z*/
                case 0:
                    Time.timeScale = 1;         // �|�[�Y����(���Ԃ𓮂���)
                    show = false;               // ���j���[�� ��\����� �ɂ���
                    pauseMenu.SetActive(false); // UI�I�u�W�F�N�g��L����
                    break;
                /*------------*/

                /*�y����������J���z*/
                case 1:
                    
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

    private IEnumerator BacktoTitleScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); // ������ҋ@ �V�[�����̉���炷����
        SceneManager.LoadScene("TitleScene");
        Initialize();
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
