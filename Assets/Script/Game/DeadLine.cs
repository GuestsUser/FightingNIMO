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
        playerIns = GameObject.Find("CreatePlayer").GetComponent<PlayerInstantiate>();  //PlayerInstantiate取得
        cinemachineTargetGroup = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>(); //CinemachineTargetGroup取得

    }

    //当たり判定（Trigger）
    private void OnTriggerEnter(Collider other)
    {
        //「Player」タグを持つコリジョンと接触した場合(落下)
        if (other.CompareTag("Player"))
        {
            TestPlayer player = other.GetComponent<TestPlayer>();

            //残りのプレイヤーが1より多い場合は（1人の時はリストから削除しない）
            if (playerIns.playerNum.Count > 1)
            {
                playerIns.playerNum.Remove(player.playerNumber);  //死んだプレイヤー（キャラクター番号）を削除する
            }

            player.isDead = true; //死亡させる
            cinemachineTargetGroup.RemoveMember(other.transform);   //追従カメラのListから削除する
        }
    }
}
