using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    [Tooltip("�J�n�O�̃J�E���g�_�E���e�L�X�g")]
    [SerializeField] public Image fadePanel;
    [Tooltip("�J�n�O�̃J�E���g�_�E���e�L�X�g")]
    [SerializeField] public Text coundDownText;

    // Game�̐i�s��
    [Tooltip("�������J�n���ꂽ���ǂ������ǂ���")]
    [SerializeField] public bool isStart;
    [Tooltip("�������s�����ǂ���")]
    [SerializeField] public bool isGame;
    [Tooltip("Result�\�����ǂ���")]
    [SerializeField] public bool isResult;
    [Tooltip("�������������ǂ���")]
    [SerializeField] public bool isGameSet;

    //SE�֘A
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameStartSE;      //FIGHT�̃^�C�~���O�̉�

    // ���Ԍn
    [Tooltip("�����J�n�܂ł̑ҋ@����")]
    [SerializeField] private float waitTime; // �C���X�y�N�^�[���璲��

    void Start()
    {
        isStart = false;
        isGame = false;
        isResult = false;
        isGameSet = false;

        coundDownText.text = "3";
        coundDownText.CrossFadeAlpha(0.0f, 0.0f, true);
        fadePanel.CrossFadeAlpha(0.0f, 0.5f, true);
    }

    void Update()
    {
        // �����J�n�O�̏ꍇ
        if(isStart == false)
        {
            // �ҋ@���Ԃ�0.0f�����傫�������ꍇ�AwaitTime����o�߂������Ԃ������Ă��� (�ǂ̃^�C�~���O�ł�waitTime��0.0f�ȏ�̒l����������Ή��o���X�g�b�v����)
            if (waitTime > 0.0f)
            {
                waitTime -= Time.deltaTime;
                if(waitTime < 3.0f)
                {
                    coundDownText.CrossFadeAlpha(1.0f, 0.0f, true);
                    coundDownText.text = Mathf.Ceil(waitTime).ToString("0");
                }
                
                
            }
            else // �����J�n
            {
                coundDownText.text = "FIGHT!";
                coundDownText.CrossFadeAlpha(0.0f, 1.0f, true);

                //SE�iFIGHT�̃^�C�~���O�̉��j
                audioSource.clip = gameStartSE;
                audioSource.PlayOneShot(gameStartSE);
                isStart = true;
                isGame = true;
            }
        }
        else if (isGame == false) // �Q�[���I���� Result��\��������
        {
            isResult = true;
        }
    }
}
