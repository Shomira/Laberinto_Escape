using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonNiveles : MonoBehaviour
{
    private Button boton;
    private GameManager gameManager;
    public int nivel;

    public int vidaPlayer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        boton = GetComponent<Button>();
        boton.onClick.AddListener(SetNivel);
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    void SetNivel()
    {
        //Debug.Log(boton.gameObject.name + " fue seleccionado");
        gameManager.ComenzarJuego(nivel);
        gameManager.vidaJugadorFuncion(vidaPlayer);

    }
}
