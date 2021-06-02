using UnityEngine;

public class IndicatePowerCell : MonoBehaviour
{
    GameObject target;
    SpriteRenderer _arrowRenderer;

    private void Awake()
    {
        _arrowRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        FindTarget();
    }

    public void FindTarget()
    {
        target = GameObject.FindWithTag("Collectible");
    }

    void Update()
    {
        if (target == null || !target.activeSelf)
            FindTarget();

        if (target == null || !target.GetComponent<SimpleCollectible>()._activeSelf)
            _arrowRenderer.enabled = false;
        else
        {
            _arrowRenderer.enabled = true;
            transform.right = target.transform.position - transform.position;
        }
    }
}
