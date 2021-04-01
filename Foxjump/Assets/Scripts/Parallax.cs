using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float moveRate;
    private float startPointX;
    private float startPointY;
    public bool locky;//默认false

    void Start()
    {
        startPointX = transform.position.x;
    }
    
    void Update()
    {
        if (locky) {
            transform.position = new Vector2(startPointX + cam.position.x * moveRate, transform.position.y);
        }
        else {
            transform.position = new Vector2(startPointX + cam.position.x * moveRate, startPointY+cam.position.y*moveRate);
        }
    }
}
