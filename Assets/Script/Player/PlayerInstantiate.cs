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
    public List<int> playerNum = new List<int>();

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
        for (int i = 0; i < 2/*プレイヤー数*/; i++)
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

            //characterNum[i]内にある値によってキャラクターを作成する
            PlayerInput input1 = PlayerInput.Instantiate(characterPrefab[dataRetation.characterNum[i]], -1, null, -1, current);    //生成
            input1.gameObject.transform.position = playerSpawnPos[i].position;  //位置
            input1.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); //回転
            playerList.Add(input1); //Listに追加
            playerNum.Add(dataRetation.characterNum[i]);
            AddTargetToGroup(input1.gameObject.transform, 1.0f, 10.0f);  //追従カメラ候補Listに追加

            //Debug.Log(input1.gameObject.GetComponent<TestPlayer>().pInput.devices[0].deviceId);
        }
    }

    //追従カメラ候補に追加する関数
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