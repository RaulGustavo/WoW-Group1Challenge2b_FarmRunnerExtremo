using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMENTARIOS IMPORTANTES
//que hace el script?
//a que esta asignado?
//que requiero?

//este codigo modifica el player
public class PlayerController : MonoBehaviour
{
    //variables de logica
    public float jumpForce = 10.0f;
    public float gravityModifier = 2.0f;
    public bool isOnGround = true; //te dice si estas en el piso
    public bool tooHigh = false;//Evita que saltes hasta el espacio
    public bool gameOver = false; //se inicializa en false siempre si no se define
    public float backSpeed = 15.0f;
    private bool isStarting = true; //indica que el mono inicio a moverse

    //variables de assets
    private Animator playerAnim;
    private AudioSource playerAudio;
    //                 _rigidbody
    private Rigidbody playerRB;

    //variables de componentes
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtyParticle;

    public AudioClip jumpSound;
    public AudioClip crashSound;

    //ORDENAR POR:
    //variables de logica, variables de assets, variables de componentes


    // Start is called before the first frame update
    void Start()
    {
        //el cable para obterner los componentes
        playerRB = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();

        //physics - componente global de unity, siempre tiene el numero de 1
        Physics.gravity *= gravityModifier; 
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarting) { //checa si inicio el juego
            //moviendo el mono de izquierda - derecha
            //mathf.lerp - interpolacion lineal gradualmente en 3 segundos
            float targetX = Mathf.Lerp(-5f, -1f, Time.time / 3.0f);

            //aplica la nueva posicion
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

            //checa si ya llego a la posicion final indicada
            if (transform.position.x >= -1f) {
                isStarting = false; //para detener el movimiento
            }
        }
        
        Jump();
        /* nocirbio
        //fix - para que el mono no se salga de la pantalla
        if (gameOver != false)
        {
            transform.Translate(Vector3.back * Time.deltaTime * backSpeed);
        }*/

    }

    //cuando colisionas con algo
    //acepta un objeto collision llamado collision
    //el objeto collision es el objeto con el que se colisiona
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetJump();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
           Die();
        }
    }


    //FUNCIONES
    void Jump() { //comportamiento del salto
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            //que o reproduzca una vez, y el volumen, la instrucción
            playerAudio.PlayOneShot(jumpSound, 1.0f);

            //depende de la masa del rigid body para que se aplique - ForceMode.Impulse
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //una vez que saltamos, no estamos en el piso (para no dar doble salto)
            isOnGround = false; 

            //trigger=accion, evento dentro de unity
            //nombre de la animacion_trig
            playerAnim.SetTrigger("Jump_trig");

            dirtyParticle.Stop();
            tooHigh=false;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !tooHigh && !gameOver){
            //que o reproduzca una vez, y el volumen, la instrucción
            playerAudio.PlayOneShot(jumpSound, 1.0f);

            //depende de la masa del rigid body para que se aplique - ForceMode.Impulse
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //Evitamos que el jugador salte hasta el infinito
            tooHigh = true; 

            //trigger=accion, evento dentro de unity
            //nombre de la animacion_trig
            playerAnim.SetTrigger("Jump_trig");
        }
    }   

    void Die() {
        //pregunta si la colision es con un objeto segun las tag asignadas al objeto
        //para que se ejecute rapido
        explosionParticle.Play();
        dirtyParticle.Stop();
        playerAudio.PlayOneShot(crashSound, 1.0f);

        gameOver = true;
        Debug.Log("Game Over!");
        //te puedes morir en cualquier momento, por eso es bool, quieres acceder todo el tiempo
        playerAnim.SetBool("Death_b", true);
        //hay diferentes maneras de morir, variaciones en morir
        playerAnim.SetInteger("DeathType_int", 1);
    }

    void ResetJump() { //cuando toca el piso
        isOnGround = true;
        dirtyParticle.Play();
    
    }

}
