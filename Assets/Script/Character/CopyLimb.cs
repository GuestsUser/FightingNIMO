using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLimb : MonoBehaviour
{
    [SerializeField] private Transform targetLimb;

    private ConfigurableJoint joint;
    private Quaternion targetIniRotate;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        targetIniRotate = targetLimb.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        joint.targetRotation = CopyRotation();
    }

    private Quaternion CopyRotation()
    {
        return Quaternion.Inverse(targetLimb.localRotation) * targetIniRotate;
    }
}
