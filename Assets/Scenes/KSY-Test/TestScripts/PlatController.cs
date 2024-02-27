using System.Collections;
using UnityEngine;

public class PlatController : MonoBehaviour
{
    private Collider2D coll;
    private Coroutine ignoreCollisionCoroutine;
    private bool canIgnore;

    void Awake()
    {
        coll = Util.GetOrAddComponent<Collider2D>(gameObject);
        ignoreCollisionCoroutine = null;
        canIgnore = false;
    }

    void Update()
    {
        Actor actor = ActorController.Instance.CurrentActor;
        if (actor == null || actor.IsDead) return;

        if (Input.GetKey(KeyCode.W) && !actor.OnLadder)
        {
            if(ignoreCollisionCoroutine != null)
            {
                canIgnore = false;
                ignoreCollisionCoroutine = null;
            }
        }
        if (Input.GetKey(KeyCode.S) && (actor.CanGoDown && actor.OnGround))
        {
            if(ignoreCollisionCoroutine == null)
            {
                canIgnore = true;
                ignoreCollisionCoroutine = StartCoroutine(IgnoreCollisionCoroutine(actor));
            }
        }
    }

    private IEnumerator IgnoreCollisionCoroutine(Actor actor)
    {
        Physics2D.IgnoreCollision(coll, actor.Coll, true);
        while (actor.CurrentGround == gameObject && canIgnore)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Physics2D.IgnoreCollision(coll, actor.Coll, false);
    }

}
