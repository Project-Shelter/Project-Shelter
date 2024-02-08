using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cainos.PixelArtMonster_Dungeon
{
    public class MonsterFlyingController : MonoBehaviour
    {
        public float speedMax = 2.0f;                                   // max move speed
        public float acc = 8.0f;                                        // acceleration
        public float brakeAcc = 8.0f;                                   // brake acceleration

        public Vector2 groundCheckSize = new Vector2(0.1f, 0.1f);       // size of the box at the character's bottom to determine whether the character is on ground
        public float movingBlendTransitionSpeed = 2.0f;                 // the transition speed of moving blend value
        public float deadGravityScale = 1.0f;                           // gravity scale when the character is dead. it actually controls whether should the flying character fall to the ground when it is dead, in most case you should set this to either 0.0 or 1.0

        [ExposeProperty]                                                // is the character dead? if dead, plays dead animation and disable control
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                isDead = value;
                pm.IsDead = isDead;

                rb2d.gravityScale = isDead ? deadGravityScale : 0.0f;
            }
        }
        private bool isDead;

        //RUNTIME INPUT PARAMETERS
        public Vector2 inputMove = Vector2.zero;                        // movement input, x for horizontal, y for vertical, x and y should be in [-1.0, 1.0]                 
        public bool inputAttack = false;                                // attack input

        private PixelMonster pm;                                        // the PixelMonster script attached the character
        private Collider2D collider2d;                                  // Collider compoent on the character
        private Rigidbody2D rb2d;                                       // Rigidbody2D component on the character

        private bool isGrounded;                                        // is the character on ground?
        private bool isMoving;                                          // is the character moving?
        private Vector2 curVel;                                         // current velocity

        private Collider2D[] groundCheckResult = new Collider2D[2];
        private ContactFilter2D contactFilter2D = new ContactFilter2D();

        private void Awake()
        {
            pm = GetComponent<PixelMonster>();
            collider2d = GetComponent<Collider2D>();
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {

            //SET MOVING BLEND
            float movingBlend = Mathf.Abs(inputMove.x);
            pm.MovingBlend = Mathf.MoveTowards(pm.MovingBlend, movingBlend, Time.deltaTime * movingBlendTransitionSpeed);

            pm.SpeedVertical = Mathf.MoveTowards(pm.SpeedVertical, inputMove.y, Time.deltaTime * movingBlendTransitionSpeed);

            //PERFORM MOVE OR ATTACK BASED ON INPUT
            Move(inputMove);
            Attack(inputAttack);

            //CHECK IF THE CHARACTER IS ON GROUND
            isGrounded = false;
            for (int i = 0; i < groundCheckResult.Length; i++) groundCheckResult[i] = null;
            Physics2D.OverlapBox(transform.position, groundCheckSize, 0, contactFilter2D, groundCheckResult);
            foreach (var c in groundCheckResult)
            {
                if (c && c.gameObject != gameObject) isGrounded = true;
            }

            pm.IsGrounded = isGrounded;
        }

        public void Move( Vector2 inputMove )
        {
            if (IsDead)
            {
                if (deadGravityScale > 0.0f) return;
                else inputMove = Vector2.zero;
            }

            //GET CURRENT SPEED FROM RIGIDBODY
            curVel = rb2d.velocity;

            //HANDLE MOVEMENT
            //has movement input
            if ( inputMove.magnitude > 0.01f )
            {
                isMoving = true;

                //if current speed is out of allowed range, let it fall to the allowed range
                bool shouldMove = true;
                if (curVel.magnitude > speedMax )
                {
                    curVel = Vector2.MoveTowards(curVel, speedMax * curVel.normalized, brakeAcc * Time.deltaTime);
                    shouldMove = false;
                }

                //otherwise, add movement acceleration to cureent velocity
                if (shouldMove)
                {
                    curVel = Vector2.MoveTowards(curVel, inputMove.normalized * speedMax , acc * Time.deltaTime );
                }
            }
            //no movement input, brake to speed zero
            else
            {
                isMoving = false;
                curVel = Vector2.MoveTowards(curVel, Vector2.zero, brakeAcc * Time.deltaTime);
            }

            rb2d.velocity = curVel;

            pm.Facing = Mathf.RoundToInt(inputMove.x);
        }

        public void Attack(bool inputAttack)
        {
            if (inputAttack) pm.Attack();
        }

        private void OnDrawGizmosSelected()
        {
            //draw the ground check box
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, groundCheckSize);
        }
    }
}
