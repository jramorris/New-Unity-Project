using UnityEngine;

public class SmallAsteroid : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // this class needs to be abstract?  Does that break pooling?
        // either way, this is duplicated in red enemy
        //if (collision.collider.CompareTag("Player"))
        //    collision.collider.GetComponent<PlayerController>().TakeDamage();
    }
}
