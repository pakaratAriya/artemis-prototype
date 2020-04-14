using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapPosition : MonoBehaviour
{
    public Vector2 origin;
    Vector3 curPos;
    Vector2 offset;
    public Image map;
    public Transform myPos;
    public float width;
    public float height;
    private float mapWidth = 100;
    private float mapHeight = 100;
    // Start is called before the first frame update
    void Start()
    {
        if(map)
        {
            mapWidth = map.rectTransform.sizeDelta.x;
            mapHeight = map.rectTransform.sizeDelta.y;
        }
        origin = GetComponent<RectTransform>().anchoredPosition;
        myPos = GetComponentInParent<Camera>().transform;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(origin.x + myPos.position.x / width * mapWidth,
            origin.y + myPos.transform.position.z / height * mapHeight);
    }
}
