using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int rutina;
    // tiempo entre rutinas
    public float cronometro;
    public Animator ani;
    // rotar al enemigo
    public Quaternion angulo;
    // grado de rotacion
    public float grado;
    public GameObject target;
    // Start is called before the first frame update
    public float velocidadEnemy;

    // Enemigo atrapa al jugador

    //Posicion Enemigo
    private float posicionRandomMax;
    private float posicionRandomMin;

    private GameManager gameManager;

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        Comportamiento_Enemigo();
        transform.Translate(Vector3.forward * velocidadEnemy * Time.deltaTime);
    }

    public void AumentoVelocidadNivel(float velocidad) {
        velocidadEnemy = velocidad;
    }

    public void Comportamiento_Enemigo()
    {
        
        // enemigo y jugador esten a una distancia mayor a 5 Vector3.Distance(transform.position, target.transform.position) > 5)
        if (Vector3.Distance(transform.position, target.transform.position) > 15)
        {
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= 2)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }
            switch (rutina)
            {
                case 0:
                    ani.SetBool("walk", true);
                    break;
                case 1:
                    // direccion de desplazamiento

                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    // para poder pasar al caso 2
                    rutina++;
                    break;
                case 2:

                    // rotacion del enemigo
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);

                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    break;
            }
        } else {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
            ani.SetBool("walk", false);
        }
    


    }

    public void posicionNewEnemigo(float posicionMaxJugador, float posicionMinJugador)
    {
        posicionRandomMax = posicionMaxJugador;
        posicionRandomMin = posicionMinJugador;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Vida restada");
            transform.position = new Vector3(Random.Range(posicionRandomMin, posicionRandomMax), 0, Random.Range(posicionRandomMin, posicionRandomMax));
            rutina = 0;
            gameManager.VidaRestada();
        }
    }
}
