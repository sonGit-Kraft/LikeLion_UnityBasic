using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    public float Speed = 3f;

    void Update()
    {
        transform.Translate(Vector3.down * Speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            // Destroy(collision.gameObject);
        }
    }
}