using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionRange;

    public Vector2 startSpeed;
    public float delayExplodeTime;
    public float hitBoxTime;
    public float destroyBombTime;

    private Rigidbody2D rig;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rig.velocity = transform.up * startSpeed.y + transform.right * startSpeed.x;

        Invoke("Explode", delayExplodeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Explode()
    {
        anim.SetTrigger("Explode");
        Invoke("GenExplosionRange", hitBoxTime);
        Invoke("DestroyThisBomb", destroyBombTime);
    }

    void GenExplosionRange()
    {
        Instantiate(explosionRange, transform.position, Quaternion.identity);
    }

    void DestroyThisBomb()
    {
        Destroy(gameObject);
    }
}
