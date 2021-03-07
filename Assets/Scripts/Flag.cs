using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour,IPlayerHitable
{
    [SerializeField]
    private CrashParticle particlePrefab;

    public void PlayerHit(PlayerSpin player)
    {
        player.AddFlag();
        Destroy(gameObject);
        SoundManager.Instance.PlayRegisterSE("旗");
        Instantiate(particlePrefab,transform.position,transform.rotation).CreateParticle(1,1,0.1f,Color.cyan);
    }
}