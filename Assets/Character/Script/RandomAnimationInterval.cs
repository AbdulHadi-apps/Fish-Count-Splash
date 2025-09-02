using System.Collections;
using UnityEngine;

public class RandomAnimationInterval : MonoBehaviour
{
    public Animator animation;

    private void Start()
    {
        StartCoroutine(PlayAnimation());
    }
    IEnumerator PlayAnimation()
    {
        while (true)
        {
            float waitTime = Random.Range(5f, 8f);
            yield return new WaitForSeconds(waitTime);
            animation.SetTrigger("SideWave");
        }
  
    }
}
