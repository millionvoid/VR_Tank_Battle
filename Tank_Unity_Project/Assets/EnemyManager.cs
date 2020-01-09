using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public GameObject player;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public float respawnTime = 5;

    private GameObject[] enemys;
    private float[] deathTime;


    void Start() {
        int n = spawnPoints.Length;
        enemys = new GameObject[n];
        deathTime = new float[n];
        for(int i = 0; i < n; ++i) {
            var e = Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation) as GameObject;
            e.GetComponent<NavMeshTest>().target = player.transform;
            enemys[i] = e;
            deathTime[i] = 0;
        }
    }

    void Update() {
        int n = spawnPoints.Length;
        for (int i = 0; i < n; ++i) {
            if(!enemys[i].activeInHierarchy) {
                if(deathTime[i] == 0) {
                    deathTime[i] = Time.time;
                } else if (deathTime[i] + respawnTime <= Time.time) {
                    enemys[i].SetActive(true);
                    enemys[i].transform.position = spawnPoints[i].position;
                    enemys[i].transform.rotation = spawnPoints[i].rotation;
                    deathTime[i] = 0;
                }
            }
        }
    }
}
