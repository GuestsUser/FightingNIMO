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
    public List<int> playerNum = new List<int>();

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
        for (int i = 0; i < 2/*�v���C���[��*/; i++)
        {
            InputDevice current = null;
            foreach (var gamepad in Gamepad.all)
            {
                if (gamepad.deviceId == dataRetation.controllerID[i])
                {
                    //Debug.Log(gamepad.deviceId);
                    current = gamepad;
                    break;
                }
            }

            //characterNum[i]���ɂ���l�ɂ���ăL�����N�^�[���쐬����
            PlayerInput input1 = PlayerInput.Instantiate(characterPrefab[dataRetation.characterNum[i]], -1, null, -1, current);    //����
            input1.gameObject.transform.position = playerSpawnPos[i].position;  //�ʒu
            input1.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); //��]
            playerList.Add(input1); //List�ɒǉ�
            playerNum.Add(dataRetation.characterNum[i]);
            AddTargetToGroup(input1.gameObject.transform, 1.0f, 10.0f);  //�Ǐ]�J�������List�ɒǉ�

            //Debug.Log(input1.gameObject.GetComponent<TestPlayer>().pInput.devices[0].deviceId);
        }
    }

    //�Ǐ]�J�������ɒǉ�����֐�
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