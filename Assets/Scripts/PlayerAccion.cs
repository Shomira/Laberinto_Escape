using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccion : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DoorFinish"))
        {
            gameManager.HasGanado();
            Debug.Log("Has Ganado");
        } else if(other.CompareTag("VidaPrefab"))
        {
            Debug.Log("Vida incrementa");
            gameManager.VidaIncrementa();
            Destroy(other.gameObject);
        } else if(other.CompareTag("TimePrefab"))
        {
            Debug.Log("Tiempo incrementa");
            gameManager.TiempoIncrementa();
            Destroy(other.gameObject);
        }

    }
}
