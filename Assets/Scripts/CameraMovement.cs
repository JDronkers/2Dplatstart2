using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;
    public float targetOrthoSize = 5f;

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = targetPosition;
        cam.orthographicSize = targetOrthoSize;
    }


}
