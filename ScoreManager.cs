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
    public DiabloController diabloScript;
    public Gun gunScript;
    public SpawnJewels spawnScript;
    public GameObject carmelita;

    public UnityEvent DamageEvent;
    public UnityEvent RepairEvent;

    private float clockSpeed = 1f;


    public void StartClock()
    {
        scoreText.text = ("Score: " + score + "/" + targetScore);

        InvokeRepeating("Clock", 0, clockSpeed);

    }

    void Clock()
    {
        timePerLevel--;
        timeText.text = ("Tiempo: " + timePerLevel);
        if (timePerLevel == 0)
        {
            CheckGameOver();
        }
    }

 
    public void AddPoints(int pointScored)
    {
        score += pointScored;
        scoreText.text = ("Puntaje: " + score + "/" + targetScore);
    }

    void CheckGameOver()
    {
        diabloScript.DestroyDemon();
        carmelita.SetActive(false);
        if (score >= targetScore)
        {
            Ganaste(score);
           
            CancelInvoke();
            gunScript.TerminaAnimacion();

           
            RepairEvent.Invoke();

        }
        else
        {
            Perdiste(score);
            CancelInvoke();
            gunScript.TerminaAnimacion();
            DamageEvent.Invoke();
        }
    }

    private void Perdiste(int score)
    {
        spawnScript.DestruyeLoQueQueda();
        TerminasteDialog.instance.SetDialog("PerdisteDialog");
        TerminasteDialog.instance.SetScore(score);

        Debug.Log("Perdiste");
    }

    private void Ganaste(int score)
    {
        spawnScript.DestruyeLoQueQueda();
        TerminasteDialog.instance.SetDialog("GanasteDialog");
        TerminasteDialog.instance.SetScore(score);

        Debug.Log("Ganaste");
    }

}

