using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 사용 불가 스크립트 알림 */
/* - Player가 기준으로 될시 페이드인, 아웃이 명령이 중단되는 현상발생 
 * 이 스크립트는 정상적인 사용이 가능한 스크립트지만 제가 만든 게임에서는 동작하지 않습니다. 
 * 이 스크립트를 대채하는 명령은 GameManager의 최하단에 있는 fadein / fadeout 명령을 확인해주시길 바랍니다. */
public class FadeManager : MonoBehaviour
{
    public Image subPanel;
    private Color color;

    private WaitForSeconds waittime = new WaitForSeconds(0.01f);

    public void Fadeout(float _speed = 0.0001f)
    {
        StartCoroutine(FadeoutPanel(_speed));
    }
    IEnumerator FadeoutPanel(float _speed)
    {
        subPanel.gameObject.SetActive(true);
        color = subPanel.color;

        while (color.a < 1f)
        {
            color.a += _speed;
            subPanel.color = color;
            yield return waittime;
        }
    }

    public void Fadein(float _speed = 0.0001f)
    {
        StartCoroutine(FadeinPanel(_speed));
    }
    IEnumerator FadeinPanel(float _speed)
    {
        color = subPanel.color;

        while (color.a > 0f)
        {
            color.a -= _speed;
            subPanel.color = color;
            yield return waittime;
        }
    }
}
