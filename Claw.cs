using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{

    public Transform origin;
    public float speed = 10f;        // tenía 4
    public Gun gun;
    public ScoreManager scoreManager;
    public DiabloController diabloScript;

    private Vector2 target;
    private int jewelValue = 1;
    private GameObject childObject;
    private LineRenderer lineRenderer;
    private bool hitJewel;
    private bool retracting;


    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }


    void Update()
    {
        //float step = speed * Time.deltaTime;
       // if (retracting == false) return;
        float step = speed * Time.unscaledDeltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);
        //transform.position = Vector2.MoveTowards(origin.position, target, step);

        /*
        Debug.Log("transform " + transform.position);
        Debug.Log("target " + target);
        Debug.Log("origen " + origin.position);
        */

        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, transform.position);

        // gun.CollectedObject();
        // Destroy(childObject);

        if (retracting == false) return; 
        if (transform.position == origin.position && retracting)
        {
            gun.CollectedObject();

            
            if (hitJewel)
            {
                Debug.Log("hitJewel = " + hitJewel);
                scoreManager.AddPoints(jewelValue);
                hitJewel = false;
            }
            Destroy(childObject);
           // StartCoroutine(EsperaXSeg(0.5f));
            //diabloScript.DestruyeDiablo();
            //gameObject.SetActive(false);
            retracting = false;
            
        }
       // retracting = false;     // esto no estaba
    }

    public void ClawTarget(Vector2 pos)
    {
        target = pos;
    }
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
       // retracting = true;   lo voy a mover de aqui
        target = origin.position;
        if (retracting == true) return;
        if (other.name == "Miner" || other.name == "Gun")
        {
            gun.CollectedObject();
            //StartCoroutine(EsperaXSeg(0.5f));  Ya ESTABA COMENTADO
            return;       // esto también es nuevo
        }
        Debug.Log("Entre a OnTrigger con other = " + other.name);
        if (other.gameObject.CompareTag("Jewel"))
        {
            hitJewel = true;
            childObject = other.gameObject;
            other.transform.SetParent(this.transform);
            retracting = true;
        }

        else if (other.gameObject.CompareTag("Rock"))
        {
            childObject = other.gameObject;
            other.transform.SetParent(this.transform);
            retracting = true;
        }

        // esto lo puse extra
        /*
        else if (other.gameObject.CompareTag("Diablo"))
        {
            //gun.CollectedObject();    esto lo quite tambien
            retracting = true;

            //StartCoroutine(EsperaXSeg(0.5f));
        }
        //retracting = false;    // esto no estaba
        */
    }

/*
    IEnumerator EsperaXSeg(float segundos)
    {
        yield return new WaitForSeconds(segundos);
         diabloScript.DestruyeDiablo();
    }
    */

    public void SetRetractingFalse()
    {
        retracting = false;
    }
}
