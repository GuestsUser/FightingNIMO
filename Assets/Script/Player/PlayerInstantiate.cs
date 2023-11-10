using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerInstantiate : MonoBehaviour
{
    private DataRetation dataRetation;

    public GameObject[] characterPrefab;    //CharacterPrefab格納配列
    public GameObject[] player;             //作成したプレイヤーを格納する配列
    public Transform[] playerSpawnPos;      //PlayerSpawn位置配列

    private void Awake()
    {
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();    //DataRetation取得
        Array.Resize(ref player, 2);
    }

    private void Start()
    {

        CreatePlayer();
    }

    //プレイヤー作成
    private void CreatePlayer()
    {
        for (int i = 0; i < 2/*プレイヤー数*/; i++)
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

            //characterNum[i]内にある値によってキャラクターを作成する
            switch (dataRetation.characterNum[i])
            {
                //クマノミ
                case 0:
                    //player[i] = Instantiate(characterPrefab[0], playerSpawnPos[i].position, Quaternion.identity);
                    PlayerInput input1 = PlayerInput.Instantiate(characterPrefab[0], -1, null, -1, current);
                    input1.gameObject.transform.position = playerSpawnPos[i].position;
                    input1.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                //サメ
                case 1:
                    //player[i] = Instantiate(characterPrefab[1], playerSpawnPos[i].position, Quaternion.identity);
                    PlayerInput input2 = PlayerInput.Instantiate(characterPrefab[1], -1, null, -1, current);
                    input2.gameObject.transform.position = playerSpawnPos[i].position;
                    input2.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                //カメ
                case 2:
                    //PlayerInput input3 = PlayerInput.Instantiate(characterPrefab[2], -1, null, -1, current);
                    //player[i] = Instantiate(characterPrefab[2], playerSpawnPos[i].position, Quaternion.identity);
                    break;
                //マンタ
                case 3:
                    //PlayerInput input4 = PlayerInput.Instantiate(characterPrefab[3], -1, null, -1, current);
                    //player[i] = Instantiate(characterPrefab[3], playerSpawnPos[i].position, Quaternion.identity);
                    break;
            }
        }
    }
}
