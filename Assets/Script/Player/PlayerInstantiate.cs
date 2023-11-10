using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerInstantiate : MonoBehaviour
{
    private DataRetation dataRetation;

    public GameObject[] characterPrefab;    //CharacterPrefab�i�[�z��
    public GameObject[] player;             //�쐬�����v���C���[���i�[����z��
    public Transform[] playerSpawnPos;      //PlayerSpawn�ʒu�z��

    private void Awake()
    {
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();    //DataRetation�擾
        Array.Resize(ref player, 2);
    }

    private void Start()
    {

        CreatePlayer();
    }

    //�v���C���[�쐬
    private void CreatePlayer()
    {
        for (int i = 0; i < 2/*�v���C���[��*/; i++)
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
                    PlayerInput input1 = PlayerInput.Instantiate(characterPrefab[0], -1, null, -1, current);
                    input1.gameObject.transform.position = playerSpawnPos[i].position;
                    input1.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                //�T��
                case 1:
                    //player[i] = Instantiate(characterPrefab[1], playerSpawnPos[i].position, Quaternion.identity);
                    PlayerInput input2 = PlayerInput.Instantiate(characterPrefab[1], -1, null, -1, current);
                    input2.gameObject.transform.position = playerSpawnPos[i].position;
                    input2.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                //�J��
                case 2:
                    //PlayerInput input3 = PlayerInput.Instantiate(characterPrefab[2], -1, null, -1, current);
                    //player[i] = Instantiate(characterPrefab[2], playerSpawnPos[i].position, Quaternion.identity);
                    break;
                //�}���^
                case 3:
                    //PlayerInput input4 = PlayerInput.Instantiate(characterPrefab[3], -1, null, -1, current);
                    //player[i] = Instantiate(characterPrefab[3], playerSpawnPos[i].position, Quaternion.identity);
                    break;
            }
        }
    }
}
