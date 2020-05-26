using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    GameObject player;
    GameObject menu;
    PlayerMove playermove;
    GameManager gamemanager;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playermove = player.GetComponent<PlayerMove>();

        menu = GameObject.FindGameObjectWithTag("Manager");
        gamemanager = menu.GetComponent<GameManager>();
    }

    public void LeftDown()
    {
        Debug.Log(player);
        playermove.inputLeft = true;
        Debug.Log("왼쪽 다운");
    }

    public void LeftUp()
    {
        playermove.inputLeft = false;
        Debug.Log("왼쪽 업");
    }

    public void RightDown()
    {
        playermove.inputRight = true;
        Debug.Log("오른쪽 다운");
    }

    public void RightUp()
    {
        playermove.inputRight = false;
        Debug.Log("오른쪽 업");
    }

    public void JumpClick()
    {
        playermove.inputJump = true;
        Debug.Log("점프 클릭");
    }

    public void MenuClick()
    {
        Debug.Log(menu);
        gamemanager.inputMenu = true;
        Debug.Log("매뉴 클릭");
    }

}
