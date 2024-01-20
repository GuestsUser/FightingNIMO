using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class CharSelectManager : MonoBehaviour
{
	[Space(10)]
	[Header("---------------------------------------------")]
	[Header("事前にオブジェクトを取得しておくもの")]
	[Tooltip("セレクト用のキャラクターを入れてください")]
	[SerializeField] private List<GameObject> characters;
	[Tooltip("プレイヤーカーソルのRectTransformの情報")]
	[SerializeField] private RectTransform cursorRT;
	[Tooltip("このプレイヤーのカーソルテキストを入れてください")]
	[SerializeField] private GameObject cursor;
	[Tooltip("このプレイヤーのカーソルテキストを入れてください（RT取得用）")]
	[SerializeField] private RectTransform cursorImage; // 変更箇所 cursorText → cursorImage
	[Tooltip("プレイヤー番号UI用のテキストを入れてください")]
	[SerializeField] private Text playerNumText;
	[Tooltip("自身のPlayerInputを取得(自動取得)")]
	[SerializeField] private PlayerInput input;
	[Tooltip("選べるキャラクターUI(自動取得)")]
	[SerializeField] private GameObject[] characterUI;  //自動的に入れられるようになっている
	[Tooltip("各キャラクターのmoving内にあるTestCharacterを入れてください")]
	[SerializeField] private SkinnedMeshRenderer[] Smr; //表示の際に、キャラクターが倒れる現象を直すためにSkinnedMeshRendererのアクティブで調整している

	/*Script関連取得*/
	[Tooltip("GameStartSystem.csを持っているオブジェクトの自動取得")]
	[SerializeField] private GameStartSystem gameStartSys;
	[Tooltip("ReceiveNotificationExample.csを持ったオブジェクトの自動取得")]
	[SerializeField] private ReceiveNotificationExample receiveNotificationExample;
	[Tooltip("DataRetation.csを持ったオブジェクトの自動取得")]
	[SerializeField] private DataRetation dataRetation;

	[Header("---------------------------------------------")]
	[Space(20)]

	[Tooltip("現在のキャラクター数（クマノミ、サメ、カメ、マンタ）")]
	[SerializeField] private int maxCharacter;
	[Tooltip("カーソル移動時の時間間隔")]
	[SerializeField] private float interval;
	[Tooltip("現在選択されているキャラクター番号")]
	[SerializeField] private int characterNum;
	[Tooltip("UIのアイコンとの距離 0がx、1がy")]
	[SerializeField] private float[] offset = { 60, 40 };
	[Tooltip("ボタン入力継続時間")]
	[SerializeField] private float count;


	[Tooltip("選択されているキャラクターを選んだかどうか")]
	[SerializeField] public bool isCharSelected;    //キャラクターを選択したらチェックが付く
	[Tooltip("左スティックのX軸を入力しているか")]
	[SerializeField] private bool push;

	[SerializeField] private bool isLeftPush;    //左十字キーが押されているかどうか
	[SerializeField] private bool isRightPush;   //右十字キーが押されているかどうか

	private bool getCharacter;  //1度だけ反応させるためのもの
	private bool isTrigger;     //ゲームパッドが切断されたかどうか

	

	private void Awake()
	{
		//------[自動取得][変更][初期化]------
        #region
        input = this.GetComponent<PlayerInput>();			//このスクリプトを持つオブジェクトのPlayerInputを取得
		cursorRT = cursor.GetComponent<RectTransform>();    //プレイヤーカーソルのRectTransformを取得

		gameStartSys = GameObject.Find("GameStartSystem").GetComponent<GameStartSystem>();
		dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();
		receiveNotificationExample = GameObject.Find("PlayerInputManager").GetComponent<ReceiveNotificationExample>();

		Array.Resize(ref characterUI, maxCharacter);    //characterUIの配列のサイズをキャラクター総数に変更する

		transform.SetSiblingIndex(transform.GetSiblingIndex() - 1); //何のために位置の入れ替えをしているのか

		if (interval == 0) { interval = 15; }               //カーソル移動時の時間間隔を15に設定
		#endregion
		//------------------------------------
	}

    private void Start()
	{
		//このプレイヤーがスポーンされたとき配列に格納する関数（遷移した時に使用するため）
		AddPlayer();

		//1P～4Pカーソルの初期位置設定
		switch (input.playerIndex)
		{
			//1P
			case 0:
				characterNum = 0;   //プレイヤーの最初の初期キャラクター設定
				/* 追加・変更箇所 */
				cursorImage.anchoredPosition = new Vector2(-(offset[0]), (offset[1]));
				playerNumText.text = "<color=#ff6363>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorTextをP1/P2/P3/P4に設定
				/* ---------- */
				break;
			//2P
			case 1:
				characterNum = 1;
				/* 追加・変更箇所 */
				cursorImage.anchoredPosition = new Vector2((offset[0]), (offset[1]));
				playerNumText.text = "<color=#33b0ff>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorTextをP1/P2/P3/P4に設定
				/* ---------- */
				break;
			//3P
			case 2:
				characterNum = 2;
				/* 追加・変更箇所 */
				cursorImage.anchoredPosition = new Vector2(-(offset[0]), -(offset[1]));
				playerNumText.text = "<color=#f4f54b>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorTextをP1/P2/P3/P4に設定
				/* ---------- */
				break;
			//4P
			case 3:
				characterNum = 3;
				/* 追加・変更箇所 */
				cursorImage.anchoredPosition = new Vector2((offset[0]), -(offset[1]));
				playerNumText.text = "<color=#4cf54b>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorTextをP1/P2/P3/P4に設定
				/* ---------- */
				break;
		}

		//このオブジェクトの名前をPlayer1～4に変更する
		this.gameObject.name = "Player" + (receiveNotificationExample.playerNum - 1);
		
		cursor.SetActive(false);	//プレイヤーカーソルの非表示
		getCharacter = false;		//初期化
		isCharSelected = false;     //初期化
		isTrigger = false;          //初期化
		isLeftPush = false;         //初期化
		isRightPush = false;        //初期化
	}

	private void Update()
	{
		//1P～4Pカーソルの位置デバッグ用
		//switch (input.playerIndex)
		//{
		//	//1P
		//	case 0:
		//		cursorImage.anchoredPosition = new Vector2(-(offset[0]), (offset[1]));
		//		break;
		//	//2P
		//	case 1:
		//		cursorImage.anchoredPosition = new Vector2((offset[0]), (offset[1]));
		//		break;
		//	//3P
		//	case 2:
		//		cursorImage.anchoredPosition = new Vector2(-(offset[0]), -(offset[1]));
		//		break;
		//	//4P
		//	case 3:
		//		cursorImage.anchoredPosition = new Vector2((offset[0]), -(offset[1]));
		//		break;
		//}

		//AddCharacterNum();      //キャラクター番号保持関数
		RemovePlayer();         //プレイヤー削除関数
		CharacterVisibility();  //キャラクター（表示/非表示）関数
		LongPress();            //長押し入力関数

		//現在がキャラクターセレクトできる状態なら
		if (gameStartSys.isCharSelect == true)
		{
			//playerNumText.GetComponent<Text>().CrossFadeAlpha(1, 0f, true);
			cursor.GetComponent<CanvasGroup>().alpha = 1;
			if (getCharacter == false)
			{
				//characterUIの取得とカーソルの表示
				getCharacter = true;
				characterUI[0] = GameObject.Find("BaseButton");
				characterUI[1] = GameObject.Find("SharkButton");
				characterUI[2] = GameObject.Find("TurtleButton");
				//characterUI[3] = GameObject.Find("MantaButton");
				cursor.SetActive(true);
			}

			if (isCharSelected == false)
			{
				int currentCharNum = characterNum;

				while (LoopArray((currentCharNum) % maxCharacter))
				{
					++currentCharNum;
				}

				characterNum = currentCharNum % maxCharacter;
			}

			//キャラクターの総数分ループ
			for (int i = 0; i < maxCharacter; i++)
			{
				if (characterNum == i)
				{
					// カーソル位置の変更
					cursorRT.position = characterUI[i].GetComponent<RectTransform>().position;
				}
			}
		}
		else
		{
			//playerNumText.GetComponent<Text>().CrossFadeAlpha(0, 0f, true);
			cursor.GetComponent<CanvasGroup>().alpha = 0;

			//リセットすることで戻った時に移動するバグをさせない
			isRightPush = false;
			isLeftPush = false;
			push = false;
		}
	}

	//プレイヤー追加関数
	private void AddPlayer()
	{
		for (int i = 0; i < dataRetation.playerList.Length; i++)
		{
			if (dataRetation.playerList[i] == null)
			{
				dataRetation.controllerID[i] = input.devices[0].deviceId;   //デバイスIDを格納する
				dataRetation.playerList[i] = this.gameObject;   //このゲームオブジェクトを格納する
				break;
			}
		}
	}

	//プレイヤー削除関数
	private void RemovePlayer()
    {
        if (isTrigger)
        {
			RemoveCharacterNum();
			Destroy(this.gameObject);
			InputSystem.FlushDisconnectedDevices();
		}
    }

	//キャラクター（表示/非表示）関数
	private void CharacterVisibility()
    {
		switch (characterNum)
		{
			//クマノミ
			case 0:
				Smr[characterNum].enabled = true;	//クマノミを表示

				//キャラクターの総数分ループし、現在選択されているキャラクター番号以外のキャラクターを非表示にする
				for (int i = 0; i < maxCharacter; i++)
				{
					if (i != characterNum)
					{
						Smr[i].enabled = false;
					}
				}
				break;
			//サメ
			case 1:
				Smr[characterNum].enabled = true;    //サメを表示
				for (int i = 0; i < maxCharacter; i++)
				{
					if (i != characterNum)
					{
						Smr[i].enabled = false;		//サメを非表示

					}
				}
				break;
                //カメ

            case 2:  
				Smr[characterNum].enabled = true;   //カメを表示
				for (int i = 0; i < maxCharacter; i++)
                {
                    if (i != characterNum)
                    {
						Smr[i].enabled = false;     //カメを非表示
					}
                }
                break;
				//マンタ
				//case 3:
				//Smr[characterNum].enabled = true;	//マンタを表示
				//for (int i = 0; i < maxCharacter; i++)
				//{
				//		if (i != characterNum)
				//		{
				//			Smr[i].enabled = false;	//マンタを非表示
				//		}
				//	}
				//	break;
		}
	}

	//キャラクター番号更新保持関数
	//	//private void AddCharacterNum()
	//   {
	//       for (int i = 0; i < dataRetation.characterNum.Length; i++)
	//       {
	//		//まだ値が格納されていないかつ、プレイヤーオブジェクト配列内に格納されているオブジェクトと同じものなら
	//           if (dataRetation.characterNum[i] == -1 && dataRetation.playerList[i] == this.gameObject)
	//           {
	//			//その配列の値の場所に、キャラクター番号を格納する
	//               dataRetation.characterNum[i] = characterNum;
	//			break;
	//           }
	//		//現在のcharacterNumと配列内の値が一緒でなく、配列内に入っているオブジェクトがこのオブジェクト同じなら
	//		else if (dataRetation.characterNum[i] != characterNum && dataRetation.playerList[i] == this.gameObject)
	//		{
	//			//キャラクタ番号を更新する
	//			dataRetation.characterNum[i] = characterNum;
	//			break;
	//		}
	//		//配列内の値が-1(既に値が格納されている)でないならば
	//		else if (dataRetation.characterNum[i] != -1)
	//           {
	//			//既に値が格納されているのでループを継続する
	//               continue;
	//           }
	//       }
	//   }


	private void RemoveCharacterNum()
    {
        for (int i = 0; i < dataRetation.characterNum.Length; i++)
        {
            if (dataRetation.characterNum[i] != -1 && dataRetation.playerList[i] == this.gameObject)
            {
				dataRetation.controllerID[i] = -1;			//コントローラーIDをを削除する
				dataRetation.characterNum[i] = -1;			//選択されていたキャラクター番号を削除する
				gameStartSys.selectCharacterNumber[i] = -1;	//決定されていたキャラクター番号を削除する
				break;
            }
        }
    }

	//既に選択されているキャラクターがいたら関数
	private bool LoopArray(int testNum)
	{
		int array = dataRetation.characterNum.Length;   //要素数（4）取得

		for (int i = 0; i < array; i++)
		{
			if (dataRetation.characterNum[i] == testNum)
			{
				return true;
			}
		}
		return false;
	}

	//プレイヤーカーソル移動処理（長押し.ver）
	private void LongPress()
	{

		int testNum2 = characterNum;
		if (push == true)
		{
			count++;    //長押し間隔調整

			//右長押し
			if (isRightPush == true)
			{
				if (count % interval == 0)
				{
					while (LoopArray((++testNum2) % maxCharacter)) { }

					characterNum = testNum2 % maxCharacter;
					//SE（移動音）
					//audioSouce.clip = moveSE;
					//audioSouce.PlayShot(moveSE);
				}
			}
			//左長押し
			else if (isLeftPush == true)
			{
				if (count % interval == 0)
				{
					while (true)
					{
						--testNum2;

						if (testNum2 < 0)
						{
							//一番最後のキャラクター番号に変更する（クマノミ->マンタ）
							//マンタモデルができたら-1削除
							testNum2 = maxCharacter - 1;
						}
						if (!LoopArray(testNum2)) { break; }
					}

					characterNum = testNum2 % maxCharacter;
				}
				//audioSouce.clip = moveSE;
				//audioSouce.PlayOneShot(moveSE);
			}
		}
	}

	/*-----------ActionInput(Start)-----------*/

	//プレイヤーカーソル移動処理（ゲームパッド：左スティック or 十字キー）
	public void OnMove(InputValue value)
	{
		int currentCharNum = characterNum;

		//現在がキャラクターセレクトできる状態かつまだキャラクターを選択していない場合（動かせる）
		if (gameStartSys.isCharSelect == true && isCharSelected == false)
		{
			//左入力処理
			if (value.Get<float>() < 0.0f)
			{
				if (push == false && isLeftPush == false)  //押された時の処理（1度だけ反応させるため）
				{
					push = true;
					isLeftPush = true;

					while (true)
					{
						--currentCharNum;

						if (currentCharNum < 0)
						{
							//一番最後のキャラクター番号に変更する（クマノミ->マンタ）
							//マンタモデルができたら-1削除
							currentCharNum = maxCharacter - 1;
						}
						if (!LoopArray(currentCharNum)) { break; }
					}

					characterNum = currentCharNum % maxCharacter;

					//SE（移動音）
					//audioSouce.clip = moveSE;
					//audioSouce.PlayOneShot(moveSE);
				}
			}
			//右入力処理
			else if (value.Get<float>() > 0.0f)
			{
				if (push == false && isRightPush == false)  //押された時の処理（1度だけ反応させるため）
				{
					push = true;
					isRightPush = true;

					while (LoopArray((++currentCharNum) % maxCharacter)) { }

					characterNum = currentCharNum % maxCharacter;

					//SE（移動音）
					//audioSouce.clip = moveSE;
					//audioSouce.PlayOneShot(moveSE);
				}
			}
			else    //何も押されていない時
			{
				//リセット
				push = false;
				isLeftPush = false;
				isRightPush = false;
				count = 0;
			}
		}
	}

	//UI用の決定処理（ゲームパッド：Aボタン）
	public void OnSubmit(InputValue value)
	{
		//キャラクターが選択されていないかつ、現在がキャラクターセレクトである場合
		if (!isCharSelected && gameStartSys.isCharSelect)
		{
			//キャラクターを選択（決定）する
			isCharSelected = true;

			//既に選択されたキャラクターがいたときの処理
			int currentCharNum = characterNum;

			while (LoopArray((currentCharNum) % maxCharacter))
			{
				--currentCharNum;
			}

			characterNum = currentCharNum % maxCharacter;

			for (int i = 0; i < dataRetation.characterNum.Length; i++)
			{
				//キャラクターを決定していないかつ、格納する場所がキャラクター番号と同じ場所なら
				if (dataRetation.characterNum[i] != characterNum && dataRetation.playerList[i] == this.gameObject)
				{
					dataRetation.characterNum[i] = characterNum;

					// ここに処理
					Color color;
					ColorUtility.TryParseHtmlString("#A1A1A1", out color);
					characterUI[characterNum].GetComponent<Image>().color = color;
				}
			}
		}
	}

	//UI用のキャンセル処理（ゲームパッド：Bボタン）
	public void OnCancel(InputValue value)
	{
		//現在がキャラクターセレクト画面かつ、キャラクターが選択されている場合
		if (isCharSelected && gameStartSys.isCharSelect)
		{
			//キャラクター選択を解除する
			isCharSelected = false;

			for (int i = 0; i < dataRetation.characterNum.Length; i++)
			{
				//キャラクターが決定されているかつ、同じ添え字の場所に入っているのがこのオブジェクトなら
				if (dataRetation.characterNum[i] == characterNum && dataRetation.playerList[i] == this.gameObject)
				{
					dataRetation.characterNum[i] = -1;   //決定されたキャラクター番号を削除する（-1を入れる）

					characterUI[characterNum].GetComponent<Image>().color = Color.white;
					break;
				}
			}
		}
	}

	//デバイス切断確認
	public void OnDeviceLost(PlayerInput pi)
	{
		//Debug.Log("ゲームパッドが切断されました。");
		isTrigger = true;
	}

	/*-----------ActionInput(End)-----------*/
}