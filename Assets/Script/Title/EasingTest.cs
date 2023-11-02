using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class EasingTest : MonoBehaviour
{
    [Tooltip("UIという名前のオブジェクトをアタッチしてください")]
    [SerializeField] private RectTransform ui;

    [SerializeField] private float duration;       // 所要時間
    [SerializeField] private float time;           // 実際にかかっている現在時間
    [SerializeField] private float length;         // 使用するSinカーブの長さ
    [SerializeField] private float turning_point;  // 使用するSinカーブの長さ
    [SerializeField] private bool symbol;          // true:+ false:-

    [SerializeField] private float TPRate;         // 進捗率 (time / fps) / duration
    [SerializeField] private float present_length; // 現在の到達地点(Sinカーブの)
    [SerializeField] private float[] max;            // 最終的に欲しい値
    //[SerializeField] private float max2;            // 最終的に欲しい値

    [Space(20.0f)]

    [SerializeField] private float source;
    [SerializeField] private float value;

    void Start()
    {
        TPRate = 0;
        value = source;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.rightShoulder.isPressed)
        {
            if (++time > duration * 60.0f) time = duration * 60.0f;
            //value = easing(duration, time, length, true, source, max[1]);
        }
        if (Gamepad.current.leftShoulder.isPressed)
        {
            if(--time < 0)time = 0;

            //value = easing(duration, time, length, true, source, max[1]);
        }

        //value = (easing(duration, time, 1.5f,false, source, max));

        if (Gamepad.current.selectButton.isPressed)
        {
            Reset();
        }

        Turn();
        
        //value = easing(duration, time, length, symbol, source, max[0]);

        //value = (easing2(duration, time, 1.5f, false, min, max, 0.5f));

        //Debug.Log($"value{value} = easing(duration{duration},time{time},length{1.5},symbol{false}) * {target_value}");
        //Debug.Log($"Sin{easing(duration,time,1.5f)}");
    }

    void Turn()
    {
        // 切り替え前までのポジションを保管
        float old_pos = easing(duration, TurningTime(duration, length, 0.5f), length, !symbol, source, max[0], false);
        Debug.Log($"old_pos{old_pos}");

        // timeがターンする地点への到着時間よりも小さい時(まだターンする地点に到着していない間)
        if (time <= TurningTime(duration, length, turning_point))
        {
            // 最初の地点から次の地点までを補完
            value = easing(duration, time, length, symbol, source, max[0],false);
        }
        else
        {

            float d = (duration  - (TurningTime(duration, length, 0.5f)/60.0f)); // durationからターンする地点での時間を引いて残り時間を取得
            float t = (time - TurningTime(duration, length, 0.5f));              // timeからターンする地点での時間を引いてeasingを0からスタート

            // ターン地点から次の地点までを補完
            value = easing(d, t,length - turning_point, true, old_pos, max[1], false);
        }

        ui.transform.localScale = new Vector3(value, value, value);
    }

    /// <summary>
    /// Sin波を使ったEasing関数 引数(所要時間,現在時間,Sinカーブの長さ,返り値の符号 true:+ false:-,始まりの値,最終的に欲しい値)
    /// </summary>
    /// <param name="duration">所要時間</param>
    /// <param name="time">現在時間</param>
    /// <param name="length">サインカーブの長さ</param>
    /// <param name="symbol">サインカーブの始まりの符号 true:+ false:-</param>
    /// <param name="source">始まりの値</param>
    /// <param name="max">最終的に欲しい値</param>
    /// <returns></returns>
    float easing(float duration, float time, float length,bool symbol,float source,float max, bool turn) 
    {
        float frame = 60.0f;                      // fps
        float t = ((time / frame) / duration);    // easingの進行状況を示す値を算出
        TPRate = t;                               // 進行率(%)
        present_length = t * length;              // 現在地点(Sinカーブから見た)

        // Sinカーブの進む方向を指定 true:+ false:- (最初に元の値から値を 増やしたい : +,減らしたい : -)
        float symbol_num = symbol ? 1.0f : -1.0f;

        // (time / frame)が duration を過ぎているなら
        if ((time / frame) >= duration)
        {
            t = 1; // tは進行率なので 1=100%で100%に固定し、関数の返り値がmaxで指定した値から変わらないようにしている
            //Debug.Log("time is over");
            return source - ((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) * (symbol_num * -1)) * Mathf.Abs(max - source);
        }

        // ターン時かどうかで変わる　ターン時はSinカーブ上で言うと　-1 から 1 までつまり距離は 2 
        if (turn)
        {
            Debug.Log("ターン");
            return source - (((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) - 1.0f) * (symbol_num * -1)) * Mathf.Abs(max - source);
        }
        // それ以外は 0 から 1 で距離は 1
        else
        {
            Debug.Log("通常");
            return source - ((float)Math.Round(Mathf.Sin(t * Mathf.PI * length), 4, MidpointRounding.AwayFromZero) * (symbol_num * -1)) * Mathf.Abs(max - source);
        }
    }

    /// <summary>
    /// Sin波のEasing関数の特定のタイミングの時間を取得する関数 引数(所要時間,Sinカーブの長さ,取得したい所(Sinカーブ上の地点))
    /// </summary>
    /// <param name="duration">所要時間</param>
    /// <param name="length">サインカーブの長さ</param>
    /// <param name="turning_point">取得したい所(Sinカーブ上の地点)</param>
    /// <returns></returns>
    float TurningTime(float duration, float length, float turning_point)
    {
        float fps = 60.0f;
        float t = duration * fps;
        float divisor = length / turning_point;

        return t / divisor;
    }

    private void Reset()
    {
        time = 0;
        TPRate = 0;
        present_length = 0;

        value = source;
    }
}
