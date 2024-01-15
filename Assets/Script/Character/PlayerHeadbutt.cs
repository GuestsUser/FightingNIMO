using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHeadbutt : MonoBehaviour
{
    [SerializeField] [Tooltip("一度実行するとこの時間分は再実行不可")] float wait = 1.5f;
    [SerializeField] [Tooltip("ヘッドバットを実行するgameObject")] Attacker head;

    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持
    float count; //時間経過記録

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
        count = wait;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RunFunction() //このコンポーネントのメイン機能
    {
        bool run = count >= wait && parent.pInput.actions["Head"].ReadValue<float>() > 0; //入力を受けられる且つ入力があればtrue
        parent.animator.SetBool("triggerHead", run);

        head.isAttack = parent.animator.GetCurrentAnimatorStateInfo(4).IsName("head"); //アニメが再生中ならアタック許可を出す
        count = (count + Time.deltaTime) * Convert.ToInt32(!run); //実行を行った場合countを0にし、しなければ経過時間の加算を行う
    }
}
