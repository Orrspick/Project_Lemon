using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 게임 진행
    public PlayerMove player;
    public GameObject[] Stages;
    public GameObject[] CStages;
    public GameObject[] IStages;
    public int stageIndex;
    public int cstageIndex;
    public int istageIndex;
    public float LimitTime;

    //UI
    public GameObject Game_menu;
    public GameObject Control_sets;
    public Text UIStage;
    public Text UITime;
    public GameObject GameOverScreen;
    
    

    public bool inputMenu = false;

    private void Awake()
    {
        Screen.SetResolution(1280, 720, true); //해상도 고정
    }

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
                Time.timeScale = 1f;
            }
            else
            {
                Game_menu.SetActive(true);
                Time.timeScale = 0f;
            }     
        }

        // 게임 시간을 체크합니다.
        LimitTime -= Time.deltaTime * Time.timeScale;
        UITime.text = "Time : " + Mathf.Round(LimitTime);
        // 게임이 0초가 되었을때 작동합니다.
        if(Mathf.Round(LimitTime) == 0)
        {
            player.OnDie();
            Time.timeScale = 0f;
            GameOverScreen.SetActive(true);
        }
    }

    public void NextStage()
    {
        if(stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            CStages[cstageIndex].SetActive(false);
            IStages[istageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerNextStage();
            Debug.Log(stageIndex + "스테이지 이동됨");

            UIStage.text = "Stage " + (stageIndex + 1);
        }
        else
        {
            Time.timeScale = 0f;
            Debug.Log("게임 클리어");
        }
    }

    public void ChageStage()
    {
        Stages[stageIndex].SetActive(false);
        CStages[cstageIndex].SetActive(true);
        Debug.Log(cstageIndex + "스테이지 변경됨");
    }

    public void InsertStage(int i)
    {
        istageIndex = i;
        IStages[istageIndex].SetActive(true);
    }

    public void AddTime()
    {
        LimitTime += 20;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            collision.attachedRigidbody.velocity = Vector2.zero;
            //collision.transform.position = new Vector3(-8, 2, 0);
            player.OnDie();
            Time.timeScale = 0f;
            GameOverScreen.SetActive(true);
            Debug.Log("낙사");
        }
    }

    void PlayerNextStage()
    {
        LimitTime = 60;
        player.transform.position = new Vector3(-17, -1, 0);
        player.VelocityZero();
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
