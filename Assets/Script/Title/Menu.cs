using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���R�s�[�p
#region �^�C�g��
#endregion



public class Menu : MonoBehaviour
{
    #region �I�u�W�F�N�g&�R���|�l���g
    [Tooltip("���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g������")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject cursor;
    [SerializeField] private RectTransform cursorRT;
    #endregion

    [Tooltip("���ړ��m�̏c�̊Ԋu")]
    [SerializeField] private float itemSpace;

    private enum menuItems // �������j���[�̍��ږ�
    {
        GAME = 0,
        CREDIT = 1,
        CONTROLS = 2,
        EXIT = 3
    }
    [Tooltip("���ݑI������Ă��郁�j���[�ԍ�")]
    [SerializeField] private menuItems menuName;
    [Tooltip("�㉺�ǂ��炩�̓��͂�����Ă��邩")]
    [SerializeField] private bool push;
    [Tooltip("���͌p������")]
    [SerializeField] private float count;
    [Tooltip("�J�[�\���ړ����̎��ԊԊu")]
    [SerializeField] private float interval;


    #region �V�[���n
    [Tooltip("�V�[���ړ��t���O")]
    [SerializeField] private bool pushScene;
    [Tooltip("�V�[���ړ����̑ҋ@����(���̊Ԃ�SE����)")]
    [SerializeField] private float waitTime; //�V�[���ړ����Ɏg�p���鏈���ҋ@����(SE����I���܂�)
    #endregion

    private float fps;

    // Start is called before the first frame update
    void Start()
    {
        #region �I�u�W�F�N�g�n
        menu = this.gameObject;
        cursor = transform.Find("Cursor").gameObject; //�q�I�u�W�F�N�g�̃J�[�\�����擾
        cursorRT = transform.Find("Cursor").gameObject.GetComponent<RectTransform>();
        #endregion

        menuName = menuItems.GAME;
        if(interval == 0) { interval = 10; }
        //interval = 10;
        #region �V�[���n
        pushScene = false;
        waitTime = 2.0f;
        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        fps = 1f / Time.deltaTime;
        Debug.Log(fps);

        if (pushScene == false)
        {
            CursorMove();
        }
        
        if (menuName == menuItems.GAME && Gamepad.current.aButton.isPressed)
        {
            Debug.Log("�V�[���ړ�");
            pushScene = true;
            StartCoroutine("GotoGameScene");
        }
    }
    void CursorMove()
    {
        // ���X�e�B�b�N����͎�
        if (Gamepad.current.leftStick.y.ReadValue() > 0)
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
        // ���X�e�B�b�N�����͎�
        else if (Gamepad.current.leftStick.y.ReadValue() < 0)
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

        #region (����+�ŊJ��)�J�[�\���̃|�W�V�����𓮂�������
        switch (menuName)
        {
            case menuItems.GAME:
                cursorRT.position = transform.Find("GAME").gameObject.GetComponent<RectTransform>().position;
                break;

            case menuItems.CREDIT:
                cursorRT.position = transform.Find("CREDIT").gameObject.GetComponent<RectTransform>().position;
                break;

            case menuItems.CONTROLS:
                cursorRT.position = transform.Find("CONTROLS").gameObject.GetComponent<RectTransform>().position;
                break;

            case menuItems.EXIT:
                cursorRT.position = transform.Find("EXIT").gameObject.GetComponent<RectTransform>().position;
                break;

            default:
                break;
        }
        #endregion
    }

    private IEnumerator GotoGameScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); //������ҋ@ �V�[�����̉���炷����
        SceneManager.LoadScene("GameScene");
    } 
}
