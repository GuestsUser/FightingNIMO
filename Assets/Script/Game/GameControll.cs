using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControll : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private Result result;
    [SerializeField] private PlayerInstantiate playerIns;
    [SerializeField] private DataRetation dataRetation;
    [SerializeField] private UpdateTime updateTime;

    //SE�֘A
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip roundWinSE;      //1���E���h���������̉�


    private void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        result = GameObject.Find("Result").GetComponent<Result>();
        playerIns = GameObject.Find("CreatePlayer").GetComponent<PlayerInstantiate>();
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();
    }

    private void Update()
    {
        IsWin();
        IsDraw();
    }

    //��������
    private void IsWin()
    {
        //�������Ԉȓ����A�Ŗ@�̃v���C���[��1�l�����̏ꍇ
        if (!updateTime.finished && gameState.isGame && playerIns.playerNum.Count <= 1)
        {
            for (int i = 0; i < dataRetation.characterNum.Length; i++)
            {
                if (dataRetation.characterNum[i] == playerIns.playerNum[0])
                {
                    //SE�i1���E���h���������̉��j
                    audioSource.clip = roundWinSE;
                    audioSource.PlayOneShot(roundWinSE);

                    //iP�̏����ɂ���
                    result.winner = i;
                    gameState.isResult = true;
                    gameState.isGame = false;
                }
            }
        }
    }

    //������������
    private void IsDraw()
    {
        //�Q�[���I���̍��}�����ꂽ�����A�c��̃v���C���[��2�l�ȏ�c���Ă���ꍇ
        if (updateTime.finished && playerIns.playerNum.Count >= 2)
        {
            //���������ɂ���
            result.winner = -1;
            gameState.isResult = true;
        }
    }
}
