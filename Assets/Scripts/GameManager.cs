using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

/* 
 * 주석 처리된 코드는 사용할 예정이 있으나 현제로써는 사용을 안하는것
 * 또는 삭제 예정인 코드임
 */

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
    public float LimitTime = 99999;
    public float Rtime = 9999;

    //UI
    public GameObject Game_main;
    public GameObject Game_menu;
    public GameObject Control_sets;
    public GameObject Upper_menu;
    public GameObject GameOverScreen;
    public Text UIStage;
    public Text UITime;
    public Text UIText;

    public bool inputMenu = false;

    public class SaveData
    {
        public Vector3 playerPositon;
        public int stageinfo;
        public float Ltime;

        public void printData()
        {
            Debug.Log("위치 : " + playerPositon);
            Debug.Log("스테이지 : " + stageinfo);
            Debug.Log("남은시간 : " + Ltime);
        }
    }

    void Awake()
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
            player.OnDie(true);
        }

        Rtime -= Time.deltaTime * Time.timeScale;
        if (Mathf.Round(Rtime) == 0)
        {
            UIText.gameObject.SetActive(false);
        }
    }

    public void GameStart()
    {
        LimitTime = 60;
        Game_main.SetActive(false);
        Control_sets.SetActive(true);
        Upper_menu.SetActive(true);
        Time.timeScale = 1f;
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

    public void LoadStage(int i, float t)
    {
        Stages[stageIndex].SetActive(false);
        stageIndex = i -1;
        NextStage();
        LimitTime = t;

        Game_main.SetActive(false);
        Game_menu.SetActive(false);
        GameOverScreen.SetActive(false);
        Control_sets.SetActive(true);
        Upper_menu.SetActive(true);
        Time.timeScale = 1f;
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
            collision.transform.position = new Vector3(-8, 2, 0);
            player.OnDie(true);
            Debug.Log("낙사");
        }
    }

    void PlayerNextStage()
    {
        LimitTime = 60;
        player.transform.position = new Vector3(-17, -1, 0);
        player.VelocityZero();
    }

    public void GameSave()
    {
        SaveData saveData = new SaveData();
        saveData.playerPositon = new Vector3(player.transform.position.x, player.transform.position.y);
        saveData.stageinfo = stageIndex;
        saveData.Ltime = LimitTime;

        File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", JsonUtility.ToJson(saveData));
    }

    public void GameLoad()
    {
        string load = File.ReadAllText(Application.persistentDataPath + "/PlayerData.json");
        SaveData loadData = JsonUtility.FromJson<SaveData>(load);

        if (load != null)
        {
            float x = loadData.playerPositon.x;
            float y = loadData.playerPositon.y;
            int stageIndex = loadData.stageinfo;
            float LimitTime = loadData.Ltime;

            LoadStage(stageIndex, LimitTime);
            player.transform.position = new Vector3(x, y, 0);

            loadData.printData();
            player.OnDie(false);
        }
        else
        {
            Debug.Log("불러올수 없습니다.");
        }
    }

    public void Retry() //다시시도 -> 매인매뉴로 활성됨
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        Game_menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void EventText(int i)
    {
        Rtime = 3.5f;
        if(i == 0)
        {
            UIText.text = "게임이 저장되었습니다.";
            UIText.gameObject.SetActive(true);
        }
        else if(i == 1)
        {
            UIText.text = "20초의 시간이 추가되었습니다.";
            UIText.gameObject.SetActive(true);
        }
        else if(i == 2)
        {
            UIText.text = "새로운 길이 열렸습니다!";
            UIText.gameObject.SetActive(true);
        }
        else if(i == 3)
        {
            UIText.text = "다른 길을 찾아보세요.";
            UIText.gameObject.SetActive(true);
        }
    }
}
