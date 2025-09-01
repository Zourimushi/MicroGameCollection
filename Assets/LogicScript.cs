using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int playerScore;
    public Text scoreText;
   
    public GameObject gameOverScreen;

    
    public void addScore(int score)
    {
        playerScore+=score;
        scoreText.text =playerScore.ToString();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [ContextMenu("gameover")]
    public void gameOver()
    {
        gameOverScreen.SetActive(true);
        
    }
    public void back()
    {
        SceneManager.LoadScene("startScene");
    }

    
}
