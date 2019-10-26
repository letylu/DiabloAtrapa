using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoDiablo : MonoBehaviour {

    GameObject diablo = null;
    Vector2 target;
    float moveSpeed;
    Vector3 directionToTarget;
    public bool escudoCarmelita;
    Vector2 start;   // punto de inicio
    bool running = false;

    Vector3 esteVector;
    Vector3 inicioVector;

    // Use this for initialization
    void Start()
    {
        if (diablo == null) return;
        moveSpeed = Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (diablo != null && running)
        {
            StartCoroutine(FollowArc(diablo.transform, start, target, 12f, moveSpeed));
        }

    }
    IEnumerator FollowArc(
         Transform mover,
         Vector2 start,
         Vector2 end,
         float radius, // Set this to negative if you want to flip the arc.
         float duration)
    {
        if (running == false) yield return null;
        if (mover == null) yield return null;       // esto es nuevo

        Vector2 difference = end - start;
        float span = difference.magnitude;

        // Override the radius if it's too small to bridge the points.
        float absRadius = Mathf.Abs(radius);
        if (span < 2f * absRadius)
            radius = absRadius = span / 2f;

        Vector2 perpendicular = new Vector2(difference.y, -difference.x) / span;
        perpendicular *= Mathf.Sign(radius) * Mathf.Sqrt(radius * radius - span * span / 4f);

        Vector2 center = start + difference / 2f + perpendicular;

        Vector2 toStart = start - center;
        float startAngle = Mathf.Atan2(toStart.y, toStart.x);

        Vector2 toEnd = end - center;
        float endAngle = Mathf.Atan2(toEnd.y, toEnd.x);



        // Choose the smaller of two angles separating the start & end
        float travel = (endAngle - startAngle + 5f * Mathf.PI) % (2f * Mathf.PI) - Mathf.PI;

        float progress = 0f;
        do
        {
            if (mover == null) yield return null;           // esto es nuevo
            float angle = startAngle + progress * travel;
            mover.position = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * absRadius;
            progress += Time.deltaTime / duration;
            yield return null;
        } while (progress < 1f);

        mover.position = end;
    }

    public void SetStartMovement(Vector2 inicio, Vector2 final, GameObject diabloCreado)  // SpawnPoint inicial
    {
        moveSpeed = Random.Range(0.1f, 0.6f);
        start = inicio;
        target = final;
        if (diabloCreado == null) return;
        diablo = diabloCreado;
        running = true;

    }

    public void StopMovement()
    {
        StopAllCoroutines();  //si no uso la corrutina

        diablo = null;
        running = false;
    }
}
