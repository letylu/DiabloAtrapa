﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{

    public GameObject claw;
    public bool isShooting;
    public Animator minerAnimator;
    public Claw clawScript;

    public DiabloController diabloScript;

    RaycastHit2D hit;
    bool playing = false;


    void Update()
    {
        if (playing == false) return;
        if (Input.GetButtonDown("Fire1") && !isShooting)
        {
            LaunchClaw();
        }

    }

    void LaunchClaw()
    {
        // cuando se va a disparar, se activa la variable de que va a disparar y se para la animación
        Debug.Log("Aprete mouse ");
        isShooting = true;
        minerAnimator.speed = 0;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        hit = Physics2D.Raycast(screenPos, Vector2.zero);
        if (hit == false)
        {
            CollectedObject();
            return;
        }

        if (hit)
        {
            Debug.Log("Entre a hit " + hit.point + hit.collider.name);
            claw.SetActive(true);
            clawScript.ClawTarget(hit.point);

            // inicializa el diablo
            if (hit.transform.gameObject.CompareTag("Jewel"))
            {
                Debug.Log("Voy a crear diablo ");
                diabloScript.CreaDiablo();
            }
        }
    }

    public void CollectedObject()
    {
        isShooting = false;
        minerAnimator.speed = 1;
        Debug.Log("salgo de collected object");
    }

    public RaycastHit2D GetHit()
    {
        return hit;
    }

    public void TerminaAnimacion()
    {
        minerAnimator.speed = 0;
        clawScript.SetRetractingFalse();
        Debug.Log("Termine animacion");
        SetPlaying(false);
        Destroy(this);
    }

    public void SetPlaying(bool flag)
    {
        playing = flag;
    }
}
