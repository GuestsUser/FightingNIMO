using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;


public class TestPlayer : MonoBehaviour
{
    [SerializeField] [Tooltip("移動速度")] float speed = 2;
    [SerializeField] [Tooltip("アニメーションを保有するゲームオブジェクト")] GameObject animatorObj;

    Animator animator; //animatorObjから取得したanimatorを保持する変数
    // Start is called before the first frame update
    void Start()
    {
        animator = animatorObj.GetComponent<Animator>(); //アニメーター取得
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moving = Vector3.zero; //今回の移動量入れ
        Keyboard keyboard= Keyboard.current;

        if (keyboard.wKey.isPressed) { moving.z -= speed; } //符号はこのテスト環境上だとカメラ向きが逆なのでこちらも反転させている
        if (keyboard.sKey.isPressed) { moving.z += speed; }
        if (keyboard.dKey.isPressed) { moving.x -= speed; }
        if (keyboard.aKey.isPressed) { moving.x += speed; }

        transform.position += moving;

        bool isPunch = keyboard.spaceKey.isPressed; //パンチ用キーを仮にspaceとしている
        if (isPunch) { animator.SetFloat("punchHoldTime", animator.GetFloat("punchHoldTime") + Time.deltaTime); } //パンチ入力持続時間を設定
        else { animator.SetFloat("punchHoldTime", 0); }

        animator.SetBool("isMoving", moving.z + moving.x != 0);
        animator.SetBool("isPunch", isPunch);
    }
}
