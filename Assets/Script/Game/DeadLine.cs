using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    [SerializeField] PlayerInstantiate playerIns;
    [SerializeField] DataRetation dataRetation;
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

    private void Start()
    {
        playerIns = GameObject.Find("CreatePlayer").GetComponent<PlayerInstantiate>();  //PlayerInstantiate�擾
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();    //DataRetation�擾
        cinemachineTargetGroup = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>(); //CinemachineTargetGroup�擾

    }

    //�����蔻��iTrigger�j
    private void OnTriggerEnter(Collider other)
    {
        //�uPlayer�v�^�O�����R���W�����ƐڐG�����ꍇ(����)
        if (other.CompareTag("Player"))
        {
            playerIns.playerNum.Remove(other.GetComponent<TestPlayer>().playerNumber);
            cinemachineTargetGroup.RemoveMember(other.transform);
            //for (int i = 0; i < 4/*�v���C���[��*/; i++)
            //{

            //    switch (other.GetComponent<TestPlayer>().playerNumber)
            //    {
            //        //�N�}�m�~
            //        case 0:
            //            playerIns.playerNum.RemoveAt(i);    //List����폜����
            //            cinemachineTargetGroup.RemoveMember(other.transform);
            //            break;
            //        //�T��
            //        case 1:
            //            playerIns.playerNum.RemoveAt(i);    //List����폜����
            //            cinemachineTargetGroup.RemoveMember(other.transform);
            //            break;
            //        //�J��
            //        case 2:
            //            playerIns.playerNum.RemoveAt(i);    //List����폜����
            //            cinemachineTargetGroup.RemoveMember(other.transform);
            //            break;
            //        //�}���^
            //        case 3:
            //            playerIns.playerNum.RemoveAt(i);    //List����폜����
            //            cinemachineTargetGroup.RemoveMember(other.transform);
            //            break;
            //    }
            //}
            //foreach(var itr in playerIns.playerNum)
            //{

            //}
        }
    }
}
