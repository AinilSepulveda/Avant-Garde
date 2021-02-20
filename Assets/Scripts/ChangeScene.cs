﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public Transform portalEnd;
    public GameObject lucesTutorial;
    public GameObject lucesNivel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<HeroController>().PortalEnd(portalEnd);

        lucesTutorial.SetActive(false);
        lucesNivel.SetActive(true);
    }
}