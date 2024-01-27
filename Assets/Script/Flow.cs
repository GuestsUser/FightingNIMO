using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow : MonoBehaviour
{
    [SerializeField] [Tooltip("������")] float power = 0.35f;
    [SerializeField] [Tooltip("���̃I�u�W�F�N�g�����։�������")] GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb == null) { return; } //���W�b�g�{�f�B���Ȃ��ꍇ�����Ȃ�

        rb.AddForce(power * (target.transform.position - other.gameObject.transform.position).normalized, ForceMode.Impulse); //�I�u�W�F�N�g�������Ă�������֗���
    }

}
