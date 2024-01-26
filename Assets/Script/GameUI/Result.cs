using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private DataRetation dataRetation;

    // �\���E���W�֘A
    [Tooltip("Player����Score��UI�����Ă�������")]
    [SerializeField] private GameObject[] playerScore;
    [Tooltip("������Particle��ǉ�")]
    [SerializeField] private GameObject particle;
    [Tooltip("ScorePanel��UI�����Ă�������")]
    [SerializeField] private RectTransform scorePanel; // �C���X�y�N�^�[������
    [Tooltip("ScorePanel�̏����ʒu���w�肵�Ă�������")]
    [SerializeField] private Vector2 initPos;          // �����n�AscorePanel����ŏ��Ɏ擾
    [Tooltip("ScorePanel�̖ڕW�n�_���w�肵�Ă�������")]
    [SerializeField] private Vector2 targetPos;        // �����������ڕW�n�_
    [Tooltip("�L�����N�^�[�A�C�R�������Ă�������")]
    [SerializeField] private Sprite[] icon;            // �L�����N�^�[�A�C�R��
    [SerializeField] private Image fadePanel;
    [SerializeField] private Text win;

    //SE�֘A
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip scoreUpSE;       //�������Ƃ���SE
    [SerializeField] private AudioClip allScoreSE;      //3�����l�����ď��������Ƃ���SE
    [SerializeField] private AudioClip resultHideSE;    //���U���g�p�l�����B���Ƃ���SE
    [SerializeField] private AudioClip resultRevealSE;  //���U���g�p�l�����o���Ƃ���SE
    private int seNum;  //���������̏ꏊ�ŕʂ�Se���^�C�~���O�����炵�Ė炷���߂̕ϐ�

    // �X�R�A�֘A
    [SerializeField] private Transform[] point; // ��UI������
    [SerializeField] static private int[] score; // ���_
    [SerializeField] private int playerNum; // �ڑ��l��
    [SerializeField] public int winner; // ����

    // �Q�[���̐i�s�󋵌n�E����֘A
    [Tooltip("Score�����Z�������ǂ���")]
    [SerializeField] private bool addScore;
    [Tooltip("Result���\�����ꂽ���ǂ���")]
    [SerializeField] private bool show;
    [Tooltip("Result�����o�I���������ǂ���")]
    [SerializeField] private bool isFinishing;

    // ���Ԍn
    [Tooltip("UI�̉��o������܂ł̑ҋ@����")]
    [SerializeField] private float waitTime; // �C���X�y�N�^�[���璲��
    [Tooltip("���_��\�����Ă�����������")]
    [SerializeField] private float showTime; // �C���X�y�N�^�[���璲��
    [Tooltip("easing�̉��o�̏��v����")]
    [SerializeField] private float duration; // �C���X�y�N�^�[���璲��
    [Tooltip("easing�̌o�ߎ���")]
    [SerializeField] private float easTime;
    [Tooltip("�ݒ肵��sin�g�̒����ɑ΂��Č��݂ǂ��܂ŗ��Ă��邩")]
    [SerializeField] private float sinRate;  // �m�F�p
    [Tooltip("Scene�J�ڂ���܂ł̎���")]
    [SerializeField] private float sceneTime; 

    [Tooltip("score���Z����easing�̌o�ߎ���")]
    [SerializeField] private float scoreEasTime;
    [Tooltip("easing�̉��o�̏��v����")]
    [SerializeField] private float scoreDuration; // �C���X�y�N�^�[���璲��

    void Start()
    {
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();

        //initPos = scorePanel.anchoredPosition;   // �����i�K�Ŏg�p
        scorePanel.anchoredPosition = initPos;     // �������I�������t��initPos�ɂ͐�ɃC���X�y�N�^�[���������Ă����A�����ʒu���Œ�

        particle = GameObject.FindWithTag("Particle");
        particle.SetActive(false);

        easTime = 0.0f;
        addScore = false;
        show = false;
        isFinishing = false;

        seNum = 0;

        /*�y���̏����z*/
        playerNum = Gamepad.all.Count;      �@�@�@�@�@// �ڑ��l���̊m�F

        // �l������scoreUI���A�N�e�B�u��
        for (int i = 0; i < playerNum; i++)
        {
            if (dataRetation.characterNum[i] != -1)
            {
                //Debug.Log($"score_{i}��L�������܂���");
                playerScore[i].SetActive(true);    // i����1P�A3P���Ό����Ă����ꍇ�ł� 1P�A2P���\������Ă��܂����߁A����Ă����v���C���[�ԍ��̎擾���K�{
                playerScore[i].transform.GetChild(1).GetComponent<Image>().sprite = icon[dataRetation.characterNum[i]];
            }

            Array.Resize(ref score, 4);     //menuItems�̐��ɂ����items�̑傫����ύX����

            // �����A�N�e�B�u��
            point = playerScore[i].transform.GetChild(0).transform.GetComponentsInChildren<Transform>(true);
            if(score[i] > 0)
            {
                for (int j = 0; j < score[i]; j++)
                {
                    point[j+1].gameObject.SetActive(true);
                }
            }
        }
        win.text = "";
        win.CrossFadeAlpha(0.0f, 0.0f, true);
        //fadePanel.CrossFadeAlpha(0.0f, 0.0f, true);
    }

    void Update()
    {
        if(isFinishing == false)
        {
            SlideInAnim();
            ScoreAnim();
        }
        else
        {
            //Debug.Log("Scene�J��");
            fadePanel.CrossFadeAlpha(1.0f, sceneTime,true);
        }   
    }

    //�X�R�A�\���o�Ă��鎞�̃A�j���[�V����
    void SlideInAnim()
    {
        // ���U���g�̕\���������ꂽ�Ȃ�
        if (gameState.isResult == true)
        {
            if(addScore == false)
            {
                addScore = true;
                if (++score[winner] == 3) {
                    gameState.isGameSet = true; //�Q�[���̌�������
                    win.text = $"{winner + 1}P WIN!";   //���������v���C���[�̃e�L�X�g��\������
                    win.CrossFadeAlpha(1.0f, 1.0f, true);

                    //SE�i3�����l�����ď��������Ƃ��̉��j
                    audioSource.clip = allScoreSE;
                    audioSource.PlayOneShot(allScoreSE);

                    particle.SetActive(true);   //�������̎�����p�[�e�B�N����\������
                }
                else
                {
                    win.text = "";
                }
            }
            
            // �ҋ@���Ԃ�0.0f�����傫�������ꍇ�AwaitTime����o�߂������Ԃ������Ă��� (�ǂ̃^�C�~���O�ł�waitTime��0.0f�ȏ�̒l����������Ή��o���X�g�b�v����)
            if (waitTime > 0.0f)
            {
                waitTime -= Time.deltaTime;
                
            }
            else
            {
                // �ݒ肵�����v���Ԃɑ΂���easTime���������ꍇeasTime�����Z
                if (easTime / 60.0f < duration) // 60.0��fps
                {
                    easTime++;

                    //���U���g�p�l���̏o��Ƃ��A�߂�Ƃ���SE����
                    if (seNum == 0)
                    {
                        seNum = 1;
                        //SE�i���U���g�p�l�����B���Ƃ��j
                        audioSource.clip = resultHideSE;
                        audioSource.PlayOneShot(resultHideSE);
                    }
                    else if(seNum == 1 && show == true)
                    {
                        seNum = 2;
                        //SE�i���U���g�p�l�����o���Ƃ���SE�j
                        audioSource.clip = resultRevealSE;
                        audioSource.PlayOneShot(resultRevealSE);
                    }
                }
                else // easTime��duration�ȏ�ɂȂ����ꍇ
                {
                    easTime = duration * 60.0f; // �Œ�
                    isFinishing = true;

                    // ���������Ă��Ȃ���Ύ��̃��E���h
                    if(gameState.isGameSet == false)
                    {
                        //dataRetation.
                        StartCoroutine("NextRound");
                    }
                    else
                    {
                        StartCoroutine("BackMenu");
                    }  
                }

                // sin�g��0.5f�̒n�_�𒴂��� ���� �܂�Result��\�������Ă��Ȃ�������  ��sin�g�̐���(sin�g��0.0f����X�^�[�g����1.0f�ɖ߂��Ă���܂ł̒�����1.0f�Ƃ��Ă���)
                if (sinRate >= 0.5f && show == false)
                {
                    show = true;
                    waitTime = showTime;    //�p�l���������Ă��鎞��

                    //SE�i�������Ƃ��j
                    audioSource.clip = scoreUpSE;
                    audioSource.PlayOneShot(scoreUpSE);
                }
                scorePanel.anchoredPosition = new Vector2(initPos.x + (Vector2.Distance(initPos, targetPos) * easing(duration, easTime, 1.0f)), initPos.y);
            }
        }
    }

    //�X�R�A���Z�A�j���[�V����
    void ScoreAnim()
    {
        float scale = 1.0f;
        if (show == true)
        {
            //Debug.Log("Result�\����");
            //Debug.Log($"Winner:{winner+1}p");
            if (winner > -1) // ���҂������ꍇ (1p��0,2p��1�Ɖ��肵�� -1�͈���������(�^�C���A�b�v))
            {
                point = playerScore[winner].transform.GetChild(0).transform.GetComponentsInChildren<Transform>(true);
                point[score[winner]].gameObject.SetActive(true);

                // �ݒ肵�����v���Ԃɑ΂���easTime���������ꍇeasTime�����Z
                if (scoreEasTime / 60.0f < scoreDuration) // 60.0��fps
                {
                    scoreEasTime++;
                }
                else // easTime��duration�ȏ�ɂȂ����ꍇ
                {
                    scoreEasTime = scoreDuration * 60.0f; // �Œ�
                }

                // scale�̊g��k���A�j���[�V����
                scale = 1.0f + easing(scoreDuration, scoreEasTime, 1.0f);
                point[score[winner]].gameObject.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
            }
        }
        
    }

    //���̃��E���h�̂��߂̃R���[�`��
    private IEnumerator NextRound()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        SceneManager.LoadScene("GameScene");
    }

    //�^�C�g���ɖ߂邽�߂̃R���[�`��
    private IEnumerator BackMenu()
    {
        yield return new WaitForSecondsRealtime(3);
        for(int i = 0; i < 4; i++)
        {
            score[i] = 0;
        }
        SceneManager.LoadScene("TitleScene");
    }

    //�A�j���[�V�����̎��̊ɋ}��t���邽�߂̊֐�
    float easing(float duration, float time, float length)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easing�̐i�s�󋵂������l���Z�o
        sinRate = t * length;

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero);
    }
}