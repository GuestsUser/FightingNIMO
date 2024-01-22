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
    [SerializeField] [Tooltip("�A�j���[�V������ۗL����Q�[���I�u�W�F�N�g")] Animator _animator;
    [SerializeField] [Tooltip("�ێ�����v���C���[�C���v�b�g")] PlayerInput _pInput;
    [SerializeField] [Tooltip("�q�b�g��������I�u�W�F�N�g���C���[")] string[] hitLayer;
    [SerializeField] [Tooltip("�n�ʈ����̃��C���[���w��ł���")] string[] floorLayer = { "Default" };

    private GameState gameState;

    int _hitMask; //�q�b�g�������郌�C���[�����ۂɗ��p�\�ɂ����`
    int _floorMask; //�n�ʃq�b�g����郌�C���[�����ۂɗ��p�\�ɂ����`

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

        foreach (string name in hitLayer) { _hitMask = _hitMask | LayerMask.GetMask(name); } //hitMask�֎擾�����r�b�g���i�[
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
        if(!gameState.isGame) { return; }   //�Q�[�����J�n����Ă��Ȃ����͎��s���Ȃ�
        if (Time.timeScale <= 0) { return; } //��~���͎��s���Ȃ�

        down.RunFunction();
        if (down.isDown)  //�_�E�����̏ꍇ���菈���݂̂����s����
        {
            jump.RunFunction();
            punch.RunFunction();
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
        if (down.isDown) { return; } //�_�E�����Ă���Α������͎��s���Ȃ�
        //jump.RunFunction();
    }
}
