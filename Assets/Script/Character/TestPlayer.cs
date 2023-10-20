using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] [Tooltip("移動速度")] float speed = 2;
    [SerializeField] [Tooltip("アニメーションを保有するゲームオブジェクト")] GameObject animatorObj;
    [SerializeField] [Tooltip("くっつきに使うジョイント")] ConfigurableJoint stickJoint;
    [SerializeField] [Tooltip("trueにする事でコントロールが変化する")] bool anotherPlayer;

    [SerializeField] [Tooltip("保持するプレイヤーインプット")] PlayerInput pInput;
    [SerializeField] [Tooltip("ダウン時にキャラを倒す為戻る力を完全に0にする対象のジョイント")] ConfigurableJoint hipJoint;
    [SerializeField] [Tooltip("ダウン時に戻る力を弱める対象とするジョイント")] ConfigurableJoint[] downJoint;
    [SerializeField] [Tooltip("ダウン時の戻る力")] float downPower;

    JointDrive[] iniXDrive; //ダウン用ジョイントの初期のx軸の戻す力を記憶
    JointDrive[] iniYZDrive; //上記のyz用

    JointDrive iniHipXDrive; //上記のhip用
    JointDrive iniHipYZDrive; //上記のyz用


    private Animator animator; //animatorObjから取得したanimatorを保持する変数

    private GameObject stickTarget; //掴む対象
    private Rigidbody stickRb; //stickjointのrigidbody

    // Start is called before the first frame update
    void Start()
    {
        //animator.keepAnimatorControllerStateOnDisable = true;
        stickRb = stickJoint.gameObject.GetComponent<Rigidbody>();
        animator = animatorObj.GetComponent<Animator>(); //アニメーター取得

        animator.keepAnimatorStateOnDisable = true;

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

        Keyboard keyboard = Keyboard.current;
        bool downRun = false;
        if (anotherPlayer) { downRun = keyboard.qKey.isPressed; }
        else { downRun = keyboard.eKey.isPressed; }

        //戻す処理の作成を行う
        if (downRun)
        {
            JointDrive jd = hipJoint.angularXDrive;
            jd.positionSpring = 0;
            hipJoint.angularXDrive = jd;

            jd = hipJoint.angularYZDrive;
            jd.positionSpring = 0;
            hipJoint.angularYZDrive = jd;
            foreach(ConfigurableJoint cj in downJoint)
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


        Vector3 moving = Vector3.zero; //今回の移動量入れ

        if (anotherPlayer)
        {
            if (keyboard.upArrowKey.isPressed) { moving.z += speed; } //符号はこのテスト環境上だとカメラ向きが逆なのでこちらも反転させている
            if (keyboard.downArrowKey.isPressed) { moving.z -= speed; }
            if (keyboard.rightArrowKey.isPressed) { moving.x += speed; }
            if (keyboard.leftArrowKey.isPressed) { moving.x -= speed; }
        }
        else
        {
            if (keyboard.wKey.isPressed) { moving.z += speed; } //符号はこのテスト環境上だとカメラ向きが逆なのでこちらも反転させている
            if (keyboard.sKey.isPressed) { moving.z -= speed; }
            if (keyboard.dKey.isPressed) { moving.x += speed; }
            if (keyboard.aKey.isPressed) { moving.x -= speed; }
        }

        transform.position += moving;

        bool isPunch = false; //パンチ用キーを仮にspaceとしている

        if (anotherPlayer) { isPunch = keyboard.mKey.isPressed; }
        else { isPunch = keyboard.spaceKey.isPressed; }

        if (isPunch) { animator.SetFloat("punchHoldTime", animator.GetFloat("punchHoldTime") + Time.deltaTime); } //パンチ入力持続時間を設定
        else
        {
            animator.SetFloat("punchHoldTime", 0);
            if (stickTarget != null)  //掴んでいた場合掴み用ジョイントを削除し掴み対象の情報もリセットする
            {
                Destroy(stickTarget.GetComponent<FixedJoint>());
                stickTarget = null;
            }
        }

        animator.SetBool("isMoving", moving.z + moving.x != 0);
        animator.SetBool("isPunch", isPunch);


        if (animator.GetFloat("punchHoldTime") < 2) { return; }
        if (stickTarget != null) { return; } //既に別オブジェクトを掴んでいれば終了

        //Debug.Log("1");
        //Debug.DrawRay(stickJoint.transform.position, stickJoint.transform.forward * 1.9f, Color.green, 0.2f);


        int mask = anotherPlayer? LayerMask.GetMask("Chara"): LayerMask.GetMask("Shark");
        RaycastHit hit;
        if (!Physics.SphereCast(stickJoint.transform.position, 1.5f, stickJoint.transform.forward , out hit, 1.9f, mask)) { return; } //ヒットがなければここで終了
        if (hit.transform.GetComponent<Rigidbody>() == null) { return; } //rigidbodyが無ければくっつき処理終了


        //Debug.Log(hit.transform.gameObject.name);

        stickTarget = hit.transform.gameObject; //掴み対象保存
        FixedJoint joint= stickTarget.AddComponent<FixedJoint>(); //掴み用ジョイント用意
        joint.connectedBody = stickRb;
        joint.breakForce = 9999;
    }
}
