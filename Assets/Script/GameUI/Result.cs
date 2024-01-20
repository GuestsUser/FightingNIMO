using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private DataRetation dataRetation;

    // 表示・座標関連
    [Tooltip("Player毎のScoreのUIを入れてください")]
    [SerializeField] private GameObject[] playerScore;
    [Tooltip("ScorePanelのUIを入れてください")]
    [SerializeField] private RectTransform scorePanel; // インスペクターから代入
    [Tooltip("ScorePanelの初期位置を指定してください")]
    [SerializeField] private Vector2 initPos;          // 初期地、scorePanelから最初に取得
    [Tooltip("ScorePanelの目標地点を指定してください")]
    [SerializeField] private Vector2 targetPos;        // 動かしたい目標地点
    [Tooltip("キャラクターアイコンを入れてください")]
    [SerializeField] private Sprite[] icon;            // キャラクターアイコン
    [SerializeField] private Image fadePanel;

    // スコア関連
    [SerializeField] private Transform[] point; // ★UIが入る
    [SerializeField] static private int[] score; // 得点
    [SerializeField] private int playerNum; // 接続人数
    [SerializeField] private int winner; // 勝者



    // ゲームの進行状況系・制御関連
    [Tooltip("Scoreを加算したかどうか")]
    [SerializeField] private bool addScore;
    [Tooltip("Resultが表示されたかどうか")]
    [SerializeField] private bool show;
    [Tooltip("Resultが演出終了したかどうか")]
    [SerializeField] private bool isFinishing;

    // 時間系
    [Tooltip("UIの演出が入るまでの待機時間")]
    [SerializeField] private float waitTime; // インスペクターから調整
    [Tooltip("得点を表示しておきたい時間")]
    [SerializeField] private float showTime; // インスペクターから調整
    [Tooltip("easingの演出の所要時間")]
    [SerializeField] private float duration; // インスペクターから調整
    [Tooltip("easingの経過時間")]
    [SerializeField] private float easTime;
    [Tooltip("設定したsin波の長さに対して現在どこまで来ているか")]
    [SerializeField] private float sinRate;  // 確認用
    [Tooltip("Scene遷移するまでの時間")]
    [SerializeField] private float sceneTime; 

    [Tooltip("score加算時のeasingの経過時間")]
    [SerializeField] private float scoreEasTime;
    [Tooltip("easingの演出の所要時間")]
    [SerializeField] private float scoreDuration; // インスペクターから調整

    // Start is called before the first frame update
    void Start()
    {
        dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();

        //initPos = scorePanel.anchoredPosition;   // 調整段階で使用
        scorePanel.anchoredPosition = initPos; // 調整し終わったら逆にinitPosには先にインスペクターから代入しておき、初期位置を固定
        easTime = 0.0f;
        addScore = false;
        show = false;
        isFinishing = false;

        /*【仮の処理】*/
        playerNum = Gamepad.all.Count;      // 接続人数の確認

        // 人数分のscoreUIをアクティブに
        for (int i = 0; i < playerNum; i++)
        {
            if (dataRetation.characterNum[i] != -1)
            {
                //Debug.Log($"score_{i}を有効化しました");
                playerScore[i].SetActive(true);    // iだと1P、3Pが対決していた場合でも 1P、2Pが表示されてしまうため、戦っていたプレイヤー番号の取得が必須
                playerScore[i].transform.GetChild(1).GetComponent<Image>().sprite = icon[dataRetation.characterNum[i]];
            }

            Array.Resize(ref score, 4);     //menuItemsの数によってitemsの大きさを変更する

            // ★をアクティブ化
            point = playerScore[i].transform.GetChild(0).transform.GetComponentsInChildren<Transform>(true);
            if(score[i] > 0)
            {
                for (int j = 0; j < score[i]; j++)
                {
                    point[j+1].gameObject.SetActive(true);
                }
            }
            
            
            
        }
        
        //fadePanel.CrossFadeAlpha(0.0f, 0.0f, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(isFinishing == false)
        {
            SlideInAnim();
            ScoreAnim();
        }
        else
        {
            Debug.Log("Scene遷移");
            fadePanel.CrossFadeAlpha(1.0f, sceneTime,true);
        }
        
    }

    void SlideInAnim()
    {
        // リザルトの表示が許可されたなら
        if (gameState.isResult == true)
        {
            if(addScore == false)
            {
                addScore = true;
                if (++score[winner] == 3) gameState.isGameSet = true;
            }
            

            // 待機時間が0.0fよりも大きかった場合、waitTimeから経過した時間を引いていく (どのタイミングでもwaitTimeに0.0f以上の値が代入されれば演出がストップする)
            if (waitTime > 0.0f)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                // 設定した所要時間に対してeasTimeが小さい場合easTimeを加算
                if (easTime / 60.0f < duration) // 60.0はfps
                {
                    easTime++;
                }
                else // easTimeがduration以上になった場合
                {
                    easTime = duration * 60.0f; // 固定
                    isFinishing = true;

                    // 決着がついていなければ次のラウンド
                    if(gameState.isGameSet == false)
                    {
                        StartCoroutine("NextRound");
                    }
                    else
                    {
                        StartCoroutine("BackMenu");
                    }
                    
                }

                // sin波が0.5fの地点を超えた かつ まだResultを表示させていなかったら  ※sin波の説明(sin波が0.0fからスタートして1.0fに戻ってくるまでの長さを1.0fとしている)
                if (sinRate >= 0.5f && show == false)
                {
                    show = true;
                    waitTime = showTime;
                    
                }

                scorePanel.anchoredPosition = new Vector2(initPos.x + (Vector2.Distance(initPos, targetPos) * easing(duration, easTime, 1.0f)), initPos.y);
            }
        }
    }

    void ScoreAnim()
    {
        float scale = 1.0f;
        if (show == true)
        {
            //Debug.Log("Result表示中");
            //Debug.Log($"Winner:{winner+1}p");
            if (winner > -1) // 勝者がいた場合 (1pが0,2pが1と仮定して -1は引き分け時(タイムアップ))
            {
                point = playerScore[winner].transform.GetChild(0).transform.GetComponentsInChildren<Transform>(true);
                //Debug.Log(point[0].name);
                point[score[winner]].gameObject.SetActive(true);

                // 設定した所要時間に対してeasTimeが小さい場合easTimeを加算
                if (scoreEasTime / 60.0f < scoreDuration) // 60.0はfps
                {
                    scoreEasTime++;
                }
                else // easTimeがduration以上になった場合
                {
                    scoreEasTime = scoreDuration * 60.0f; // 固定
                }

                

                // scaleの拡大縮小アニメーション
                scale = 1.0f + easing(scoreDuration, scoreEasTime, 1.0f);
                point[score[winner]].gameObject.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
            }
        }
        
    }

    private IEnumerator NextRound()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        SceneManager.LoadScene("GameScene");
    }
    private IEnumerator BackMenu()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene("TitleScene");
    }

    float easing(float duration, float time, float length)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easingの進行状況を示す値を算出
        sinRate = t * length;

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero);
    }
}