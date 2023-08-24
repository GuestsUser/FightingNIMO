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
    private enum menuItems // �������j���[�̍��ږ�
    {
        GAME = 0,
        CREDIT = 1,
        CONTROLS = 2,
        EXIT = 3
    }

    #region �C���X�y�N�^�[����̑�����K�vor�������������
    [Header("�y���O�ɓ����̕K�{�z")]
        [Tooltip("���j���[�̍��ڂ�UI�I�u�W�F�N�g�����Ă�������")]
        [SerializeField] private GameObject[] menuItem;

        /*�y��������Ƃ���z*/
        [Tooltip("���ړ��m�̏c�̊Ԋu(���̒l����͂��Ă�������)")]
        [SerializeField] private float itemSpace;
        [Tooltip("�J�[�\���ړ����̎��ԊԊu")]
        [SerializeField] private float interval;
        [Tooltip("�I�����ڂ̃J���[")]
        [SerializeField] private Color selectionItemColor;
        [Tooltip("��I�����ڂ̃J���[")]
        [SerializeField] private Color notSelectionItemColor;
        /*-------------*/
    #endregion

    #region ����m�F�p�ɕ\���������
    [Header("�y����m�F�p�z")]
        [Tooltip("���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g������")]
        [SerializeField] private GameObject menu;
        [Tooltip("���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�̎q�v�f�̃J�[�\���I�u�W�F�N�g������")]
        [SerializeField] private GameObject cursor;
        [Tooltip("�J�[�\���̃����_�[�g�����X�t�H�[����񂪓���")]
        [SerializeField] private RectTransform cursorRT;
        [Tooltip("���ݑI������Ă��郁�j���[�ԍ�")]
        [SerializeField] private menuItems menuName;
        [Tooltip("�㉺�ǂ��炩�̓��͂�����Ă��邩")]
        [SerializeField] private bool push;
        [Tooltip("���͌p������")]
        [SerializeField] private float count;
    #endregion

    private string[] item;
    //private float fps;

    // Start is called before the first frame update
    void Start()
    {
        /*�y�I�u�W�F�N�g���̎擾�z*/
        menu = this.gameObject;
        menu.SetActive(true);
        cursor = transform.Find("Cursor").gameObject; //�q�I�u�W�F�N�g�̃J�[�\�����擾
        cursorRT = cursor.GetComponent<RectTransform>();
        /*-------------------*/

        /*�y���̑��ϐ��̏������z*/
        menuName = menuItems.GAME;
        push = false;
        count = 0;
        /*----------------*/

        /*�y���O�̒������K�v�Ȓl���������������ꍇ�̃f�t�H���g�̒l�z*/
        if (interval == 0) { interval = 10; }                                                                                           // �������ł̑I�����ڂ��ړ����鎞�ԊԊu�̐ݒ�
        if (itemSpace == 0) { itemSpace = 120; }                                                                                        // ���j���[�̍��ړ��m��Y���W�Ԋu
        if (selectionItemColor.a == 0) { selectionItemColor.a = 1; }                                                                    // �����x���}�b�N�X�ɐݒ�
        if (notSelectionItemColor.a == 0) { notSelectionItemColor.a = 1; }                                                              // �����x���}�b�N�X�ɐݒ�
        if (selectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#008fd9", out selectionItemColor); }        // ���F�ɐݒ�
        if (notSelectionItemColor == new Color(0, 0, 0, 1)) { ColorUtility.TryParseHtmlString("#ffffff", out notSelectionItemColor); }  // ���F�ɐݒ�
        /*----------------------------------------------*/

        /*�y�������ߌn�z*/
        Array.Resize(ref item, menuItem.Length);                                                         // �z��̃T�C�Y�����j���[���ڂƓ������ɐݒ�
        for (int i = 0;i < menuItem.Length; i++)
        {
            menuItem[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0,i * -itemSpace);  // ���j���[���ړ��m��Y���W�Ԋu���w�肵���Ԋu�ɐݒ�
            item[i] = menuItem[i].name;                                                                  // �z��̒��Ƀ��j���[���ڂ̖��O����
        }
        /*----------------------------------------------*/
    }

    // Update is called once per frame
    void Update()
    {
        //fps = 1f / Time.deltaTime;
        //Debug.Log(fps);

        // �J�[�\���ړ��֐�
        CursorMove();

        // GAME���ڂ��I������Ă����ԂŌ������������
        if (menuName == menuItems.GAME && Gamepad.current.aButton.wasPressedThisFrame)
        {
            menu.SetActive(false);
        }
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
                if (--menuName < menuItems.GAME) menuName = menuItems.EXIT;
            }
            else               // ���������̏���
            {
                count++;
                if (count % interval == 0)
                {
                    if (--menuName < menuItems.GAME) menuName = menuItems.EXIT;
                }
            }

        }
        // ���X�e�B�b�N�����͎� or �\���L�[�����͎�
        else if (Gamepad.current.leftStick.y.ReadValue() < 0 || Gamepad.current.dpad.down.isPressed)
        {
            if (push == false)
            {
                push = true;
                if (++menuName > menuItems.EXIT) menuName = menuItems.GAME;
            }
            else
            {
                count++;
                if (count % interval == 0)
                {
                    if (++menuName > menuItems.EXIT) menuName = menuItems.GAME;
                }
            }
        }
        else
        {
            push = false;
            count = 0;
        }
        #endregion

        #region �J�[�\���̃|�W�V�����̕ύX�ƃ��j���[���ڂ̐F�̕ύX
        switch (menuName)
        {
            case (menuItems)0:
                // �J�[�\���ʒu�̕ύX
                cursorRT.position = menuItem[(int)menuName].GetComponent<RectTransform>().position;

                // �����̐F�̕ύX
                menuItem[(int)menuName].GetComponent<Text>().color = selectionItemColor;
                for(menuItems i = menuItems.GAME; i < menuItems.EXIT + 1; i++)
                {
                    if(i != menuName)
                    {
                        menuItem[(int)i].GetComponent<Text>().color = notSelectionItemColor;
                    }
                }
                break;

            case (menuItems)1:
                // �J�[�\���ʒu�̕ύX
                cursorRT.position = menuItem[(int)menuName].gameObject.GetComponent<RectTransform>().position;

                // �����̐F�̕ύX
                menuItem[(int)menuName].GetComponent<Text>().color = selectionItemColor;
                for (menuItems i = menuItems.GAME; i < menuItems.EXIT + 1; i++)
                {
                    if (i != menuName)
                    {
                        menuItem[(int)i].GetComponent<Text>().color = notSelectionItemColor;
                    }
                }
                break;

            case (menuItems)2:
                // �J�[�\���ʒu�̕ύX
                cursorRT.position = menuItem[(int)menuName].gameObject.GetComponent<RectTransform>().position;

                // �����̐F�̕ύX
                menuItem[(int)menuName].GetComponent<Text>().color = selectionItemColor;
                for (menuItems i = menuItems.GAME; i < menuItems.EXIT + 1; i++)
                {
                    if (i != menuName)
                    {
                        menuItem[(int)i].GetComponent<Text>().color = notSelectionItemColor;
                    }
                }
                break;

            case (menuItems)3:
                // �J�[�\���ʒu�̕ύX
                cursorRT.position = menuItem[(int)menuName].gameObject.GetComponent<RectTransform>().position;

                // �����̐F�̕ύX
                menuItem[(int)menuName].GetComponent<Text>().color = selectionItemColor;
                for (menuItems i = menuItems.GAME; i < menuItems.EXIT + 1; i++)
                {
                    if (i != menuName)
                    {

                        menuItem[(int)i].GetComponent<Text>().color = notSelectionItemColor;
                    }
                }
                break;

            default:
                break;
        }
        #endregion
    }

   
}
