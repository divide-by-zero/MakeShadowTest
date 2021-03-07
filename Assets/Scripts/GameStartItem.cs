using UnityEngine;

public class GameStartItem : MonoBehaviour, IPlayerHitable
{
    public void PlayerHit(PlayerSpin player)
    {
        naichilab.RankingLoader.Instance.ShowRanking();
    }
}