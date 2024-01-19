using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] [Tooltip("�������͂�90�x���܂łɊ|���鎞�ԁA�t���[���P��")] float turnTime = 14;

    TestPlayer parent; //���̃X�N���v�g�𐧌䂷��e�X�N���v�g��ێ�

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        Vector2 move = parent.pInput.actions["Move"].ReadValue<Vector2>();
        float norm = MathF.Abs(move.x) + MathF.Abs(move.y); //�X�e�B�b�N�̓|����ł��鍇�v����ʂ̎擾

        if (norm <= 0) { return; } //���삪������ΏI��

        float nowDirection = transform.eulerAngles.y;
        float direction = (MathF.Atan2(move.y, move.x) * Mathf.Rad2Deg - 90) * -1; //�i�s����

        float rotateRange = AngleRangeCulc(direction, nowDirection); //���݉�]����ڕW��]�܂ł̊p�x����
        float sub = rotateRange / MathF.Abs(rotateRange); //�������g�Ŋ��鎖�Ői�s�����̕����擾
        float moveAg = 90 / turnTime * sub; //���삷��p�x��

        float result = nowDirection + moveAg * norm;
        if (sub >= 1 && moveAg * norm > rotateRange) { result = direction; } //����p��direction�Ɉ�v����悤�ɒ���
        if (sub <= -1 && moveAg * norm < rotateRange) { result = direction; }

        if (rotateRange != 0) { transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, result, transform.rotation.eulerAngles.z); }
    }


    float Angle360Fit(float ag)
    {
        return ag < 0 ? 360 - (MathF.Abs(ag) % 360) : ag; //ag���}�C�i�X�������ꍇ0�`360�͈͂Ɏ��܂�悤�ɂ���
    }

    float AngleFitDisplay(float ag)
    {
        float fitAg = Angle360Fit(ag) % 360 ; //�O�̈׊p�x��0�`360�͈̔͂ɔ[�߂�
        return fitAg >= 180 && fitAg < 360 ? (360 - fitAg) * -1 : fitAg; //ag��180�`360�������ꍇ0�`-180�̃f�B�X�v���C�\���Ɠ����`���ɕύX����
    }


    float AngleRangeCulc(float a1,float a2) //0����360�A-180����180���A���̂܂܌v�Z����Ɛ��l�̐؂�ڂł��������Ȃ�p�x�ԋ�����K���ȕ��ɂ��ē��o
    {
        float normalRange = AngleFitDisplay(a1) - AngleFitDisplay(a2); //-180�`180�v�Z
        float fitRange = Angle360Fit(a1) - Angle360Fit(a2); //0�`360�v�Z
        return MathF.Abs(normalRange) < MathF.Abs(fitRange) ? normalRange : fitRange; //����������Ԃ�
    }
}
