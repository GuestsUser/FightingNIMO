using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterCreation : MonoBehaviour
{
    public List<GameObject> characters;
    private int childCount;
    //Default Index of the charcter
    private int selectionIndex = 0;

    PlayerInput playerInput;

    [SerializeField] string playerprefix; //�v���C���[���ʗp�̖��O

    //Gamepad gamepad[4] = new Gamepad;

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
        childCount = this.transform.childCount;
    }
    private void Start()
    {
        characters = new List<GameObject>();
        foreach(Transform t in transform)
        {
            characters.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        characters[selectionIndex].SetActive(true);
        characters[childCount - 1].SetActive(true);

        this.gameObject.name = "Player" + playerInput.user.index;
        
    }

    private void Update()
    {
        //bool aButtonInput = Input.GetButtonDown("a");
        //bool aButtonInput_ = Input.GetButtonDown(this.gameObject.name + "a");
        //Debug.Log($"{aButtonInput}{aButtonInput_}");
    }


    //�L�����N�^�[�I��
    public void Select(int index)
    {
        //for (int i = 0; i < Gamepad.all.Count; i++)
        //{
        //    var gamepad = Gamepad.all[i];
        //    //gamepad.deviceId.aButton.isPressed
        //    if (i == 0 && gamepad.aButton.isPressed)
        //    {

        //    }
        //}

        
        //if (playerInput.user.index == 0)
        //{
        //    Debug.Log("Player0�̃L�����N�^�[�ύX");
        //    GameObject.Find("Player0").GetComponent<CharacterCreation>().characters[selectionIndex].SetActive(false);
        //    GameObject.Find("Player0").GetComponent<CharacterCreation>().selectionIndex = index;
        //    GameObject.Find("Player0").GetComponent<CharacterCreation>().characters[selectionIndex].SetActive(true);
        //}
        //else if(playerInput.user.index == 1)
        //{
        //    Debug.Log("Player1�̃L�����N�^�[�ύX");
        //    GameObject.Find("Player1").GetComponent<CharacterCreation>().characters[selectionIndex].SetActive(false);
        //    GameObject.Find("Player1").GetComponent<CharacterCreation>().selectionIndex = index;
        //    GameObject.Find("Player1").GetComponent<CharacterCreation>().characters[selectionIndex].SetActive(true);
        //}

        //�󂯎�����ԍ����I�����ꂽ�ԍ��Ɠ����ꍇ�A��������
        if (index == selectionIndex)
        {
            return;
        }
        //�󂯎�����ԍ���0��菬�����A
        //�܂��͎󂯎�����ԍ����L�����N�^�[�̗v�f���ȏゾ������
        if(index < 0 || index >= characters.Count)
        {
            Debug.Log(index);
            Debug.Log(selectionIndex);
            return;
        }
        //characters[selectionIndex].SetActive(false);
        //selectionIndex = index;
        //characters[selectionIndex].SetActive(true);
        //Debug.Log("�ύX����܂���");
    }
}
