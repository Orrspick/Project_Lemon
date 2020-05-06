using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject Game_menu;
    public GameObject Control_sets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 게임 메뉴를 구현합니다.
        /* timeScale 을 0으로 주면 일시정지(매뉴 활성화)
         * timeScale 을 1으로 주면 정지해제(매뉴 비활성화) */

        if (Input.GetButtonDown("Cancel")) //PC 테스트
        {
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
    public void GameExit()
    {
        Application.Quit();
    }
}
