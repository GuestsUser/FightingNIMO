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

    [SerializeField] [Tooltip("拳をオブジェクトに引き寄せる為の検知半径")] float searchRad = 2.5f;
    [SerializeField] [Tooltip("この半径に入ったオブジェクトを掴む")] float gripRad = 1.1f;
    [SerializeField] [Tooltip("引き寄せ範囲に入ったオブジェクトへ向かう力")] float gatherPower = 15f;

    Rigidbody[] stickRb; //stickjointのrigidbody
    Attacker[] handAttack; //くっつきオブジェクトの攻撃スクリプト
    float[] holdTime; //ボタン長押し時間記録

    FixedJoint[] stick; //実際の掴みに使用するfixedjoint

    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持
    PlayerDown down;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
        down = GetComponent<PlayerDown>();

        stickRb = new Rigidbody[stickJoint.Length];
        handAttack = new Attacker[stickJoint.Length];

        stick = new FixedJoint[stickJoint.Length];
        holdTime = new float[stickJoint.Length];
        for (int i = 0; i < stickJoint.Length; ++i)
        {
            stickRb[i] = stickJoint[i].GetComponent<Rigidbody>();
            handAttack[i] = stickJoint[i].gameObject.GetComponent<Attacker>();
        }
    }

    public void RunFunction() //このコンポーネントのメイン機能
    {
        for (int i = 0; i < stickJoint.Length; ++i)
        {
            bool isPunch = false; //ダウン状態なら強制false
            if (!parent.isDisableInput) { isPunch = parent.pInput.actions[punchParaName[i]].ReadValue<float>() != 0; } //パンチ入力があったか記録

            if ((!isPunch) && stick[i] != null)  //掴んでいた場合掴み用ジョイントを削除し掴み対象の情報もリセットする
            {
                Destroy(stick[i]);
                stick[i] = null;
            }
            holdTime[i] = (holdTime[i] + Time.deltaTime) * Convert.ToInt32(isPunch); //実行を行った場合0にし、しなければ経過時間の加算を行う

            parent.animator.SetBool(animeParaName[i], isPunch);
            if (parent.isDisableInput) { continue; } //ダウン状態の場合実行はここまで

            handAttack[i].isAttack = isPunch && holdTime[i] < stickStartTime; //パンチを押している且つくっつきに移行しない段階ならダメージ発生
            if (holdTime[i] < stickStartTime) { continue; }
            if (stick[i] != null) { continue; } //既に別オブジェクトを掴んでいれば終了

            RaycastHit hit;
            while (true)
            {
                if (!Physics.SphereCast(stickJoint[i].transform.position, searchRad, stickJoint[i].transform.forward, out hit, 0.1f, parent.hitMask)) { break; } //サーチ範囲になければ終了
                if (hit.transform.GetComponent<Rigidbody>() == null) { break; } //ヒットオブジェクトにrigidBodyが無ければ終了

                stickRb[i].AddForce((hit.transform.position - stickJoint[i].transform.position).normalized * gatherPower, ForceMode.Impulse); //サーチオブジェクトへ手を動かす
                break;
            }

            if (!Physics.SphereCast(stickJoint[i].transform.position, gripRad, stickJoint[i].transform.forward, out hit, 0.1f, parent.hitMask)) { continue; } //ヒットがなければここで終了
            if (hit.transform.GetComponent<Rigidbody>() == null) { continue; } //rigidbodyが無ければくっつき処理終了

            stick[i] = stickJoint[i].gameObject.AddComponent<FixedJoint>(); //掴み用ジョイント用意
            stick[i].connectedBody = hit.transform.GetComponent<Rigidbody>();
            stick[i].breakForce = 99999;
        }
    }
}
