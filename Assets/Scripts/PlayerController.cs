using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMENTARIOS IMPORTANTES
//que hace el script?
//a que esta asignado?
//que requiero?

public class PlayerController : MonoBehaviour
{
    //                 _rigidbody
    private Rigidbody playerRB;

    public float jumpForce = 10.0f;
    public float gravityModifier = 2.0f;

    //teceta si estas en el piso
    public bool isOnGround = true;
    //se inicializa en false siempre si no se define
    public bool gameOver = false;

    private Animator playerAnim;
    private AudioSource playerAudio;

    //la orden de que necesitamos 
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtyParticle;

    public AudioClip jumpSound;
    public AudioClip crashSound;

    //ORDENAR POR:
    //variables de logica, variables de assets, variables de componentes


    // Start is called before the first frame update
    void Start()
    {
        //el cable
        playerRB = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        
        //physics - componente global de unity, siempre tiene el numero de 1
        Physics.gravity *= gravityModifier; 

        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
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
    void Jump() {
            //comportamiento de salto
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            //que o reproduzca una vez, y el volumen, la instrucción
            playerAudio.PlayOneShot(jumpSound, 1.0f);

            //depende de la masa del rigid body para que se aplique - ForceMode.Impulse
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //una vez que saltamos, no estamos en el piso
            isOnGround = false;

            //trigger=accion, evento dentro de unity
            //nombre de la animacion_trig
            playerAnim.SetTrigger("Jump_trig");

            dirtyParticle.Stop();
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

    void ResetJump() {
        isOnGround = true;
        dirtyParticle.Play();
    }

}
