using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] [Tooltip("くっつきに使うジョイント")] ConfigurableJoint[] stickJoint;
    [SerializeField] [Tooltip("パンチ入力を取得する為のinputSystem上の名前を入れる")] string[] punchParaName;
    [SerializeField] [Tooltip("アニメのbool値を切り替える為の名前を入れる")] string[] animeParaName;
    [SerializeField] [Tooltip("ボタン長押しからこれだけの時間が経過したらくっつきを開始する")] float stickStartTime = 1;

    private Rigidbody[] stickRb; //stickjointのrigidbody
    private Attacker[] handAttack; //くっつきオブジェクトの攻撃スクリプト
    private GameObject[] stickTarget; //掴む対象
    private float[] holdTime; //ボタン長押し時間記録

    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持

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

    public void RunFunction() //このコンポーネントのメイン機能
    {
        for (int i = 0; i < stickJoint.Length; ++i)
        {
            bool isPunch = parent.pInput.actions[punchParaName[i]].ReadValue<float>() != 0; //パンチ入力があったか記録
            if ((!isPunch) && stickTarget[i] != null)  //掴んでいた場合掴み用ジョイントを削除し掴み対象の情報もリセットする
            {
                foreach (var itr in stickTarget[i].GetComponents<FixedJoint>())
                {
                    if (itr.connectedBody == stickRb[i]) //自分の手に繋げられているfixedJointを検索
                    {
                        Destroy(itr);
                        stickTarget[i] = null;
                        break;
                    }
                }
            }
            holdTime[i] = (holdTime[i] + Time.deltaTime) * Convert.ToInt32(isPunch); //実行を行った場合0にし、しなければ経過時間の加算を行う

            parent.animator.SetBool(animeParaName[i], isPunch);

            handAttack[i].isAttack = isPunch && holdTime[i] < stickStartTime; //パンチを押している且つくっつきに移行しない段階ならダメージ発生
            if (holdTime[i] < stickStartTime) { continue; }
            if (stickTarget[i] != null) { continue; } //既に別オブジェクトを掴んでいれば終了

            RaycastHit hit;
            if (!Physics.SphereCast(stickJoint[i].transform.position, 1.5f, stickJoint[i].transform.forward, out hit, 1.9f, parent.hitMask)) { continue; } //ヒットがなければここで終了
            if (hit.transform.GetComponent<Rigidbody>() == null) { continue; } //rigidbodyが無ければくっつき処理終了

            stickTarget[i] = hit.transform.gameObject; //掴み対象保存
            FixedJoint joint = stickTarget[i].AddComponent<FixedJoint>(); //掴み用ジョイント用意
            joint.connectedBody = stickRb[i];
            joint.breakForce = 9999;
        }
    }
}
