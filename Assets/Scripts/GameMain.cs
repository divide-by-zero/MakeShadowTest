using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameMain : MonoBehaviour
{
    [SerializeField]
    private int RequireFlagCount;

    [SerializeField]
    private PlayerSpin player;

    [SerializeField]
    private TMP_Text flagText;

    [SerializeField]
    private TMP_Text titleText;

    private GameState<GameMain> state = new GameState<GameMain>();

    // Start is called before the first frame update
    void Start()
    {
        state.ChangeState(this,new TitleState());
        state.Execute(this,gameObject.GetCancellationTokenOnDestroy()).Forget();
    }

    // Update is called once per frame
    void Update()
    {
        flagText.text = $"{player.FlagCnt}/{RequireFlagCount}";
    }

    class MainState : GameState<GameMain>.IGameState
    {
        private float time;
        public GameState<GameMain>.IGameState Initialize(GameMain context)
        {
            context.player.Pause(false);
            return null;
        }

        public async UniTask<GameState<GameMain>.IGameState> ExecuteAsync(GameMain context,CancellationToken ct)
        {
            time += Time.deltaTime;

            if (context.player.FlagCnt >= context.RequireFlagCount)
            {
                return new ResultState(TimeSpan.FromSeconds(time));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("TitleScene");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("2DGame");
            }
            return null;
        }
    }

    class ResultState : GameState<GameMain>.IGameState
    {
        private bool IsUnload { set; get; }
        public ResultState(TimeSpan time)
        {
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(time);

            void OnSceneManagerOnsceneUnloaded(Scene _)
            {
                IsUnload = true;
                SceneManager.sceneUnloaded -= OnSceneManagerOnsceneUnloaded;
            }

            SceneManager.sceneUnloaded += OnSceneManagerOnsceneUnloaded;
        }

        public GameState<GameMain>.IGameState Initialize(GameMain context)
        {
            return null;
        }

        public async UniTask<GameState<GameMain>.IGameState> ExecuteAsync(GameMain context, CancellationToken ct)
        {
            //Result（Ranking)画面が閉じられたら、タイトルへ
            if (IsUnload)
            {
                SceneManager.LoadScene("TitleScene");
            }
            return null;
        }
    }
}

