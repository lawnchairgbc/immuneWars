﻿// Creep.cs
// General-purpose enemy movement and stats script. Also defines enemy behaviour.

using UnityEngine;
using System.Collections;

public class Creep : MonoBehaviour
{
    // Target for despawning
    private Transform target;

    // The navmesh agent for this 
    NavMeshAgent agent;

    /// <summary>
    /// How much health the unit currently has.
    /// </summary>
	public int Health = 50;

    /// <summary>
    /// How much damage the unit inflicts when it successfully leaks through.
    /// </summary>
    public int LeakDamage = 1;

    //The player script is referenced to count how many creeps are killed, for score and win condition.
    private GameObject scoreKeeperOb;

    // Use this for initialization
    void Start ()
    {
        // The creep is referenced with the NavMesh so it can interact.
        agent = GetComponent<NavMeshAgent>();

        // The creep references the scoreKeeper so that kill count and score can be kept.
        scoreKeeperOb = GameObject.Find("ScoreKeeper");

        // Creeps are instantiated each wave, and references are made to the despawn point for pathing.
        GameObject despawnGameObject = GameObject.FindWithTag("Despawn");

        if (despawnGameObject != null)
        {
            target = despawnGameObject.transform;
        }
        if (despawnGameObject == null)
        {
            Debug.Log("Cannot find despawn object");
        }

    }

    /// <summary>
    /// Called whenever enemy takes damage
    /// </summary>
    /// <param name="damage">How much damage the enemy will take</param>
	void TakeDamage(int damage)
    {
		Health -= damage;
		if (Health <= 0)
        {
            scoreKeeperOb.GetComponent<ScoreKeeper>().addScore();   //Run addScore function in ScoreKeeper.
            Destroy(gameObject);
		}
	}

    // Update
	void Update ()
    {
        if (agent && target)
        {
            agent.SetDestination(target.position);
        }
    }

    // OnTriggerEnter
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Despawn"))
        {
            scoreKeeperOb.GetComponent<ScoreKeeper>().enemiesMissed();   //Run addKills function in ScoreKeeper.
            Destroy(gameObject);
        }
    }

}
