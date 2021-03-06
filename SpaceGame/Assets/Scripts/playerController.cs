using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    CircleCollider2D playerCollider;
    Animator anim;
    AudioSource source;

    public playerInput input;

    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip coinSound;
    public AudioClip slowDownSound;
    public AudioClip speedUpSound;

    public float timeSlowLength;
    public int damage = 1;
    public float invincibility = 1;
    public GameObject gameOver;
    public Text gameOverText;
    public GameObject weaponUnlockText;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Meteor")
        {
            if (StaticVars.lives > 0 && StaticVars.isInvincible == false)
            {
                StaticVars.lives -= damage;
                StaticVars.isInvincible = true;

                if (StaticVars.lives > 0)
                    source.PlayOneShot(hitSound);

                if (anim != null)
                {
                    anim.updateMode = AnimatorUpdateMode.UnscaledTime;
                    anim.Play("invincible");
                }

                StartCoroutine(StopInvincible());
            }

            if (StaticVars.lives <= 0)
                Death();
        }

        else if (coll.gameObject.tag == "Coin")
        {
            StaticVars.coins += 1;
            coll.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            source.PlayOneShot(coinSound, 1);

            if (StaticVars.coins == input.laserUnlock || StaticVars.coins == input.missileUnlock)
                StartCoroutine(WeaponUnlock());
        }

        else if (coll.gameObject.tag == "Life")
        {
            if (StaticVars.lives < 3)
                StaticVars.lives += 1;
            coll.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            source.PlayOneShot(coinSound, 1);
        }

        else if (coll.gameObject.tag == "Stopwatch")
        {
            coll.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(StopWatch());
        }
    }

    void Death()
    {
        playerCollider.enabled = false;
        source.PlayOneShot(deathSound);
        if (anim != null)
        {
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            anim.Play("die");
        }
        StaticVars.paused = true;
        gameOver.SetActive(true);
        gameOverText.text = "Game\nOver";
    }

    IEnumerator StopInvincible()
    {
        yield return new WaitForSeconds(invincibility);
        StaticVars.isInvincible = false;
    }

    IEnumerator StopWatch()
    {
        source.PlayOneShot(slowDownSound, 2);
        StaticVars.slowMotion = true;
        print("slow");
        //		yield return new WaitForSeconds (timeSlowLength - 7);
        yield return new WaitForSeconds(10);
        StaticVars.slowMotion = false;
        source.PlayOneShot(speedUpSound, 1.5f);
        print("normal");
    }

    IEnumerator WeaponUnlock()
    {
        weaponUnlockText.SetActive(true);
        yield return new WaitForSeconds(2);
        weaponUnlockText.SetActive(false);

    }
}
