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
        [Tooltip("���j���[�̍��ڂ̐����w�肵�AUI�I�u�W�F�N�g�����Ă�������")]
        [SerializeField] private GameObject[] menuObj;
        [Tooltip("���j���[�O���[�v(��̐e�I�u�W�F�N�g)�̎q�v�f�̃J�[�\�������Ă�������")]
        [SerializeField] private GameObject cursor;
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
    #endregion

    private string[] item; // �g�p����Ă��郁�j���[���ږ���ۑ�����string�^�z��
    //private float fps;

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
        
        for (int i = 0; i < menuObj.Length; i++)
        {
            menuObj[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -itemSpace);  // ���j���[���ړ��m��Y���W�Ԋu���w�肵���Ԋu�ɐݒ�
            item[i] = menuObj[i].name;                                                                   // �z��̒��Ƀ��j���[���ڂ̖��O����
        }
        /*--------------*/

        /*�y���̑��ϐ��̏������z*/
        menuName = item[0];
        menuNum = 0;
        push = false;
        count = 0;
        /*----------------------*/
        
    }

    // Update is called once per frame
    void Update()
    {
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        // �J�[�\���ړ��֐�
        CursorMove();

        Decision();
        
    }
    void CursorMove()
    {
        #region �I�����ڂ̕ύX
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
        for (int i = 0;i < menuObj.Length;i++)
        {
            if(menuName == item[i])
            {
                // �J�[�\���ʒu�̕ύX
                cursorRT.position = menuObj[i].GetComponent<RectTransform>().position;

                // �����̐F�̕ύX
                menuObj[i].GetComponent<Text>().color = selectionItemColor;
                for (int j = 0; j < menuObj.Length; j++)
                {
                    if (item[j] != menuName)
                    {
                        menuObj[j].GetComponent<Text>().color = notSelectionItemColor;
                    }
                }
            }
        }
        #endregion
    }

    void Decision()
    {
        
        // ��ԏ�̍��ڂ��I������Ă����ԂŌ������������
        if (menuName == item[0] && Gamepad.current.aButton.wasPressedThisFrame)
        {
            menu.SetActive(false);
        }

    }

   
}
