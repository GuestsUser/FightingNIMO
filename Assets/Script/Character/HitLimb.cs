using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLimb : MonoBehaviour
{
    [SerializeField] [Tooltip("�_�E�����i��X�N���v�g������")] PlayerDown parent;

    [SerializeField] [Tooltip("�󂯂�_���[�W�ɂ��̔{�����|����")] float hitRate;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (parent.IsInvincible() || parent.IsIgnoreLayer(collision.gameObject)) { return; } //�e�����G���q�b�g�����Ȃ��I�u�W�F�N�g�ւ̃q�b�g�������ꍇ�I���
        Attacker wepon = collision.gameObject.GetComponent<Attacker>();
        if (wepon == null || (!wepon.isAttack)) { return; } //�q�b�g�I�u�W�F�N�g����U���p�X�N���v�g��������Ȃ����͍U����ԂłȂ��ꍇ�I���

        rb.AddForce(wepon.knockBack * collision.contacts[0].normal, ForceMode.Impulse);
        parent.Damage(wepon.damage * hitRate);
    }
}
