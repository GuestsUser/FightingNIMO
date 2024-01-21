using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    [SerializeField] PlayerInstantiate playerIns;
    [SerializeField] DataRetation dataRetation;

    private void Start()
    {
        playerIns = GameObject.Find("CreatePlayer").GetComponent<PlayerInstantiate>();  //PlayerInstantiate取得
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();    //DataRetation取得
    }

    //当たり判定（Trigger）
    private void OnTriggerEnter(Collider other)
    {
        //「Player」タグを持つコリジョンと接触した場合(落下)
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < 3/*プレイヤー数*/; i++)
            {
                switch (dataRetation.characterNum[i])
                {
                    //クマノミ
                    case 0:
                        playerIns.playerList.RemoveAt(i);   //Listから削除する
                        other.gameObject.SetActive(false);  //非アクティブにする
                        break;
                    //サメ
                    case 1:
                        playerIns.playerList.RemoveAt(i);   //Listから削除する
                        other.gameObject.SetActive(false);  //非アクティブにする
                        break;
                    //カメ
                    case 2:
                        playerIns.playerList.RemoveAt(i);   //Listから削除する
                        other.gameObject.SetActive(false);  //非アクティブにする
                        break;
                    //マンタ
                    case 3:
                        playerIns.playerList.RemoveAt(i);   //Listから削除する
                        other.gameObject.SetActive(false);  //非アクティブにする
                        break;
                }
            }
        }
    }
}
