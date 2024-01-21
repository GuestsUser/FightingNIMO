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
            for (int i = 0; i < 3/*�v���C���[��*/; i++)
            {
                switch (dataRetation.characterNum[i])
                {
                    //�N�}�m�~
                    case 0:
                        playerIns.playerList.RemoveAt(i);   //List����폜����
                        cinemachineTargetGroup.RemoveMember(other.transform);
                        //other.gameObject.SetActive(false);  //��A�N�e�B�u�ɂ���
                        break;
                    //�T��
                    case 1:
                        playerIns.playerList.RemoveAt(i);   //List����폜����
                        cinemachineTargetGroup.RemoveMember(other.transform);
                        //other.gameObject.SetActive(false);  //��A�N�e�B�u�ɂ���
                        break;
                    //�J��
                    case 2:
                        playerIns.playerList.RemoveAt(i);   //List����폜����
                        cinemachineTargetGroup.RemoveMember(other.transform);
                        //other.gameObject.SetActive(false);  //��A�N�e�B�u�ɂ���
                        break;
                    //�}���^
                    case 3:
                        playerIns.playerList.RemoveAt(i);   //List����폜����
                        //other.gameObject.SetActive(false);  //��A�N�e�B�u�ɂ���
                        break;
                }
            }
        }
    }
}
