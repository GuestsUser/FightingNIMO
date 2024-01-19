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
        if (Time.timeScale <= 0) { return; } //停止中は実行しない

        down.RunFunction();
        if (down.isDown) { return; } //ダウンしていれば他処理は実行しない
    }
    void FixedUpdate()
    {
        if (down.isDown) { return; } //ダウンしていれば他処理は実行しない
        //jump.RunFunction();
    }
}
