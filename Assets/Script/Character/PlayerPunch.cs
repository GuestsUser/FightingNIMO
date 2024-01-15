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

    private Rigidbody[] stickRb; //stickjoint��rigidbody
    private Attacker[] handAttack; //�������I�u�W�F�N�g�̍U���X�N���v�g
    private GameObject[] stickTarget; //�͂ޑΏ�
    private float[] holdTime; //�{�^�����������ԋL�^

    TestPlayer parent; //���̃X�N���v�g�𐧌䂷��e�X�N���v�g��ێ�

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();

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

    // Update is called once per frame
    void Update()
    {
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        for (int i = 0; i < stickJoint.Length; ++i)
        {
            bool isPunch = parent.pInput.actions[punchParaName[i]].ReadValue<float>() != 0; //�p���`���͂����������L�^
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

            handAttack[i].isAttack = isPunch && holdTime[i] < stickStartTime; //�p���`�������Ă��銎�������Ɉڍs���Ȃ��i�K�Ȃ�_���[�W����
            if (holdTime[i] < stickStartTime) { continue; }
            if (stickTarget[i] != null) { continue; } //���ɕʃI�u�W�F�N�g��͂�ł���ΏI��

            RaycastHit hit;
            if (!Physics.SphereCast(stickJoint[i].transform.position, 1.5f, stickJoint[i].transform.forward, out hit, 1.9f, parent.hitMask)) { continue; } //�q�b�g���Ȃ���΂����ŏI��
            if (hit.transform.GetComponent<Rigidbody>() == null) { continue; } //rigidbody��������΂����������I��

            stickTarget[i] = hit.transform.gameObject; //�͂ݑΏەۑ�
            FixedJoint joint = stickTarget[i].AddComponent<FixedJoint>(); //�͂ݗp�W���C���g�p��
            joint.connectedBody = stickRb[i];
            joint.breakForce = 9999;
        }
    }
}
