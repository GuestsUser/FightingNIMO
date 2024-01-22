using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLift : MonoBehaviour
{
    TestPlayer parent; //���̃X�N���v�g�𐧌䂷��e�X�N���v�g��ێ�

    void Start()
    {
        parent = GetComponent<TestPlayer>();
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        bool isLift = parent.pInput.actions["Lift"].ReadValue<float>() != 0; //�p���`���͂����������L�^

        parent.animator.SetBool("isLift", isLift);
    }
}
