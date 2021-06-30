using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ghost : MonoBehaviour
{
    private bool onDamage;
    public Slider slider;
    private SpriteRenderer spriteRenderer;

    public LayerMask whatisPlayer;
    Animator animator;

    private bool chase;

    public float speed;

    public float maxX;
    public float minX;

    public float maxY;
    public float minY;

    public int hp;
    private float dam;

    private GameObject player;

    Vector3 randompos;

    // Start is called before the first frame update
    void Start()
    {
        onDamage = true;
        player = GameObject.Find("Hero");
        dam = (float)1 / hp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        chase = false;
        StartCoroutine(RandomPos());
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        if (!chase&&onDamage)
        {
            chase=Physics2D.OverlapCircle(transform.position, 5.5f, whatisPlayer);
            transform.Translate(randompos * speed * Time.deltaTime);
        }
        if (chase && onDamage) 
        {
            chase = true;
            ChasePlayer();
        }
    }
    IEnumerator RandomPos()
    {
        while (!chase)
        {
            randompos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
            yield return new WaitForSeconds(2);
        }
    }
    void ChasePlayer()
    {
        if (PlayerMovement.player.transform.position.x < this.transform.position.x)
        {
            if (this.transform.localScale != new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z))
            {
                this.transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y,transform.localScale.z);
                gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x + 180, gameObject.transform.GetChild(0).rotation.z);
            }
        }
        if (PlayerMovement.player.transform.position.x > this.transform.position.x)
        {
            if (this.transform.localScale != new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z))
            {
                this.transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
                gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x , gameObject.transform.GetChild(0).rotation.z);
            }
        }

        transform.Translate((PlayerMovement.player.transform.position - transform.position)*speed*Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement.HpBar(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword") && onDamage)
        {
            HpBar();
            OnDamage();
        }
    }
    void Look()
    {
        if (player.transform.position.x < transform.position.x)
        {
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x + 180, gameObject.transform.GetChild(0).rotation.z);
            transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z);
        }
        if (player.transform.position.x > transform.position.x)
        {
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.z);
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
        }
    }
    void OnDamage()
    {
        hp--;
        onDamage = false;
        StartCoroutine(Blink());
        if (hp < 1)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator Blink()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.04f);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.04f);
        spriteRenderer.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.04f);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.04f);
        spriteRenderer.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.04f);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        onDamage = true;
    }
    public void HpBar()
    {
        slider.value -= dam;
    }
}
