using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] [Tooltip("プレイヤーにヒットした場合与える基礎ダメージ")] float _damage = 15;
    [SerializeField] [Tooltip("ヒットの時に吹き飛ばす力")] float _knockBack = 75;


    [SerializeField] [Tooltip("この武器がダメージがヒットでダメージを与えられるか入れる、trueならヒットしたらダメージを与える")] bool _isAttack;
    public bool isAttack { get { return _isAttack; } set { _isAttack = value; } }
    public float damage { get { return _damage; } }
    public float knockBack { get { return _knockBack; } }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
