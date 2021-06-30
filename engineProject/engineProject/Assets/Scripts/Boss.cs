using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private GameObject pool;
    public GameObject BossFire;
    public GameObject hellGato;
    public GameObject fire;
    public GameObject BosshellGato;

    public LayerMask whatisPlayer;

    public Slider slider;

    private SpriteRenderer spriteRenderer;

    private bool onDamage;

    private Vector3 gellGatoSpawnPos;

    private bool bossAttackC;
    private bool fireAttack;
    private bool randSpawn;

    private bool dead = false;

    private int life;
    public int hp;
    private float dam;
    // Start is called before the first frame update
    void Start()
    {
        pool = GameObject.Find("Pool");
        life = hp;
        dam = (float)1/hp;
        slider = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Slider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(BossAttackCheck());
        StartCoroutine(FireAttackCheck());
        StartCoroutine(RandSpawn());
    }

    // Update is called once per frame
    void Update() {
        if (Physics2D.OverlapCircle(transform.position, 15f, whatisPlayer))
        {
            if(bossAttackC)
            {
                Attack();
                bossAttackC = false;
            }
            if (fireAttack)
            {
                Fire(PlayerMovement.player.transform);
                fireAttack = false;
            }
            if (randSpawn)
            {
                gellGatoSpawnPos = new Vector3(Random.Range(this.transform.position.x - 5, this.transform.position.x + 5), Random.Range(this.transform.position.y, this.transform.position.y + 2f), 0);
                randSpawn = false;
                Spawn();
            }
        }
        Look();
    }
    void Spawn()
    {
        Instantiate(hellGato, gellGatoSpawnPos,Quaternion.identity);
    }
    void Look()
    {
        if (PlayerMovement.player.transform.position.x < transform.position.x)
        {
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x + 180, gameObject.transform.GetChild(0).rotation.z);
            transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z);
        }
        if (PlayerMovement.player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.z);
        }
    }
    IEnumerator BossAttackCheck()
    {
        while (!dead)
        {
            if (Random.Range(0, 15) < 2)
            {
                bossAttackC = true;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator FireAttackCheck()
    {
        while(!dead){
            if (Random.Range(0, 10) < 3)
            {
                fireAttack = true;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator RandSpawn()
    {
        while (!dead)
        {
            if (Random.Range(0, 10) < 3)
            {
                randSpawn = true;
            }
            yield return new WaitForSeconds(3);
        }
    }
    void Fire(Transform player)
    {
        Instantiate(fire,new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z),Quaternion.identity);
    }
    void Attack()
    {
        pool.transform.GetChild(0).gameObject.transform.localPosition=Vector3.zero;
        pool.transform.GetChild(0).gameObject.SetActive(true);
        pool.transform.GetChild(0).gameObject.transform.position = new Vector2(transform.position.x, transform.position.y + 5);
        pool.transform.GetChild(0).gameObject.transform.parent = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            HpBar();
            OnDamage();
        }
    }
    void OnDamage()
    {
        onDamage = false;
        life--;
        StartCoroutine(Blink());
        if (life < 1)
        {
            if (hp == 50)
            {
                StartCoroutine(Win());
            }
            else
            {
                Dead();
            }
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
    private void Dead()
    {
        dead = true;
        if (BosshellGato != null)
        {
            Instantiate(BosshellGato, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
    private IEnumerator Win()
    {
        dead = true;
        Instantiate(BossFire, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        PlayerMovement.stopButton.SetActive(false);
        PlayerMovement.win.SetActive(true);
        Time.timeScale = 0;
        Destroy(this.gameObject);
    }
}