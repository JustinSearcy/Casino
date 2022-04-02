using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretch : MonoBehaviour
{
    [SerializeField] Vector2 squash = new Vector2(1.02f, 0.98f);
    [SerializeField] Vector2 stretch = new Vector2(0.98f, 1.02f);
    [SerializeField] float speed = 0.75f;

    void Start()
    {
        StartCoroutine(StartSquashAndStretch());
    }

    IEnumerator StartSquashAndStretch()
    {
        LeanTween.scale(gameObject, squash, speed / 2);
        yield return new WaitForSeconds(speed / 2);
        LeanTween.scale(gameObject, stretch, speed).setLoopPingPong();
    }

    private void StopSquashAndStretch()
    {
        LeanTween.scale(gameObject, Vector2.one, speed);
    }
}
