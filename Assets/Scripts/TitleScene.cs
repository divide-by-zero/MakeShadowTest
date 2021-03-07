using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField]
    private PlayerSpin player;

    private bool isFirst = true;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.gravity = new Vector2(0,-10);
        Physics.gravity = Physics2D.gravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirst && Input.GetMouseButtonDown(0))
        {
            player.Pause(false);
            isFirst = false;
        }
    }

    public void ShowRanking()
    {
        if (naichilab.RankingLoader.Instance.IsShowing) return;
        naichilab.RankingLoader.Instance.ShowRanking();
        player.Pause(true);
        naichilab.RankingLoader.Instance.OnUnload = () =>
        {
            player.Pause(false);
        };
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("2DGame");
    }

}
