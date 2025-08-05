using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public Item[] itemPrefabs;
    public float dropRadius = 12f;

    private Transform _playerTransform;
    private float timer = 0f;

    [SerializeField]
    private float interval = 20f;

    void Start()
    {
        _playerTransform  = GameObject.Find("Player").transform;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer>=interval)
        {
            DropItemNearPlayer();
            timer= 0f;
        }
    }
    void DropItemNearPlayer()
    {
        //if (_playerTransform == null || itemPrefabs.Length == 0) { return; }

        var playerPosition = _playerTransform.position;

        Vector2 randomCircle = Random.insideUnitCircle.normalized * dropRadius;
        Vector3 spwnPos = new Vector3(
            playerPosition.x + randomCircle.x,
            playerPosition.y + randomCircle.y);

        var randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
        var go = Instantiate(randomItem, spwnPos, Quaternion.identity, transform);
        Debug.Log("Dropped." + go.name);
    }
}


