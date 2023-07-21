using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private float wait_time; //�V�[���ړ����Ɏg�p���鏈���ҋ@����(SE����I���܂�)

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
        yield return new WaitForSecondsRealtime(wait_time); //������ҋ@ �V�[�����̉���炷����
        SceneManager.LoadScene("GameScene");
    } 
}
