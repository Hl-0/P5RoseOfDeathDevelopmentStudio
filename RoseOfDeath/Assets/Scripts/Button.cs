using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public Button restartButton;
    public bool isGameActive;
    public GameObject titleScreen;
    public float spawnRate = 1.0f;
    private int score;
    public TextMeshProUGUI livesText;
    private int lives;
    public GameObject pauseScreen;
    private bool paused;
    private Button button;
    private GameManager gameManager;
    public int Difficulty;
    // Start is called before the first frame update

    // Start is called before the first frame update

    void Start()
    {
        button = GetComponent<Button>();
        if (scoreText != null)
        {
            scoreText.gameObject.AddComponent<EventTrigger>();
            EventTrigger trigger = scoreText.gameObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { DeleteText(); });
            trigger.triggers.Add(entry);
        }
        if (gameOverText != null)
        {
            gameOverText.gameObject.AddComponent<EventTrigger>();
            EventTrigger trigger = gameOverText.gameObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { DeleteText(); });
            trigger.triggers.Add(entry);
        }
    }

    void DeleteText()
    {
        if (gameOverText != null)
        {
            gameOverText.text = "GameOver";
        }
        if (scoreText != null)
        {
            scoreText.text = "Score: ";
        }
    }

    void StartGame()
    {
        Debug.Log(button.gameObject.name + " was cliked");
        gameManager.StartGame(Difficulty);
    }

    void ChangePaused()
    {
        if (!paused)
        {
            paused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            paused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangePaused();
        }

    }
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score:" + score;
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }
    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        score = 0;
        spawnRate = spawnRate /= (difficulty);
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        titleScreen.gameObject.SetActive(false);
        UpdateLives(3);
    }
    public void UpdateLives(int livesToChange)
    {
        lives += livesToChange;
        livesText.text = "Lives" + lives;
        if (lives <= 0)
        {
            GameOver();
        }
    }
}