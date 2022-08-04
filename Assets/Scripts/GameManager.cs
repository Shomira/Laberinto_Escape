using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;



public class GameManager : MonoBehaviour

{
    // Juego
    private GameObject nivelJugar;
    private GameObject playerObjInst;
    private bool juegoEjecutado = false;
    private int nivelJugando;
    private int contClic = 0;
    private float tiempoRestante;
    private Vector3 posicionRandom;
    private int crearPersonajesCont = 0;
    public AudioSource musicJuego;
    public GameObject camaraMinimizadaLaberinto;

    [Header("Jugador")]
    private GameObject playerObj;
    public GameObject playerPredefinido;
    public CinemachineVirtualCamera playerFollowCamObj;
    private Vector3 posicionRandomPlayer;

    private float posicionRandomMax;
    private float posicionRandomMin;

    [Header("Power UP")]
    // Vida
    public GameObject vidaPrefab;
    public int[] vidaInst;
    private GameObject vidaPrefabInst;
    private int vidaInsPorNivel;
    private int contPrefabVidaInst = 0;
    // tiempo
    public GameObject tiempoPrefab;
    private GameObject tiempoPrefabInst;
    private float tiempoSpawn = 0;

    [Header("Enemigo")]
    public GameObject enemigoPredefinido;
    private GameObject enemigoInst;
    private int contNewPosicionEnemigo;
    public float[] velocidadEnemigo;
    private Vector3 posicionRandomEnemigo;
    private int contNewAtributosEnemigo;


    [Header("Pantalla Principal")]
    public GameObject pantallaPrincipal;
    public GameObject titulo;

    [Header("Niveles")]
    public GameObject camaraNiveles;
    public GameObject[] nivelesDisponibles;
    public float[] tiemposNiveles;
    private GameObject objectNivel;
    public Vector3[] posicionCameraSecuNiveles;
    public float[] posicionRandomNiveles;


    [Header("Informacion Jugador")]
    public GameObject informacionJugador;
    public Button pauseBtnJugador;
    public TextMeshProUGUI vidaTexto;
    public TextMeshProUGUI nivelTexto;
    public TextMeshProUGUI tiempoTexto;
    private float targetTime;
    private int vidaRestadaPlayer;
    private int vidaJugadorReiniciar;

    [Header("Pantallas Jugando")]
    public GameObject botones_pantallas;
    public Image fondoOscuro;
    public TextMeshProUGUI titleNivel_Pause_Superado;

    [Header("Game Over")]
    public TextMeshProUGUI titleGameOver;
    public Button reiniciarBtnGameOver;
    public Button homeBtnGameOver;
    private bool isGameOverActive = false;

    [Header("Pause")]
    public TextMeshProUGUI titlePause;
    public Button cancelBtnPause;
    private bool isPauseActive = false;

    [Header("Finalizado")]
    public TextMeshProUGUI titleFinalizado;
    public TextMeshProUGUI paragraphVidasFinalizado;
    public TextMeshProUGUI paragraphTiempoFinalizado;
    public GameObject informacionFinalizado;
    private bool isFinalActive = false;
    public Button nextBtnFinalizado;
    private bool isNextNivel = false;

    private void Start()
    {
        camaraMinimizadaLaberinto.SetActive(true);
    }

    public void ComenzarJuego(int nivel)
    {
        nivelJugando = nivel;
        IniciarJuego();
        //Debug.Log(nivelJugando + "empezado");
    }

    public void IniciarJuego()
    {
        vidaInsPorNivel = vidaInst[nivelJugando - 1];
        contPrefabVidaInst = 0;
        camaraNiveles.transform.position = posicionCameraSecuNiveles[nivelJugando - 1];
        nivelTexto.text = Mathf.Round(nivelJugando).ToString();
        objectNivel = nivelesDisponibles[nivelJugando - 1];
        targetTime = tiemposNiveles[nivelJugando - 1];
        tiempoRestante = targetTime;
        pantallaPrincipal.SetActive(false);
        titulo.SetActive(false);
        informacionJugador.SetActive(true);
        nivelJugar = Instantiate(objectNivel);
        posicionRandomNivelesPlayer();
        posicionRandomEnemigo = new Vector3(Random.Range(posicionRandomMin, posicionRandomMax), 0, Random.Range(posicionRandomMin, posicionRandomMax));
        posicionRandomPlayer = new Vector3(Random.Range(posicionRandomMin, posicionRandomMax), 0, Random.Range(posicionRandomMin, posicionRandomMax));
        contNewPosicionEnemigo = 0;
        contNewAtributosEnemigo = 0;
        crearPersonajesCont = 0;
        juegoEjecutado = true;
        musicJuego.mute = false;
    }
 
    // Update is called once per frame
    void Update()
    {
        if (juegoEjecutado)
        {
            tiempoSpawn += Time.deltaTime;
            FuncionTiempo();

            pauseBtnJugador.onClick.AddListener(Pause);

            if (posicionRandomEnemigo == posicionRandomPlayer && contNewPosicionEnemigo == 0)
            {
                posicionRandomEnemigo = new Vector3(Random.Range(posicionRandomMin, posicionRandomMax), 0, Random.Range(posicionRandomMin, posicionRandomMax));
                contNewPosicionEnemigo++;
            }

            if (!isNextNivel && crearPersonajesCont == 0 && contNewPosicionEnemigo++ > 0)
            {
                CrearJugador();
                enemigoInst = Instantiate(enemigoPredefinido, posicionRandomEnemigo, enemigoPredefinido.transform.rotation);
                crearPersonajesCont++;
            }

            if (contNewAtributosEnemigo == 0 && enemigoInst != null)
            {
                enemigoInst.GetComponent<Enemigo>().posicionNewEnemigo(posicionRandomMax, posicionRandomMin);
                enemigoInst.GetComponent<Enemigo>().AumentoVelocidadNivel(velocidadEnemigo[nivelJugando - 1]);
                contNewAtributosEnemigo++;
            }

            if (playerObjInst.activeSelf == false)
            {
                playerObjInst.SetActive(true);
            } else if(enemigoInst.activeSelf == false)
            {
                enemigoInst.SetActive(true);
            }

            if (vidaRestadaPlayer == 0)
            {
                GameOver();
            }

            if (contPrefabVidaInst < vidaInsPorNivel && vidaInsPorNivel != 0)
            {
                vidaPrefabInst = Instantiate(vidaPrefab);
                vidaPrefabInst.transform.position = new Vector3(Random.Range(posicionRandomMin, posicionRandomMax), 1, Random.Range(posicionRandomMin, posicionRandomMax));
                contPrefabVidaInst++;
            }

            if(tiempoSpawn >= 15.0f)
            {
                SpawnObjTiempoPrefab();
                tiempoSpawn = 0;
            }
        }

        if (isGameOverActive || isPauseActive || isFinalActive)
        {
            reiniciarBtnGameOver.onClick.AddListener(ReiniciarJuego);
            homeBtnGameOver.onClick.AddListener(Home_Mostrar);
        }
    }

    public void TiempoIncrementa()
    {
        //Debug.Log(targetTime);
        targetTime += Random.Range(3.0f, 6.0f);
        //Debug.Log(targetTime);
    }
    public void vidaJugadorFuncion(int vidaJugador)
    {
        vidaRestadaPlayer = vidaJugador;
        vidaJugadorReiniciar = vidaJugador;
        vidaTexto.text = Mathf.Round(vidaRestadaPlayer).ToString();

    }

    public void VidaIncrementa()
    {
        vidaRestadaPlayer++;
        vidaTexto.text = Mathf.Round(vidaRestadaPlayer).ToString();

    }
    public void VidaRestada()
    {
        vidaRestadaPlayer -= 1;
        vidaTexto.text = Mathf.Round(vidaRestadaPlayer).ToString();
    }

    public void SpawnObjTiempoPrefab()
    {
        posicionRandom = new Vector3(Random.Range(posicionRandomMin, posicionRandomMax), 1.0f, Random.Range(posicionRandomMin, posicionRandomMax));
        tiempoPrefabInst = Instantiate(tiempoPrefab, posicionRandom, tiempoPrefab.transform.rotation);
        //Debug.Log("Tiempo Spawn");
        StartCoroutine(SpawnObjTiempoPrefabDestroy());
    }

    IEnumerator SpawnObjTiempoPrefabDestroy()
    {
        yield return new WaitForSeconds(8.0f);
        Destroy(tiempoPrefabInst);
        //Debug.Log("Tiempo Destruido");
    }

    private void FuncionTiempo()
    {
        if (targetTime <= 0.5555555f) // Finalizar el juego cuando el cronometro llegue a 0
        {
            GameOver();
        }
        else
        {
            targetTime -= Time.deltaTime;
        }

        if (targetTime <= 30.5555555f && targetTime >= 11.0f) // Cambiar el color cuando el tiempo esta entre 30 y 10
        {
            tiempoTexto.color = Color.yellow;
        }
        else if (targetTime <= 10.5555555f) // Cambiar el color cuando el tiempo es menor de 10
        {
            tiempoTexto.color = Color.red;
        } else
        {
            tiempoTexto.color = Color.white;
        }

        tiempoTexto.text = Mathf.Round(targetTime).ToString(); // Cronometro para reducir el tiempo
    }

    public void JugadorSeleccionado(GameObject playerSelect)
    {
        playerObj = playerSelect;
    }

    private void CrearJugador()
    {
        if (playerObj != null)
        {
            playerObjInst = Instantiate(playerObj);
        }
        else
        {
            playerObjInst = Instantiate(playerPredefinido);
        }
        playerFollowCamObj.Follow = GameObject.FindWithTag("CinemachineTarget").transform;
        playerObjInst.transform.position = posicionRandomPlayer;
        
    }

    public void posicionRandomNivelesPlayer()
    {
        posicionRandomMax = posicionRandomNiveles[nivelJugando - 1];
        posicionRandomMin = posicionRandomNiveles[nivelJugando - 1] * -1;
    }


    private void DestruirVidas()
    {
        GameObject[] power = GameObject.FindGameObjectsWithTag("VidaPrefab");

        for (int i = 0; i < power.Length; i++)
        {
            Destroy(power[i]);
        }
        contPrefabVidaInst = 0;
    }

    public void GameOver()
    {
        juegoEjecutado = false;
        musicJuego.mute = true;
        contClic = 0;
        fondoOscuro.gameObject.SetActive(true);
        isGameOverActive = true;
        enemigoInst.SetActive(false);
        playerObjInst.SetActive(false);
        informacionJugador.SetActive(false);
        titleGameOver.gameObject.SetActive(true);
        botones_pantallas.SetActive(true);
        //Debug.Log("Game Over");
    }

    public void Pause()
    {
        contClic = 0;
        titleNivel_Pause_Superado.transform.localPosition = new Vector3(0.0f, 185.0f, 0.0f);
        botones_pantallas.transform.localPosition = new Vector3(0.0f, -130.0f, 0.0f);
        isPauseActive = true;
        fondoOscuro.gameObject.SetActive(true);
        juegoEjecutado = false;
        musicJuego.mute = true;
        playerObjInst.SetActive(false);
        informacionJugador.SetActive(false);
        botones_pantallas.SetActive(true);
        titlePause.gameObject.SetActive(true);
        titleNivel_Pause_Superado.text = "Nivel " + nivelJugando;
        titleNivel_Pause_Superado.gameObject.SetActive(true);
        cancelBtnPause.gameObject.SetActive(true);
        cancelBtnPause.onClick.AddListener(Cancelar_Pause);
        enemigoInst.SetActive(false);
    }
    void Cancelar_Pause()
    {
        if(contClic == 0)
        {
            contClic++;
            informacionJugador.SetActive(true);
            botones_pantallas.SetActive(false);
            fondoOscuro.gameObject.SetActive(false);
            isPauseDesactive();
            isPauseActive = false;
            juegoEjecutado = true;
            musicJuego.mute = false;
        }
    }

    void ReiniciarJuego()
    {
        if(contClic == 0)
        {
            contClic++;

            DestruirVidas();

            vidaRestadaPlayer = vidaJugadorReiniciar;
            vidaTexto.text = Mathf.Round(vidaRestadaPlayer).ToString();
            isPauseDesactive();
            isGameOverDesactive();
            isHasganadoDesactive();
            botones_pantallas.SetActive(false);
            fondoOscuro.gameObject.SetActive(false);
            informacionJugador.SetActive(true);
            posicionRandomPlayer = new Vector3(Random.Range(posicionRandomMin, posicionRandomMax), 0, Random.Range(posicionRandomMin, posicionRandomMax));
            posicionRandomEnemigo = new Vector3(Random.Range(posicionRandomMin, posicionRandomMax), 0, Random.Range(posicionRandomMin, posicionRandomMax));
            targetTime = tiemposNiveles[nivelJugando - 1];
            vidaInsPorNivel = vidaInst[nivelJugando - 1];
            juegoEjecutado = true;
            musicJuego.mute = false;
        }
    }

    void isPauseDesactive()
    {
        if (isPauseActive == true)
        {
            if (cancelBtnPause.gameObject.activeSelf == true)
            {
                cancelBtnPause.gameObject.SetActive(false);
            }
            titlePause.gameObject.SetActive(false);
            titleNivel_Pause_Superado.gameObject.SetActive(false);
        }
    }

    void isGameOverDesactive()
    {
        if (isGameOverActive == true)
        {
            titleGameOver.gameObject.SetActive(false);
            isGameOverActive = false;
        }
    }

    void isHasganadoDesactive()
    {
        if (isFinalActive == true)
        {
            titleFinalizado.gameObject.SetActive(false);
            informacionFinalizado.SetActive(false);
            titleNivel_Pause_Superado.gameObject.SetActive(false);
            nextBtnFinalizado.gameObject.SetActive(false);
            isFinalActive = false;
        }
    }

    void Home_Mostrar()
    {
        DestruirVidas();
        juegoEjecutado = false;
        musicJuego.mute = true;
        isNextNivel = false;
        isPauseDesactive();
        isGameOverDesactive();
        isHasganadoDesactive();
        informacionJugador.SetActive(false);
        botones_pantallas.SetActive(false);
        fondoOscuro.gameObject.SetActive(false);
        titulo.SetActive(true);
        pantallaPrincipal.SetActive(true);
        Destroy(enemigoInst);
        Destroy(playerObjInst);
        Destroy(nivelJugar);
    }

    public void HasGanado()
    {
        DestruirVidas();
        contClic = 0;
        paragraphTiempoFinalizado.text = Mathf.Round(tiempoRestante - targetTime).ToString();
        paragraphVidasFinalizado.text = vidaRestadaPlayer.ToString();
        juegoEjecutado = false;
        musicJuego.mute = true;
        titleNivel_Pause_Superado.transform.localPosition = new Vector3(0.0f, 285.0f, 0.0f);
        botones_pantallas.transform.localPosition = new Vector3(0.0f, -322.0f, 0.0f);
        fondoOscuro.gameObject.SetActive(true);
        titleFinalizado.gameObject.SetActive(true);
        titleNivel_Pause_Superado.text = "Nivel " + nivelJugando;
        titleNivel_Pause_Superado.gameObject.SetActive(true);
        informacionFinalizado.SetActive(true);
        informacionJugador.SetActive(false);
        botones_pantallas.SetActive(true);
        isFinalActive = true;
        enemigoInst.SetActive(false);
        playerObjInst.SetActive(false);
        if (nivelJugando < 6)
        {
            nextBtnFinalizado.gameObject.SetActive(true);
            nextBtnFinalizado.onClick.AddListener(SiguienteNivel);
        }
    }

    private void SiguienteNivel()
    {
        if (contClic == 0)
        {
            contClic++;
            Destroy(nivelJugar);
            isNextNivel = true;
            nextBtnFinalizado.gameObject.SetActive(false);
            fondoOscuro.gameObject.SetActive(false);
            titleFinalizado.gameObject.SetActive(false);
            titleNivel_Pause_Superado.gameObject.SetActive(false);
            informacionFinalizado.SetActive(false);
            botones_pantallas.SetActive(false);
            isFinalActive = false;
            ComenzarJuego(nivelJugando + 1);
        }
    }


}
