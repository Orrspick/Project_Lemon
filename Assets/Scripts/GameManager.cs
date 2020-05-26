using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject Game_menu;
    public GameObject Control_sets;
    public PlayerMove player;
    public GameObject[] Stages;

    public int stageInfo;
    public int stageIndex;
    public int time;
    public int tottime;
    

    public bool inputMenu = false;

    private void Awake()
    {
        Screen.SetResolution(1280, 720, true); //해상도 고정
    }

    // Update is called once per frame
    void Update()
    {
        // 게임 메뉴를 구현합니다.
        /* timeScale 을 0으로 주면 일시정지(매뉴 활성화)
         * timeScale 을 1으로 주면 정지해제(매뉴 비활성화) */

        if (Input.GetButtonDown("Cancel") || inputMenu)
        {
            inputMenu = false;
            if (Game_menu.activeSelf)
            {
                Game_menu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                Game_menu.SetActive(true);
                Time.timeScale = 0;
            }     
        }

    }

    public void NextStage()
    {
        if(stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerNextStage();
            Debug.Log(stageIndex + "스테이지 이동됨");
        }
        else
        {
            Time.timeScale = 0;
            Debug.Log("게임 클리어");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            collision.attachedRigidbody.velocity = Vector2.zero;
            collision.transform.position = new Vector3(-8, 2, 0);
            player.OnDie();

            Debug.Log("낙사");
        }
    }

    void PlayerNextStage()
    {
        player.transform.position = new Vector3(-8, 2, 0);
        player.VelocityZero();
    }

    void GameExit()
    {
        Application.Quit();
    }
}
