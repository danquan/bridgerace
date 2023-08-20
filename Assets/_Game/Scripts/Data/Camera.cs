using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float speed = 20.0f;

    private Vector3 defaultPos;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.position - (new Vector3(0, 1, 0));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, defaultPos + player.transform.position, speed);
    }
}
