using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour
{
    private List<GameObject> characters;

    //Default Index of the charcter
    private int selectionIndex = 0;

    private void Start()
    {
        characters = new List<GameObject>();
        foreach(Transform t in transform)
        {
            characters.Add(t.gameObject);
            t.gameObject.SetActive(false);
            Debug.Log("����");
        }
        characters[selectionIndex].SetActive(true);
    }

    private void Update()
    {

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
        if(index < 0 || index >= characters.Count)
        {
            Debug.Log(index);
            Debug.Log(selectionIndex);
            return;
        }
        characters[selectionIndex].SetActive(false);
        selectionIndex = index;
        characters[selectionIndex].SetActive(true);
        //Debug.Log("�ύX����܂���");
    }
}
