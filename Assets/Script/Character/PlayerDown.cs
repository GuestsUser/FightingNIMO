using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerDown : MonoBehaviour
{
    [SerializeField] [Tooltip("ダウン時にキャラを倒す為戻る力を完全に0にする対象のジョイント")] ConfigurableJoint hipJoint;
    [SerializeField] [Tooltip("ダウン時に戻る力を弱める対象とするジョイント")] ConfigurableJoint[] downJoint;
    [SerializeField] [Tooltip("ダウン時の戻る力")] float downPower = 5;

    bool _isDown; //ダウンしているかを記録する変数

    public bool isDown { get { return _isDown; } }

    JointDrive[] iniXDrive; //ダウン用ジョイントの初期のx軸の戻す力を記憶
    JointDrive[] iniYZDrive; //上記のyz用

    JointDrive iniHipXDrive; //上記のhip用
    JointDrive iniHipYZDrive; //上記のyz用

    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持


    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();

        iniHipXDrive = hipJoint.angularXDrive;
        iniHipYZDrive = hipJoint.angularYZDrive;

        Array.Resize(ref iniXDrive, downJoint.Length);
        Array.Resize(ref iniYZDrive, downJoint.Length);
        for (int i = 0; i < iniXDrive.Length; ++i)
        {
            iniXDrive[i] = downJoint[i].angularXDrive;
            iniYZDrive[i] = downJoint[i].angularYZDrive;
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RunFunction() //このコンポーネントのメイン機能
    {
        _isDown = parent.pInput.actions["DebugDown"].ReadValue<float>() != 0;
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
}
