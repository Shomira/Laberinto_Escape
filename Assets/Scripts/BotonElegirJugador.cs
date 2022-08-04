using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotonElegirJugador : MonoBehaviour
{
    private Button boton;
    public GameObject botonesNiveles;
    public TextMeshProUGUI tipoPersonaje;
    public GameObject elegirPersonajeContenedor;

    // Start is called before the first frame update
    void Start()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(ElegirPersonaje);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ElegirPersonaje()
    {
        botonesNiveles.SetActive(false);
        boton.gameObject.SetActive(false);
        tipoPersonaje.gameObject.SetActive(false);
        elegirPersonajeContenedor.SetActive(true);
    } 
}
