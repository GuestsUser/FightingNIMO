using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(AudioSource))]

public class PlayerDown : MonoBehaviour
{
    [SerializeField] [Tooltip("体力、0になるとダウンする")] float hp = 150;
    [SerializeField] [Tooltip("ダウンした時の時間")] float downTimeBase = 4f;
    [SerializeField] [Tooltip("短い間隔でダウンする事によってダウン時間がこの倍率分増加していく")] float timeAugRate = 0.3f;
    [SerializeField] [Tooltip("増加時間倍率の最大値")] float maxRate = 2f;
    [SerializeField] [Tooltip("ダメージを受けずにcountがこの時間に達すると体力を回復しダウン回数を1減らす")] float healTime = -10f;

    [SerializeField] [Tooltip("無敵時間、ダメージを受けるとこの時間分はヒット判定を取らない")] float invincible = 0.2f;
    [SerializeField] [Tooltip("このレイヤー名gameObjectはヒットを無効化する")] string[] ignoreLayer = { "Default" };

    [SerializeField] [Tooltip("ダウン時にキャラを倒す為戻る力を完全に0にする対象のジョイント")] ConfigurableJoint hipJoint;
    [SerializeField] [Tooltip("ダウン時に戻る力を弱める対象とするジョイント")] ConfigurableJoint[] downJoint;
    [SerializeField] [Tooltip("ダウン時の戻る力")] float downPower = 5;

    [SerializeField] [Tooltip("ダメージを受けた時流すサウンド")] AudioClip[] damageSound;
    [SerializeField] [Tooltip("ダウンした時に流すサウンド")] AudioClip downSound;

    AudioSource audioPlayer;
    float count = 0; //復帰までの時間や無敵時間の終了をカウントする変数、値が0以下の場合ダメージを受けられるカウントダウン形式
    float iniHp; //復帰時にhpを戻す為初期体力を記録する
    bool _isDown; //ダウンしているかを記録する変数
    public bool isDown { get { return _isDown; } }
    int downCt = 0; //ダウンした回数を記録

    JointDrive[] iniXDrive; //ダウン用ジョイントの初期のx軸の戻す力を記憶
    JointDrive[] iniYZDrive; //上記のyz用

    JointDrive iniHipXDrive; //上記のhip用
    JointDrive iniHipYZDrive; //上記のyz用

    // Start is called before the first frame update
    void Start()
    {
        iniHipXDrive = hipJoint.angularXDrive;
        iniHipYZDrive = hipJoint.angularYZDrive;

        Array.Resize(ref iniXDrive, downJoint.Length);
        Array.Resize(ref iniYZDrive, downJoint.Length);
        for (int i = 0; i < iniXDrive.Length; ++i)
        {
            iniXDrive[i] = downJoint[i].angularXDrive;
            iniYZDrive[i] = downJoint[i].angularYZDrive;
        }

        iniHp = hp;
        audioPlayer = GetComponent<AudioSource>();
    }

    public void RunFunction() //このコンポーネントのメイン機能
    {
        if (count <= 0 && isDown) //ダウン中にカウントが切れた場合体力を戻してダウン解除
        {
            hp = iniHp;
            _isDown = false;
        }

        if (count <= healTime)
        {
            count = 0; //カウントリセット
            hp = iniHp;
            if (downCt > 0) { --downCt; } //1回以上ダウンした事がある場合ダウン回数を減らす
        }

        count -= Time.deltaTime;

        //_isDown = parent.pInput.actions["DebugDown"].ReadValue<float>() != 0;
        if (isDown) //ここ辺りの処理は後で分割予定
        {
            JointDrive jd = hipJoint.angularXDrive;
            jd.positionSpring = 0;
            hipJoint.angularXDrive = jd;

            jd = hipJoint.angularYZDrive;
            jd.positionSpring = 0;
            hipJoint.angularYZDrive = jd;
            foreach (ConfigurableJoint cj in downJoint)
            {
                jd = cj.angularXDrive;
                jd.positionSpring = downPower;
                cj.angularXDrive = jd;

                jd = cj.angularYZDrive;
                jd.positionSpring = downPower;
                cj.angularYZDrive = jd;
            }
            return; //ダウン状態なら他の入力を受け付けない
        }
        else
        {
            hipJoint.angularXDrive = iniHipXDrive;
            hipJoint.angularYZDrive = iniHipYZDrive;
            foreach (ConfigurableJoint cj in downJoint)
            {
                cj.angularXDrive = iniHipXDrive;
                cj.angularYZDrive = iniHipYZDrive;
            }

        }
    }

    public void Damage(float dmg) //dmg分だけダメージを受ける
    {
        audioPlayer.PlayOneShot(damageSound[UnityEngine.Random.Range(0, damageSound.Length)]);
        hp -= dmg;
        _isDown = hp <= 0;
        if (isDown)
        {
            downCt++; //ダウン回数カウントアップ

            float rate = timeAugRate * downCt;
            if (rate > maxRate) { rate = maxRate; } //倍率を最大値に丸める
            count = downTimeBase * rate; //ダウン用の無敵時間を入れる

            audioPlayer.PlayOneShot(downSound);
            return;
        }

        count = invincible; //ここまで来た場合通常の無敵時間
    }


    public bool IsInvincible() { return count > 0; } //countが0を超えている場合無敵状態としてtrueを返す

    public bool IsIgnoreLayer(GameObject obj) //objがヒット無効のレイヤーでないかチェックする、trueだったなら無効レイヤー
    {
        foreach (string name in ignoreLayer)
        {
            if (LayerMask.GetMask(name) == obj.layer) { return true; } //objがignoreLayerの何れかだった場合無効なのでtrue
        }
        return false; //ここまでこれればfalse
    }
}
