using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Transform transform;
    public float rotSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
            transform.RotateAround(transform.position, new Vector3(0, -1, 0), x);
            float y = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
            transform.RotateAround(transform.position, new Vector3(1, 0, 0), y);
        }
    }

}
