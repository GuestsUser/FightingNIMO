using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(PlayerDown))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(PlayerMoving))]
[RequireComponent(typeof(PlayerPunch))]
[RequireComponent(typeof(PlayerLift))]

public class TestPlayer : MonoBehaviour
{
    [SerializeField] [Tooltip("アニメーションを保有するゲームオブジェクト")] Animator _animator;
    [SerializeField] [Tooltip("保持するプレイヤーインプット")] PlayerInput _pInput;

    public PlayerInput pInput { get { return _pInput; } }
    public Animator animator { get { return _animator; } }

    PlayerDown down;
    PlayerRotation rotation;
    PlayerMoving moving;
    PlayerPunch rPunch;
    PlayerLift lift;

    // Start is called before the first frame update
    void Start()
    {
        animator.keepAnimatorStateOnDisable = true;

        down = GetComponent<PlayerDown>();
        rotation = GetComponent<PlayerRotation>();
        moving = GetComponent<PlayerMoving>();
        rPunch = GetComponent<PlayerPunch>();
        lift = GetComponent<PlayerLift>();
    }

    // Update is called once per frame
    void Update()
    {
        down.RunFunction();
        if (down.isDown) { return; } //ダウンしていれば他処理は実行しない

        rotation.RunFunction();
        moving.RunFunction();
        rPunch.RunFunction();
        lift.RunFunction();
    }
}
