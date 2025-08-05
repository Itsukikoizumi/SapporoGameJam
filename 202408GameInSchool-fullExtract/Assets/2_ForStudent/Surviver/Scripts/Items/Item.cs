using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Surviver;

public class Item : MonoBehaviour
{
    public enum Kind
    {
        Boots,
        Barrier,
        Bat,
        Stun,
    }

    public Kind kind;

    private PlayerCharacter _player;

    // Start is called before the first frame update
    void Start()
    {
        var obj = GameObject.Find("Player");
        _player = obj.GetComponent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 ItemHit = transform.position;
        Vector2 playeritem = _player.transform.position;
        Vector2 dir = ItemHit - playeritem;
        float d =dir.magnitude; ;
        float i1 = 1.0f;
        float P2 = 1.0f;

        if(d<i1+P2)
        {
            _player.ItemAbility(kind);

            Destroy(gameObject);
        }
    }
}
