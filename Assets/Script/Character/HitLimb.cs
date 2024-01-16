using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLimb : MonoBehaviour
{
    [SerializeField] [Tooltip("ダウンを司るスクリプトを入れる")] PlayerDown parent;

    [SerializeField] [Tooltip("受けるダメージにこの倍率が掛かる")] float hitRate;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (parent.IsInvincible() || parent.IsIgnoreLayer(collision.gameObject)) { return; } //親が無敵かヒットを取らないオブジェクトへのヒットだった場合終わり
        Attacker wepon = collision.gameObject.GetComponent<Attacker>();
        if (wepon == null || (!wepon.isAttack)) { return; } //ヒットオブジェクトから攻撃用スクリプトが見つからない又は攻撃状態でない場合終わり

        rb.AddForce(wepon.knockBack * collision.contacts[0].normal, ForceMode.Impulse);
        parent.Damage(wepon.damage * hitRate);
    }
}
