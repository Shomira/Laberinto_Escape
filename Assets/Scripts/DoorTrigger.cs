using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private Door Door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!Door.IsOpen)
            {
                Door.Open(other.transform.position);
            }
        }
        //Debug.Log("door in");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Door.IsOpen)
            {
                Door.Close();
            }
        }

        //Debug.Log("door out");
    }
}
