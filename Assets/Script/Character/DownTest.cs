using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerDown))]

public class DownTest : MonoBehaviour
{
    PlayerDown down;

    // Start is called before the first frame update
    void Start()
    {
        down = GetComponent<PlayerDown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0) { return; } //��~���͎��s���Ȃ�

        down.RunFunction();
        if (down.isDown) { return; } //�_�E�����Ă���Α������͎��s���Ȃ�
    }
    void FixedUpdate()
    {
        if (down.isDown) { return; } //�_�E�����Ă���Α������͎��s���Ȃ�
        //jump.RunFunction();
    }
}
