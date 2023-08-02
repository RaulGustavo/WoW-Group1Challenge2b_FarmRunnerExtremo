using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//este codigo modifica el background y los objetos
public class MoveLeft : MonoBehaviour
{
    public float speed = 15.0f; //velocidad del objeto
    private float bound = -15.0f; //limite del objeto para destruirlo

    //referencia a objeto tipo player controler - los scripts son clases
    private PlayerController playerControllerScript;

    void Start()
    {
        //comunicacion entre scripts
        //player=nombre del objeto - poner nombres intuitivos
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //solamente si no perdiste
        if (playerControllerScript.gameOver == false)
        {
             //se mueve siempre a la izquierda
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (transform.position.x < bound && !gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
