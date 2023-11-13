using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerDown : MonoBehaviour
{
    [SerializeField] [Tooltip("�_�E�����ɃL������|���ז߂�͂����S��0�ɂ���Ώۂ̃W���C���g")] ConfigurableJoint hipJoint;
    [SerializeField] [Tooltip("�_�E�����ɖ߂�͂���߂�ΏۂƂ���W���C���g")] ConfigurableJoint[] downJoint;
    [SerializeField] [Tooltip("�_�E�����̖߂��")] float downPower = 5;

    bool _isDown; //�_�E�����Ă��邩���L�^����ϐ�

    public bool isDown { get { return _isDown; } }

    JointDrive[] iniXDrive; //�_�E���p�W���C���g�̏�����x���̖߂��͂��L��
    JointDrive[] iniYZDrive; //��L��yz�p

    JointDrive iniHipXDrive; //��L��hip�p
    JointDrive iniHipYZDrive; //��L��yz�p

    TestPlayer parent; //���̃X�N���v�g�𐧌䂷��e�X�N���v�g��ێ�


    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();

        iniHipXDrive = hipJoint.angularXDrive;
        iniHipYZDrive = hipJoint.angularYZDrive;

        Array.Resize(ref iniXDrive, downJoint.Length);
        Array.Resize(ref iniYZDrive, downJoint.Length);
        for (int i = 0; i < iniXDrive.Length; ++i)
        {
            iniXDrive[i] = downJoint[i].angularXDrive;
            iniYZDrive[i] = downJoint[i].angularYZDrive;
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        _isDown = parent.pInput.actions["DebugDown"].ReadValue<float>() != 0;
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
}
