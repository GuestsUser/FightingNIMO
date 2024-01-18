using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] [Tooltip("�������Ɏg���W���C���g")] ConfigurableJoint[] stickJoint;
    [SerializeField] [Tooltip("�p���`���͂��擾����ׂ�inputSystem��̖��O������")] string[] punchParaName;
    [SerializeField] [Tooltip("�A�j����bool�l��؂�ւ���ׂ̖��O������")] string[] animeParaName;
    [SerializeField] [Tooltip("�{�^�����������炱�ꂾ���̎��Ԃ��o�߂����炭�������J�n����")] float stickStartTime = 1;

    [SerializeField] [Tooltip("�����I�u�W�F�N�g�Ɉ����񂹂�ׂ̌��m���a")] float searchRad = 2.5f;
    [SerializeField] [Tooltip("���̔��a�ɓ������I�u�W�F�N�g��͂�")] float gripRad = 1.1f;
    [SerializeField] [Tooltip("�����񂹔͈͂ɓ������I�u�W�F�N�g�֌�������")] float gatherPower = 15f;

    private Rigidbody[] stickRb; //stickjoint��rigidbody
    private Attacker[] handAttack; //�������I�u�W�F�N�g�̍U���X�N���v�g
    private GameObject[] stickTarget; //�͂ޑΏ�
    private float[] holdTime; //�{�^�����������ԋL�^

    TestPlayer parent; //���̃X�N���v�g�𐧌䂷��e�X�N���v�g��ێ�
    PlayerDown down;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
        down = GetComponent<PlayerDown>();

        stickRb = new Rigidbody[stickJoint.Length];
        handAttack = new Attacker[stickJoint.Length];

        stickTarget = new GameObject[stickJoint.Length];
        holdTime = new float[stickJoint.Length];
        for (int i = 0; i < stickJoint.Length; ++i)
        {
            stickRb[i] = stickJoint[i].GetComponent<Rigidbody>();
            handAttack[i] = stickJoint[i].gameObject.GetComponent<Attacker>();
        }
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        for (int i = 0; i < stickJoint.Length; ++i)
        {
            bool isPunch = false; //�_�E����ԂȂ狭��false
            if (!down.isDown) { isPunch = parent.pInput.actions[punchParaName[i]].ReadValue<float>() != 0; } //�p���`���͂����������L�^

            if ((!isPunch) && stickTarget[i] != null)  //�͂�ł����ꍇ�͂ݗp�W���C���g���폜���͂ݑΏۂ̏������Z�b�g����
            {
                foreach (var itr in stickTarget[i].GetComponents<FixedJoint>())
                {
                    if (itr.connectedBody == stickRb[i]) //�����̎�Ɍq�����Ă���fixedJoint������
                    {
                        Destroy(itr);
                        stickTarget[i] = null;
                        break;
                    }
                }
            }
            holdTime[i] = (holdTime[i] + Time.deltaTime) * Convert.ToInt32(isPunch); //���s���s�����ꍇ0�ɂ��A���Ȃ���Όo�ߎ��Ԃ̉��Z���s��

            parent.animator.SetBool(animeParaName[i], isPunch);
            if (down.isDown) { continue; } //�_�E����Ԃ̏ꍇ���s�͂����܂�

            handAttack[i].isAttack = isPunch && holdTime[i] < stickStartTime; //�p���`�������Ă��銎�������Ɉڍs���Ȃ��i�K�Ȃ�_���[�W����
            if (holdTime[i] < stickStartTime) { continue; }
            if (stickTarget[i] != null) { continue; } //���ɕʃI�u�W�F�N�g��͂�ł���ΏI��

            RaycastHit hit;
            while (true)
            {
                if (!Physics.SphereCast(stickJoint[i].transform.position, searchRad, stickJoint[i].transform.forward, out hit, 0.1f, parent.hitMask)) { break; } //�T�[�`�͈͂ɂȂ���ΏI��
                if (hit.transform.GetComponent<Rigidbody>() == null) { break; } //�q�b�g�I�u�W�F�N�g��rigidBody��������ΏI��

                stickRb[i].AddForce((hit.transform.position - stickJoint[i].transform.position).normalized * gatherPower, ForceMode.Impulse); //�T�[�`�I�u�W�F�N�g�֎�𓮂���
                break;
            }

            if (!Physics.SphereCast(stickJoint[i].transform.position, gripRad, stickJoint[i].transform.forward, out hit, 0.1f, parent.hitMask)) { continue; } //�q�b�g���Ȃ���΂����ŏI��
            if (hit.transform.GetComponent<Rigidbody>() == null) { continue; } //rigidbody��������΂����������I��

            stickTarget[i] = hit.transform.gameObject; //�͂ݑΏەۑ�
            FixedJoint joint = stickTarget[i].AddComponent<FixedJoint>(); //�͂ݗp�W���C���g�p��
            joint.connectedBody = stickRb[i];
            joint.breakForce = 9999;
        }
    }
}
