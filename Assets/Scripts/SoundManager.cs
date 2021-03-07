using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //超シンプルシングルトン
    public static SoundManager Instance { private set; get; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    /// <summary>
    /// 登録用データ AudioClip(Address) に別名を付ける
    /// </summary>
    [Serializable]
    public class RegisterSeData
    {
        public string name;
        public AudioClip audioClip;
    }

    /// <summary>
    /// 登録用データのリスト
    /// </summary>
    [SerializeField]
    private List<RegisterSeData> registerSeDataList;

    /// <summary>
    /// 管理用データ どのAudioClip(Address)をいつ、どのAudioSourceで再生したのかを管理
    /// </summary>
    class SoundFormat
    {
        public AudioClip soundAddress;
        public int playedFrame;
        public int startSample;
        public AudioSource playedAudioSource;
    }

    /// <summary>
    /// AudioSource(音の発生源)を先に用意 とりあえず20
    /// </summary>
    private AudioSource[] audioSourceList = new AudioSource[20];

    /// <summary>
    /// ピッチ変更のための値
    /// </summary>
    private readonly float pitchUnit = Mathf.Pow(2.0f, 1.0f / 12.0f);

    private void Start()
    {
        foreach (var registerSeData in registerSeDataList)
        {
            RegisterSe(registerSeData.name,registerSeData.audioClip);
        }
        var s = Clips.RandomAt();
        PlayBGM(s, true);
    }

    public AudioClip[] Clips;

    void Update()
    {
        if (AudioBgmPlayers.Source.loop == false)
        {
            if (AudioBgmPlayers?.Source.time >= AudioBgmPlayers.Source.clip.length - 3.0f)
            {
                var s = Clips.RandomAt();    //今再生しているもの以外の曲を選ぶ?
                PlayBGM(s, false);
            }
        }
    }

    private const int playableFrameDistance = 5;
    private Dictionary<string, SoundFormat> sndList = new Dictionary<string, SoundFormat>();
    
    public void RegisterSe(string name, AudioClip se, int startSample = 0)
    {
        //登録済み
        if (sndList.ContainsKey(name))
        {
            return;
        }

        var snd = new SoundFormat();
        snd.playedFrame = -1000;
        snd.startSample = startSample;
        snd.soundAddress = se;
        sndList.Add(name, snd);
    }

    public void PlayRegisterSE(string name, int pitch = 0)
    {
        if (sndList.ContainsKey(name) == false)
        {
            Debug.Log("<color=green>Target Sound Not Regist[" + name + "]</color>");
            return; //その名前登録されてないよ
        }
        var snd = sndList[name];
        if (Time.frameCount - snd.playedFrame < playableFrameDistance) return; //そんな短いフレームでの再生はゆるざん！

        snd.playedFrame = Time.frameCount;
        Play(GetEmptyAudioSource(), snd.soundAddress, snd.startSample, pitch);
    }

    public void Play(AudioSource audio, AudioClip clip, int startSample, int pitch = 0)
    {
        if (audio == null) return;
        // audio.outputAudioMixerGroup = MixerSeGroup;
        audio.clip = clip;
        audio.pitch = Mathf.Pow(pitchUnit, pitch);
        audio.timeSamples = startSample;
        audio.volume = 1.0f;
        audio.loop = false;
        audio.Play();
    }


    private AudioSource GetEmptyAudioSource()
    {
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            if (audioSourceList[i] == null)
            {
                audioSourceList[i] = gameObject.AddComponent<AudioSource>();
                return audioSourceList[i];
            }
            if (audioSourceList[i].isPlaying == false)
            {
                return audioSourceList[i];
            }
        }
        return null;
    }

    public AudioPlayer AudioBgmPlayers { get; set; }
    public void PlayBGM(AudioClip clip, bool isLoop = true)
    {
        if (AudioBgmPlayers != null && AudioBgmPlayers.Source.clip == clip) return;//すでに同じ曲がBGMになっている場合は再生スキップ
        var mainAuidioPlayer = gameObject.AddComponent<AudioPlayer>();
        mainAuidioPlayer.Source = GetEmptyAudioSource();
        mainAuidioPlayer.Source.loop = isLoop;
        mainAuidioPlayer.Source.clip = clip;
        mainAuidioPlayer.IsFade = true;
        mainAuidioPlayer.FadeInSeconds = 1.5f;

        mainAuidioPlayer.Play();
        AudioBgmPlayers = mainAuidioPlayer;
    }
}