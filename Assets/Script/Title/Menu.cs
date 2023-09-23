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
        [Tooltip("�v���C���[�C���v�b�g�}�l�[�W���[�����Ă�������")]
        [SerializeField] private PlayerInputManager PIManeger;
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
        [Tooltip("Credit��Controls���������Ƃ��̊ɋ}�̐i�݋")]
        [SerializeField] private bool easTime;
        [Tooltip("Credit��Controls���������Ƃ��̐؂�ւ�莞��")]
        [SerializeField] private float duration;
    #endregion

    private string[] item; // �g�p����Ă��郁�j���[���ږ���ۑ�����string�^�z��
    private Gamepad[] gamePad; // �ڑ�����Ă���Q�[���p�b�h��ۑ�����string�^�z��
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
        
        /*------------------------------------------*/

        /*�y�������ߌn�z*/
        Array.Resize(ref item, menuObj.Length);                                                          // �z��̃T�C�Y�����j���[���ڂƓ������ɐݒ�
        Array.Resize(ref gamePad, Gamepad.all.Count);
        
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
        /*----------------------*/

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            gamePad[i] = Gamepad.all[i];
        }
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        // �J�[�\���ړ��֐�
        if (decision == false)
        {
            CursorMove();
            Decision();
        }
        
        
    }
    void CursorMove()
    {
        #region �I�����ڂ̕ύX
        // ���X�e�B�b�N����͎� or �\���L�[����͎�
        if (gamePad[0].leftStick.y.ReadValue() > 0 || gamePad[0].dpad.up.isPressed)
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
        else if (gamePad[0].leftStick.y.ReadValue() < 0 || gamePad[0].dpad.down.isPressed)
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
        if (gamePad[0].aButton.wasPressedThisFrame)
        {
            decision = true; // ����t���OON

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
        menuObj[0].GetComponent<Text>().CrossFadeColor(notSelectionItemColor, 0.25f, true, true); // ���������X�ɔ��F�ɖ߂�
    }
    private IEnumerator PushGame()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���

        gameStart.onCharaSelect = true;
        for (int i = 0; i < characterUI.Length; i++)
        {
            characterUI[i].SetActive(true);
        }
        logo.SetActive(false);
        menu.SetActive(false);
    }
    private IEnumerator PushCredit()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���

        /*�yCREDIT�����������Ƃ��̏����z*/
        ui.anchoredPosition = new Vector2(1920, 0);
        /*--------------------------*/

        //logo.SetActive(false);
        //menu.SetActive(false);
    }

    private IEnumerator PushControls()
    {
        CursorFade();

        yield return new WaitForSecondsRealtime(0.25f);                                                        // ������ҋ@ �{�^�������������o�̂���

        /*�yCONTROLS��I�������Ƃ��̏����z*/
       
        ui.anchoredPosition = new Vector2(-1920, 0);
        /*--------------------------*/

        //logo.SetActive(false);
        //menu.SetActive(false);
    }
}
