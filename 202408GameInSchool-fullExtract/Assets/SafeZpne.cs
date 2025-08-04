using Surviver;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SafeZpne : MonoBehaviour
{
    /// <summary>
    /// 有効時間、単位は秒
    /// </summary>
    [SerializeField] private float Sec = 10f;

    [SerializeField] private string nextSceneName;
    [SerializeField] private int nextSceneIndex = -1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerCharacter"))
        {
            SceneManager.Instance.OnNextScene();
        }
    }
}   

 
