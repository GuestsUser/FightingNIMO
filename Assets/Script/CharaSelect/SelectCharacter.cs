using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class SelectCharacter : MonoBehaviour
{
    [Header("�y���O�ɓ������́z")]
    [Tooltip("GameStart�X�N���v�g���A�^�b�`���Ă�������")]
    GameStart gameStart;
    [Tooltip("�g�p����L�����N�^�[�����Ă�������")]
    [SerializeField] private List<GameObject> characters;
    [Tooltip("���̃v���C���[�̃J�[�\�������Ă�������")]
    [SerializeField] private GameObject cursor;
    //[Tooltip("�^�C�g���̃��j���[�I�u�W�F�N�g�����Ă�������")]
    //[SerializeField] private GameObject menu;
    [Tooltip("���̃v���C���[�̃J�[�\���̃e�L�X�g�����Ă�������")]
    [SerializeField] private RectTransform cursorText;
    [Tooltip("�J�[�\���ړ����̎��ԊԊu")]
    [SerializeField] private Text PlayerNum;
    [Tooltip("���̃v���C���[�̃J�[�\�������Ă�������")]
    [SerializeField] private int maxChara;
    [Tooltip("�J�[�\���ړ����̎��ԊԊu")]
    [SerializeField] private float interval;

    [Header("�y����m�F�p�z")]
    [Tooltip("�I�ׂ�L�����N�^�[")]
    [SerializeField] private GameObject[] characterUI;
    [Tooltip("�J�[�\���̃����_�[�g�����X�t�H�[����񂪓���")]
    [SerializeField] private RectTransform cursorRT;
    [Tooltip("���ݑI������Ă��郁�j���[�ԍ�")]
    [SerializeField] private int menuNum;
    [Tooltip("�L�����N�^�[�m��t���O")]
    [SerializeField] public bool charSubmitFlg;

    //[Tooltip("���ݑI������Ă��郁�j���[��")]
    //[SerializeField] private string menuName;
    //[Tooltip("���ړ��m�̏c�̊Ԋu(���̒l����͂��Ă�������)")]
    //[SerializeField] private float itemSpace;
    [Tooltip("���͌p������")]
    [SerializeField] private float count;
    [Tooltip("���X�e�B�b�N��X������͂��Ă��邩")]
    [SerializeField] private bool push;

    private bool getCharacter;

    [SerializeField] private PlayerInput input;

    //private string[] item; // �g�p����Ă���L�����N�^�[����ۑ�����string�^�z��
    //private string parentParentName;
    void Awake()
    {
        input = this.GetComponent<PlayerInput>();        // PlayerInput���擾
        cursorRT = cursor.GetComponent<RectTransform>(); // �J�[�\����RectTransform���擾
        gameStart = GameObject.Find("GameStart").GetComponent<GameStart>();
        //menu = GameObject.Find("Menu");
        
        /*�y�L�����N�^�[�A�C�R��UI�z*/
        Array.Resize(ref characterUI, maxChara); // �z��̃T�C�Y���L�����N�^�[�̐��ŏ�����

        //characterObj[2] = GameObject.Find("");
        //characterObj[3] = GameObject.Find("");
        /*--------------------*/

        transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (interval == 0) { interval = 10; }
        //Array.Resize(ref item, maxChara);
        //for (int i = 0; i < maxChara; i++)
        //{
        //    item[i] = characterUI[i].name;
        //}
        menuNum = 0;
        //menuName = characterUI[0].name;
        //�@�v���C���[�J�[�\���e�L�X�g��P1/P2/P3/P4�ɐݒ�
        PlayerNum.text = "P" + (input.user.index + 1);

        switch (input.user.index)
        {
            case 0:
                cursorText.anchoredPosition = new Vector2(-160, 100);
                //cursorText.localPosition = new Vector2(-160, 100);
                break;
            case 1:
                cursorText.anchoredPosition = new Vector2(160, 100);
                //cursorText.localPosition = new Vector2(160, 100);
                break;
            case 2:
                cursorText.anchoredPosition = new Vector2(-160, -100);
                //cursorText.localPosition = new Vector2(-160, -100);
                break;
            case 3:
                cursorText.anchoredPosition = new Vector2(160, -100);
                //cursorText.localPosition = new Vector2(160, -100);
                break;
        }
        cursor.SetActive(false);
        getCharacter = false;
        charSubmitFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        //menuName = item[menuNum];
        switch (menuNum)
        {
            case 0: // �x�[�X
                characters[menuNum].SetActive(true);
                for(int i = 0;i < maxChara; i++)
                {
                    if(i != menuNum)
                    {
                        characters[i].SetActive(false);
                    }
                }
                break;
            case 1: // �T��
                characters[menuNum].SetActive(true);
                for (int i = 0; i < maxChara; i++)
                {
                    if (i != menuNum)
                    {
                        characters[i].SetActive(false);
                    }
                }
                break;
            //case 2:
            //    break;
            //case 3:
            //    break;
        }
        
        if (gameStart.onCharaSelect == true)
        {
            /*�ycharacterUI�̎擾�ƃJ�[�\���̕\���z*/
            if (getCharacter == false)
            {
                getCharacter = true;
                characterUI[0] = GameObject.Find("BaseButton");
                characterUI[1] = GameObject.Find("SharkButton");
                //characterUI[2] = GameObject.Find("BaseButton");
                //characterUI[3] = GameObject.Find("SharkButton");
                Debug.Log("�L�����N�^�[UI�擾");
                cursor.SetActive(true);
            }
            /*------------------------------*/

            #region �J�[�\���̈ړ�����(���ڂ̐��ɂ���Ď����Ń��[�v�����ύX����A�ړ��ł���|�W�V��������������)
            for (int i = 0; i < maxChara; i++)
            {
                if (menuNum == i)
                {
                    // �J�[�\���ʒu�̕ύX
                    cursorRT.position = characterUI[i].GetComponent<RectTransform>().position;

                    // �����̐F�̕ύX
                    //characterObj[i].GetComponent<Text>().color = selectionItemColor;
                    //for (int j = 0; j < menuObj.Length; j++)
                    //{
                    //    if (item[j] != menuName)
                    //    {
                    //        characterObj[j].GetComponent<Text>().color = notSelectionItemColor;
                    //    }
                    //}
                }
            }
            #endregion
        }


    }

    /*�yOn+Action��(InputValue value)���ŃA�N�V�����}�b�v�̃R�[���o�b�N�֐����`�z*/

    public void OnMove(InputValue value)
    {
        //Debug.Log($"{this.name}:{value.Get<float>()}");
        //var value = context.ReadValue<Vector2>();
        if (gameStart.onCharaSelect == true && charSubmitFlg == false)
        {
            if (value.Get<float>() > 0)
            {
                if (push == false) // �����ꂽ���̏���
                {
                    push = true;
                    if (--menuNum < 0) menuNum = maxChara - 1;
                    //audioSouce.clip = moveSE;
                    //audioSouce.PlayOneShot(moveSE);
                }
                else               // ���������̏���
                {
                    count++;
                    if (count % interval == 0)
                    {
                        if (--menuNum < 0) menuNum = maxChara - 1;
                        //audioSouce.clip = moveSE;
                        //audioSouce.PlayOneShot(moveSE);
                    }
                }
            }
            else if (value.Get<float>() < 0)
            {
                if (push == false)
                {
                    push = true;
                    if (++menuNum > maxChara - 1) menuNum = 0;
                    //audioSouce.clip = moveSE;
                    //audioSouce.PlayOneShot(moveSE);
                }
                else
                {
                    count++;
                    if (count % interval == 0)
                    {
                        if (++menuNum > maxChara - 1) menuNum = 0;
                        //audioSouce.clip = moveSE;
                        //audioSouce.PlayOneShot(moveSE);
                    }
                }
            }
            else
            {
                push = false;
                count = 0;
            }
            
        }



    }

    public void OnSubmit(InputValue value)
    {
        if (gameStart.onCharaSelect)
        {
            Debug.Log("�v���C�L�����N�^�[���m��");
            // �v���C�L�����N�^�[���m��
            if (!charSubmitFlg)
            {
                charSubmitFlg = true;
            }
        }
    }

    public void OnCancel(InputValue value)
    {
        //if(�m��t���O == true) �m��t���O = false
        if (charSubmitFlg)
        {
            charSubmitFlg = false;
        }
    }
}
