using UnityEngine;

public class TriggerDetect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Объект вошел в триггер: " + other.name);
            
        Destroy(other.gameObject);
        
        Debug.Log("Объект УНИЧТОЖЕН");
    }
}
