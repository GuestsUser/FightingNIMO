using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using System;

public class PlayerLift : MonoBehaviour
{
    TestPlayer parent; //このスクリプトを制御する親スクリプトを保持

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<TestPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunFunction() //このコンポーネントのメイン機能
    {
        bool isLift = parent.pInput.actions["Lift"].ReadValue<float>() != 0; //パンチ入力があったか記録

        parent.animator.SetBool("isLift", isLift);
    }


}
