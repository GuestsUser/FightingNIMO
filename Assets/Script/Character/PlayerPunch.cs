using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] [Tooltip("�������Ɏg���W���C���g")] ConfigurableJoint stickJoint;
    private Rigidbody stickRb; //stickjoint��rigidbody
    private GameObject stickTarget; //�͂ޑΏ�

    TestPlayer parent; //���̃X�N���v�g�𐧌䂷��e�X�N���v�g��ێ�

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
        stickRb = stickJoint.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RunFunction() //���̃R���|�[�l���g�̃��C���@�\
    {
        bool isPunch = parent.pInput.actions["RPunch"].ReadValue<float>() != 0; //�p���`���͂����������L�^

        if (isPunch) { parent.animator.SetFloat("punchHoldTime", parent.animator.GetFloat("punchHoldTime") + Time.deltaTime); } //�p���`���͎������Ԃ�ݒ�
        else
        {
            parent.animator.SetFloat("punchHoldTime", 0);
            if (stickTarget != null)  //�͂�ł����ꍇ�͂ݗp�W���C���g���폜���͂ݑΏۂ̏������Z�b�g����
            {
                Destroy(stickTarget.GetComponent<FixedJoint>());
                stickTarget = null;
            }
        }
        parent.animator.SetBool("isPunch", isPunch);

        if (parent.animator.GetFloat("punchHoldTime") < 2) { return; }
        if (stickTarget != null) { return; } //���ɕʃI�u�W�F�N�g��͂�ł���ΏI��

        RaycastHit hit;
        if (!Physics.SphereCast(stickJoint.transform.position, 1.5f, stickJoint.transform.forward, out hit, 1.9f, parent.hitMask)) { return; } //�q�b�g���Ȃ���΂����ŏI��
        if (hit.transform.GetComponent<Rigidbody>() == null) { return; } //rigidbody��������΂����������I��

        stickTarget = hit.transform.gameObject; //�͂ݑΏەۑ�
        FixedJoint joint = stickTarget.AddComponent<FixedJoint>(); //�͂ݗp�W���C���g�p��
        joint.connectedBody = stickRb;
        joint.breakForce = 9999;
    }
}
