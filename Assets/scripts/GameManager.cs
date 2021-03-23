using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalScore;
    public int stageScore;
    public int stageIndex;

    public Text UIhealth;
    public Text UIscore;
    public Text UIStage;
    public GameObject UIrestartBtn;

    public int health;

    public GameObject[] Stages;

    public PlayerMove player;
    
    private void Update() {
        UIscore.text = (totalScore + stageScore).ToString();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            health = 0;
            HealthDown();
        }
    }

    public void NextStage() {
        // 점수 계산
        totalScore += stageScore;
        stageScore = 0;

        // 이전 스테이지 비활성화
        Stages[stageIndex].SetActive(false);
        
        // 스테이지 전환
        if (++stageIndex < Stages.Length) {
            UIStage.text = "STAGE " + (stageIndex + 1);
            Stages[stageIndex].SetActive(true);
            player.Reposition();
        }
        else {  // 게임 클리어

            // 게임 시간 멈추기
            Time.timeScale = 0;

            // 결과
            UIStage.text = "YOU WIN!";

            // 다시 시작 버튼
            UIrestartBtn.SetActive(true);
        }

    }

    public void HealthDown() {
        if(--health < 0) {
            // 플레이어 죽는 모션
            player.OnDie();

            // 결과창

            // 다시시작 버튼
            UIrestartBtn.SetActive(true);

            // 시간 정지
            Time.timeScale = 0;
        }

        UIhealth.text = health.ToString();
    }

    public void Restart() {
        // 시간을 다시 흐르게
        Time.timeScale = 1;

        // 씬 불러오기
        SceneManager.LoadScene(0);
    }

}
