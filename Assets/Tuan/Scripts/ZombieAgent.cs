using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace tuan
{
    public class ZombieAgent : Agent
    {
        [SerializeField]CharacterController controller;
        [SerializeField] float speed;
        [SerializeField] Transform groundCheck;
        [SerializeField] LayerMask groundMask;
        [SerializeField] float groundDistance;
        [SerializeField] float gravity;
        [SerializeField] float jumpHeight;
        Vector3 velocity, moveVec;
        [SerializeField]bool isGrounded;

        private void FixedUpdate()
        {
            ZombiePhysic();
        }

        public override void OnEpisodeBegin()
        {
            transform.localPosition = new Vector3(-2, 0.9f, 0);
        }
        public override void OnActionReceived(ActionBuffers actions)
        {
            int moveForward = actions.DiscreteActions[0];

            if (moveForward == 1)
            {
                if (isGrounded)
                {
                    moveVec = transform.forward.normalized;
                }
                else
                {
                    moveVec = Vector3.zero;
                }
            }

            int jump = actions.DiscreteActions[1];
            if (jump == 1)
            {
                if (isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }
            int turn = actions.DiscreteActions[2];
            switch (turn)
            {
                case 2:
                    transform.Rotate(Vector3.up, 360 * Time.fixedDeltaTime);
                    break;
                case 1:
                    transform.Rotate(Vector3.up, -360 * Time.fixedDeltaTime);
                    break;
            }
        }
        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.localPosition);
        }
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<int> movement = actionsOut.DiscreteActions;
            movement[0] = Input.GetKey(KeyCode.W) ? 1 : 0;
            movement[1] = Input.GetKey(KeyCode.Space) ? 1 : 0;
            movement[2] = Input.GetKey(KeyCode.A) ? (Input.GetKey(KeyCode.D) == Input.GetKey(KeyCode.A) ? 0 : 1) : Input.GetKey(KeyCode.D) ? 2 : 0;
        }

        void ZombiePhysic()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * 2 * Time.fixedDeltaTime;
            controller.Move(velocity * Time.fixedDeltaTime);
            if (isGrounded)
            {
                controller.Move(moveVec * speed * Time.fixedDeltaTime);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag== "Wall")
            {
                SetReward(-1);
                EndEpisode();
            }
            if (collision.gameObject.tag == "Player")
            {
                SetReward(1);
                EndEpisode();
            }
        }
    }
}


