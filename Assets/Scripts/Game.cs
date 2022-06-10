using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Game : MonoBehaviour
{
    public float maxX = 2.0f;
    public float y = 1.0f;
    public GameObject[] enemyPrefabs;
    public float createEnemyInterval = 3.0f;

    public TextMeshProUGUI hpComp;
    public TextMeshProUGUI scoreComp;
    public TextMeshProUGUI highestScoreComp;

    public GameObject gameOver;

    private float currentScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
        InvokeRepeating("CreateEnemy", 0, createEnemyInterval);
    }

    public void Init(int hp, float score)
    {
        ChangeHp(hp);
        ChangeScore(score);
        currentScore = score;
        float highestScore = PlayerPrefs.GetFloat(GameEnum.HighestScore);
        highestScoreComp.SetText($"{highestScore}");
    }

    public void ChangeHp(int hp)
    {
        hpComp.SetText($"{hp}");
    }

    public void ChangeScore(float score)
    {
        if (Time.timeScale != 0)
        {
            float scoreText = Mathf.Ceil(score);
            scoreComp.SetText($"{scoreText}");
            currentScore = scoreText;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(
            enemyPrefabs[index],
            new Vector3(Random.Range(-maxX, maxX), y, 1),
            Quaternion.identity
        );
    }

    public void GameOver()
    {
        float score = PlayerPrefs.GetFloat(GameEnum.HighestScore);
        if (currentScore > score)
        {
            PlayerPrefs.SetFloat(GameEnum.HighestScore, currentScore);
        }
        PlayerPrefs.Save();
        Time.timeScale = 0;
        gameOver.SetActive(true);
    }

    public void OnClickAgain()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void OnClickExit()
    {
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
