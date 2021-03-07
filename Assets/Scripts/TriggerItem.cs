using UnityEngine;
using UnityEngine.Events;

public class TriggerItem : MonoBehaviour, IPlayerHitable
{
    public UnityEvent OnPlayerHit;

    public void PlayerHit(PlayerSpin player)
    {
        OnPlayerHit?.Invoke();
    }
}