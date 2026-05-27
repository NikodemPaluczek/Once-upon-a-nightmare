using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaxHitReceiver : MonoBehaviour
{
    [SerializeField] private GameObject wax;

    private bool hasFirstHit = false;

    private List<ParticleCollisionEvent> events =
        new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        if (hasFirstHit) return;

        ParticleSystem ps = other.GetComponent<ParticleSystem>();

        if (ps == null) return;

        int count = ps.GetCollisionEvents(gameObject, events);

        if (count > 0)
        {
            hasFirstHit = true;

            Vector3 hitPos = events[0].intersection;

            StartCoroutine(SpawnWaxCoroutine(hitPos));
        }
    }

    private IEnumerator SpawnWaxCoroutine(Vector3 hitPos)
    {
        yield return new WaitForSeconds(2f);

        wax.transform.position = hitPos;
        wax.SetActive(true);

        SleepManager.OnSleepStateChanged += ShowHideWax;
    }

    public void ShowHideWax(bool isSleeping)
    {
        wax.SetActive(!isSleeping);
    }
}