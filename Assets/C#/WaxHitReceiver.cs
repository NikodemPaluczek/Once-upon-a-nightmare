using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WaxHitReceiver : MonoBehaviour
{
    [SerializeField] private GameObject wax;
    private bool hasFirstHit = false;

    private void OnParticleCollision(GameObject other)
    {
        if (hasFirstHit) return;

        ParticleSystem ps = other.GetComponent<ParticleSystem>();
        if (ps == null) return;
        StartCoroutine(SpawnVaxCoroutine(ps));

    }
    private IEnumerator SpawnVaxCoroutine(ParticleSystem ps)
    {
        yield return new WaitForSeconds(2);
        List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
        int count = ps.GetCollisionEvents(gameObject, events);

        if (count > 0)
        {
            Vector3 hitPos = events[0].intersection;

            hasFirstHit = true;

            wax.transform.position = hitPos;
            SleepManager.OnSleepStateChanged += ShowHideWax;

        }
    }
    public void ShowHideWax(bool isSleeping)
    {
        wax.SetActive(!isSleeping);
    }
}