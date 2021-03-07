using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    /// <summary>
    /// オーディオソース。
    /// </summary>
    public AudioSource Source;

    /// <summary>
    /// フェードイン再生を行うかどうか。
    /// </summary>
    public bool IsFade;

    /// <summary>
    /// フェードインする時の時間。
    /// </summary>
    public double FadeInSeconds = 1.0;

    /// <summary>
    /// フェードイン再生中かどうか
    /// </summary>
    bool IsFadePlaying = false;

    /// <summary>
    /// フェードアウト再生中かどうか
    /// </summary>
    bool IsFadeStopping = false;

    /// <summary>
    /// フェードアウトする時の時間。
    /// </summary>
    double FadeOutSeconds = 1.0;

    /// <summary>
    /// フェードイン/アウト経過時間。
    /// </summary>
    double FadeDeltaTime = 0;

    /// <summary>
    /// 基本ボリューム。
    /// </summary>
    float BaseVolume;

    public bool IsFinish { private set; get; }

    /// <summary>
    /// フレーム毎処理。
    /// </summary>
    void Update()
    {
        // フェードイン
        if (IsFadePlaying)
        {
            FadeDeltaTime += Time.deltaTime;
            if (FadeDeltaTime >= FadeInSeconds)
            {
                FadeDeltaTime = FadeInSeconds;
                IsFadePlaying = false;
            }
            Source.volume = (float)(FadeDeltaTime / FadeInSeconds) * BaseVolume;
        }

        // フェードアウト
        if (IsFadeStopping)
        {
            FadeDeltaTime += Time.deltaTime;
            if (FadeDeltaTime >= FadeOutSeconds)
            {
                FadeDeltaTime = FadeOutSeconds;
                IsFadePlaying = false;
                Stop();
            }
            Source.volume = (float)(1.0 - FadeDeltaTime / FadeOutSeconds) * BaseVolume;
        }

        if (IsFinish == false && Source.loop == false)
        {
            if (Source.isPlaying == false) IsFinish = true;
        }
    }

    /// <summary>
    /// 再生を行います。
    /// </summary>
    public void Play()
    {
        // 自分がBgmの場合には、他のBgmの再生を停止させます。
        var bgm = SoundManager.Instance.AudioBgmPlayers;
        if (bgm != null && bgm != this)
        {
            if (bgm.Source.isPlaying)
            {
                if (IsFade)
                {
                    bgm.StopFadeOut(FadeInSeconds);
                }
                else
                {
                    bgm.Stop();
                }
            }
        }

        BaseVolume = 0.3f;
        FadeDeltaTime = 0;
        Source.volume = 0;
        Source.Play();
        IsFadePlaying = true;
        IsFadeStopping = false;
        IsFinish = false;
    }

    /// <summary>
    /// 停止を行います。
    /// </summary>
    public void Stop()
    {
        Source.Stop();
        Destroy(this);
    }

    /// <summary>
    /// 停止を行います。
    /// <param name="fadeSec">フェードアウト完了までの秒数。</param>
    /// </summary>
    public void StopFadeOut(double fadeSec)
    {
        BaseVolume = Source.volume;
        FadeDeltaTime = 0;
        FadeOutSeconds = fadeSec;
        IsFadeStopping = true;
        IsFadePlaying = false;
    }

    /// <summary>
    /// 一時停止を行います。
    /// </summary>
    public void Pause()
    {
        Source.Pause();
    }
}