using UnityEngine;

public class Item : MonoBehaviour
{
    public float ItemVelocity = 20f;
    Rigidbody2D rig = null;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.AddForce(new Vector3(ItemVelocity, ItemVelocity, 0f)); 
    }

    void Update()
    {
        
    }
}
