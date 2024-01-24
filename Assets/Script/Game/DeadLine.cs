using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    [SerializeField] private PlayerInstantiate playerIns;
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

    private void Start()
    {
        playerIns = GameObject.Find("CreatePlayer").GetComponent<PlayerInstantiate>();  //PlayerInstantiate�擾
        cinemachineTargetGroup = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>(); //CinemachineTargetGroup�擾

    }

    //�����蔻��iTrigger�j
    private void OnTriggerEnter(Collider other)
    {
        //�uPlayer�v�^�O�����R���W�����ƐڐG�����ꍇ(����)
        if (other.CompareTag("Player"))
        {
            TestPlayer player = other.GetComponent<TestPlayer>();

            //�c��̃v���C���[��1��葽���ꍇ�́i1�l�̎��̓��X�g����폜���Ȃ��j
            if (playerIns.playerNum.Count > 1)
            {
                playerIns.playerNum.Remove(player.playerNumber);  //���񂾃v���C���[�i�L�����N�^�[�ԍ��j���폜����
            }

            player.isDead = true; //���S������
            cinemachineTargetGroup.RemoveMember(other.transform);   //�Ǐ]�J������List����폜����
        }
    }
}
