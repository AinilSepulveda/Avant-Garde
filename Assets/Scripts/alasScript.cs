﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alasScript : MonoBehaviour
{
    public bool caca;
    MobManager mobManager;
    public GameObject[] spawnpoints;
    BoxCollider boxCollider;
    UnityEngine.AI.NavMeshObstacle obstacle;
    MeshRenderer mesh;
    private void Start()
    {
        mobManager = FindObjectOfType<MobManager>();
        boxCollider = GetComponent<BoxCollider>();
        mesh = GetComponent<MeshRenderer>();
        obstacle = GetComponent< UnityEngine.AI.NavMeshObstacle> ();
    }

    private void Update()
    {
       // boxCollider.isTrigger = caca;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" )
        {
            for (int i = 0; i < spawnpoints.Length; i++)
            {
                spawnpoints[i].SetActive(caca);
                
                mesh.enabled = false;
                obstacle.enabled = false;
            }
            mobManager.SpawnWave();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           // caca = false;
            mesh.enabled = true;
            obstacle.enabled = true;
            //boxCollider.isTrigger = caca;

        }
    }

}