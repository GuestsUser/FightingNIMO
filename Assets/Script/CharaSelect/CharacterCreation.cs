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


        //�󂯎�����ԍ����I�����ꂽ�ԍ��Ɠ����ꍇ�A��������
        if (index == selectionIndex)
        {
            return;
        }
        //�󂯎�����ԍ���0��菬�����A
        //�܂��͎󂯎�����ԍ����L�����N�^�[�̗v�f���ȏゾ������
        if (index < 0 || index >= characters.Count)
        {
            Debug.Log(index);
            Debug.Log(selectionIndex);
            return;
        }
    }
}
