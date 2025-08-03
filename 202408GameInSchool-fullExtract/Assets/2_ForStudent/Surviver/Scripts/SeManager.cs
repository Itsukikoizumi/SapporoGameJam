using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Hellmade.Sound;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// もし3d空間において効果音を出したい場合、こいつの子にやらせます。もとのオブジェクトが消えると音きえちゃうし
/// </summary>
public class SeManager : SingletonMonoBehaviour<SeManager>
{
    private List<Transform> _list = new List<Transform>();

    protected override void doAwake()
    {
        Prepare(20);
    }

    public void ClearAll()
    {
        foreach (var e in _list)
        {
            Destroy(e.gameObject);
        }
        _list.Clear();
    }

    public void HideAll()
    {
        foreach (var e in _list)
        {
            e.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 指定されたTransformと同じ位置に、オブジェクトを生成して、そこから音を出すようにします。
    /// </summary>
    /// <param name="name">ファイル名</param>
    /// <param name="volume">音量</param>
    /// <param name="transform">音を鳴らす根源</param>
    public static void Play(string name, float volume, Transform sourceTransform)
    {
        var obj = Instance.Create();

        // 同じ位置にオブジェクト用意
        obj.position = sourceTransform.position;

        EazySoundManager.PlaySound(AudioLoader.SE(name), volume, false, obj);
    }

    /// <summary>
    /// 非アクティブであらかじめ用意しておきます
    /// </summary>
    public void Prepare(int num)
    {
        // 必要数に対する、現状の数
        var diff = num - _list.Count();

        // 差の数だけ生成。 足りていれば負となり作られない
        for (var i = 0; i < diff; i++)
        {
            // 作って初期化して隠す
            var newObject = CreateInternal();
            newObject.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 暇しているオブジェを用意します。
    /// </summary>
    public Transform Create()
    {
        // 控えに回っているオブジェがいないか確認
        var freeObject = _list.FirstOrDefault(e => !e.gameObject.activeInHierarchy);

        if(_list.Count() >= 1000)
        {
            // 1000個以上になったらさすがに出させない。一応配列の頭のを使いまわさせる
            freeObject = _list[0];
        }

        if (freeObject == null)
        {
            // いなければ作成
            freeObject = CreateInternal();
        }


        freeObject.transform.parent = transform;
        freeObject.gameObject.SetActive(true);
        // 3秒後に消します
        DOVirtual.DelayedCall(2f, () => freeObject.gameObject.SetActive(false));

        return freeObject;
    }

    /// <summary>
    /// 新規に作って返します。
    /// </summary>
    protected virtual Transform CreateInternal()
    {
        // poolクラスをアタッチしているオブジェを親にします。
        var newObj = new GameObject();
        newObj.transform.parent = transform;

        // リストに登録
        _list.Add(newObj.transform);

        return newObj.transform;
    }
}
