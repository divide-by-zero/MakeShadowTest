using System.Threading;
using Cysharp.Threading.Tasks;

public class GameState<TContext>
{
    private IGameState currentState;
    public void ChangeState(TContext context, IGameState state)
    {
        currentState = state;
        var nextState = currentState?.Initialize(context);
        if (nextState != null && nextState != currentState)
        {
            ChangeState(context, nextState);
        }
    }

    public async UniTaskVoid Execute(TContext context,CancellationToken ct)
    {
        while (ct.IsCancellationRequested == false)
        {
            if (currentState == null) break;
            var nextState = await currentState.ExecuteAsync(context,ct);
            if (nextState != null && nextState != currentState)
            {
                ChangeState(context, nextState);
            }

            await UniTask.Yield();
        }
    }

    public interface IGameState
    {
        IGameState Initialize(TContext context);
        UniTask<IGameState> ExecuteAsync(TContext context,CancellationToken ct);
    }
}