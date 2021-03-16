using System.Collections;
using UnityEngine;

public class BaseEnemy : PooledMonoBehavior
{
    Coroutine _becomeInactiveCoroutine;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerController>().TakeDamage();

        if (collision.CompareTag("Map") && _becomeInactiveCoroutine != null)
        {
            StopCoroutine(_becomeInactiveCoroutine);
            _becomeInactiveCoroutine = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
            _becomeInactiveCoroutine = StartCoroutine("BecomeInactive");
    }

    IEnumerator BecomeInactive()
    {
        yield return new WaitForSeconds(2f);
        SetInactive();
    }

    void SetInactive()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            collision.collider.GetComponent<PlayerController>().TakeDamage();
    }
}
