using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerLift : MonoBehaviour
{
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
        bool isLift = parent.pInput.actions["Lift"].ReadValue<float>() != 0; //�p���`���͂����������L�^

        parent.animator.SetBool("isLift", isLift);
    }


}
