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
        [SerializeField] private GameStart gameStart;
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
        menu = this.gameObject;
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

            Zoom(0.8f, easTime);

            if (Gamepad.current.bButton.wasReleasedThisFrame)
            {
                for (int i = 0; i < characterUI.Length; i++)
                {
                    characterUI[i].SetActive(false);
                }

                gameStart.onCharaSelect = false;

                logo.SetActive(true);
                menu.SetActive(true);
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
        

        //yield return new WaitForSecondsRealtime(duration[1] - 0.25f);

        //gameStart.onCharaSelect = true;

        //logo.SetActive(false);
        //menu.SetActive(false);
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
        float t = ((easTime / frame) / duration); // easing�̐i�s�󋵂������l���Z�o
        TPRate = t * length;
        //Debug.Log($"easing�̐i��{t * length}");
        //Debug.Log($"Sin�̒l{ (float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) }");

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length),4, MidpointRounding.AwayFromZero);
    }

    float easing2(float duration, float time, float min, float max,float length)
    {
        float frame = 60.0f;                    // fps
        float t = ((easTime / frame) / duration); // easing�̐i�s�󋵂������l���Z�o

        float easedValue = Mathf.Sin(t * Mathf.PI * length) - 1;

        // �X�P�[�����O�ƃI�t�Z�b�g�K�p
    
        float result = min + (max - min) * easedValue;

        return Mathf.Round(result * 10000.0f) / 10000.0f;
    }

    // duration:max�l�ɂ���܂łɂ����鎞�� max:�ŏI�I�Ȓl delay:�x�点�������� time:����������ŃJ�E���g�A�b�v���Ă���ϐ�
    float Lerp(float duration,float min,float max,float length, float time)
    {
        float delay = (TPRate / length) * duration; // �o�ߎ���
        float Remain_time = duration - delay;�@�@�@ // �ŏI�n�_�܂ł̎c�莞�Ԃ��v�Z

        float velocity = (max - min) / duration;    // �ŏI�n�_�ɂ��ǂ蒅�����߂ɕK�v�ȑ��x�̌v�Z
        float acceleration = velocity / duration;�@ // �ŏI�n�_�ɂ��ǂ蒅�����߂�

        add_value = velocity + acceleration;�@�@�@�@// 

        if(TPRate <= length)
        {
            velocity = 0;
            add_value = velocity;

            return min;
        }
        //min += add_value;
        //min = max;
        return min;
    }

    void Zoom(float duration,float time)
    {
        Debug.Log("zoom��");

        /*�y�J������UI��min��max�̃|�W�V�����A�X�P�[���̐ݒ�z*/
        //float[] cam_posX = { (125.0f / 6.0f) * 0.3f, cSPos.x - 0 };  // { min, max }
        cam_posY = new float[2] { (50.0f / 6.0f) * 0.3f, cSPos.y - 5.0f - 2.5f };
        cam_posZ = new float[2] { (-200.0f / 6.0f) * 0.3f, cSPos.z + 20.0f + 10.0f };

        ui_zoom_scale = new float[2] { 1.0f - 0.7f, 40.0f - (1.0f - 0.7f) }; // { min, max }
        //float ui_out_scale = 1 - 0.7f;              // UI��min
        //float ui_in_scale = 40.0f - zoom_out_scale; // UI��max


        float length = 1.5f; // ����Sin(�g�̒���)

        //float cam_scaleX = cSPos.x + easing(duration, easTime, (length)) * cam_posX[0] - Mathf.Abs((easing(duration, easTime, (length)) - 1.0f) / 2.0f) * cam_posX[1] * Convert.ToInt32(TPRate >= 0.5);
        cam_scaleY = cSPos.y + easing(duration, time, (length)) * cam_posY[0] - Mathf.Abs((easing(duration, time, (length)) - 1.0f) / 2.0f) * cam_posY[1] * Convert.ToInt32(TPRate >= 0.5f);
        cam_scaleZ = cSPos.z + easing(duration, time, (length)) * cam_posZ[0] - Mathf.Abs((easing(duration, time, (length)) - 1.0f) / 2.0f) * cam_posZ[1] * Convert.ToInt32(TPRate >= 0.5f);
        
        ui_scale = 1.0f - easing(duration, time, (length)) * ui_zoom_scale[0] + Mathf.Abs((easing(duration, time, (length)) - 1.0f) / 2.0f) * ui_zoom_scale[1] * Convert.ToInt32(TPRate >= 0.5f);

        //Debug.Log($"cam_scaleY{cam_scaleY},cam_scaleZ{cam_scaleZ},ui_scale{ui_scale}");


        cam.transform.position = new Vector3(0, cam_scaleY, cam_scaleZ);
        
        ui.transform.localScale = new Vector3(ui_scale, ui_scale, ui_scale);


        /*�yUI���ʂ�߂��鉉�o�z*/
        if (ui.transform.localScale.x > 26.0f)  //10
        {
            //logo.SetActive(false);
            //menu.SetActive(false);
            for (int i = 0; i < menuObj.Length; i++)
            {
                menuObj[i].GetComponent<Text>().CrossFadeAlpha(0, 0f, true);
            }

        }
        /*----------------------*/

        /*�y�L�����N�^�[�Z���N�g��ʂ�UI�̗L�����z*/
        if (cam.transform.position == TPos)
        {
            //Debug.Log("�L�����N�^�[�Z���N�g�I��");
            for (int i = 0; i < characterUI.Length; i++)
            {
                characterUI[i].SetActive(true);
            }

            gameStart.onCharaSelect = true;

            logo.SetActive(false);
            menu.SetActive(false);
        }
        else
        {
            //Debug.Log($"{cam.transform.position}");
        }
        /*-----------------------------------------*/
    }

}
