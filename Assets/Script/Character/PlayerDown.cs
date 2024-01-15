using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerDown : MonoBehaviour
{
    [SerializeField] [Tooltip("�̗́A0�ɂȂ�ƃ_�E������")] float hp = 150;
    [SerializeField] [Tooltip("�_�E���������̎���")] float downTimeBase = 5f;

    [SerializeField] [Tooltip("���G���ԁA�_���[�W���󂯂�Ƃ��̎��ԕ��̓q�b�g��������Ȃ�")] float invincible = 0.2f;
    [SerializeField] [Tooltip("���̃��C���[��gameObject�̓q�b�g�𖳌�������")] string[] ignoreLayer = { "Default" };

    [SerializeField] [Tooltip("�_�E�����ɃL������|���ז߂�͂����S��0�ɂ���Ώۂ̃W���C���g")] ConfigurableJoint hipJoint;
    [SerializeField] [Tooltip("�_�E�����ɖ߂�͂���߂�ΏۂƂ���W���C���g")] ConfigurableJoint[] downJoint;
    [SerializeField] [Tooltip("�_�E�����̖߂��")] float downPower = 5;

    float count = 0; //���A�܂ł̎��Ԃ△�G���Ԃ̏I�����J�E���g����ϐ��A�l��0�ȉ��̏ꍇ�_���[�W���󂯂���J�E���g�_�E���`��
    float iniHp; //���A����hp��߂��׏����̗͂��L�^����
    bool _isDown; //�_�E�����Ă��邩���L�^����ϐ�
    public bool isDown { get { return _isDown; } }
    int downCt = 0; //�_�E�������񐔂��L�^

    JointDrive[] iniXDrive; //�_�E���p�W���C���g�̏�����x���̖߂��͂��L��
    JointDrive[] iniYZDrive; //��L��yz�p

    JointDrive iniHipXDrive; //��L��hip�p
    JointDrive iniHipYZDrive; //��L��yz�p

    // Start is called before the first frame update
    void Start()
    {
        iniHipXDrive = hipJoint.angularXDrive;
        iniHipYZDrive = hipJoint.angularYZDrive;

        Array.Resize(ref iniXDrive, downJoint.Length);
        Array.Resize(ref iniYZDrive, downJoint.Length);
        for (int i = 0; i < iniXDrive.Length; ++i)
        {
            iniXDrive[i] = downJoint[i].angularXDrive;
            iniYZDrive[i] = downJoint[i].angularYZDrive;
        }

        iniHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        if (count <= 0 && isDown) //�_�E�����ɃJ�E���g���؂ꂽ�ꍇ�̗͂�߂��ă_�E������
        {
            hp = iniHp;
            _isDown = false;
        }

        count -= Time.deltaTime;

        //_isDown = parent.pInput.actions["DebugDown"].ReadValue<float>() != 0;
        if (isDown) //�����ӂ�̏����͌�ŕ����\��
        {
            JointDrive jd = hipJoint.angularXDrive;
            jd.positionSpring = 0;
            hipJoint.angularXDrive = jd;

            jd = hipJoint.angularYZDrive;
            jd.positionSpring = 0;
            hipJoint.angularYZDrive = jd;
            foreach (ConfigurableJoint cj in downJoint)
            {
                jd = cj.angularXDrive;
                jd.positionSpring = downPower;
                cj.angularXDrive = jd;

                jd = cj.angularYZDrive;
                jd.positionSpring = downPower;
                cj.angularYZDrive = jd;
            }
            return; //�_�E����ԂȂ瑼�̓��͂��󂯕t���Ȃ�
        }
        else
        {
            hipJoint.angularXDrive = iniHipXDrive;
            hipJoint.angularYZDrive = iniHipYZDrive;
            foreach (ConfigurableJoint cj in downJoint)
            {
                cj.angularXDrive = iniHipXDrive;
                cj.angularYZDrive = iniHipYZDrive;
            }

        }
    }

    public void Damage(float dmg) //dmg�������_���[�W���󂯂�
    {
        hp -= dmg;

        _isDown = hp <= 0;
        if (isDown)
        {
            count = downTimeBase; //�_�E���p�̖��G���Ԃ�����
            downCt++; //�_�E���񐔃J�E���g�A�b�v
            return;
        }

        count = invincible; //�����܂ŗ����ꍇ�ʏ�̖��G����
    }


    public bool IsInvincible() { return count > 0; } //count��0�𒴂��Ă���ꍇ���G��ԂƂ���true��Ԃ�

    public bool IsIgnoreLayer(GameObject obj) //obj���q�b�g�����̃��C���[�łȂ����`�F�b�N����Atrue�������Ȃ疳�����C���[
    {
        foreach (string name in ignoreLayer)
        {
            if (LayerMask.GetMask(name) == obj.layer) { return true; } //obj��ignoreLayer�̉��ꂩ�������ꍇ�����Ȃ̂�true
        }
        return false; //�����܂ł�����false
    }
}
