using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonEnemy : MonoBehaviour
{
    private bool ondamage;

    public Slider slider;

    private SpriteRenderer spriteRenderer;

    public string spawnanimationName;
    public string deadanimationName;

    private bool spawn=true;
    private bool live;

    Animator animator;

    private BoxCollider2D boxCollider2D;
    CapsuleCollider2D capsuleCollider2D;
    Rigidbody2D rb;
    public LayerMask groundLayers;

    public Transform groundCheck;

    private bool isFacingRight = true;

    RaycastHit2D hit1;
    RaycastHit2D hit2;

    public float idleSpeed;
    public float animationIdleSpeed;

    public float chaseSpeed;
    public float animationChaseSpeed;

    private float speed;

    public int hp;
    private float dam;

    private float y;

    private Vector2 rayRotation=Vector2.right;

    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid= GetComponent<Rigidbody2D>();

        y =180;
        boxCollider2D = GetComponent<BoxCollider2D>();
        ondamage = true;

        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        dam = (float)1/ hp;
        slider = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Slider>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void ChasePlayer()
    {
        animator.speed = animationChaseSpeed;
        if (PlayerMovement.player.transform.position.x < this.transform.position.x)
        {
            if(this.transform.localScale!= new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z))
            {
                this.transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z);
            }
            speed = chaseSpeed;
        }
        if (PlayerMovement.player.transform.position.x > this.transform.position.x)
        {
            if (this.transform.localScale != new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z))
            {
                this.transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
            }
            speed = -chaseSpeed;
        }
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
    private void FixedUpdate()
    {
        if (PlayerMovement.cdam)
        {
            capsuleCollider2D.enabled = true;
        }
        if (spawn&& (!live))
        {
            live = true;
            StartCoroutine(Spawn());
        }
        if(!spawn&&ondamage)
        {
            if (hit1 = Physics2D.Raycast(transform.position, rayRotation, 7, LayerMask.GetMask("Player")))
            {
                ChasePlayer();
            }
            if (hit1.transform == null)
            {
                if(Physics2D.Raycast(transform.position, rayRotation, 3, LayerMask.GetMask("Tree"))){
                    rayRotation = -rayRotation;
                    isFacingRight = !isFacingRight;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    y += 180;
                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, y, gameObject.transform.GetChild(0).rotation.z);
                }
                hit2 = Physics2D.Raycast(groundCheck.position, -transform.up, 1.1f, groundLayers);
                if (hit2.collider != false)
                {
                    animator.speed = animationIdleSpeed;

                    if (isFacingRight)
                    {
                        transform.Translate(-idleSpeed * Time.deltaTime, 0, 0);
                    }
                    else
                    {
                        transform.Translate(idleSpeed * Time.deltaTime, 0, 0);
                    }
                }
                else
                {
                    rayRotation = -rayRotation;
                    isFacingRight = !isFacingRight;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    y += 180;
                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, y, gameObject.transform.GetChild(0).rotation.z);
                }
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            if (ondamage)
            {
                HpBar();
                OnHitPush(collision.transform);
                OnDamage();
            }
        }
        if (collision.CompareTag("Player")&& !live)
        {
            PlayerMovement.HpBar(1);
            capsuleCollider2D.enabled = false;
        }
    }
    void OnDamage()
    {
        hp--;
        ondamage = false;
        StartCoroutine(Blink());
        if (hp < 0&&(!live))
        {
            rigid.velocity = Vector2.zero;
            StartCoroutine(Dead());
        }
    }
    
    void OnHitPush(Transform transform)
    {
        rigid.velocity = new Vector2(this.transform.position.x- transform.position.x, 0).normalized*2.5f;
    }
    IEnumerator Spawn()
    {
        animator.Play(spawnanimationName);
        yield return new WaitForSeconds(1f);
        spawn = false;
        live = false;
    }
    IEnumerator Dead()
    {
        boxCollider2D.enabled = false;
        capsuleCollider2D.enabled = false;
        rb.gravityScale = 0;
        live = true;
        spawn = true;

        animator.speed = 1;
        animator.Play(deadanimationName);
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
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
        rigid.velocity = Vector2.zero;
        ondamage = true;
    }
    public void HpBar()
    {
        slider.value -= dam;
    }
}
