using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private Result result;
    [SerializeField] private PlayerInstantiate playerIns;
    [SerializeField] private DataRetation dataRetation;
    [SerializeField] private UpdateTime updateTime;

    private void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        result = GameObject.Find("Result").GetComponent<Result>();
        playerIns = GameObject.Find("CreatePlayer").GetComponent<PlayerInstantiate>();
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();
        //updateTime = GameObject.Find("TimeLimit").GetComponent<UpdateTime>();
    }

    private void Update()
    {
        IsWin();
        IsDraw();
    }

    //��������
    private void IsWin()
    {
        //�܂��������Ԉȓ����A���������A�v���C���[�i�L�����N�^�[�ԍ��j���X�g��1�ȉ��̏ꍇ
        if (!updateTime.finished && gameState.isGame && playerIns.playerNum.Count <= 1)
        {
            for (int i = 0; i < dataRetation.characterNum.Length; i++)
            {
                if (dataRetation.characterNum[i] == playerIns.playerNum[0])
                {
                    result.winner = i;
                    gameState.isResult = true;
                    break;
                }
            }
        }
    }

    //������������
    private void IsDraw()
    {
        //�Q�[���I���̍��}�����ꂽ�����A�c��̃v���C���[��2�l�ȏ�c���Ă���ꍇ��
        if (updateTime.finished && playerIns.playerNum.Count >= 2)
        {
            //���������ɂ���
            result.winner = -1;
            gameState.isResult = true;
        }
    }
}
