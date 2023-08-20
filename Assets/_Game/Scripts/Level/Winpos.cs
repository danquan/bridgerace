using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winpos : MonoBehaviour
{
    [SerializeField] public Transform tf = null;
    [SerializeField] private Stair[] stair = new Stair[1];

    private void Start()
    {
        OnInit();   
    }
    public void OnInit()
    {
        if (tf == null)
            tf = this.GetComponent<Transform>();
    }
}
