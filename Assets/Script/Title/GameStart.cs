using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [Tooltip("���݂̉�ʂ��L�����N�^�[�Z���N�g��ʂ��ǂ���")]
    [SerializeField] public bool onCharaSelect;

    #region �V�[���n
    [Tooltip("�V�[���ړ��t���O")]
    [SerializeField] private bool pushScene;
    public bool _pushScene { get { return pushScene; } } //���̃X�N���v�g�ł�����𐧌䂷��p
    [Tooltip("�V�[���ړ����̑ҋ@����(���̊Ԃ�SE����)")]
    [SerializeField] private float waitTime; //�V�[���ړ����Ɏg�p���鏈���ҋ@����(SE����I���܂�)
    #endregion

    [Tooltip("����ok�t���O(���̃t���O���I���̎��X�^�[�g�{�^���������ƃQ�[���V�[���Ɉړ�)")]
    [SerializeField] private bool ready;

    [SerializeField] private ReceiveNotificationExample receiveNotificationExample;
    private bool allSubmit;
    [SerializeField] private bool[] isSubmit;

    [SerializeField] private GameObject readyUI;


    // Start is called before the first frame update
    void Start()
    {
        #region �V�[���n
        pushScene = false;
        waitTime = 2.0f;
        #endregion
        ready = false;
        onCharaSelect = false;
    }

    // Update is called once per frame
    void Update()
    {
        Array.Resize(ref isSubmit, receiveNotificationExample.playerNum);

        CheckReady();

        if (pushScene == false)
        {
            if (ready == true && Gamepad.current.startButton.isPressed)
            {
                pushScene = true;
                //Debug.Log("start");
                StartCoroutine("GotoGameScene");
            }
        }
       
    }

    private void CheckReady()
    {
        for(int i = 0; i < receiveNotificationExample.playerNum; i++)
        {
            Debug.Log($"Player{i}");
            if(receiveNotificationExample.count == 0)
            {
                isSubmit[i] = GameObject.Find($"Player{i}").GetComponent<SelectCharacter>().charSubmitFlg;
            }
            
        }

        for (int i = 0; i < receiveNotificationExample.playerNum; i++)
        {

            if (isSubmit[i] == false)
            {
                // �ЂƂł�false�ł����for���𔲂���
                Debug.Log("�L�����N�^�[�I�𒆂ł�");
                readyUI.SetActive(false);
                ready = false;
                break;
            }

            // �Ō�̃t���O�ɂ��ǂ蒅�����Ƃ�
            if (isSubmit[receiveNotificationExample.playerNum-1])
            {
                Debug.Log("����ok");
                readyUI.SetActive(true);
                ready = true;
            }
        }
    }

    private IEnumerator GotoGameScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); //������ҋ@ �V�[�����̉���炷����
        SceneManager.LoadScene("GameScene");
    }
}
