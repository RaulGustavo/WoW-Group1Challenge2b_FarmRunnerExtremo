using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{

    private Vector3 startPos;

    //un diseñador le gustaria cambiar este numero? no = private
    private float repeatWidth = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        //asignando la posicion actual del fondo
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        //si te mueves hasta este punto, se regresa
        if (transform.position.x < startPos.x - repeatWidth)
        {
            transform.position = startPos;
        }
    }
}
