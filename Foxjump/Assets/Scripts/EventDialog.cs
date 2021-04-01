using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDialog : MonoBehaviour
{
    public GameObject enterPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enterPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enterPanel.SetActive(false);
        }
    }
}
