using UnityEngine;

public class movement : MonoBehaviour
{

    [SerializeField]
    GameObject MainCamera;
    float speed = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        MainCamera = gameObject;
    }


    // To test when the user moving in Unity Scene only
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            MainCamera.transform.Translate(0,0,speed*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) {
            MainCamera.transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) {
            MainCamera.transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D)) {
            MainCamera.transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }
}
