using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
   GameObject player;
   PlayerMove playermove;

    void Awake()
    {
       player = GameObject.FindGameObjectWithTag("Player");
       playermove = player.GetComponent<PlayerMove>();
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

}
