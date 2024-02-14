using System.Collections;
using UnityEngine;

// MonoBehaviour를 상속받지 않은 클래스에서 Coroutine 사용을 돕는 클래스
public class CoroutineHandler : MonoBehaviour
{
    static protected CoroutineHandler instance;
    static public CoroutineHandler Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject o = new GameObject("CoroutineHandler");
                instance = o.AddComponent<CoroutineHandler>();
            }

            return instance;
        }
    }

    public void OnDisable()
    {
        if (instance)
            Destroy(instance.gameObject);
    }

    static public Coroutine StartStaticCoroutine(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }
    static public void StopStaticCoroutine(Coroutine coroutine)
    {
        Instance.StopCoroutine(coroutine);
    }

    private void OnDestroy()
    {
        Instance.StopAllCoroutines();
    }
}