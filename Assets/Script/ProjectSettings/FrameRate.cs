using System.Collections;
using System.Threading;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    /*�b�ԃt���[���ƌŒ�t���[����ύX���鏈���Q�ƌ��F https://blog.unity.com/ja/technology/precise-framerates-in-unity */

    public float Rate = 60.0f;/*�b�ԃt���[�����[�g��ύX����ϐ�(Update�֐�)*/
    public float FixedRate = 60f;/*�Œ�t���[�����[�g��ύX����ϐ��iFixedUpdate�֐��j*/
    float currentFrameTime;/*�ڕW�t���[�����[�g*/
    void Start()
    {
        Time.fixedDeltaTime = 1f / FixedRate;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 9999;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
    }
    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / Rate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }
}
