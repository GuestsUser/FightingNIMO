using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow : MonoBehaviour
{
    [SerializeField] [Tooltip("流す力")] float power = 0.35f;
    [SerializeField] [Tooltip("このオブジェクト方向へ押し流す")] GameObject target;

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
        if (rb == null) { return; } //リジットボディがない場合流さない

        rb.AddForce(power * (target.transform.position - other.gameObject.transform.position).normalized, ForceMode.Impulse); //オブジェクトが向いている方向へ流す
    }

}
