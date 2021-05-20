using UnityEngine;

public class IndicatePowerCell : MonoBehaviour
{
    GameObject target;

    private void Start()
    {
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
        if (target == null)
            return;

        transform.right = target.transform.position - transform.position;
    }
}
