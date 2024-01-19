using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] [Tooltip("�ڒn����ɗ��p����R���C�_�[")] BoxCollider[] foot;
    [SerializeField] [Tooltip("�W�����v�̍ۂ̏㏸�p���[�ō��l")] float jumpPower = 132.0f;
    [SerializeField] [Tooltip("�W�����v�̏㏸�͂��Ȃ��Ȃ鎞��")] float forceStamina = 1.0f;

    enum Section //�W�����v�Ɋւ��鏈���̓��ǂ�����s���邩�L������
    {
        none, //�㏸�n�ł͉������Ȃ�
        jumpUp, //�㏸�������s��
        jumpDown, //�����������s��
        fallDown, //���R�����̃A�j���Đ��p
    }

    float count = 0; //�W�����v���n�߂Ă���̎��Ԍo�ߋL�^
    float nowPower = 0; //���݂̏㏸��
    bool oldPushJump = false; //�O�t���[���ŃW�����v�{�^���������Ă����ꍇtrue��
    Section run = Section.none; //�㏸�n�����̓��ǂ�����s���ׂ����L�^

    TestPlayer parent; //���̃X�N���v�g�𐧌䂷��e�X�N���v�g��ێ�
    PlayerMoving moving; //�ړ��p�X�N���v�g�擾
    PlayerDown down;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
        moving = GetComponent<PlayerMoving>();
        down = GetComponent<PlayerDown>();
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        bool isGround = false; //�ڒn���Ă邩�ǂ���������
        foreach (var itr in foot)
        {
            RaycastHit hit;
            if (Physics.BoxCast(itr.transform.position, itr.size, Vector3.down, out hit, Quaternion.Euler(Vector3.zero), 1, parent.floorMask))
            {
                count = 0; //�W�����v���\�ɂ���׃J�E���g���Z�b�g
                nowPower = 0;
                isGround = true; //�Б��ł��q�b�g������ΐڒn�Ƃ���
            }
        }
        parent.animator.SetBool("triggerJump", false); //���펞�͏��false
        parent.animator.SetFloat("airTime", (parent.animator.GetFloat("airTime") + Time.deltaTime) * Convert.ToInt32(!isGround)); //�������Ă���Α؋󎞊Ԃ��L�^

        bool pushJump = false; //�_�E�����Ă���Γ��͂Ɋւ�炸false�ƂȂ�
        if (!down.isDown) //�_�E�����Ă���Γ��͌n��������
        {
            pushJump = parent.pInput.actions["Jump"].ReadValue<float>() > 0;
            if (parent.animator.GetCurrentAnimatorStateInfo(1).IsName("wait"))
            {
                if (isGround)
                {
                    bool pullJump = pushJump == false && oldPushJump == true; //�W�����v�{�^���𗣂����u�ԂȂ�true
                    bool noDash = moving.GetMoveMode() != PlayerMoving.MoveMode.dash; //�_�b�V����Ԃł͂Ȃ��ꍇtrue
                    if (pullJump && noDash)
                    {
                        run = Section.jumpUp;
                        nowPower = jumpPower;
                        parent.animator.SetBool("triggerJump", true); //�{�^�����͂����������ڒn���Ă����true
                    }
                }
                else { run = Section.jumpDown; }
            }
        }
        oldPushJump = pushJump;

        if (run == Section.jumpUp) { JumpUp(isGround); }
        if (run == Section.jumpDown) { JumpDown(isGround); }
    }

    void JumpUp(bool isGround)
    {
        if (count > forceStamina)
        {
            run = Section.jumpDown;
            count = 0;
            return;
        }


        nowPower = jumpPower - (jumpPower / forceStamina * count);
        Vector3 pos = gameObject.transform.position;
        pos.y += (Mathf.Abs(Physics.gravity.y) + nowPower) * Time.deltaTime;
        gameObject.transform.position = pos;

        count += Time.deltaTime;
    }

    void JumpDown(bool isGround)
    {
        if (isGround)
        {
            run = Section.none;
            return;
        }

        nowPower = -jumpPower / forceStamina * count;
        Vector3 pos = gameObject.transform.position;
        pos.y += (Mathf.Abs(Physics.gravity.y) + nowPower) * Time.deltaTime;
        gameObject.transform.position = pos;

        count += Time.deltaTime;
    }

}
