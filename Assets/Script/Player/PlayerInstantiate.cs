using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine;

public class PlayerInstantiate : MonoBehaviour
{
    [SerializeField] private DataRetation dataRetation;
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;


    public GameObject[] characterPrefab;    //CharacterPrefab�i�[�z��
    public Transform[] playerSpawnPos;      //PlayerSpawn�ʒu�z��

    public List<PlayerInput> playerList= new List<PlayerInput>();  //�쐬�����v���C���[��List������

    private void Awake()
    {
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();    //DataRetation�擾
        cinemachineTargetGroup = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>(); //CinemachineTargetGroup�擾
    }

    private void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        for (int i = 0; i < 3/*�v���C���[��*/; i++)
        {
            InputDevice current = null;
            foreach (var gamepad in Gamepad.all)
            {
                if (gamepad.deviceId == dataRetation.controllerID[i])
                {
                    current = gamepad;
                    break;
                }
            }

            //characterNum[i]���ɂ���l�ɂ���ăL�����N�^�[���쐬����
            switch (dataRetation.characterNum[i])
            {
                //�N�}�m�~
                case 0:
                    //player[i] = Instantiate(characterPrefab[0], playerSpawnPos[i].position, Quaternion.identity);

                    PlayerInput input1 = PlayerInput.Instantiate(characterPrefab[0], -1, null, -1, current);    //����
                    input1.gameObject.transform.position = playerSpawnPos[i].position;  //�ʒu
                    input1.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); //��]
                    playerList.Add(input1); //List�ɒǉ�
                    AddTargetToGroup(input1.gameObject.transform, 1.0f, 10.0f);  //Target���X�g�ɒǉ�
                    break;
                //�T��
                case 1:
                    //player[i] = Instantiate(characterPrefab[1], playerSpawnPos[i].position, Quaternion.identity);

                    PlayerInput input2 = PlayerInput.Instantiate(characterPrefab[1], -1, null, -1, current);
                    input2.gameObject.transform.position = playerSpawnPos[i].position;
                    input2.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    playerList.Add(input2);
                    AddTargetToGroup(input2.gameObject.transform, 1.0f, 10.0f);
                    break;
                //�J��
                case 2:
                    //PlayerInput input3 = PlayerInput.Instantiate(characterPrefab[2], -1, null, -1, current);
                    //player[i] = Instantiate(characterPrefab[2], playerSpawnPos[i].position, Quaternion.identity);

                    PlayerInput input3 = PlayerInput.Instantiate(characterPrefab[2], -1, null, -1, current);    //����
                    input3.gameObject.transform.position = playerSpawnPos[i].position;  //�ʒu
                    input3.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); //��]
                    playerList.Add(input3); //List�ɒǉ�
                    AddTargetToGroup(input3.gameObject.transform, 1.0f, 10.0f);  //Target���X�g�ɒǉ�
                    break;
                //�}���^
                case 3:
                    //PlayerInput input4 = PlayerInput.Instantiate(characterPrefab[3], -1, null, -1, current);
                    //player[i] = Instantiate(characterPrefab[3], playerSpawnPos[i].position, Quaternion.identity);

                    PlayerInput input4 = PlayerInput.Instantiate(characterPrefab[2], -1, null, -1, current);    //����
                    input4.gameObject.transform.position = playerSpawnPos[i].position;  //�ʒu
                    input4.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); //��]
                    playerList.Add(input4); //List�ɒǉ�
                    AddTargetToGroup(input4.gameObject.transform, 1.0f, 5.0f);  //Target���X�g�ɒǉ�
                    break;
            }
        }
    }

    //�J�����ǐ՗p
    void AddTargetToGroup(Transform targetTransform, float weight, float radius)
    {
        //�^�[�Q�b�g��V�����쐬���A���X�g�ɒǉ�����
        CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
        target.target = targetTransform;    //�I�u�W�F�N�g
        target.weight = weight;             //��
        target.radius = radius;             //���a

        //���X�g�ɒǉ�
        cinemachineTargetGroup.AddMember(targetTransform, weight, radius);
    }
}
