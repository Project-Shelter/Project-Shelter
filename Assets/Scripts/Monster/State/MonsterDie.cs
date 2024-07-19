using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MonsterDie : MonsterBaseState
{
    public MonsterDie(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        StateMachine.Owner.Coll.enabled = false;
        // FallDown -> Blink -> SetActive(false)
        StateMachine.Owner.StartCoroutine(FallDown(5f));
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {

    }

    protected override void ChangeFromState()
    {
        if(!StateMachine.Owner.IsDead)
        {
            // 추후 리스폰 구현
            //StateMachine.SetState("Respawn");
        }
    }

    private IEnumerator FallDown(float rotateByFrame)
    {
        float fallDir = -1 * Mathf.Sign(StateMachine.Owner.GetVelocity().x);
        float targetAngle = 90 * fallDir;
        float angle = StateMachine.Owner.Tr.rotation.eulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;

        while (Mathf.Abs(angle - targetAngle) > 0.1f)
        {
            StateMachine.Owner.Tr.rotation = 
                Quaternion.Euler(0, 0, Mathf.MoveTowards(StateMachine.Owner.Tr.rotation.eulerAngles.z, targetAngle, rotateByFrame));
            angle = StateMachine.Owner.Tr.rotation.eulerAngles.z;
            angle = (angle > 180) ? angle - 360 : angle;
            yield return null;
        }

        StateMachine.Owner.StartCoroutine(Blink(0.2f, 5));
    }

    private IEnumerator Blink(float blinkTime, int repeatCount)
    {
        int count = 0;
        while (count < repeatCount)
        {
            float startTime = Time.time;
            while (startTime + blinkTime >= Time.time)
            {
                StateMachine.Owner.Sprite.color = new Color(0,0,0,0);
                yield return null;
            }
            startTime = Time.time;
            while (startTime + blinkTime >= Time.time)
            {
                StateMachine.Owner.Sprite.color = Color.white;
                yield return null;
            }
            count++;
        }

        StateMachine.Owner.gameObject.SetActive(false);
    }
}
