﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SkeletonManager : MonoBehaviour
{
    public float minHeight = -4;
    public float maxHeight = 2.5f;
    public float distanceFromCenter = 15;
    public GameObject skeletonPrefab;

    public float timeBetweenSpawn = 1;
    public float lastSpawnTime;

    public UnityEvent skeletonDestroyed;
    
    public void SpawnSkeleton()
    {
        var newSkeleton = Instantiate(skeletonPrefab);
        var heading = Random.Range(0, 2);
        var height = Random.Range(minHeight, maxHeight);
        if (heading == 0)
        {
            newSkeleton.transform.position = new Vector3(-distanceFromCenter, height, height);
        }
        else
        {
            newSkeleton.transform.Rotate(0,180,0);
            newSkeleton.transform.position = new Vector3(distanceFromCenter, height, height);
        }

        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime > timeBetweenSpawn)
        {
            SpawnSkeleton();
        }

        if (Input.touchCount > 0)
        {
            var camRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(Vector2OriginFromRay(camRay), Vector2DirectionFromRay(camRay));
            if (hit.collider != null)
            {
                Debug.Log("hit");
                var walkerScript = hit.collider.gameObject.GetComponent<WalkerScript>();
                if (walkerScript != null)
                {
                    skeletonDestroyed.Invoke();
                    AddExperience();
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(Vector2OriginFromRay(camRay), Vector2DirectionFromRay(camRay));
            if (hit.collider != null)
            {
                Debug.Log("hit");
                var walkerScript = hit.collider.gameObject.GetComponent<WalkerScript>();
                if (walkerScript != null)
                {
                    skeletonDestroyed.Invoke();
                    AddExperience();
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    void AddExperience()
    {
        PlayerData.PlayerExperience++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).origin + (Camera.main.ScreenPointToRay(Input.mousePosition).direction * float.MaxValue));
    }

    private Vector2 Vector2OriginFromRay(Ray input)
    {
        return new Vector2(input.origin.x, input.origin.y);
    }
    private Vector2 Vector2DirectionFromRay(Ray input)
    {
        return new Vector2(input.direction.x, input.direction.y);
    }
}
