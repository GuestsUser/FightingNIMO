using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private bool show; // true:表示 false:非表示
    [SerializeField] private float waitTime; // シーン移動時に使用する処理待機時間(SEが鳴り終わるまで)
    [SerializeField] private GameObject pauseMenu;

    private float fps;
    private void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");
    }

    // Start is called before the first frame update
    void Start()
    {
        waitTime = 2.0f;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        fps = 1f / Time.deltaTime;
        Debug.Log(fps);

        /* 【表示状態】 */
        if (show)
        {
            /* 【タイトルに戻る】 */
            if (Gamepad.current.aButton.isPressed && !Gamepad.current.bButton.isPressed)
            {
                StartCoroutine("BacktoTitleScene");
            }

            /* 【ゲームに戻る】 */
            if (Gamepad.current.bButton.isPressed && !Gamepad.current.aButton.isPressed)
            {
                Time.timeScale = 1; // ポーズ解除(時間を動かす)
                show = false; // メニューを 非表示状態 にする
                pauseMenu.SetActive(false);
            }
        }

        /* 【非表示状態】 */
        else
        {
            if (Gamepad.current.startButton.isPressed && !Gamepad.current.bButton.isPressed)
            {
                show = true;
                Time.timeScale = 0; // ポーズ(時間を止める)
                pauseMenu.SetActive(true);
            }
        }
    }

    private void Initialize()
    {
        Time.timeScale = 1; // ポーズ解除(時間を動かす)
        show = false; // メニューを 非表示状態 にする
        pauseMenu.SetActive(false);
    }

    private IEnumerator BacktoTitleScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); // 処理を待機 シーン時の音を鳴らすため
        SceneManager.LoadScene("TitleScene");
        Initialize();
    }
}
