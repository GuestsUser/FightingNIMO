using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;


public class TestPlayer : MonoBehaviour
{
    [SerializeField] [Tooltip("�ړ����x")] float speed = 2;
    [SerializeField] [Tooltip("�A�j���[�V������ۗL����Q�[���I�u�W�F�N�g")] GameObject animatorObj;

    Animator animator; //animatorObj����擾����animator��ێ�����ϐ�
    // Start is called before the first frame update
    void Start()
    {
        animator = animatorObj.GetComponent<Animator>(); //�A�j���[�^�[�擾
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moving = Vector3.zero; //����̈ړ��ʓ���
        Keyboard keyboard= Keyboard.current;

        if (keyboard.wKey.isPressed) { moving.z -= speed; } //�����͂��̃e�X�g���ゾ�ƃJ�����������t�Ȃ̂ł���������]�����Ă���
        if (keyboard.sKey.isPressed) { moving.z += speed; }
        if (keyboard.dKey.isPressed) { moving.x -= speed; }
        if (keyboard.aKey.isPressed) { moving.x += speed; }

        transform.position += moving;

        bool isPunch = keyboard.spaceKey.isPressed; //�p���`�p�L�[������space�Ƃ��Ă���
        if (isPunch) { animator.SetFloat("punchHoldTime", animator.GetFloat("punchHoldTime") + Time.deltaTime); } //�p���`���͎������Ԃ�ݒ�
        else { animator.SetFloat("punchHoldTime", 0); }

        animator.SetBool("isMoving", moving.z + moving.x != 0);
        animator.SetBool("isPunch", isPunch);
    }
}
