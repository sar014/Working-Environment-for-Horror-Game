using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth.currentHealth<=0)
        {
            gameOver();
        }
    }

    public void gameOver()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        playerHealth.currentHealth=100;
        SceneManager.LoadScene(0);
    }


}
