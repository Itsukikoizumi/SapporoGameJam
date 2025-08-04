using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// オブジェクトプールを行うクラスの基底クラスです。
/// 生成したオブジェクトを非アクティブで保持しておき、必要なときにアクティブにして使いまわします。
/// </summary>
/// <typeparam name="T"></typeparam>
public class PoolBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    protected T _prefab;

    protected List<T> _list = new List<T>();

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
    public T Create()
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
        
        return freeObject;
    }

    /// <summary>
    /// 新規に作って返します。
    /// </summary>
    protected virtual T CreateInternal()
    {
        // poolクラスをアタッチしているオブジェを親にします。
        var newObj = Instantiate(_prefab, this.transform);

        // リストに登録
        _list.Add(newObj);

        return newObj;
    }
}