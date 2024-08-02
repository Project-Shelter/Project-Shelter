using System.Collections;
using UnityEngine;

public class ActorDie : ActorBaseState
{
    public ActorDie(Actor actor) : base(actor) { }
    public override void EnterState()
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsDead, true);
        Actor.Coll.enabled = false;
        // FallDown -> Blink -> SetActive(false)
        Actor.StartCoroutine(FallDown(5f));
    }

    public override void UpdateState() 
    {
        base.UpdateState();
        //if(Actor.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    Managers.Instance.GameOverAction?.Invoke();
        //}
    }

    public override void FixedUpdateState() { }

    public override void UpdateWithNoCtrl() { }

    public override void ExitState() 
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsDead, Actor.IsDead);
    }

    protected override void ChangeFromState() { }


    private IEnumerator FallDown(float rotateByFrame)
    {
        float fallDir = -1 * Mathf.Sign(Actor.GetVelocity().x);
        float targetAngle = 90 * fallDir;
        float angle = Actor.Tr.rotation.eulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;

        while (Mathf.Abs(angle - targetAngle) > 0.1f)
        {
            Actor.Tr.rotation =
                Quaternion.Euler(0, 0, Mathf.MoveTowards(angle, targetAngle, rotateByFrame));
            angle = Actor.Tr.rotation.eulerAngles.z;
            angle = (angle > 180) ? angle - 360 : angle;
            yield return null;
        }

        Actor.StartCoroutine(Blink(0.2f, 5));
    }

    private IEnumerator Blink(float blinkTime, int repeatCount)
    {
        int count = 0;
        SpriteRenderer sprite = Actor.GetComponent<SpriteRenderer>();
        while (count < repeatCount)
        {
            float startTime = Time.time;
            while (startTime + blinkTime >= Time.time)
            {
                sprite.color = new Color(0, 0, 0, 0);
                yield return null;
            }
            startTime = Time.time;
            while (startTime + blinkTime >= Time.time)
            {
                sprite.color = Color.white;
                yield return null;
            }
            count++;
        }

        Actor.gameObject.SetActive(false);
        GameScene scene = Managers.SceneManager.CurrentScene as GameScene;

        if(Actor.Controller.CurrentActor == Actor) scene.GameOverAction?.Invoke();
    }
}
