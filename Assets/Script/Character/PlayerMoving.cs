using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerMoving : MonoBehaviour
{
    public enum MoveMode //動作の状態を表す列挙型
    {
        wait, //動いていない
        walk, //歩き
        dash, //走り
    }

    [SerializeField] [Tooltip("移動速度")] float speed = 0.2f;
    [SerializeField] [Tooltip("ダッシュ時の速度")] float dashSpeed = 0.27f;
    [SerializeField] [Tooltip("この時間ジャンプを長押しするとダッシュする")] float dashTriggerTime = 0.31f;

    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持

    float count = 0;
    MoveMode moveMode = MoveMode.wait;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
    }

    public void RunFunction() //このコンポーネントのメイン機能
    {
        count = (count + Time.deltaTime) * Convert.ToInt32(parent.pInput.actions["Jump"].ReadValue<float>() > 0); //入力があれば時間経過記録、そうでなければリセット

        Vector2 move = parent.pInput.actions["Move"].ReadValue<Vector2>();
        float norm = MathF.Abs(move.x) + MathF.Abs(move.y); //スティックの倒し具合である合計動作量の取得

        moveMode = MoveMode.wait;
        if (norm != 0) { moveMode = MoveMode.walk; } //動きがあればwalkに
        if (moveMode != MoveMode.wait && count > dashTriggerTime) { moveMode = MoveMode.dash; } //指定時間以上ジャンプ長押し且つ止まってなければdashに
        parent.animator.SetInteger("moveMode", ((int)moveMode));

        float useSpeed = speed; //今回の動作で使用する速度
        switch (moveMode)
        {
            case MoveMode.wait: return; //操作が無ければ終了
            case MoveMode.walk: break;
            case MoveMode.dash: useSpeed = dashSpeed; break; //ダッシュ状態なら速度をダッシュ用に変更

        }
        transform.position += transform.forward * useSpeed * norm;
    }

    public MoveMode GetMoveMode() { return moveMode; }
}
