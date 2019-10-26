using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnJewels : MonoBehaviour
{

    public int xRange = 10;
    public int yRange = 3;
    public int numObjects = 8;

    public GameObject[] objects;
    public float[] probabilidades;
    GameObject[] objectsEscogidos;
    GameObject[] objetosInstanciados;

    // nuevo
    // [Range(min: 0.1f, max:1f)]  // clamp Step to some reasonable values
    // public float Step = 1f;
    public float radius;

   // private List<Vector2> spawnablePositions;
    public Collider2D[] colliders;
    public Camera cam;
    public ScoreManager scoreScript;
    public Gun gunScript;
    float maxWidth;

    // Use this for initialization
    public void StartGame()
    {
        

        if(cam == null)
        {
            cam = Camera.main;
        }

        Vector3 upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
        Vector3 targetWidth = cam.ScreenToWorldPoint(upperCorner);

        float objectWidth = objects[0].GetComponent<Renderer>().bounds.extents.x;
        maxWidth = targetWidth.x - objectWidth;



        Shuffle(objects);
        objectsEscogidos = ChooseSet(numObjects);
        Debug.Log("objetos escogidos length = " + objectsEscogidos.Length);
        //HazNumMaxSpawn(numObjects);

        HazNumMaxSpawn(objectsEscogidos);

        //agregué esto
        scoreScript.StartClock();
        gunScript.SetPlaying(true);
    }


    // crea todos los objetos de una sola vez. Lo voy a comentar porque voy a poner algo diferente
    /*
    void Spawn()

    {
        for (int i = 0; i <= numObjects; i++)
        {
            //Vector3 spawnLoc = new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), 0);
            Vector3 spawnLoc = new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), 0);
            int objectPick = Random.Range(0, objects.Length);
            // Instantiate(objects[objectPick], spawnLoc, Random.rotation);
            Instantiate(objects[objectPick], spawnLoc, Quaternion.identity);
        }
    }
    */

        
   

    bool PreventSpawnOverLap( Vector3 spawnPos2)
    {

        colliders = Physics2D.OverlapCircleAll(transform.position, radius);
       // colliders = Physics2D.OverlapCircleAll(spawnPos1, radius);
       //Debug.Log("posicion 1 " + spawnPos1);
       // Debug.Log("posicion 2 " + spawnPos2);
       // Debug.Log("Colliders.Length " + colliders.Length);
        for (int i = 0; i < colliders.Length; i++)
        //for(int i = 0; i< spawnablePositions.Count; i++ )
        {
            Vector3 centerPoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x + 0.8f;
            float height = colliders[i].bounds.extents.y + 1.5f;
            float leftExtent = centerPoint.x - width;
            float rightExtent = centerPoint.x + width;
            float lowerExtent = centerPoint.y - height;
            float upperExtent = centerPoint.y + height;

            if (spawnPos2.x >= leftExtent && spawnPos2.x <= rightExtent)
            {
                if (spawnPos2.y >= lowerExtent && spawnPos2.y <= upperExtent)
                {
                    /*
                    Debug.Log("centerPoint = " + centerPoint);
                    Debug.Log("leftExtent = " + leftExtent);
                    Debug.Log("rightExtent = " + rightExtent);
                    Debug.Log("lowerExtent = " + lowerExtent);
                    Debug.Log("upperExtent = " + upperExtent);
                    */
                   // Debug.Log("regreso falso 1");
                    return false;
                }
            }
        }
       // Debug.Log("regreso true ");
        return true;
    }


    // void HazNumMaxSpawn(int numObjects)
    void HazNumMaxSpawn(GameObject[] esteArreglo)
    {
        //int objectPick = Random.Range(0, objects.Length);
       
        Vector3 spawnPos2 = new Vector3(0, 0, 0);
        //GameObject object1;
        
        //GameObject object2;
        int safetyNet = 0;

       // Debug.Log("puse este objeto = " + object1.name + "en posicion = " + spawnPos1);
        bool canSpawnHere = false;
        objetosInstanciados = new GameObject[esteArreglo.Length];
        //for (int i = 0; i < numObjects; i++)
        for (int i = 0; i < esteArreglo.Length; i++)
        {
            while (!canSpawnHere)
            {
                safetyNet++;
                if(safetyNet >= 50)
                {
                    Debug.Log("Demasiados intentos ");
                    break;
                }
                //spawnPosX = Random.Range(-xRange, xRange);
                float spawnPosX = Random.Range(-(maxWidth ), (maxWidth ));
                float spawnPosY = Random.Range(-yRange, yRange);
                spawnPos2 = new Vector3(spawnPosX, spawnPosY, 0);
                //object2 = Instantiate(objects[i], spawnPos2, Quaternion.identity);
                canSpawnHere = PreventSpawnOverLap(spawnPos2 );
                
                if (canSpawnHere)
                {
                    break;
                }
            }
            //object1 = Instantiate(objects[i], spawnPos2, Quaternion.identity);
            //object1 = Instantiate(esteArreglo[i], spawnPos2, Quaternion.identity);

            objetosInstanciados[i] = Instantiate(esteArreglo[i], spawnPos2, Quaternion.identity) as GameObject;
            // Debug.Log("siguiente objeto = " + object1.name + "posicion = " + spawnPos2);

            //spawnPos1 = spawnPos2;
            canSpawnHere = false;
        }
    }

    GameObject[] ChooseSet (int numRequired)
    {
        GameObject[] result = new GameObject[numRequired];
        int numToChoose = numRequired;

        for (int numLeft = objects.Length; numLeft > 0; numLeft--)
        {
            float prob = (float)numToChoose / (float)numLeft;
            if (Random.value <= prob)
            {
                numToChoose--;
                result[numToChoose] = objects[numLeft - 1];

                if (numToChoose == 0)
                {
                    break;
                }
            }
        }
        return result;
    }

    void Shuffle (GameObject[] deck)
    {
        for (int i = 0; i < deck.Length; i++)
        {
            GameObject temp = deck[i];
            int randomIndex = Random.Range(0, deck.Length);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    float Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }


    public void DestruyeLoQueQueda()
    {
        for (int i = 0; i < objetosInstanciados.Length; i++)
        {
            if(objetosInstanciados[i] != null)
            {
                //objetosInstanciados[i].SetActive(false);
                Destroy(objetosInstanciados[i]);
            }
        }

        
    }
}
