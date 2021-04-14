﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ManageIndicators : MonoBehaviour
{
    [SerializeField] UnityEngine.Camera mainCamera;
    [SerializeField] Transform _playerTransform;
    [SerializeField] Indy _prefab;

    Indy indicator;
    Vector3 normalizer = new Vector3(0.5f, 0.5f, 0);
    List<Indy> _indicators = new List<Indy>();
    public List<Transform> asteroids = new List<Transform>();

    private float xPos;
    private float yPos;
    private int currentIndex;

    void LateUpdate()
    {
        // TODO instantiate and track arrows
        // - prefill queue?
        // TODO add asteroid to queue in OnDequeue (OnExitPool)
        // - done (kinda)
        // TODO set asteroid . indicator
        // TODO set indicators to inactive
        // - done

        //var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        currentIndex = 0;

        // asteroids add themselves to this List via their OnExitPool event
        // it's not really a separation of concerns...
        foreach (Transform asteroidTransform in asteroids)
        {
            // get relevant indicator
            try
            {
                indicator = _indicators[currentIndex];
            }
            catch (ArgumentOutOfRangeException)
            {
                indicator = Instantiate(_prefab);
                _indicators.Add(indicator);
            }

            Vector3 screenPosition = mainCamera.WorldToViewportPoint(asteroidTransform.position);
            if (screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y < 0 || screenPosition.y > 1)
            {
                indicator.GetComponent<Renderer>().enabled = true;

                screenPosition -= normalizer;
                float slope = screenPosition.y / screenPosition.x;
                if (Mathf.Abs(screenPosition.x) > Mathf.Abs(screenPosition.y))
                {
                    // left/right => solving for y
                    if (screenPosition.x > 0)
                    {
                        xPos = 0.5f;
                    }
                    else
                    {
                        xPos = -0.5f;
                    }
                    yPos = slope * xPos;
                }
                else
                {
                    // top/bottom => solving for x
                    if (screenPosition.y > 0)
                    {
                        yPos = 0.5f;
                    }
                    else
                    {
                        yPos = -0.5f;
                    }
                    xPos = yPos / slope;
                }

                xPos = Mathf.Clamp(xPos + 0.5f, 0.03f, 0.97f);
                yPos = Mathf.Clamp(yPos + 0.5f, 0.05f, 0.95f);
                indicator.transform.position = mainCamera.ViewportToWorldPoint(new Vector3(xPos, yPos, screenPosition.z));

            } else
            {
                // don't display if asteroid is visible
                indicator.GetComponent<Renderer>().enabled = false;
            }
            // increment current index
            currentIndex += 1;
        }

        for (int i = currentIndex; i < _indicators.Count; i++)
        {
            indicator = _indicators[currentIndex];
            indicator.gameObject.SetActive(false);
        }
    }
}
