using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(PlayerDown))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(PlayerMoving))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerPunch))]
[RequireComponent(typeof(PlayerLift))]
[RequireComponent(typeof(PlayerHeadbutt))]

public class TestPlayer : MonoBehaviour
{
    [SerializeField] [Tooltip("アニメーションを保有するゲームオブジェクト")] Animator _animator;
    [SerializeField] [Tooltip("保持するプレイヤーインプット")] PlayerInput _pInput;
    [SerializeField] [Tooltip("ヒット判定を取るオブジェクトレイヤー")] string[] hitLayer;
    [SerializeField] [Tooltip("地面扱いのレイヤーを指定できる")] string[] floorLayer = { "Default" };

    [SerializeField] [Tooltip("キャラを識別する為の番号")] int _playerNumber;
    [Tooltip("trueにすると死亡という事で動けなくなる")] public bool isDead = false;

    bool _isDisableInput = false; //trueだと入力が受けられない状態として処理する
    private GameState gameState;

    int _hitMask; //ヒット判定を取るレイヤーを実際に利用可能にした形
    int _floorMask; //地面ヒットを取るレイヤーを実際に利用可能にした形

    public bool isDisableInput { get { return _isDisableInput; } }
    public int playerNumber { get { return _playerNumber; } }
    public int hitMask { get { return _hitMask; } }
    public int floorMask { get { return _floorMask; } }
    public PlayerInput pInput { get { return _pInput; } }
    public Animator animator { get { return _animator; } }

    PlayerDown down;
    PlayerRotation rotation;
    PlayerMoving moving;
    PlayerJump jump;
    PlayerPunch punch;
    PlayerLift lift;
    PlayerHeadbutt head;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();

        foreach (string name in hitLayer) { _hitMask = _hitMask | LayerMask.GetMask(name); } //hitMaskへ取得したビットを格納
        foreach (string name in floorLayer) { _floorMask = _floorMask | LayerMask.GetMask(name); }

        animator.keepAnimatorStateOnDisable = true;

        down = GetComponent<PlayerDown>();
        rotation = GetComponent<PlayerRotation>();
        moving = GetComponent<PlayerMoving>();
        jump = GetComponent<PlayerJump>();
        punch = GetComponent<PlayerPunch>();
        lift = GetComponent<PlayerLift>();
        head = GetComponent<PlayerHeadbutt>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0) { return; } //停止中は実行しない
        down.RunFunction();

        _isDisableInput = isDead || down.isDown || (!gameState.isGame && !gameState.isResult); //死亡、ダウン、ゲーム非実行状態ならtrue
        if (isDisableInput)  //入力が受けられない場合専用処理
        {
            jump.RunFunction();
            punch.RunFunction();
            head.RunFunction();
            return;
        }

        jump.RunFunction();
        rotation.RunFunction();
        moving.RunFunction();

        punch.RunFunction();
        lift.RunFunction();

        head.RunFunction();
    }
    void FixedUpdate()
    {
        if (down.isDown) { return; } //ダウンしていれば他処理は実行しない
        //jump.RunFunction();
    }
}
