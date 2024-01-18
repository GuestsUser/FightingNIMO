using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] [Tooltip("方向入力で90度回るまでに掛かる時間、フレーム単位")] float turnTime = 14;

    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
    }

    public void RunFunction() //このコンポーネントのメイン機能
    {
        Vector2 move = parent.pInput.actions["Move"].ReadValue<Vector2>();
        float norm = MathF.Abs(move.x) + MathF.Abs(move.y); //スティックの倒し具合である合計動作量の取得

        if (norm <= 0) { return; } //操作が無ければ終了

        float nowDirection = transform.eulerAngles.y;
        float direction = (MathF.Atan2(move.y, move.x) * Mathf.Rad2Deg - 90) * -1; //進行方向

        float rotateRange = AngleRangeCulc(direction, nowDirection); //現在回転から目標回転までの角度距離
        float sub = rotateRange / MathF.Abs(rotateRange); //自分自身で割る事で進行方向の符号取得
        float moveAg = 90 / turnTime * sub; //動作する角度量

        float result = nowDirection + moveAg * norm;
        if (sub >= 1 && moveAg * norm > rotateRange) { result = direction; } //動作角をdirectionに一致するように調整
        if (sub <= -1 && moveAg * norm < rotateRange) { result = direction; }

        if (rotateRange != 0) { transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, result, transform.rotation.eulerAngles.z); }
    }


    float Angle360Fit(float ag)
    {
        return ag < 0 ? 360 - (MathF.Abs(ag) % 360) : ag; //agがマイナスだった場合0～360範囲に収まるようにする
    }

    float AngleFitDisplay(float ag)
    {
        float fitAg = Angle360Fit(ag) % 360 ; //念の為角度を0～360の範囲に納める
        return fitAg >= 180 && fitAg < 360 ? (360 - fitAg) * -1 : fitAg; //agが180～360だった場合0～-180のディスプレイ表示と同じ形式に変更する
    }


    float AngleRangeCulc(float a1,float a2) //0から360、-180から180等、そのまま計算すると数値の切れ目でおかしくなる角度間距離を適正な物にして導出
    {
        float normalRange = AngleFitDisplay(a1) - AngleFitDisplay(a2); //-180～180計算
        float fitRange = Angle360Fit(a1) - Angle360Fit(a2); //0～360計算
        return MathF.Abs(normalRange) < MathF.Abs(fitRange) ? normalRange : fitRange; //小さい方を返す
    }
}
