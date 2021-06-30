using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BAttack : MonoBehaviour
{
    private GameObject pool;
    public float speed;
    private Vector2 dir;
    private bool attacking;
    private bool attack;
    private void OnEnable()
    {
        attack = true;
        attacking = false;
        StartCoroutine(Attack());
    }
    private void Update()
    {
        if (attacking)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player"&&attack)
        {
            PlayerMovement.HpBar(2.5f);
            attack = false;
        }
    }
    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        //x= (int)(PlayerMovement.player.transform.position.x - this.transform.position.x);
        //y = (int)(PlayerMovement.player.transform.position.y - this.transform.position.y);
        dir = (PlayerMovement.player.transform.position - transform.position).normalized;

        attacking = true;
        yield return new WaitForSeconds(4f);
        pool = GameObject.Find("Pool");
        gameObject.transform.parent = pool.transform;
        gameObject.SetActive(false);
    }
}
