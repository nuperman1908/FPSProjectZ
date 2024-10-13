using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
namespace Tuan
{
    public class BotController : MonoBehaviour
    {
        public NavMeshAgent agent;
        public Transform player;
        public LayerMask groundLayer, playerLayer;

        [Header("move")]
        public Vector3 walkPoint;
        public float walkPointRange, timeChangeWalkPoint;
        bool walkPointSet;

        [Header("Attack")]
        public float timeBtwAtk;
        public bool attacked;

        [Header("State")]
        public float sightRange;
        public float atkRange;
        public bool isPlayerInSight, isPlayerInAtkRange;

        private void Awake()
        {
            player = GameObject.Find("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }
        private void Update()
        {
            isPlayerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            isPlayerInAtkRange = Physics.CheckSphere(transform.position, atkRange, playerLayer);

            if (!isPlayerInSight && !isPlayerInAtkRange) Patrolling();
            if (isPlayerInSight && !isPlayerInAtkRange) ChasePlayer();
            if (isPlayerInSight && isPlayerInAtkRange) AttackPlayer();
        }

        private void Patrolling()
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
            {
                agent.SetDestination(walkPoint);
            }
            
        }

        private void SearchWalkPoint()
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomZ = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
            {
                walkPointSet = true;
            }
        }

        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
        }        

        private void AttackPlayer()
        {
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!attacked)
            {
                //code atk
                Debug.LogWarning("enemy atk");
                //

                attacked = true;
                Invoke(nameof(ResetAttack), timeBtwAtk);
            }
        }

        private void ResetAttack()
        {
            attacked = false;
        }

        
    }

}