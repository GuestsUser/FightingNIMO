using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine;

public class PlayerInstantiate : MonoBehaviour
{
    [SerializeField] private DataRetation dataRetation;
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;


    public GameObject[] characterPrefab;    //CharacterPrefab格納配列
    public Transform[] playerSpawnPos;      //PlayerSpawn位置配列

    public List<PlayerInput> playerList= new List<PlayerInput>();  //作成したプレイヤーをList化する

    private void Awake()
    {
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();    //DataRetation取得
        cinemachineTargetGroup = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>(); //CinemachineTargetGroup取得
    }

    private void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        for (int i = 0; i < 3/*プレイヤー数*/; i++)
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

                    PlayerInput input1 = PlayerInput.Instantiate(characterPrefab[0], -1, null, -1, current);    //生成
                    input1.gameObject.transform.position = playerSpawnPos[i].position;  //位置
                    input1.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); //回転
                    playerList.Add(input1); //Listに追加
                    AddTargetToGroup(input1.gameObject.transform, 1.0f, 10.0f);  //Targetリストに追加
                    break;
                //サメ
                case 1:
                    //player[i] = Instantiate(characterPrefab[1], playerSpawnPos[i].position, Quaternion.identity);

                    PlayerInput input2 = PlayerInput.Instantiate(characterPrefab[1], -1, null, -1, current);
                    input2.gameObject.transform.position = playerSpawnPos[i].position;
                    input2.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    playerList.Add(input2);
                    AddTargetToGroup(input2.gameObject.transform, 1.0f, 10.0f);
                    break;
                //カメ
                case 2:
                    //PlayerInput input3 = PlayerInput.Instantiate(characterPrefab[2], -1, null, -1, current);
                    //player[i] = Instantiate(characterPrefab[2], playerSpawnPos[i].position, Quaternion.identity);

                    PlayerInput input3 = PlayerInput.Instantiate(characterPrefab[2], -1, null, -1, current);    //生成
                    input3.gameObject.transform.position = playerSpawnPos[i].position;  //位置
                    input3.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); //回転
                    playerList.Add(input3); //Listに追加
                    AddTargetToGroup(input3.gameObject.transform, 1.0f, 10.0f);  //Targetリストに追加
                    break;
                //マンタ
                case 3:
                    //PlayerInput input4 = PlayerInput.Instantiate(characterPrefab[3], -1, null, -1, current);
                    //player[i] = Instantiate(characterPrefab[3], playerSpawnPos[i].position, Quaternion.identity);

                    PlayerInput input4 = PlayerInput.Instantiate(characterPrefab[2], -1, null, -1, current);    //生成
                    input4.gameObject.transform.position = playerSpawnPos[i].position;  //位置
                    input4.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); //回転
                    playerList.Add(input4); //Listに追加
                    AddTargetToGroup(input4.gameObject.transform, 1.0f, 5.0f);  //Targetリストに追加
                    break;
            }
        }
    }

    //カメラ追跡用
    void AddTargetToGroup(Transform targetTransform, float weight, float radius)
    {
        //ターゲットを新しく作成し、リストに追加する
        CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
        target.target = targetTransform;    //オブジェクト
        target.weight = weight;             //幅
        target.radius = radius;             //半径

        //リストに追加
        cinemachineTargetGroup.AddMember(targetTransform, weight, radius);
    }
}
