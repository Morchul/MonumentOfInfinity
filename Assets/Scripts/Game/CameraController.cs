using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] float moveSpeed, rotateSpeed;
    [SerializeField] Transform pivotPoint;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(pivotPoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        pivotPoint.Rotate(-Vector3.up, Input.GetAxis("SecondHorizontal") * rotateSpeed * Time.deltaTime, Space.World);
        pivotPoint.Translate(Vector3.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        pivotPoint.Translate(Vector3.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime);
        Zoom();
        
    }

    [SerializeField] float zoomSpeed;
    public void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y < 20 || Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y > 5)
            transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime);
    }
}
