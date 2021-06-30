using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public Transform player;

    public Renderer mun;
    public Renderer mountain;
    public Renderer background;

    private Vector2 playerPos;

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y,100);
        playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        mun.material.mainTextureOffset = new Vector2(0.15f+playerPos.x * 0.00003f, 0.38f+playerPos.y * 0.003f);
        mountain.material.mainTextureOffset = new Vector2(playerPos.x * 0.0037f, 0.7f+playerPos.y * 0.0037f);
        background.material.mainTextureOffset = new Vector2(playerPos.x * 0.005f, 0.75f+playerPos.y * 0.01f);
    }
}
