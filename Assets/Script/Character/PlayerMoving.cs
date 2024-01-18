using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerMoving : MonoBehaviour
{
    public enum MoveMode //����̏�Ԃ�\���񋓌^
    {
        wait, //�����Ă��Ȃ�
        walk, //����
        dash, //����
    }

    [SerializeField] [Tooltip("�ړ����x")] float speed = 0.2f;
    [SerializeField] [Tooltip("�_�b�V�����̑��x")] float dashSpeed = 0.27f;
    [SerializeField] [Tooltip("���̎��ԃW�����v�𒷉�������ƃ_�b�V������")] float dashTriggerTime = 0.31f;

    TestPlayer parent; //���̃X�N���v�g�𐧌䂷��e�X�N���v�g��ێ�

    float count = 0;
    MoveMode moveMode = MoveMode.wait;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        count = (count + Time.deltaTime) * Convert.ToInt32(parent.pInput.actions["Jump"].ReadValue<float>() > 0); //���͂�����Ύ��Ԍo�ߋL�^�A�����łȂ���΃��Z�b�g

        Vector2 move = parent.pInput.actions["Move"].ReadValue<Vector2>();
        float norm = MathF.Abs(move.x) + MathF.Abs(move.y); //�X�e�B�b�N�̓|����ł��鍇�v����ʂ̎擾

        moveMode = MoveMode.wait;
        if (norm != 0) { moveMode = MoveMode.walk; } //�����������walk��
        if (moveMode != MoveMode.wait && count > dashTriggerTime) { moveMode = MoveMode.dash; } //�w�莞�Ԉȏ�W�����v���������~�܂��ĂȂ����dash��
        parent.animator.SetInteger("moveMode", ((int)moveMode));

        float useSpeed = speed; //����̓���Ŏg�p���鑬�x
        switch (moveMode)
        {
            case MoveMode.wait: return; //���삪������ΏI��
            case MoveMode.walk: break;
            case MoveMode.dash: useSpeed = dashSpeed; break; //�_�b�V����ԂȂ瑬�x���_�b�V���p�ɕύX

        }
        transform.position += transform.forward * useSpeed * norm;
    }

    public MoveMode GetMoveMode() { return moveMode; }
}
