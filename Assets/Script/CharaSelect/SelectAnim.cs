using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SelectAnim : MonoBehaviour
{
    [SerializeField] private CharSelectManager CSM;
    [SerializeField] private GameObject circle;
    [SerializeField] private GameObject[] circles;
    [SerializeField] private Image backImage;
    [SerializeField] private Color readyColor;
    [SerializeField] private GameObject check;
    [SerializeField] private float[] easTimes;
    [SerializeField] private float[] stopTimes;
    [SerializeField] private float stopTime;
    [SerializeField] private float maxY;
    [SerializeField] private float delay;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        Array.Resize(ref easTimes, circles.Length);   //circles�̐��ɂ����easTimes�̑傫����ύX����
        Array.Resize(ref stopTimes, circles.Length);  //circles�̐��ɂ����stopTimes�̑傫����ύX����

        for(int i = 0; i < circles.Length; i++)
        {
            easTimes[i] = 45.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CSM.isCharSelected)
        {
            //00FF9E
            backImage.color = readyColor;
            circle.GetComponent<CanvasGroup>().alpha = 0.0f;
            check.SetActive(true);
        }
        else
        {
            backImage.color = Color.white;
            circle.GetComponent<CanvasGroup>().alpha = 1.0f;
            check.SetActive(false);
            CircleAnim();
        }
        
    }

    void CircleAnim()
    {
        for(int i = 0; i < circles.Length; i++)
        {
            circles[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(circles[i].GetComponent<RectTransform>().anchoredPosition.x, (maxY / 2.0f) * easing(speed, easTimes[i], 0.5f));

            // �_�������ʒu�ɖ߂��Ă����Ƃ�
            if ((maxY / 2.0f) * easing(speed, easTimes[i], 0.5f) == 0)
            {
                stopTimes[i]++;
                if (stopTimes[i] > stopTime)
                {
                    stopTimes[i] = 0;
                }
            }

            // stopTimes��0�ł����
            if (stopTimes[i] == 0)
            {
                if (i > 0)
                {
                    if (easTimes[i - 1] > delay)
                    {
                        easTimes[i]++;
                    }

                }
                else
                {
                    easTimes[0]++;
                }
            }
        }
    }

    float easing(float duration, float time, float length)
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easing�̐i�s�󋵂������l���Z�o

        return (float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) + 1;
    }
}
