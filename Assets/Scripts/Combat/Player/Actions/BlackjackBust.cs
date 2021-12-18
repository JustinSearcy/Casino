//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BlackjackBust : MonoBehaviour
//{
//    [SerializeField] float introTime = 1f;
//    [SerializeField] float endRotation = 15f;
//    [SerializeField] float finalScale = 1f;
//    [SerializeField] GameObject bustParticles = null;

//    private void Start()
//    {
//        LeanTween.scale(this.gameObject, new Vector2(finalScale, finalScale), introTime).setEaseInQuint();
//        LeanTween.rotate(this.gameObject, new Vector3(0, 0, endRotation), introTime).setEaseInQuint();
//        StartCoroutine(Particles());
//    }

//    IEnumerator Particles()
//    {
//        yield return new WaitForSeconds(introTime);
//        GameObject particles = Instantiate(bustParticles, new Vector2(0, 1f), Quaternion.identity);
//        Destroy(particles, 1f);
//        FindObjectOfType<ChipSystem>().LoseChips(10);
//    }
//}
