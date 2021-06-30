using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HellGato : MonoBehaviour
{
    public Slider slider;

    private SpriteRenderer spriteRenderer;

    public GameObject sk1;
    public GameObject sk2;
    public GameObject gh1;
    public GameObject gh2;

    public float speed;

    private bool initPos=true;
    private int rand;

    private bool spawn = true;

    private int life;
    public int hp;
    private float dam;
    private bool ondamage;

    private void Start()
    {
        ondamage = true;
        life = hp;
        dam = (float)1 / hp;
        slider = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Slider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, 3, LayerMask.GetMask("Tree")))
        {
            gameObject.transform.GetChild(0).localScale = new Vector3(-transform.GetChild(0).localScale.x, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (rand == 0)
            {
                rand = 1;
            }
            else if (rand == 1)
            {
                rand = 0;
            }
        }
        if (initPos)
        {
            rand = Random.Range(0, 2);
            initPos = !initPos;
        }

        if (ondamage)
        {
            switch (rand)
            {
                case 0:
                    transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                    break;
                case 1:
                    transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z);
                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(gameObject.transform.GetChild(0).rotation.x, gameObject.transform.GetChild(0).rotation.x + 180, gameObject.transform.GetChild(0).rotation.z);
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                    break;
            }
        }
        if (spawn)
        {
            Invoke("Spawn", 3);
        }
    }
    void Spawn()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                if (sk1 != null)
                {
                    Instantiate(sk1, new Vector3(transform.position.x,transform.position.y+2,10), Quaternion.Euler(new Vector3(0, 180, 0)));
                    Destroy(this.gameObject);
                }

                break;
            case 1:
                if (sk2 != null)
                {
                    Instantiate(sk2, new Vector3(transform.position.x, transform.position.y + 2, 10), Quaternion.Euler(new Vector3(0, 180, 0)));
                    Destroy(this.gameObject);
                }

                break;
            case 2:
                Instantiate(gh1, new Vector3(transform.position.x, transform.position.y + 2, 10), Quaternion.identity);
                Destroy(this.gameObject);
                break;
            case 3:
                Instantiate(gh2, new Vector3(transform.position.x, transform.position.y + 2, 10), Quaternion.identity);
                Destroy(this.gameObject);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword")&&ondamage)
        {
            HpBar();
            OnDamage();
        }
        if (collision.CompareTag("Player"))
        {
            PlayerMovement.HpBar(1);
        }
    }
    void OnDamage()
    {
        ondamage = false;
        life--;
        StartCoroutine(Blink());
        if (life < 1)
        {
            Debug.Log(hp);
            if (hp > 2)
            {
                StartCoroutine(Win());
            }
            else 
            {
                Destroy(gameObject);
            }
        }
    }
    private IEnumerator Win()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerMovement.stopButton.SetActive(false);
        PlayerMovement.win.SetActive(true);
        Time.timeScale = 0;
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
        ondamage = true;
    }
    public void HpBar()
    {
        slider.value -= dam;
    }
}
