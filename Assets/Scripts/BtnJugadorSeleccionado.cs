using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BtnJugadorSeleccionado : MonoBehaviour
{
    private Button boton;
    private GameManager gameManager;
    public GameObject botonesNiveles;
    public TextMeshProUGUI tipoPersonaje;
    public GameObject elegirPersonajeBtn;
    public GameObject elegirPersonajeContenedor;

    [Header("Jugador")]
    public GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        boton = GetComponent<Button>();
        boton.onClick.AddListener(PersonajeSeleccionado);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PersonajeSeleccionado()
    {
        if (boton.name == "BtnJugadorMale")
        {
            tipoPersonaje.text = "Hombre: Joe";
        } else if (boton.name == "BtnJugadorFemale") {
            tipoPersonaje.text = "Mujer: Megan";
        }
        gameManager.JugadorSeleccionado(playerObj);
        botonesNiveles.SetActive(true);
        tipoPersonaje.gameObject.SetActive(true);
        elegirPersonajeBtn.SetActive(true);
        elegirPersonajeContenedor.SetActive(false);
    }
}
