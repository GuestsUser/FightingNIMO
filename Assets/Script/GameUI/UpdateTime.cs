using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTime : MonoBehaviour
{
    [Tooltip("���̃e�L�X�g")]
    [SerializeField] private Text[] minutesText;
    [Tooltip("�b�̃e�L�X�g")]
    [SerializeField] private Text[] secondsText;

    [Tooltip("��")]
    [SerializeField] private int minutes;

    [Tooltip("�b")]
    [SerializeField] private int seconds;

    [Tooltip("�c�莞��")]
    [SerializeField] private float timeLimit;

    string tMinutes;
    string tSeconds;

    bool finished;

    // Start is called before the first frame update
    void Start()
    {
        minutes = 3;
        seconds = 0;

        timeLimit = (minutes * 60) + seconds;

        tMinutes = $"00";
        tSeconds = $"00";

        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[���̐i�s�󋵂��Ǘ����Ă���X�N���v�g����擾�A�����i�s�󋵂��擾
        //if (isGame)
        //{
            // �������Ԃ�0�b�ȉ��Ȃ牽�����Ȃ�
            if (timeLimit <= 0f)
            {
                timeLimit = 0;
                finished = true; // �I���̍��}

                Debug.Log("�������ԏI��");
                return;
            }
            else
            {
                ChangeTime();
            }
        //}
    }

    void ChangeTime()
    {
        timeLimit -= Time.deltaTime;         // �o�ߎ��Ԃ������Ă���

        minutes = (int)timeLimit / 60;
        seconds = (int)timeLimit - minutes * 60;

        tMinutes = $"0{minutes.ToString()}";

        // seconds��2���̏ꍇ
        if(seconds >= 10)
        {
            tSeconds = $"{seconds.ToString()}";
        }
        // seconds��1���̏ꍇ
        else
        {
            tSeconds = $"0{seconds.ToString()}";
        }

        // Text�����ꂼ��1�����擾
        minutesText[0].text = tMinutes.Substring(0, 1); // ����0�����ڂ���1�����擾
        minutesText[1].text = tMinutes.Substring(1, 1); // ����1�����ڂ���1�����擾
        secondsText[0].text = tSeconds.Substring(0, 1); // �b��0�����ڂ���1�����擾
        secondsText[1].text = tSeconds.Substring(1, 1); // �b��1�����ڂ���1�����擾

    }

}
