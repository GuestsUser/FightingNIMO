using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHeadbutt : MonoBehaviour
{
    [SerializeField] [Tooltip("��x���s����Ƃ��̎��ԕ��͍Ď��s�s��")] float wait = 1.5f;
    [SerializeField] [Tooltip("�w�b�h�o�b�g�����s����gameObject")] Attacker head;

    TestPlayer parent; //���̃X�N���v�g�𐧌䂷��e�X�N���v�g��ێ�
    float count; //���Ԍo�ߋL�^

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
        count = wait;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        bool run = count >= wait && parent.pInput.actions["Head"].ReadValue<float>() > 0; //���͂��󂯂��銎���͂������true
        parent.animator.SetBool("triggerHead", run);

        head.isAttack = parent.animator.GetCurrentAnimatorStateInfo(4).IsName("head"); //�A�j�����Đ����Ȃ�A�^�b�N�����o��
        count = (count + Time.deltaTime) * Convert.ToInt32(!run); //���s���s�����ꍇcount��0�ɂ��A���Ȃ���Όo�ߎ��Ԃ̉��Z���s��
    }
}
