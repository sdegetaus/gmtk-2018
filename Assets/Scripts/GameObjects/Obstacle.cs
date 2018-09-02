﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Obstacle : MonoBehaviour, IPooledObject {
    public enum ObstacleType
    {
        SmallWall,
        MediumWall,
        LargeWall,
        ThornObstacle,
        Spring
    }
    [SerializeField] ObstacleType type;
    bool isJumpable;
    bool isMoveable;

    Image numberImage;
    public Sprite[] spritesNumbers;

    private void Start()
    {
        numberImage = GetComponentInChildren<Image>();
        switch (type)
        {
            case ObstacleType.SmallWall:
                isJumpable = true;
                isMoveable = true;
                break;
            case ObstacleType.MediumWall:
                isJumpable = false;
                isMoveable = true;
                break;
            case ObstacleType.LargeWall:
                isJumpable = false;
                isMoveable = false;
                break;
            case ObstacleType.ThornObstacle:
                isJumpable = true;
                isMoveable = false;
                break;
        }

    }

    public void OnObjectSpawn(Vector3 spawnTransform)
    {
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                if(!type.Equals(ObstacleType.ThornObstacle))
                    this.transform.position = spawnTransform + new Vector3(0, 0, 1.5f);
                break;
            case 1:
                if (type.Equals(ObstacleType.LargeWall))
                {
                    r = Random.Range(1, 3);
                    this.transform.position = spawnTransform + new Vector3(0, 0, 1.5f * Mathf.Pow(-1, r));
                }
                break;
            case 2:
                if (!type.Equals(ObstacleType.ThornObstacle))
                    this.transform.position = spawnTransform + new Vector3(0, 0, -1.5f);
                break;
        }

        switch (type)
        {
            case ObstacleType.ThornObstacle:
                ElementSpawner.instance.InstantiateSpring();
                break;
        }
    
    }

    public void SetUpNumber(int num)
    {
        numberImage.sprite = spritesNumbers[num - 1];
    }

    public void Translate(Vector3 translation)
    {

    }
}
