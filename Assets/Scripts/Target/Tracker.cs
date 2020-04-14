using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    private TextMesh tm;
    public string text;

    private void Start()
    {
        tm = GetComponentInChildren<TextMesh>();
        tm.text = text;
        tm.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        tm.gameObject.SetActive(true);
        GetComponent<MeshRenderer>().materials[0].color = Color.green;
    }

    public void HideText()
    {
        tm.gameObject.SetActive(false);
        GetComponent<MeshRenderer>().materials[0].color = Color.white;
    }
}
