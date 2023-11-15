using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] [Tooltip("移動速度")] float speed = 0.2f;

    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RunFunction() //このコンポーネントのメイン機能
    {
        Vector2 move = parent.pInput.actions["Move"].ReadValue<Vector2>();
        float norm = MathF.Abs(move.x) + MathF.Abs(move.y); //スティックの倒し具合である合計動作量の取得

        parent.animator.SetBool("isMoving", norm != 0);

        if (norm <= 0) { return; } //操作が無ければ終了
        transform.position += transform.forward * speed * norm;
    }

}
