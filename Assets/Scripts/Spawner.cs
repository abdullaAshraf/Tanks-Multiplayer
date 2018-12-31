using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject enemy;
    public Transform[] spwanSpots;

    private float timeBtwSpawns;
    public float startTimeBtwSpawns;

	// Use this for initialization
	void Start () {
        timeBtwSpawns = startTimeBtwSpawns;
    }
	
	// Update is called once per frame
	void Update () {
		if(timeBtwSpawns <= 0)
        {
            int randPos = Random.Range(0, spwanSpots.Length);
            Instantiate(enemy, spwanSpots[randPos].position , Quaternion.identity);
            timeBtwSpawns = startTimeBtwSpawns;
        }
        else
        {
            //timeBtwSpawns -= Time.deltaTime;
        }
	}
}
