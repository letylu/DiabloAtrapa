using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiabloController : MonoBehaviour {
    // public AudioClip BoomSound;

    public GameObject DiabloPrefab;
    public Transform[] spawnPoints;
    public MovimientoDiablo movimientoScript;
    public Gun gunScript;
    Transform esteSpawnPoint;
    GameObject diablo;    // el prefab del diablo
    RaycastHit2D hit;
    GameObject diabloInicial;


	// Crea el diablo e inicia el movimiento del mismo al target
	public void CreaDiablo () {
        if (spawnPoints == null) return;
        esteSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if (diabloInicial != null)
        {
            movimientoScript.StopMovement();
            Destroy(diabloInicial);

        }
        
        diabloInicial = Instantiate(DiabloPrefab, esteSpawnPoint.position, Quaternion.identity);
        hit = gunScript.GetHit();

        Vector3 estaPosicion = hit.collider.bounds.center;
        Vector2 alliVa = new Vector2(estaPosicion.x, estaPosicion.y);
        movimientoScript.SetStartMovement(esteSpawnPoint.position, alliVa, diabloInicial);
        StartCoroutine(EsperaXSeg(2f, diabloInicial));
    }

    void DestruyeDiablo(GameObject diablo)
    {
        movimientoScript.StopMovement();
        StopAllCoroutines();
        Destroy(diablo);
    }

    IEnumerator EsperaXSeg(float segundos, GameObject diablo)
    {
        yield return new WaitForSeconds(segundos);
        DestruyeDiablo(diablo);
    }

    public void DestroyDemon()
    {
      
        DestruyeDiablo(diabloInicial);

    }

}
