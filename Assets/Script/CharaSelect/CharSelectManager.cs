using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class CharSelectManager : MonoBehaviour
{
	[Space(10)]
	[Header("---------------------------------------------")]
	[Header("���O�ɃI�u�W�F�N�g���擾���Ă�������")]
	[Tooltip("�Z���N�g�p�̃L�����N�^�[�����Ă�������")]
	[SerializeField] private List<GameObject> characters;
	[Tooltip("�v���C���[�J�[�\����RectTransform�̏��")]
	[SerializeField] private RectTransform cursorRT;
	[Tooltip("���̃v���C���[�̃J�[�\���e�L�X�g�����Ă�������")]
	[SerializeField] private GameObject cursor;
	[Tooltip("���̃v���C���[�̃J�[�\���e�L�X�g�����Ă��������iRT�擾�p�j")]
	[SerializeField] private RectTransform cursorImage; // �ύX�ӏ� cursorText �� cursorImage
	[Tooltip("�v���C���[�ԍ�UI�p�̃e�L�X�g�����Ă�������")]
	[SerializeField] private Text playerNumText;
	[Tooltip("���g��PlayerInput���擾(�����擾)")]
	[SerializeField] private PlayerInput input;
	[Tooltip("�I�ׂ�L�����N�^�[UI(�����擾)")]
	[SerializeField] private GameObject[] characterUI;  //�����I�ɓ������悤�ɂȂ��Ă���
	[Tooltip("�e�L�����N�^�[��moving���ɂ���TestCharacter�����Ă�������")]
	[SerializeField] private SkinnedMeshRenderer[] Smr; //�\���̍ۂɁA�L�����N�^�[���|��錻�ۂ𒼂����߂�SkinnedMeshRenderer�̃A�N�e�B�u�Œ������Ă���

	/*Script�֘A�擾*/
	[Tooltip("GameStartSystem.cs�������Ă���I�u�W�F�N�g�̎����擾")]
	[SerializeField] private GameStartSystem gameStartSys;
	[Tooltip("ReceiveNotificationExample.cs���������I�u�W�F�N�g�̎����擾")]
	[SerializeField] private ReceiveNotificationExample receiveNotificationExample;
	[Tooltip("DataRetation.cs���������I�u�W�F�N�g�̎����擾")]
	[SerializeField] private DataRetation dataRetation;

	[Header("---------------------------------------------")]
	[Space(20)]

	[Tooltip("���݂̃L�����N�^�[���i�N�}�m�~�A�T���A�J���A�}���^�j")]
	[SerializeField] private int maxCharacter;
	[Tooltip("�J�[�\���ړ����̎��ԊԊu")]
	[SerializeField] private float interval;
	[Tooltip("���ݑI������Ă���L�����N�^�[�ԍ�")]
	[SerializeField] private int characterNum;
	[Tooltip("UI�̃A�C�R���Ƃ̋��� 0��x�A1��y")]
	[SerializeField] private float[] offset = { 60, 40 };
	[Tooltip("�{�^�����͌p������")]
	[SerializeField] private float count;


	[Tooltip("�I������Ă���L�����N�^�[��I�񂾂��ǂ���")]
	[SerializeField] public bool isCharSelected;    //�L�����N�^�[��I��������`�F�b�N���t��
	[Tooltip("���X�e�B�b�N��X������͂��Ă��邩")]
	[SerializeField] private bool push;

	[SerializeField] private bool isLeftPush;    //���\���L�[��������Ă��邩�ǂ���
	[SerializeField] private bool isRightPush;   //�E�\���L�[��������Ă��邩�ǂ���

	private bool getCharacter;  //1�x�������������邽�߂̂���
	private bool isTrigger;     //�Q�[���p�b�h���ؒf���ꂽ���ǂ���

	

	private void Awake()
	{
		//------[�����擾][�ύX][������]------
        #region
        input = this.GetComponent<PlayerInput>();			//���̃X�N���v�g�����I�u�W�F�N�g��PlayerInput���擾
		cursorRT = cursor.GetComponent<RectTransform>();    //�v���C���[�J�[�\����RectTransform���擾

		gameStartSys = GameObject.Find("GameStartSystem").GetComponent<GameStartSystem>();
		dataRetation = GameObject.Find("DataRetation").GetComponent<DataRetation>();
		receiveNotificationExample = GameObject.Find("PlayerInputManager").GetComponent<ReceiveNotificationExample>();

		Array.Resize(ref characterUI, maxCharacter);    //characterUI�̔z��̃T�C�Y���L�����N�^�[�����ɕύX����

		transform.SetSiblingIndex(transform.GetSiblingIndex() - 1); //���̂��߂Ɉʒu�̓���ւ������Ă���̂�

		if (interval == 0) { interval = 15; }               //�J�[�\���ړ����̎��ԊԊu��15�ɐݒ�
		#endregion
		//------------------------------------
	}

    private void Start()
	{
		//���̃v���C���[���X�|�[�����ꂽ�Ƃ��z��Ɋi�[����֐��i�J�ڂ������Ɏg�p���邽�߁j
		AddPlayer();

		//1P�`4P�J�[�\���̏����ʒu�ݒ�
		switch (input.playerIndex)
		{
			//1P
			case 0:
				characterNum = 0;   //�v���C���[�̍ŏ��̏����L�����N�^�[�ݒ�
				/* �ǉ��E�ύX�ӏ� */
				cursorImage.anchoredPosition = new Vector2(-(offset[0]), (offset[1]));
				playerNumText.text = "<color=#ff6363>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorText��P1/P2/P3/P4�ɐݒ�
				/* ---------- */
				break;
			//2P
			case 1:
				characterNum = 1;
				/* �ǉ��E�ύX�ӏ� */
				cursorImage.anchoredPosition = new Vector2((offset[0]), (offset[1]));
				playerNumText.text = "<color=#33b0ff>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorText��P1/P2/P3/P4�ɐݒ�
				/* ---------- */
				break;
			//3P
			case 2:
				characterNum = 2;
				/* �ǉ��E�ύX�ӏ� */
				cursorImage.anchoredPosition = new Vector2(-(offset[0]), -(offset[1]));
				playerNumText.text = "<color=#f4f54b>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorText��P1/P2/P3/P4�ɐݒ�
				/* ---------- */
				break;
			//4P
			case 3:
				characterNum = 3;
				/* �ǉ��E�ύX�ӏ� */
				cursorImage.anchoredPosition = new Vector2((offset[0]), -(offset[1]));
				playerNumText.text = "<color=#4cf54b>" + (input.playerIndex + 1) + "P</color>"; //PlayerCursorText��P1/P2/P3/P4�ɐݒ�
				/* ---------- */
				break;
		}

		//���̃I�u�W�F�N�g�̖��O��Player1�`4�ɕύX����
		this.gameObject.name = "Player" + (receiveNotificationExample.playerNum - 1);
		
		cursor.SetActive(false);	//�v���C���[�J�[�\���̔�\��
		getCharacter = false;		//������
		isCharSelected = false;     //������
		isTrigger = false;          //������
		isLeftPush = false;         //������
		isRightPush = false;        //������
	}

	private void Update()
	{
		//1P�`4P�J�[�\���̈ʒu�f�o�b�O�p
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

		//AddCharacterNum();      //�L�����N�^�[�ԍ��ێ��֐�
		RemovePlayer();         //�v���C���[�폜�֐�
		CharacterVisibility();  //�L�����N�^�[�i�\��/��\���j�֐�
		LongPress();            //���������͊֐�

		//���݂��L�����N�^�[�Z���N�g�ł����ԂȂ�
		if (gameStartSys.isCharSelect == true)
		{
			//playerNumText.GetComponent<Text>().CrossFadeAlpha(1, 0f, true);
			cursor.GetComponent<CanvasGroup>().alpha = 1;
			if (getCharacter == false)
			{
				//characterUI�̎擾�ƃJ�[�\���̕\��
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

			//�L�����N�^�[�̑��������[�v
			for (int i = 0; i < maxCharacter; i++)
			{
				if (characterNum == i)
				{
					// �J�[�\���ʒu�̕ύX
					cursorRT.position = characterUI[i].GetComponent<RectTransform>().position;
				}
			}
		}
		else
		{
			//playerNumText.GetComponent<Text>().CrossFadeAlpha(0, 0f, true);
			cursor.GetComponent<CanvasGroup>().alpha = 0;

			//���Z�b�g���邱�ƂŖ߂������Ɉړ�����o�O�������Ȃ�
			isRightPush = false;
			isLeftPush = false;
			push = false;
		}
	}

	//�v���C���[�ǉ��֐�
	private void AddPlayer()
	{
		for (int i = 0; i < dataRetation.playerList.Length; i++)
		{
			if (dataRetation.playerList[i] == null)
			{
				dataRetation.controllerID[i] = input.devices[0].deviceId;   //�f�o�C�XID���i�[����
				dataRetation.playerList[i] = this.gameObject;   //���̃Q�[���I�u�W�F�N�g���i�[����
				break;
			}
		}
	}

	//�v���C���[�폜�֐�
	private void RemovePlayer()
    {
        if (isTrigger)
        {
			RemoveCharacterNum();
			Destroy(this.gameObject);
			InputSystem.FlushDisconnectedDevices();
		}
    }

	//�L�����N�^�[�i�\��/��\���j�֐�
	private void CharacterVisibility()
    {
		switch (characterNum)
		{
			//�N�}�m�~
			case 0:
				Smr[characterNum].enabled = true;	//�N�}�m�~��\��

				//�L�����N�^�[�̑��������[�v���A���ݑI������Ă���L�����N�^�[�ԍ��ȊO�̃L�����N�^�[���\���ɂ���
				for (int i = 0; i < maxCharacter; i++)
				{
					if (i != characterNum)
					{
						Smr[i].enabled = false;
					}
				}
				break;
			//�T��
			case 1:
				Smr[characterNum].enabled = true;    //�T����\��
				for (int i = 0; i < maxCharacter; i++)
				{
					if (i != characterNum)
					{
						Smr[i].enabled = false;		//�T�����\��

					}
				}
				break;
                //�J��

            case 2:  
				Smr[characterNum].enabled = true;   //�J����\��
				for (int i = 0; i < maxCharacter; i++)
                {
                    if (i != characterNum)
                    {
						Smr[i].enabled = false;     //�J�����\��
					}
                }
                break;
				//�}���^
				//case 3:
				//Smr[characterNum].enabled = true;	//�}���^��\��
				//for (int i = 0; i < maxCharacter; i++)
				//{
				//		if (i != characterNum)
				//		{
				//			Smr[i].enabled = false;	//�}���^���\��
				//		}
				//	}
				//	break;
		}
	}

	//�L�����N�^�[�ԍ��X�V�ێ��֐�
	//	//private void AddCharacterNum()
	//   {
	//       for (int i = 0; i < dataRetation.characterNum.Length; i++)
	//       {
	//		//�܂��l���i�[����Ă��Ȃ����A�v���C���[�I�u�W�F�N�g�z����Ɋi�[����Ă���I�u�W�F�N�g�Ɠ������̂Ȃ�
	//           if (dataRetation.characterNum[i] == -1 && dataRetation.playerList[i] == this.gameObject)
	//           {
	//			//���̔z��̒l�̏ꏊ�ɁA�L�����N�^�[�ԍ����i�[����
	//               dataRetation.characterNum[i] = characterNum;
	//			break;
	//           }
	//		//���݂�characterNum�Ɣz����̒l���ꏏ�łȂ��A�z����ɓ����Ă���I�u�W�F�N�g�����̃I�u�W�F�N�g�����Ȃ�
	//		else if (dataRetation.characterNum[i] != characterNum && dataRetation.playerList[i] == this.gameObject)
	//		{
	//			//�L�����N�^�ԍ����X�V����
	//			dataRetation.characterNum[i] = characterNum;
	//			break;
	//		}
	//		//�z����̒l��-1(���ɒl���i�[����Ă���)�łȂ��Ȃ��
	//		else if (dataRetation.characterNum[i] != -1)
	//           {
	//			//���ɒl���i�[����Ă���̂Ń��[�v���p������
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
				dataRetation.controllerID[i] = -1;			//�R���g���[���[ID�����폜����
				dataRetation.characterNum[i] = -1;			//�I������Ă����L�����N�^�[�ԍ����폜����
				gameStartSys.selectCharacterNumber[i] = -1;	//���肳��Ă����L�����N�^�[�ԍ����폜����
				break;
            }
        }
    }

	//���ɑI������Ă���L�����N�^�[��������֐�
	private bool LoopArray(int testNum)
	{
		int array = dataRetation.characterNum.Length;   //�v�f���i4�j�擾

		for (int i = 0; i < array; i++)
		{
			if (dataRetation.characterNum[i] == testNum)
			{
				return true;
			}
		}
		return false;
	}

	//�v���C���[�J�[�\���ړ������i������.ver�j
	private void LongPress()
	{

		int testNum2 = characterNum;
		if (push == true)
		{
			count++;    //�������Ԋu����

			//�E������
			if (isRightPush == true)
			{
				if (count % interval == 0)
				{
					while (LoopArray((++testNum2) % maxCharacter)) { }

					characterNum = testNum2 % maxCharacter;
					//SE�i�ړ����j
					//audioSouce.clip = moveSE;
					//audioSouce.PlayShot(moveSE);
				}
			}
			//��������
			else if (isLeftPush == true)
			{
				if (count % interval == 0)
				{
					while (true)
					{
						--testNum2;

						if (testNum2 < 0)
						{
							//��ԍŌ�̃L�����N�^�[�ԍ��ɕύX����i�N�}�m�~->�}���^�j
							//�}���^���f�����ł�����-1�폜
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

	//�v���C���[�J�[�\���ړ������i�Q�[���p�b�h�F���X�e�B�b�N or �\���L�[�j
	public void OnMove(InputValue value)
	{
		int currentCharNum = characterNum;

		//���݂��L�����N�^�[�Z���N�g�ł����Ԃ��܂��L�����N�^�[��I�����Ă��Ȃ��ꍇ�i��������j
		if (gameStartSys.isCharSelect == true && isCharSelected == false)
		{
			//�����͏���
			if (value.Get<float>() < 0.0f)
			{
				if (push == false && isLeftPush == false)  //�����ꂽ���̏����i1�x�������������邽�߁j
				{
					push = true;
					isLeftPush = true;

					while (true)
					{
						--currentCharNum;

						if (currentCharNum < 0)
						{
							//��ԍŌ�̃L�����N�^�[�ԍ��ɕύX����i�N�}�m�~->�}���^�j
							//�}���^���f�����ł�����-1�폜
							currentCharNum = maxCharacter - 1;
						}
						if (!LoopArray(currentCharNum)) { break; }
					}

					characterNum = currentCharNum % maxCharacter;

					//SE�i�ړ����j
					//audioSouce.clip = moveSE;
					//audioSouce.PlayOneShot(moveSE);
				}
			}
			//�E���͏���
			else if (value.Get<float>() > 0.0f)
			{
				if (push == false && isRightPush == false)  //�����ꂽ���̏����i1�x�������������邽�߁j
				{
					push = true;
					isRightPush = true;

					while (LoopArray((++currentCharNum) % maxCharacter)) { }

					characterNum = currentCharNum % maxCharacter;

					//SE�i�ړ����j
					//audioSouce.clip = moveSE;
					//audioSouce.PlayOneShot(moveSE);
				}
			}
			else    //����������Ă��Ȃ���
			{
				//���Z�b�g
				push = false;
				isLeftPush = false;
				isRightPush = false;
				count = 0;
			}
		}
	}

	//UI�p�̌��菈���i�Q�[���p�b�h�FA�{�^���j
	public void OnSubmit(InputValue value)
	{
		//�L�����N�^�[���I������Ă��Ȃ����A���݂��L�����N�^�[�Z���N�g�ł���ꍇ
		if (!isCharSelected && gameStartSys.isCharSelect)
		{
			//�L�����N�^�[��I���i����j����
			isCharSelected = true;

			//���ɑI�����ꂽ�L�����N�^�[�������Ƃ��̏���
			int currentCharNum = characterNum;

			while (LoopArray((currentCharNum) % maxCharacter))
			{
				--currentCharNum;
			}

			characterNum = currentCharNum % maxCharacter;

			for (int i = 0; i < dataRetation.characterNum.Length; i++)
			{
				//�L�����N�^�[�����肵�Ă��Ȃ����A�i�[����ꏊ���L�����N�^�[�ԍ��Ɠ����ꏊ�Ȃ�
				if (dataRetation.characterNum[i] != characterNum && dataRetation.playerList[i] == this.gameObject)
				{
					dataRetation.characterNum[i] = characterNum;

					// �����ɏ���
					Color color;
					ColorUtility.TryParseHtmlString("#A1A1A1", out color);
					characterUI[characterNum].GetComponent<Image>().color = color;
				}
			}
		}
	}

	//UI�p�̃L�����Z�������i�Q�[���p�b�h�FB�{�^���j
	public void OnCancel(InputValue value)
	{
		//���݂��L�����N�^�[�Z���N�g��ʂ��A�L�����N�^�[���I������Ă���ꍇ
		if (isCharSelected && gameStartSys.isCharSelect)
		{
			//�L�����N�^�[�I������������
			isCharSelected = false;

			for (int i = 0; i < dataRetation.characterNum.Length; i++)
			{
				//�L�����N�^�[�����肳��Ă��邩�A�����Y�����̏ꏊ�ɓ����Ă���̂����̃I�u�W�F�N�g�Ȃ�
				if (dataRetation.characterNum[i] == characterNum && dataRetation.playerList[i] == this.gameObject)
				{
					dataRetation.characterNum[i] = -1;   //���肳�ꂽ�L�����N�^�[�ԍ����폜����i-1������j

					characterUI[characterNum].GetComponent<Image>().color = Color.white;
					break;
				}
			}
		}
	}

	//�f�o�C�X�ؒf�m�F
	public void OnDeviceLost(PlayerInput pi)
	{
		//Debug.Log("�Q�[���p�b�h���ؒf����܂����B");
		isTrigger = true;
	}

	/*-----------ActionInput(End)-----------*/
}