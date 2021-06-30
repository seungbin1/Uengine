using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private bool fire=false;
    public LayerMask whatisPlayer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OnFire());
        Destroy(this.gameObject, 1.01f);
    }

    // Update is called once per frame
    IEnumerator OnFire()
    {
        yield return new WaitForSeconds(0.95f);
        fire = true;
    }
    private void FixedUpdate()
    {
        if (Physics2D.OverlapCircle(transform.position, 2f, whatisPlayer)&& fire)
        {
            PlayerMovement.HpBar(1.5f);
            fire = false;
        }
    }
}
