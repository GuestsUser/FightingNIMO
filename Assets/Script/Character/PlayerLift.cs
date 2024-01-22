using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLift : MonoBehaviour
{
    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持

    void Start()
    {
        parent = GetComponent<TestPlayer>();
    }

    public void RunFunction() //このコンポーネントのメイン機能
    {
        bool isLift = parent.pInput.actions["Lift"].ReadValue<float>() != 0; //パンチ入力があったか記録

        parent.animator.SetBool("isLift", isLift);
    }
}
