using UnityEngine;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    [SerializeField] private Fade fade;

    public Fade Fade => fade;

    protected override void doAwake()
    {
    }

    public void FadeIn(float time, System.Action onComplete = null)
    {
        fade.FadeIn(time, onComplete);
    }

    public void FadeOut(float time, System.Action onComplete = null)
    {
        fade.FadeOut(time, onComplete);
    }

    /// <summary>
    /// フェードをインし、アウトする。
    /// シーン遷移とかに使う
    /// </summary>
    /// <param name="time">フェードにかかる時間。演出全体で2倍。</param>
    /// <param name="onFadeIn">画面を黒くしたあとに行うコールバック</param>
    /// <param name="onFadeOut">画面が明けたときのコールバック</param>
    public void FadeInOut(float time, System.Action onFadeIn = null, System.Action onFadeOut = null)
    {
        fade.FadeIn(time, () => {
            onFadeIn?.Invoke();
            fade.FadeOut(time, onFadeOut); 
        });
    }
}