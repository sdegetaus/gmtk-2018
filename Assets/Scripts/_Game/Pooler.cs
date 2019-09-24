﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pooler : MonoBehaviour {

    [Serializable]
    public struct Pool {
        public PoolTag type;
        public int count;
        public GameObject prefab;
    }

    [Header("Pools")]
    public Pool obstacleGroupPool;
    public Pool arrowsPool;

    // Private Variables
    private List<Pool> pools = new List<Pool>();
    private Dictionary<PoolTag, Queue<GameObject>> poolGroup = new Dictionary<PoolTag, Queue<GameObject>>();

    // Class References
    private GameManager gameManager;

    private void Start() {
        pools.Clear();
        pools.Add(obstacleGroupPool);
        pools.Add(arrowsPool);
    }

    public void InitializePool(UnityAction finishedCallback = null) {
        gameManager = GameManager.instance;
        StartCoroutine(
            InitializePoolCoroutine(finishedCallback)
        );
    }

    private IEnumerator InitializePoolCoroutine(UnityAction onFinished = null) {

        // initialize obstacleGroupPool
        Queue<GameObject> poolObject = new Queue<GameObject>();
        Transform poolObjectsTransform = (!Helper.IsMobile()) ? gameManager.obstacleManager.gameObject.transform : null;

        for (int i = 0; i < obstacleGroupPool.count; i++) {
            GameObject obj = Instantiate(obstacleGroupPool.prefab, poolObjectsTransform);
            obj.name += " " + i;
            obj.SetActive(false);
            poolObject.Enqueue(obj);
            yield return new WaitForEndOfFrame();
        }
        poolGroup.Add(obstacleGroupPool.type, poolObject);

        yield return null;

        // initialize arrowsPool
        poolObject = new Queue<GameObject>();
        poolObjectsTransform = (!Helper.IsMobile()) ? gameManager.arrowsManager.gameObject.transform : null;

        for (int i = 0; i < arrowsPool.count; i++) {
            GameObject obj = Instantiate(arrowsPool.prefab, poolObjectsTransform);
            obj.name += " " + i;
            obj.SetActive(false);
            poolObject.Enqueue(obj);
            yield return new WaitForEndOfFrame();
        }
        poolGroup.Add(arrowsPool.type, poolObject);

        yield return null;

        onFinished?.Invoke();
    }

    public GameObject Spawn(PoolTag tag, Vector3 position = default) {

        if (!poolGroup.ContainsKey(tag)) {
            Debug.LogError("PoolTag of type " + tag.ToString() + " doesn't exist!");
            return null;
        }

        GameObject objectToSpawn = poolGroup[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = Quaternion.identity;

        poolGroup[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}