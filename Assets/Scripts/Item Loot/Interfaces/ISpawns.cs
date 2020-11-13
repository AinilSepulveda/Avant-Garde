using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawns //un interfaz para poder spawnear items sin importar el tipo de item 
{
    Rigidbody itemSpawned { get; set; }
    Renderer itemMaterial { get; set; }
    ItemPickUp itemType { get; set; }

    void CreateSpawn();
}

//importante que tiene que tener todas las clases establecidas y las funciones, no se pueden poner variables. 