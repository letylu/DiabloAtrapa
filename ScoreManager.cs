using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{

    public int score = 0;
    public int targetScore = 4;
    public Text scoreText;
    public Text timeText;
    public int timePerLevel = 30;
   // public GameObject youWon;
   // public GameObject gameOver;
    public DiabloController diabloScript;
    public Gun gunScript;
    public SpawnJewels spawnScript;
    public GameObject carmelita;

    public UnityEvent DamageEvent;
    public UnityEvent RepairEvent;

    private float clockSpeed = 1f;


    // voy a ponerlo como una rutina que se activa con StartGame
    /*
    void Awake()
    {
        scoreText.text = ("Score: " + score + "/" + targetScore);

        InvokeRepeating("Clock", 0, clockSpeed);
        
    }
    */

    public void StartClock()
    {
        scoreText.text = ("Score: " + score + "/" + targetScore);

        InvokeRepeating("Clock", 0, clockSpeed);

    }

    /*
    private void Start()
    {
       // youWon.SetActive(false);
       // gameOver.SetActive(false);
    }
    */

    void Clock()
    {
        timePerLevel--;
        timeText.text = ("Tiempo: " + timePerLevel);
       // if (running == false) return;
        if (timePerLevel == 0)
        {
            CheckGameOver();
        }
    }

    /*
    private void Update()
    {
        if(running == false)
        {
            CancelInvoke();
        }
    }
*/
    public void AddPoints(int pointScored)
    {
        score += pointScored;
        scoreText.text = ("Puntaje: " + score + "/" + targetScore);
    }

    void CheckGameOver()
    {
        diabloScript.DestroyDemon();
        carmelita.SetActive(false);
       // gunScript.CollectedObject();
        if (score >= targetScore)
        {
           // youWon.SetActive(true);
            Ganaste(score);
           // Time.timeScale = 0;
           
            CancelInvoke();
            gunScript.TerminaAnimacion();

           
            RepairEvent.Invoke();

        }
        else
        {
            //gameOver.SetActive(true);
            Perdiste(score);
            //Time.timeScale = 0;
            
            CancelInvoke();
            gunScript.TerminaAnimacion();
            
            DamageEvent.Invoke();
        }
    }

    private void Perdiste(int score)
    {
        //Play level completed sound effect
        // AudioClips.instance.PlayCompletedSFX();

        spawnScript.DestruyeLoQueQueda();
        TerminasteDialog.instance.SetDialog("PerdisteDialog");
        TerminasteDialog.instance.SetScore(score);

        Debug.Log("Perdiste");
    }

    private void Ganaste(int score)
    {
        //Play level completed sound effect
        //AudioClips.instance.PlayCompletedSFX();
        spawnScript.DestruyeLoQueQueda();
        TerminasteDialog.instance.SetDialog("GanasteDialog");
        TerminasteDialog.instance.SetScore(score);

        Debug.Log("Ganaste");
    }

}

