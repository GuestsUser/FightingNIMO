using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] [Tooltip("�v���C���[�Ƀq�b�g�����ꍇ�^�����b�_���[�W")] float _damage = 15;
    [SerializeField] [Tooltip("�q�b�g�̎��ɐ�����΂���")] float _knockBack = 75;


    [SerializeField] [Tooltip("���̕��킪�_���[�W���q�b�g�Ń_���[�W��^�����邩�����Atrue�Ȃ�q�b�g������_���[�W��^����")] bool _isAttack;
    public bool isAttack { get { return _isAttack; } set { _isAttack = value; } }
    public float damage { get { return _damage; } }
    public float knockBack { get { return _knockBack; } }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
