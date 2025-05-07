using UnityEngine;

public class Missile : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.up * 3 * Time.deltaTime);
    }
}
