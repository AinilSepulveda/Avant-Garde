using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeathBar : MonoBehaviour
{
    Vector3 localScale;
    [SerializeField]
    CharacterStats enemyHealth;

    // Start is called before the first frame update
    void Awake()
    {
        enemyHealth = transform.parent.GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth != null)
        {
            

            localScale.x = (float)enemyHealth.characterDefinition.currentHeath / enemyHealth.characterDefinition.maxHealth;
            localScale.y = 0.3f;

            transform.localScale = localScale;
        }

        transform.LookAt(Camera.main.transform);
    }
}
