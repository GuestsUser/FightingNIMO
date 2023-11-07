using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

// ���R�s�[�p
#region (����+�ŕ���/�J��)�^�C�g��
/*������region�̐����p*/
#endregion



public class Menu : MonoBehaviour
{

    #region �C���X�y�N�^�[����̑�����K�vor�������������
    [Header("�y���O�ɓ������́z")]
        /*�y���O�̑�����K�{�z*/
        [Tooltip("UI�Ƃ������O�̃I�u�W�F�N�g���A�^�b�`���Ă�������")]
        [SerializeField] private RectTransform ui;
        [Tooltip("GameStart�X�N���v�g���A�^�b�`���Ă�������")]
        [SerializeField] private GameStartSystem gameStart;
        [Tooltip("���j���[�O���[�v(��̐e�I�u�W�F�N�g)�̎q�v�f�̃J�[�\�������Ă�������")]
        [SerializeField] private GameObject cursor;
        [Tooltip("���j���[�̍��ڂ̐����w�肵�AUI�I�u�W�F�N�g�����Ă�������")]
        [SerializeField] private GameObject[] menuObj;
        [Tooltip("���S�����Ă�������")]
        [SerializeField] private GameObject logo;
        [Tooltip("�I�ׂ�L�����N�^�[")]
        [SerializeField] private GameObject[] characterUI;
        [Tooltip("Credit��UI�����Ă�������")]
        [SerializeField] private GameObject creditUI;
        [Tooltip("Controls��UI�����Ă�������")]
        [SerializeField] private GameObject controlsUI;
        [Tooltip("�v���C���[�C���v�b�g�}�l�[�W���[�����Ă�������")]
        [SerializeField] private PlayerInputManager PIManeger;
        [Tooltip("�J�����I�u�W�F�N�g�����Ă�������")]
        [SerializeField] private Camera cam;
        // ��
        private AudioSource audioSouce;
        [SerializeField] private AudioClip desisionSE;
        [SerializeField] private AudioClip cancelSE;
        [SerializeField] private AudioClip moveSE;
        [SerializeField] private AudioClip openMenuSE;
        [SerializeField] private AudioClip closeMenuSE;

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
        [Tooltip("�wGAME�x�����������̃J�����̃^�[�Q�b�g�|�W�V���������Ă�������")]
        [SerializeField] private Vector3 TPos;
        /*------------------*/
    #endregion

    #region ����m�F�p�ɕ\���������
    [Header("�y����m�F�p�z")]
        [Tooltip("���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g������")]
        [SerializeField] private GameObject menu;
        [Tooltip("�J�[�\���̃����_�[�g�����X�t�H�[����񂪓���")]
        [SerializeField] private RectTransform cursorRT;
        [Tooltip("���ݑI������Ă��郁�j���[�ԍ�")]
        [SerializeField] private int menuNum;
        [Tooltip("���ݑI������Ă��郁�j���[��")]
        [SerializeField] private string menuName;
        [Tooltip("�㉺�ǂ��炩�̓��͂�����Ă��邩")]
        [SerializeField] private bool push;
        [Tooltip("���͌p������")]
        [SerializeField] private float count;
        [Tooltip("Credit��Controls���������Ƃ��̊ɋ}�̐i�݋(0~1�̓_P)")]
        [SerializeField] private bool decision;
        [Tooltip("Credit��Controls���������Ƃ��̊ɋ}�̐i�݋(0~1�̓_P)")]
        [SerializeField] private float easTime;
        [Tooltip("Game��Game�ȊO���������Ƃ��̐؂�ւ�莞��(�b)")]
        [SerializeField] private float[] duration;
        [Tooltip("UI��߂����ǂ���")]
        [SerializeField] private bool backUI;
        [Tooltip("UI�𓮂������ǂ���")]
        [SerializeField] private bool moveUI;
        [Tooltip("�wGAME�������ꂽ���ǂ����x")]
        [SerializeField] private bool game;
    #endregion

    private string[] item; // �g�p����Ă��郁�j���[���ږ���ۑ�����string�^�z��
    //private Gamepad[] gamePad; // �ڑ�����Ă���Q�[���p�b�h��ۑ�����string�^�z��
    private Vector3 cSPos;
    private float TPRate; //�i����
    private float add_value;

    private float cam_scaleY;
    private float cam_scaleZ;
    private float ui_scale;
    float[] cam_posY = new float[2] {0f, 0f};
    float[] cam_posZ = new float[2] {0f, 0f};

    float[] ui_zoom_scale = new float[2] { 0f, 0f };
    //float acceleration = velocity / duration;
    //private float fps;

    //private void Awake()
    //{
    //    gameObject.GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha(0f,0f,true);
    //}

    // Start is called before the first frame update
    void Start()
    {
        /*�y�I�u�W�F�N�g���̎擾�z*/
        //menu = this.gameObject;
        menu.SetActive(true);
        if(cursor == null) { Debug.LogError("�J�[�\���I�u�W�F�N�g���A�^�b�`����Ă��܂���"); }
        cursorRT = cursor.GetComponent<RectTransform>();
        audioSouce = GetComponent<AudioSource>();
        /*-------------------*/


        /*�y���O�̒������K�v�Ȓl���������������ꍇ�z*/
        for (int i = 0; i < menuObj.Length; i++)
        {
            if (menuObj[i] == null) { Debug.LogError($"menuObj[{i}]�ɃI�u�W�F�N�g���A�^�b�`����Ă��܂���"); }
        }
        if (interval == 0) { interval = 10; }                                                                                           // �������ł̑I�����ڂ��ړ����鎞�ԊԊu�̐ݒ�
        if (itemSpace == 0) { itemSpace = 120; }                                                                                        // ���j���[�̍��ړ��m��Y���W�Ԋu
        if (selectionItemColor.a == 0) { selectionItemColor.a = 1; }                                                                    // �����x���}�b�N�X�ɐݒ�
        if (notSelectionItemColor.a == 0) { notSelectionItemColor.a = 1; }                                                              // �����x���}�b�N�X�ɐݒ�
        if (selectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#008fd9", out selectionItemColor); }        // ���F�ɐݒ�
        if (notSelectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#ffffff", out notSelectionItemColor); }  // ���F�ɐݒ�
        if (TPos == Vector3.zero) { TPos = new Vector3(0, 5, -20); }
        if (duration[0] == 0.0f) { duration[0] = 0.5f; }
        if (duration[1] == 0.0f) { duration[1] = 0.8f; }
        /*------------------------------------------*/

        /*�y�������ߌn�z*/
        Array.Resize(ref item, menuObj.Length);                                                          // �z��̃T�C�Y�����j���[���ڂƓ������ɐݒ�
        //Array.Resize(ref gamePad, Gamepad.all.Count);
        
        for (int i = 0; i < menuObj.Length; i++)
        {
            menuObj[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);  // ���j���[���ړ��m��Y���W�Ԋu���w�肵���Ԋu�ɐݒ�
            menuObj[i].GetComponent<Text>().CrossFadeColor(notSelectionItemColor,0f,true,true);
            item[i] = menuObj[i].name;                                                                   // �z��̒��Ƀ��j���[���ڂ̖��O����
        }
        
        for (int i = 0;i < characterUI.Length; i++)
        {
            characterUI[i].SetActive(false);
        }
        /*--------------*/

        /*�y���̑��ϐ��̏������z*/
        menuName = item[0];
        menuNum = 0;
        push = false;
        count = 0;
        decision = false;
        backUI = true;
        moveUI = false;
        cSPos = cam.transform.position;
        game = false;
        TPRate = 0;
        add_value = 0;
        easTime = 0f;

        cam_scaleY = 0.0f;
        cam_scaleZ = 0.0f;
        ui_scale = 0.0f;
        //duration = new float[2];
        //duration[0] = 0.5f;
        //duration[1] = 0.8f;
        /*----------------------*/

    }

    // Update is called once per frame
    void Update()
    {
        //if (Gamepad.current.xButton.wasPressedThisFrame)
        //{
        //    easTime--;
        //}
        //if (Gamepad.current.yButton.wasPressedThisFrame)
        //{
        //    easTime++;
        //}
        //for (int i = 0; i < Gamepad.all.Count; i++)
        //{
        //    gamePad[i] = Gamepad.all[i];
        //}
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        // �J�[�\���ړ��֐�
        if (decision == false)
        {
            CursorMove();
            Decision();
            //easTime = 0;
            moveUI = false; // �߂�Ȃ���A�ł�����ƂȂ���decision��false�Ȃ̂�moveUI��true�ɂȂ�Ƃ�����肪���������߂����ł�false�ɂ���
        }

        /*�y�wCREDIT�x�ƁwCONTROLS�x���莞�̉��o�z*/
        else if (decision == true && (menuNum > 0 && menuNum < menuObj.Length - 1) && moveUI)
        {

            easTime++;
            if (easTime / 60.0f > duration[0])
            {
                easTime = duration[0] * 60.0f;

                if (Gamepad.current.bButton.wasPressedThisFrame)
                {
                    backUI = true;
                    easTime = 0;
                    Debug.Log("easTime������");
                }
            }

            if(backUI == false)
            {
                Debug.Log("UI�ړ�");
                switch (menuNum)
                {
                    case 1:
                        ui.anchoredPosition = new Vector2(easing(duration[0], easTime, (1.0f / 2.0f)) * 1920.0f, 0);
                        //Debug.Log($"duration[0]{duration[0]},easTime{easTime}");
                        //Debug.Log($"easing(duration[0], easTime, (1.0f / 2.0f)){easing(duration[0], easTime, (1.0f / 2.0f))} * 1920 = {easing(duration[0], easTime, (1 / 2)) * 1920}");
                        cam.transform.localRotation = Quaternion.Euler(0.0f, easing(duration[0], easTime, (1.0f / 2.0f)) * -90.0f, 0.0f);
                        break;
                    case 2:
                        ui.anchoredPosition = new Vector2(easing(duration[0], easTime, (1.0f / 2.0f)) * -1920.0f, 0);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, easing(duration[0], easTime, (1.0f / 2.0f)) * 90.0f, 0.0f);
                        break;
                }
                
            }
            else
            {
                /*�yMENU�ɖ߂鏈���z*/
                Debug.Log("�߂�");
                switch (menuNum)
                {
                    case 1:

                        ui.anchoredPosition = new Vector2(1920.0f - (easing(duration[0], easTime, (1.0f / 2.0f)) * 1920.0f), 0.0f);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, -90.0f + easing(duration[0], easTime, (1.0f / 2.0f)) * 90.0f, 0.0f);
                        break;
                    case 2:
                        ui.anchoredPosition = new Vector2(-1920.0f + (easing(duration[0], easTime, (1.0f / 2.0f)) * 1920.0f), 0.0f);
                        cam.transform.localRotation = Quaternion.Euler(0.0f, 90.0f - easing(duration[0], easTime,(1.0f / 2.0f)) * 90, 0.0f);
                        break;
                }
                
               
                StartCoroutine("BackMenu");
                
            }
           
        }
        /*------------------------------------------*/
        /*�y�wGAME�x���莞�̉��o�z*/
        if (game == true)
        {
            /*�y����͂���őΉ� credit�͍���RawImage�ɂȂ�\��z*/
            creditUI.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, true);
            controlsUI.GetComponentInChildren<RawImage>().CrossFadeAlpha(0.0f,0.0f,true);
            easTime++;
            if (easTime / 60.0f > 0.8f)
            {
                easTime = 0.8f * 60.0f;
                cam.transform.position = TPos;
            }

            //Debug.Log($"easTime{easTime}");

            
           

            if (backUI == false)
            {
                Zoom(0.8f, easTime);

                if(moveUI == false)
                {
                    if (Gamepad.current.bButton.wasReleasedThisFrame)
                    {
                        for (int i = 0; i < characterUI.Length; i++)
                        {
                            characterUI[i].SetActive(false);
                            Debug.Log("�L�����N�^�[UI����\���ɂȂ�܂���");
                        }

                        gameStart.isCharSelect = false;

                        logo.SetActive(true);
                        menu.SetActive(true);
                        backUI = true;
                        easTime = 0;
                    }
                }
                
            }
            else
            {
                Debug.Log("���j���[�ɖ߂�");
                ZoomOut(0.8f, easTime);
            }


           
        }
        /*------------------------*/

    }
    void CursorMove()
    {
        #region �I�����ڂ̕ύX
        //Gamepad.current.
        // ���X�e�B�b�N����͎� or �\���L�[����͎�
        if (Gamepad.current.leftStick.y.ReadValue() > 0 || Gamepad.current.dpad.up.isPressed)
        {
            if (push == false) // �����ꂽ���̏���
            {
                push = true;
                if (--menuNum < 0) menuNum = menuObj.Length - 1;
                audioSouce.clip = moveSE;
                audioSouce.PlayOneShot(moveSE);
            }
            else               // ���������̏���
            {
                count++;
                if (count % interval == 0)
                {
                    if (--menuNum < 0) menuNum = menuObj.Length - 1;
                    audioSouce.clip = moveSE;
                    audioSouce.PlayOneShot(moveSE);
                }
            }

        }
        // ���X�e�B�b�N�����͎� or �\���L�[�����͎�
        else if (Gamepad.current.leftStick.y.ReadValue() < 0 || Gamepad.current.dpad.down.isPressed)
        {
            if (push == false)
            {
                push = true;
                if (++menuNum > menuObj.Length - 1) menuNum = 0;
                audioSouce.clip = moveSE;
                audioSouce.PlayOneShot(moveSE);
            }
            else
            {
                count++;
                if (count % interval == 0)
                {
                    if (++menuNum > menuObj.Length - 1) menuNum = 0;
                    audioSouce.clip = moveSE;
                    audioSouce.PlayOneShot(moveSE);
                }
            }
        }
        else
        {
            push = false;
            count = 0;
        }
        menuName = item[menuNum];
        #endregion

        #region �J�[�\���̈ړ�����(���ڂ̐��ɂ���Ď����Ń��[�v�����ύX����A�ړ��ł���|�W�V��������������)
        for (int i = 0; i < menuObj.Length; i++)
        {
            if (menuName == item[i])
            {
                // �J�[�\���ʒu�̕ύX
                cursorRT.position = menuObj[i].GetComponent<RectTransform>().position;

                // �����̐F�̕ύX
                //menuObj[i].GetComponent<Text>().color = selectionItemColor;
                menuObj[i].GetComponent<Text>().CrossFadeColor(selectionItemColor, 0.1f, true, true);
                for (int j = 0; j < menuObj.Length; j++)
                {
                    if (item[j] != menuName)
                    {
                        //menuObj[j].GetComponent<Text>().color = notSelectionItemColor;
                        menuObj[j].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.1f, true, true);
                    }
                }
            }
        }
        #endregion
    }
    

    void Decision()
    {

        // ����{�^��(a�{�^��)���������Ƃ�
        if (Gamepad.current.aButton.wasPressedThisFrame)
        {
            easTime = 0;
            decision = true; // ����t���OON
            backUI = false;
            switch (menuNum)
            {
                /*�yGAME�z*/
                case 0:
                    StartCoroutine("PushGame");
                    break;
                /*-------*/

                /*�yCREDIT�z*/
                case 1:
                    StartCoroutine("PushCredit");
                    break;
                /*---------*/

                /*�yCONTROLS�z*/
                case 2:
                    StartCoroutine("PushControls");
                    break;
                /*-----------*/

                /*�yEXIT�z*/
                case 3:
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                    break;
                /*-------*/

                default:
                    break;
            }
        }


        //// ��ԏ�̍��ڂ��I������Ă����ԂŌ������������
        //if (menuName == item[0] && gamePad[0].buttonSouth.wasPressedThisFrame)
        //{
        //    decision = true;
        //    StartCoroutine("PushGame");
            
        //}

        //// �l�Ԗڂ̍��ڂ��I������Ă����ԂŌ�������������iswitch�ł܂Ƃ߂�邩���j
        //if(menuName == item[3] && gamePad[0].buttonSouth.wasPressedThisFrame)
        //{
        //    #if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //    #else
        //        Application.Quit();
        //    #endif
        //}
    }

    void CursorFade()
    {
        cursor.GetComponent<RawImage>().CrossFadeAlpha(0, 0.25f, true);                           // �J�[�\�������X�ɏ���
        menuObj[menuNum].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.25f, true, true); // ���������X�ɔ��F�ɖ߂�
    }
    private IEnumerator PushGame()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���
        
        game = true;

        moveUI = true;

       
    }
    private IEnumerator PushCredit()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���

        moveUI = true;

        //yield return new WaitForSecondsRealtime(duration[0] - 0.25f);                                                     // ������ҋ@ UI���ړ����鉉�o�̂���

        /*�yCREDIT�����������Ƃ��̏����z*/

        //ui.anchoredPosition = new Vector2(easing(duration,easTime) * 1920, 0);
        /*--------------------------*/

        //logo.SetActive(false);
        //menu.SetActive(false);
    }

    private IEnumerator PushControls()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���

        moveUI = true;

        //yield return new WaitForSecondsRealtime(duration[0] - 0.25f);                                                     // ������ҋ@ UI���ړ����鉉�o�̂���

        /*�yCONTROLS��I�������Ƃ��̏����z*/

        //ui.anchoredPosition = new Vector2(-1920, 0);
        /*--------------------------*/

        //logo.SetActive(false);
        //menu.SetActive(false);
    }
    private IEnumerator BackMenu()
    {
        yield return new WaitForSecondsRealtime(duration[0]);
        cursor.GetComponent<RawImage>().CrossFadeAlpha(1, 0.1f, true);                           // �J�[�\�������X�ɖ߂�
        decision = false;
        backUI = false;
        moveUI = false;
    }
    float easing(float duration,float time,float length)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration); // easing�̐i�s�󋵂������l���Z�o
        TPRate = t * length;

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length),4, MidpointRounding.AwayFromZero);
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
        TPRate = t;                               // �i�s��(%)
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

    
    void Zoom(float duration,float time)
    {
        /*�y�J������UI�̈����Ɗ��̎��ɗ~�����l�z*/
        cam_posY = new float[2] { 65.0f, 5.0f};
        cam_posZ = new float[2] { -260.0f, -20.0f};

        ui_zoom_scale = new float[2] { 0.7f, 40.0f};
        /*----------------------------------------*/

        float length = 1.5f; // ����Sin(�g�̒���)

        /*�yUI�ƃJ�����̃C�[�W���O�z*/
        if (time < TurningTime(duration,length,0.5f))
        {
            ui_scale = easing3(duration, time, length, false, 1.0f, ui_zoom_scale[0], false);

            cam_scaleY = easing3(duration, time, length, false, 50.0f, cam_posY[0], false);
            cam_scaleZ = easing3(duration, time, length, false, -200.0f, cam_posZ[0], false);
        }
        else
        {
            float ui_old_pos = easing3(duration, TurningTime(duration, length, 0.5f), length, false, 1.0f, ui_zoom_scale[0], false);

            float old_cam_scaleY = easing3(duration, TurningTime(duration, length, 0.5f), length, false, 50.0f, cam_posY[0], false);
            float old_cam_scaleZ = easing3(duration, TurningTime(duration, length, 0.5f), length, true, -200.0f, cam_posZ[0], false);

            ui_scale = easing3(duration, time, length, true, ui_old_pos, ui_zoom_scale[1], true);

            cam_scaleY = easing3(duration, time, length, false, old_cam_scaleY, cam_posY[1], true);
            cam_scaleZ = easing3(duration, time, length, true, old_cam_scaleZ, cam_posZ[1], true);

        }
        /*--------------------------*/

        //Debug.Log($"cam_scaleY{cam_scaleY},cam_scaleZ{cam_scaleZ},ui_scale{ui_scale}");


        cam.transform.position = new Vector3(0, cam_scaleY, cam_scaleZ);
        
        ui.transform.localScale = new Vector3(ui_scale, ui_scale, ui_scale);


        /*�yUI���ʂ�߂��鉉�o�z*/
        if (ui.transform.localScale.x > 26.0f)  //10
        {
            for (int i = 0; i < menuObj.Length; i++)
            {
                menuObj[i].GetComponent<Text>().CrossFadeAlpha(0, 0f, true);
            }
            Debug.Log("���j���[UI����\���ɂȂ�܂���");
        }
        /*----------------------*/

        TPos = new Vector3(0, cam_posY[1], cam_posZ[1]);

        /*�y�L�����N�^�[�Z���N�g��ʂ�UI�̗L�����z*/
        if (cam.transform.position == TPos)
        {
            moveUI = false;
            //Debug.Log("�L�����N�^�[�Z���N�g�I��");
            for (int i = 0; i < characterUI.Length; i++)
            {
                characterUI[i].SetActive(true);
                Debug.Log("�L�����N�^�[UI��\�����܂���");
            }

            gameStart.isCharSelect = true;

            logo.SetActive(false);
            menu.SetActive(false);
        }
        else
        {
            //Debug.Log($"{cam.transform.position}");
        }
        /*-----------------------------------------*/
    }
    void ZoomOut(float duration, float time)
    {
        /*�y�J������UI�̈����Ɗ��̎��ɗ~�����l�z*/
        ui_zoom_scale = new float[2] { 68.0f, 1.0f };

        cam_posY = new float[2] { -15.0f, 50.0f };
        cam_posZ = new float[2] { 40.0f, -200.0f };
        /*----------------------------------------*/

        float length = 0.5f; // ����Sin(�g�̒���)

        ui_scale = easing3(duration, time, length, false, 40.0f, ui_zoom_scale[1], false);

        cam_scaleY = easing3(duration, time, length, true, 5.0f, cam_posY[1], false);
        cam_scaleZ = easing3(duration, time, length, false, -20.0f, cam_posZ[1], false);

        /*�yUI�ƃJ�����̃C�[�W���O�z*/
        //if (time < TurningTime(duration, length, 0.5f))
        //{
        //    ui_scale = easing3(duration, time, length, true, 40.0f, ui_zoom_scale[0], false);

        //    cam_scaleY = easing3(duration, time, length, false, 5.0f, cam_posY[0], false);
        //    cam_scaleZ = easing3(duration, time, length, true, -20.0f, cam_posZ[0], false);
        //}
        //else
        //{
        //    float ui_old_pos = easing3(duration, TurningTime(duration, length, 0.5f), length, true, 40.0f, ui_zoom_scale[0], false);

        //    float old_cam_scaleY = easing3(duration, TurningTime(duration, length, 0.5f), length, false, 5.0f, cam_posY[0], false);
        //    float old_cam_scaleZ = easing3(duration, TurningTime(duration, length, 0.5f), length, true, -20.0f, cam_posZ[0], false);
        //    // Debug.Log($"old_cam_scaleZ{-old_cam_scaleZ}");

        //    ui_scale = easing3(duration, time, length, false, ui_old_pos, ui_zoom_scale[1], true);

        //    cam_scaleY = easing3(duration, time, length, true, old_cam_scaleY, cam_posY[1], true);
        //    cam_scaleZ = easing3(duration, time, length, false, old_cam_scaleZ, cam_posZ[1], true);

        //}
        /*--------------------------*/


        cam.transform.position = new Vector3(0, cam_scaleY, cam_scaleZ);

        ui.transform.localScale = new Vector3(ui_scale, ui_scale, ui_scale);


        /*�yUI���ʂ�߂��鉉�o�z*/
        if (ui.transform.localScale.x < 26.0f)  //10
        {
            //logo.SetActive(false);
            //menu.SetActive(false);
            for (int i = 0; i < menuObj.Length; i++)
            {
                menuObj[i].GetComponent<Text>().CrossFadeAlpha(1.0f, 0f, true);
            }
            Debug.Log("���j���[UI���\������Ă��܂�");
        }
        /*----------------------*/

        TPos = new Vector3(0, cam_posY[1], cam_posZ[1]);

        /*�y�L�����N�^�[�Z���N�g��ʂ�UI�̗L�����z*/
        if (cam.transform.position == TPos)
        {
            game = false;
            backUI = false;
            StartCoroutine("BackMenu");
        }
        else
        {
            //Debug.Log($"{cam.transform.position}");
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
    float TurningTime(float duration,float length,float turning_point)
    {
        float fps = 60.0f;
        float t = duration * fps;
        float divisor = length / turning_point;

        return t / divisor;
    }
}