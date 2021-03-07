using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public partial class GameMain
{
    private class TitleState : GameState<GameMain>.IGameState
    {
        public GameState<GameMain>.IGameState Initialize(GameMain context)
        {
            Physics2D.gravity = new Vector2(0, -10);
            Physics.gravity = Physics2D.gravity;
            return null;
        }

        public async UniTask<GameState<GameMain>.IGameState> ExecuteAsync(GameMain context, CancellationToken ct)
        {
            for(var cnt = 3;cnt >= 1;cnt--)
            {
                SoundManager.Instance.PlayRegisterSE("ポン");
                context.titleText.text = ""+cnt;
                await UniTask.Delay(500);
            }

            context.titleText.text = "GO!";
            SoundManager.Instance.PlayRegisterSE("ピン");
            await UniTask.Delay(500);
            context.titleText.enabled = false;
            return new MainState();
        }
    }
}