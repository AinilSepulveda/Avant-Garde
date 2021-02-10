using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrollingtxt : MonoBehaviour
{
    public float duration = 1f;
    public float speed;

    public TextMesh textMesh;
    private float startTime;

    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime < duration)
        {
            transform.LookAt(Camera.main.transform);
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetColor(Color color)
    {
        textMesh.color = color;
    }
    public void SetText(string text)
    {
        textMesh.text = text;
    }

}
