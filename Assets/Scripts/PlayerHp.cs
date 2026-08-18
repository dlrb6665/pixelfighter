using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    public int hp;
    public int Blinks;
    public float time;
    public float dieTime;
    public float hitBoxCdTime;

    private Renderer myRender;
    private Animator anim;
    private ScreenFlash sf;
    private Rigidbody2D rb2d;
    private PolygonCollider2D polygonCollider2D;


    // Start is called before the first frame update
    void Start()
    {
        HpBar.HpMax = hp;
        HpBar.HpCurrent = hp;
        myRender = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        sf = GetComponent<ScreenFlash>();
        rb2d = GetComponent<Rigidbody2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(int damage)
    {
        sf.FlashScreen();
        hp -= damage;
        if (hp < 0)
        {
            hp = 0;
        }
        HpBar.HpCurrent = hp;
        if (hp <= 0)
        {
            rb2d.velocity = new Vector2(0, 0);
            //rb2d.gravityScale = 0.0f;
            GameController.isGameAlive = false;
            anim.SetTrigger("Die");
            Invoke("KillPlayer", dieTime);
        }
        BlinkPlayer(Blinks,time);
        polygonCollider2D.enabled = false;
        StartCoroutine(ShowPlayerHitBox());
    }

    IEnumerator ShowPlayerHitBox()
    {
        yield return new WaitForSeconds(hitBoxCdTime);
        polygonCollider2D.enabled = true;
    }

    void KillPlayer()
    {
        Destroy(gameObject);
    }

    void BlinkPlayer(int numBlinks,float seconds)
    {
        StartCoroutine(DoBlinks(numBlinks, seconds));
    }

    IEnumerator DoBlinks(int numBlinks, float seconds)
    {
        for(int i = 0; i < numBlinks * 2; i++)
        {
            myRender.enabled = !myRender.enabled;
            yield return new WaitForSeconds(seconds);
        }
        myRender.enabled = true;
    }
}
