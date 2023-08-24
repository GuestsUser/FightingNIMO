using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private bool show; // true:�\�� false:��\��
    [SerializeField] private float waitTime; // �V�[���ړ����Ɏg�p���鏈���ҋ@����(SE����I���܂�)
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

        /* �y�\����ԁz */
        if (show)
        {
            /* �y�^�C�g���ɖ߂�z */
            if (Gamepad.current.aButton.isPressed && !Gamepad.current.bButton.isPressed)
            {
                StartCoroutine("BacktoTitleScene");
            }

            /* �y�Q�[���ɖ߂�z */
            if (Gamepad.current.bButton.isPressed && !Gamepad.current.aButton.isPressed)
            {
                Time.timeScale = 1; // �|�[�Y����(���Ԃ𓮂���)
                show = false; // ���j���[�� ��\����� �ɂ���
                pauseMenu.SetActive(false);
            }
        }

        /* �y��\����ԁz */
        else
        {
            if (Gamepad.current.startButton.isPressed && !Gamepad.current.bButton.isPressed)
            {
                show = true;
                Time.timeScale = 0; // �|�[�Y(���Ԃ��~�߂�)
                pauseMenu.SetActive(true);
            }
        }
    }

    private void Initialize()
    {
        Time.timeScale = 1; // �|�[�Y����(���Ԃ𓮂���)
        show = false; // ���j���[�� ��\����� �ɂ���
        pauseMenu.SetActive(false);
    }

    private IEnumerator BacktoTitleScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); // ������ҋ@ �V�[�����̉���炷����
        SceneManager.LoadScene("TitleScene");
        Initialize();
    }
}
