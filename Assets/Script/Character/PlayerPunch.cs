using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] [Tooltip("くっつきに使うジョイント")] ConfigurableJoint stickJoint;
    private Rigidbody stickRb; //stickjointのrigidbody
    private GameObject stickTarget; //掴む対象

    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持

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

    public void RunFunction() //このコンポーネントのメイン機能
    {
        bool isPunch = parent.pInput.actions["RPunch"].ReadValue<float>() != 0; //パンチ入力があったか記録

        if (isPunch) { parent.animator.SetFloat("punchHoldTime", parent.animator.GetFloat("punchHoldTime") + Time.deltaTime); } //パンチ入力持続時間を設定
        else
        {
            parent.animator.SetFloat("punchHoldTime", 0);
            if (stickTarget != null)  //掴んでいた場合掴み用ジョイントを削除し掴み対象の情報もリセットする
            {
                Destroy(stickTarget.GetComponent<FixedJoint>());
                stickTarget = null;
            }
        }
        parent.animator.SetBool("isPunch", isPunch);

        if (parent.animator.GetFloat("punchHoldTime") < 2) { return; }
        if (stickTarget != null) { return; } //既に別オブジェクトを掴んでいれば終了

        RaycastHit hit;
        if (!Physics.SphereCast(stickJoint.transform.position, 1.5f, stickJoint.transform.forward, out hit, 1.9f, parent.hitMask)) { return; } //ヒットがなければここで終了
        if (hit.transform.GetComponent<Rigidbody>() == null) { return; } //rigidbodyが無ければくっつき処理終了

        stickTarget = hit.transform.gameObject; //掴み対象保存
        FixedJoint joint = stickTarget.AddComponent<FixedJoint>(); //掴み用ジョイント用意
        joint.connectedBody = stickRb;
        joint.breakForce = 9999;
    }
}
