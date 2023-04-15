using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int MaxHealth = 100;

    public int health = 50;

    [Header("Heal properties")]
    [SerializeField] private int _healAmount = 5;
    [SerializeField] private float _healDuration = 3.0f;
    [SerializeField] private float _healTick = 0.5f;

    private void Start()
    {
        ReceiveHealing();
    }

    public void ReceiveHealing()
    {
        StartCoroutine(Heal(_healAmount, _healTick, _healDuration));
    }

    private IEnumerator Heal(int healAmount, float healTick, float healDuration)
    {
        Debug.Log($"Health at healing start: {health}");
        float healStart = Time.time;
        while (health + healAmount <= MaxHealth)
        {
            if (Time.time - healStart >= healDuration)
            {
                Debug.Log($"Health after heal cycle: {health}");
                Debug.Log("Heal cycle completed");
                yield break;
            }

            health += healAmount;
            yield return new WaitForSeconds(healTick);
            Debug.Log($"Current health: {health}, time healing: {Time.time - healStart:F2}");
        }
        if (health + healAmount > MaxHealth) health = MaxHealth; 
        yield return new WaitForSeconds(healTick);
        Debug.Log($"Current health: {health}, time healing: {Time.time - healStart:F2}");

        Debug.Log("Health is full");
        yield break;
    }
}
