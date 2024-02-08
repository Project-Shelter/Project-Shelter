using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtMonster_Dungeon
{
    public class PixelMonster : MonoBehaviour
    {

        //reference to objects inside the character prefab
        #region OBJECTS
        public Animator animator;
        public new Renderer renderer;
        public GameObject fx;
        #endregion

        //reference to relevant prefabs
        #region PREFAB
        public GameObject dieFxPrefab;
        #endregion

        //those parameters should only be changed in runtime, mainly wrappers for animator parameters
        #region RUNTIME

        //character facing  1:facing right   -1:facing left
        [ExposeProperty]
        public int Facing
        {
            get { return facing; }
            set
            {
                if (value == 0) return;
                facing = value;

                animator.transform.localScale = new Vector3(1.0f, 1.0f, facing);

                //Vector3 pos = animator.transform.localPosition;
                //pos.x = 0.064f * -facing;
                //animator.transform.localPosition = pos;
            }
        }
        [SerializeField, HideInInspector]
        private int facing = 1;

        //is the character hiding? only work for certain characters
        [ExposeProperty]
        public bool IsHiding
        {
            get { return isHiding; }
            set
            {
                isHiding = value;
                animator.SetBool("IsHiding", isHiding);
            }
        }
        [SerializeField, HideInInspector]
        private bool isHiding;

        //is the character in jump prepare mode ( the animation played before the character actually jump)
        [ExposeProperty]
        public bool IsInJumpPrepare
        {
            get { return isInJumpPrepare; }
            set
            {
                isInJumpPrepare = value;
                animator.SetBool("IsJumpPrepare", isInJumpPrepare);
            }
        }
        [SerializeField, HideInInspector]
        private bool isInJumpPrepare;

        //is the character on ground?
        [ExposeProperty]
        public bool IsGrounded
        {
            get { return isGrounded; }
            set
            {
                isGrounded = value;
                animator.SetBool("IsGrounded", isGrounded);
            }
        }
        [SerializeField, HideInInspector]
        private bool isGrounded;

        //is the character dead?
        [ExposeProperty]
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                isDead = value;
                animator.SetBool("IsDead", isDead);

                if (fx) fx.SetActive(!isDead);
            }
        }
        [SerializeField, HideInInspector]
        private bool isDead;

        //moving animation blend
        //0.0:idle,  0.5:walk,  1.0:run
        [ExposeProperty]
        public float MovingBlend
        {
            get
            {
                return movingBlend;
            }
            set
            {
                movingBlend = value;
                animator.SetFloat("MovingBlend", movingBlend);
            }
        }
        [SerializeField, HideInInspector]
        private float movingBlend;

        //vertical speed
        //determines whether the animation should be jumping or falling
        public float SpeedVertical
        {
            get { return speedVertical; }
            set
            {
                speedVertical = value;
                animator.SetFloat("SpeedVertical", speedVertical);
            }
        }
        private float speedVertical;

        //is the character in attack animation
        public bool IsAttacking
        {
            get
            {
                var stateInfo = animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Attack"));

                if (stateInfo.IsName("Attack")) return true;
                return false;
            }
        }

        //when character get injured from front or back
        public void InjuredFront()
        {
            animator.SetTrigger("InjuredFront");
        }
        public void InjuredBack()
        {
            animator.SetTrigger("InjuredBack");
        }

        //perform an attack
        public void Attack()
        {
            animator.SetTrigger("Attack");
        }

        //handle OnDieFx event
        //instantiate the die fx prefab if there is
        public void OnDieFx()
        {
            if (dieFxPrefab)
            {
                Instantiate(dieFxPrefab, transform.position, Quaternion.identity, transform.parent);
            }
        }

        private void Start()
        {
            //set a random offset for loop animation
            animator.SetFloat("LoopCycleOffset", Random.value);
        }

        #endregion

    }
}
