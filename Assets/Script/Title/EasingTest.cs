using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class EasingTest : MonoBehaviour
{
    [SerializeField] private float duration;       // ���v����
    [SerializeField] private float time;           // ���ۂɂ������Ă��錻�ݎ���
    [SerializeField] private float length;         // �g�p����Sin�J�[�u�̒���
    [SerializeField] private float turning_point;  // �g�p����Sin�J�[�u�̒���

    [SerializeField] private float TPRate;         // �i���� (time / fps) / duration
    [SerializeField] private float present_length; // ���݂̓��B�n�_(Sin�J�[�u��)
    [SerializeField] private float[] max;            // �ŏI�I�ɗ~�����l
    //[SerializeField] private float max2;            // �ŏI�I�ɗ~�����l

    [Space(20.0f)]

    [SerializeField] private float source;
    [SerializeField] private float value;

    void Start()
    {
        TPRate = 0;
        value = source;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.rightShoulder.isPressed)
        {
            time++;
            //value = easing(duration, time, length, true, source, max[1]);
        }
        if (Gamepad.current.leftShoulder.isPressed)
        {
            time--;
            //value = easing(duration, time, length, true, source, max[1]);
        }

        //value = (easing(duration, time, 1.5f,false, source, max));

        if (Gamepad.current.selectButton.isPressed)
        {
            Reset();
        }

        Turn();

        //value = (easing2(duration, time, 1.5f, false, min, max, 0.5f));

        //Debug.Log($"value{value} = easing(duration{duration},time{time},length{1.5},symbol{false}) * {target_value}");
        //Debug.Log($"Sin{easing(duration,time,1.5f)}");
    }

    void Turn()
    {
        Debug.Log($"�؂�ւ�����{TurningTime(duration, length, turning_point)}");

        if (time < TurningTime(duration, length, turning_point))
        {
            value = easing(duration, time, length, false, 1.0f, max[0]);
        }
        else
        {

            float old_pos = easing(duration, TurningTime(duration, length, 0.5f), length, false, 1.0f, 0.7f);
            
            float d = (duration  - (TurningTime(duration, length, 0.5f)/60.0f)); // duration����^�[������n�_�ł̎��Ԃ������Ďc�莞�Ԃ��擾
            float t = (time - TurningTime(duration, length, 0.5f));              // time����^�[������n�_�ł̎��Ԃ�������easing��0����X�^�[�g

            value = easing(d, t, 0.5f, true, old_pos, max[1]);
        }
    }

    /// <summary>
    /// Sin�g���g����Easing�֐� ����(���v����,���ݎ���,Sin�J�[�u�̒���,�Ԃ�l�̕��� true:+ false:-,�n�܂�̒l,�ŏI�I�ɗ~�����l)
    /// </summary>
    /// <param name="duration">���v����</param>
    /// <param name="time">���ݎ���</param>
    /// <param name="length">�T�C���J�[�u�̒���</param>
    /// <param name="symbol">�T�C���J�[�u�̎n�܂�̕��� true:+ false:-</param>
    /// <param name="source">�n�܂�̒l</param>
    /// <param name="max">�ŏI�I�ɗ~�����l</param>
    /// <returns></returns>
    float easing(float duration, float time, float length,bool symbol,float source,float max) 
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easing�̐i�s�󋵂������l���Z�o
        TPRate = t;                               // �i�s��(%)
        present_length = t * length;              // ���ݒn�_(Sin�J�[�u���猩��)

        // �Ԃ�l�̕����̐ݒ�
        float symbol_num = symbol ? 1.0f : -1.0f;
        //Debug.Log($"symbol_num{symbol_num}");

        if ((time / frame) > duration)
        {
            t = 1;
            Debug.Log("time is over");
            return source * ((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) * symbol_num) * (max / source);
        }

        return source * ((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) * symbol_num) * (max / source);
    }

    /// <summary>
    /// Sin�g��Easing�֐��̓���̃^�C�~���O�̎��Ԃ��擾����֐� ����(���v����,Sin�J�[�u�̒���,�擾��������(Sin�J�[�u��̒n�_))
    /// </summary>
    /// <param name="duration">���v����</param>
    /// <param name="length">�T�C���J�[�u�̒���</param>
    /// <param name="turning_point">�擾��������(Sin�J�[�u��̒n�_)</param>
    /// <returns></returns>
    float TurningTime(float duration, float length, float turning_point)
    {
        float fps = 60.0f;
        float t = duration * fps;
        float divisor = length / turning_point;

        return t / divisor;
    }

    private void Reset()
    {
        time = 0;
        TPRate = 0;
        present_length = 0;

        value = source;
    }
}
