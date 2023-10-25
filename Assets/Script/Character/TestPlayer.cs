using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] [Tooltip("�ړ����x")] float speed = 2;
    [SerializeField] [Tooltip("�A�j���[�V������ۗL����Q�[���I�u�W�F�N�g")] GameObject animatorObj;
    [SerializeField] [Tooltip("�������Ɏg���W���C���g")] ConfigurableJoint stickJoint;
    [SerializeField] [Tooltip("true�ɂ��鎖�ŃR���g���[�����ω�����")] bool anotherPlayer;

    [SerializeField] [Tooltip("�ێ�����v���C���[�C���v�b�g")] PlayerInput pInput;
    [SerializeField] [Tooltip("�_�E�����ɃL������|���ז߂�͂����S��0�ɂ���Ώۂ̃W���C���g")] ConfigurableJoint hipJoint;
    [SerializeField] [Tooltip("�_�E�����ɖ߂�͂���߂�ΏۂƂ���W���C���g")] ConfigurableJoint[] downJoint;
    [SerializeField] [Tooltip("�_�E�����̖߂��")] float downPower;

    JointDrive[] iniXDrive; //�_�E���p�W���C���g�̏�����x���̖߂��͂��L��
    JointDrive[] iniYZDrive; //��L��yz�p

    JointDrive iniHipXDrive; //��L��hip�p
    JointDrive iniHipYZDrive; //��L��yz�p


    private Animator animator; //animatorObj����擾����animator��ێ�����ϐ�

    private GameObject stickTarget; //�͂ޑΏ�
    private Rigidbody stickRb; //stickjoint��rigidbody

    // Start is called before the first frame update
    void Start()
    {
        //animator.keepAnimatorControllerStateOnDisable = true;
        stickRb = stickJoint.gameObject.GetComponent<Rigidbody>();
        animator = animatorObj.GetComponent<Animator>(); //�A�j���[�^�[�擾

        animator.keepAnimatorStateOnDisable = true;

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

        Keyboard keyboard = Keyboard.current;
        bool downRun = false;
        if (anotherPlayer) { downRun = keyboard.qKey.isPressed; }
        else { downRun = keyboard.eKey.isPressed; }

        //�߂������̍쐬���s��
        if (downRun)
        {
            JointDrive jd = hipJoint.angularXDrive;
            jd.positionSpring = 0;
            hipJoint.angularXDrive = jd;

            jd = hipJoint.angularYZDrive;
            jd.positionSpring = 0;
            hipJoint.angularYZDrive = jd;
            foreach(ConfigurableJoint cj in downJoint)
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


        Vector3 moving = Vector3.zero; //����̈ړ��ʓ���

        if (anotherPlayer)
        {
            if (keyboard.upArrowKey.isPressed) { moving.z += speed; } //�����͂��̃e�X�g���ゾ�ƃJ�����������t�Ȃ̂ł���������]�����Ă���
            if (keyboard.downArrowKey.isPressed) { moving.z -= speed; }
            if (keyboard.rightArrowKey.isPressed) { moving.x += speed; }
            if (keyboard.leftArrowKey.isPressed) { moving.x -= speed; }
        }
        else
        {
            if (keyboard.wKey.isPressed) { moving.z += speed; } //�����͂��̃e�X�g���ゾ�ƃJ�����������t�Ȃ̂ł���������]�����Ă���
            if (keyboard.sKey.isPressed) { moving.z -= speed; }
            if (keyboard.dKey.isPressed) { moving.x += speed; }
            if (keyboard.aKey.isPressed) { moving.x -= speed; }
        }

        transform.position += moving;

        bool isPunch = false; //�p���`�p�L�[������space�Ƃ��Ă���

        if (anotherPlayer) { isPunch = keyboard.mKey.isPressed; }
        else { isPunch = keyboard.spaceKey.isPressed; }

        if (isPunch) { animator.SetFloat("punchHoldTime", animator.GetFloat("punchHoldTime") + Time.deltaTime); } //�p���`���͎������Ԃ�ݒ�
        else
        {
            animator.SetFloat("punchHoldTime", 0);
            if (stickTarget != null)  //�͂�ł����ꍇ�͂ݗp�W���C���g���폜���͂ݑΏۂ̏������Z�b�g����
            {
                Destroy(stickTarget.GetComponent<FixedJoint>());
                stickTarget = null;
            }
        }

        animator.SetBool("isMoving", moving.z + moving.x != 0);
        animator.SetBool("isPunch", isPunch);


        if (animator.GetFloat("punchHoldTime") < 2) { return; }
        if (stickTarget != null) { return; } //���ɕʃI�u�W�F�N�g��͂�ł���ΏI��

        //Debug.Log("1");
        //Debug.DrawRay(stickJoint.transform.position, stickJoint.transform.forward * 1.9f, Color.green, 0.2f);


        int mask = anotherPlayer? LayerMask.GetMask("Chara"): LayerMask.GetMask("Shark");
        RaycastHit hit;
        if (!Physics.SphereCast(stickJoint.transform.position, 1.5f, stickJoint.transform.forward , out hit, 1.9f, mask)) { return; } //�q�b�g���Ȃ���΂����ŏI��
        if (hit.transform.GetComponent<Rigidbody>() == null) { return; } //rigidbody��������΂����������I��


        //Debug.Log(hit.transform.gameObject.name);

        stickTarget = hit.transform.gameObject; //�͂ݑΏەۑ�
        FixedJoint joint= stickTarget.AddComponent<FixedJoint>(); //�͂ݗp�W���C���g�p��
        joint.connectedBody = stickRb;
        joint.breakForce = 9999;
    }
}
