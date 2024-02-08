using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] bool self_target;
    [SerializeField] SpriteRenderer sprite_target;
    [SerializeField] Sprite[] sprite_ani;
    [SerializeField] float time; // 외부적인 시간요소가 들어오면 배제
    [SerializeField] float waitTime;

    [SerializeField] Animator ani;
    [SerializeField] float delayTime;
    int reset_count = 0;

    public AudioSource audioSource;
    public AudioClip[] audioClipKnock;


    private void Start()
    {
        if (sprite_ani.Length == 0)
        {
            //Log.Warning($"Empty ani array in {gameObject.name}");
            enabled = false;
            return;
        }

        if (self_target)
        {
            sprite_target = GetComponent<SpriteRenderer>();
        }

        Invoke("OnTimerEnd", delayTime); // 외부적인 시간요소가 들어오면 배제
    }

    public void soundeffect(int numm)
    {
        audioSource.PlayOneShot(audioClipKnock[numm]);
    }
    //
    public void Aniwa(string trigg)
    {
        ani.SetTrigger(trigg);
    }
}