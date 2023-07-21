using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private float wait_time; //シーン移動時に使用する処理待機時間(SEが鳴り終わるまで)

    // Start is called before the first frame update
    void Start()
    {
        wait_time = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.aButton.isPressed)
        {
            StartCoroutine("GotoGameScene");
        }
    }

    private IEnumerator GotoGameScene()
    {
        yield return new WaitForSecondsRealtime(wait_time); //処理を待機 シーン時の音を鳴らすため
        SceneManager.LoadScene("GameScene");
    } 
}
