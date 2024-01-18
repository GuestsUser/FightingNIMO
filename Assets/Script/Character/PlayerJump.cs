using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] [Tooltip("接地判定に利用するコライダー")] BoxCollider[] foot;
    [SerializeField] [Tooltip("ジャンプの際の上昇パワー最高値")] float jumpPower = 132.0f;
    [SerializeField] [Tooltip("ジャンプの上昇力がなくなる時間")] float forceStamina = 1.0f;

    enum Section //ジャンプに関する処理の内どれを実行するか記した物
    {
        none, //上昇系では何もしない
        jumpUp, //上昇処理を行う
        jumpDown, //落下処理を行う
        fallDown, //自由落下のアニメ再生用
    }

    float count = 0; //ジャンプを始めてからの時間経過記録
    float nowPower = 0; //現在の上昇力
    bool oldPushJump = false; //前フレームでジャンプボタンを押していた場合trueに
    Section run = Section.none; //上昇系処理の内どれを実行すべきか記録

    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持
    PlayerMoving moving; //移動用スクリプト取得
    PlayerDown down;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
        moving = GetComponent<PlayerMoving>();
        down = GetComponent<PlayerDown>();
    }

    public void RunFunction() //このコンポーネントのメイン機能
    {
        bool isGround = false; //接地してるかどうか判定取り
        foreach (var itr in foot)
        {
            RaycastHit hit;
            if (Physics.BoxCast(itr.transform.position, itr.size, Vector3.down, out hit, Quaternion.Euler(Vector3.zero), 1, parent.floorMask))
            {
                count = 0; //ジャンプを可能にする為カウントリセット
                nowPower = 0;
                isGround = true; //片足でもヒットがあれば接地とする
            }
        }
        parent.animator.SetBool("triggerJump", false); //平常時は常にfalse
        parent.animator.SetFloat("airTime", (parent.animator.GetFloat("airTime") + Time.deltaTime) * Convert.ToInt32(!isGround)); //落下していれば滞空時間を記録

        bool pushJump = false; //ダウンしていれば入力に関わらずfalseとなる
        if (!down.isDown) //ダウンしていれば入力系判定を取る
        {
            pushJump = parent.pInput.actions["Jump"].ReadValue<float>() > 0;
            if (parent.animator.GetCurrentAnimatorStateInfo(1).IsName("wait"))
            {
                if (isGround)
                {
                    bool pullJump = pushJump == false && oldPushJump == true; //ジャンプボタンを離した瞬間ならtrue
                    bool noDash = moving.GetMoveMode() != PlayerMoving.MoveMode.dash; //ダッシュ状態ではない場合true
                    if (pullJump && noDash)
                    {
                        run = Section.jumpUp;
                        nowPower = jumpPower;
                        parent.animator.SetBool("triggerJump", true); //ボタン入力があった且つ接地していればtrue
                    }
                }
                else { run = Section.jumpDown; }
            }
        }
        oldPushJump = pushJump;

        if (run == Section.jumpUp) { JumpUp(isGround); }
        if (run == Section.jumpDown) { JumpDown(isGround); }
    }

    void JumpUp(bool isGround)
    {
        if (count > forceStamina)
        {
            run = Section.jumpDown;
            count = 0;
            return;
        }


        nowPower = jumpPower - (jumpPower / forceStamina * count);
        Vector3 pos = gameObject.transform.position;
        pos.y += (Mathf.Abs(Physics.gravity.y) + nowPower) * Time.deltaTime;
        gameObject.transform.position = pos;

        count += Time.deltaTime;
    }

    void JumpDown(bool isGround)
    {
        if (isGround)
        {
            run = Section.none;
            return;
        }

        nowPower = -jumpPower / forceStamina * count;
        Vector3 pos = gameObject.transform.position;
        pos.y += (Mathf.Abs(Physics.gravity.y) + nowPower) * Time.deltaTime;
        gameObject.transform.position = pos;

        count += Time.deltaTime;
    }

}
