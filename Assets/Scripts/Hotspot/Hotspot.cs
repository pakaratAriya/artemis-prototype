using UnityEngine;

public class Hotspot : MonoBehaviour {

    public delegate void HotspotEntered();
    public static event HotspotEntered OnEntered;

    public delegate void HotspotExited();
    public static event HotspotExited OnExited;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("ENTERED: " + gameObject.name);
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        if (OnEntered != null) {
            OnEntered();
        }
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log("EXITED: " + gameObject.name);
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        if (OnExited != null) {
            OnExited();
        }
    }
}