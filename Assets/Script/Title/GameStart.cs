using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{

    #region �V�[���n
    [Tooltip("�V�[���ړ��t���O")]
    [SerializeField] private bool pushScene;
    public bool _pushScene { get { return pushScene; } } //���̃X�N���v�g�ł�����𐧌䂷��p
    [Tooltip("�V�[���ړ����̑ҋ@����(���̊Ԃ�SE����)")]
    [SerializeField] private float waitTime; //�V�[���ړ����Ɏg�p���鏈���ҋ@����(SE����I���܂�)
    #endregion

    [Tooltip("����ok�t���O(���̃t���O���I���̎��X�^�[�g�{�^���������ƃQ�[���V�[���Ɉړ�)")]
    [SerializeField] private bool ready;


    // Start is called before the first frame update
    void Start()
    {
        #region �V�[���n
        pushScene = false;
        waitTime = 2.0f;
        #endregion
        ready = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(pushScene == false)
        {
            if (ready == true && Gamepad.current.startButton.isPressed)
            {
                pushScene = true;
                //Debug.Log("start");
                StartCoroutine("GotoGameScene");
            }
        }
       
    }
    private IEnumerator GotoGameScene()
    {
        yield return new WaitForSecondsRealtime(waitTime); //������ҋ@ �V�[�����̉���炷����
        SceneManager.LoadScene("GameScene");
    }
}
