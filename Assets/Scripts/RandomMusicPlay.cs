using UnityEngine;
using System.Collections;

public class RandomMusicPlay : MonoBehaviour
{
    public AudioClip[] Clips;
	// Use this for initialization
	void Start ()
    {
        var s = Clips.RandomAt();
        SoundManager.Instance.PlayBGM(s,true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (SoundManager.Instance.AudioBgmPlayers.Source.loop == false)
        {
            if (SoundManager.Instance.AudioBgmPlayers?.Source.time >= SoundManager.Instance.AudioBgmPlayers.Source.clip.length - 3.0f)
            {
                var s = Clips.RandomAt();    //今再生しているもの以外の曲を選ぶ
                SoundManager.Instance.PlayBGM(s, false);
            }
        }
    }
}
