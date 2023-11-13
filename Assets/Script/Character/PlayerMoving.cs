using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] [Tooltip("�ړ����x")] float speed = 0.2f;

    TestPlayer parent; //���̃X�N���v�g�𐧌䂷��e�X�N���v�g��ێ�

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        Vector2 move = parent.pInput.actions["Move"].ReadValue<Vector2>();
        float norm = MathF.Abs(move.x) + MathF.Abs(move.y); //�X�e�B�b�N�̓|����ł��鍇�v����ʂ̎擾

        parent.animator.SetBool("isMoving", norm != 0);

        if (norm <= 0) { return; } //���삪������ΏI��
        transform.position += transform.forward * speed * norm;
    }

}
