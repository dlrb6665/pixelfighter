using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRange : MonoBehaviour
{
    public int damage;
    public float destroyTime;

    private PlayerHp playerHp;

    // Start is called before the first frame update
    void Start()
    {
        playerHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHp>();
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("对敌人造成伤害");
            other.GetComponent<Enemy>().TakeDamage(damage);
        }

        if (other.gameObject.CompareTag("Player")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            if (playerHp != null)
            {
                playerHp.DamagePlayer(damage);
            }
        }
    }
}
