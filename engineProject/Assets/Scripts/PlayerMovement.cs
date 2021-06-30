using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private bool playerDie;
    public GameObject loseUI;
    public GameObject stopUIButton;
    public GameObject winUI;

    public static GameObject lose;
    public static GameObject stopButton;
    public static GameObject win;
    public static GameObject player;
    public static bool attacking;
    public static float hp;
    private static float damage;
    private static bool hit;
    public static bool cdam;
    private static float dam;

    private float ladderRayDirection;
    private RaycastHit2D hit1;

    private bool onladder;
    RaycastHit2D hit2;
    private SpriteRenderer spriteRenderer;
    public Slider slider;
    public PolygonCollider2D childCol;
    Animator animator;
    Rigidbody2D rigidbody2D;

    public LayerMask layerMask;
    public LayerMask ladderMask;

    private float colM;

    private float ladder;

    public float run;
    public float speed;

    public float jump;
    private bool jumpcheck;


    // Start is called before the first frame update
    public static void Init()
    {
        attacking = false;
        hp = 20;
        damage = 0;
        hit = false;
        cdam = true;
        dam = (float)1 / hp;
        player = GameObject.Find("Hero");
    }
    void Start()
    {
        lose = loseUI;
        stopButton = stopUIButton;
        win = winUI;
        Init();

        ladderRayDirection = 1;
        spriteRenderer = GetComponent<SpriteRenderer>();

        colM = 0.01f;
        childCol.enabled = false;

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerDie)
        {
            if (transform.localScale.x < 0)
            {
                ladderRayDirection = -1;
            }
            else
            {
                ladderRayDirection = 1;
            }
            hit2 = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(ladderRayDirection, 0), 1, ladderMask);
            if (hit)
            {
                slider.value -= dam * damage;
                StartCoroutine(Hit());
                if (hp < 1)
                {
                    StartCoroutine(Lose());
                }
            }
            if (this.transform.position.y < -33)
            {
                StartCoroutine(Lose());
            }
            if (Input.GetMouseButtonDown(0) && (!attacking))
            {
                PlayerAttack();
                attacking = true;
                StartCoroutine(Attacking());
            }
            transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);

            if (!attacking)
            {
                if (onladder)
                {
                    ladder = Input.GetAxis("Vertical");
                    Ladder();
                }
                run = Input.GetAxis("Horizontal");
                if (0 != run)
                {
                    PlayerRun();
                }
                if (hit2.collider == null)
                {
                    hit1 = Physics2D.BoxCast(transform.position, new Vector2(0.675f, 0.1f), 0, Vector2.down, 2, layerMask);

                    if (hit1.transform != null)
                        jumpcheck = true;
                    else
                        jumpcheck = false;
                    onladder = false;
                    if (!jumpcheck && !onladder)
                    {
                        animator.Play("PlayerJump");
                    }
                    if (jumpcheck && run == 0)
                    {
                        animator.Play("PlayerIdle");
                    }
                }
                else if (hit2.collider != null)
                {
                    onladder = true;
                    jumpcheck = true;
                }
                if (Input.GetKeyDown(KeyCode.Space) && jumpcheck)
                {
                    PlayerJump();
                }
            }
        }
    }
    void Ladder()
    {
        float y = ladder * speed*0.05f;
        transform.position += new Vector3(0, y, 0);
    }
    IEnumerator Attacking()
    {
        childCol.transform.localPosition=new Vector3(0 +colM, 0, 0);
        yield return new WaitForSeconds(0.3f);
        attacking = false;
        childCol.enabled = false;
        colM = -colM;
    }
    void PlayerRun()
    {
        if (jumpcheck)
        {
            animator.Play("PlayerRun");
        }
        if (run < 0)
        {
            transform.localScale=new Vector3(-5, 5, 5);
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x + 180, gameObject.transform.GetChild(0).rotation.z);
        }
        if(run > 0)
        {
            transform.localScale = new Vector3(5, 5, 5);
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.z);
        }
        float translation = run * speed * Time.deltaTime;
        transform.Translate(translation, 0, 0);
    }
    void PlayerJump()
    {
        jumpcheck = false;
        rigidbody2D.velocity = new Vector2(0, jump);
        if (!jumpcheck)
        {
            animator.Play("PlayerJump");
        }
    }

    void PlayerAttack()
    {
        animator.Play("PlayerAttack");

        childCol.enabled = true;
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x>transform.position.x)
        {
            transform.localScale = new Vector3(5, 5, 5);
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.z);
        }
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
        {
            transform.localScale = new Vector3(-5, 5, 5);
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x + 180, gameObject.transform.GetChild(0).rotation.z);
        }
    }


    public static void HpBar(float d)
    {
        if (cdam)
        {
            cdam = false;
            hit = true;
            hp -= d;
            damage = d;
        }
    }
    IEnumerator Hit()
    {
        hit = false;
        for (int i = 0; i < 6; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
        cdam = true;
    }

    IEnumerator Lose()
    {
        playerDie = true;
        animator.Play("PlayerDie");
        yield return new WaitForSeconds(1f);
        lose.SetActive(true);
        stopButton.SetActive(false);
        Time.timeScale = 0;
    }
}