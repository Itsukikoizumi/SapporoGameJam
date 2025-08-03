using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Glimmer : MonoBehaviour
{
    public Color Color1 = Color.cyan;
    public Color Color2 = Color.magenta;

    private bool _isColor1 = true;
    [SerializeField]
    private SpriteRenderer _sprite;

    private void Update()
    {
        _isColor1 = !_isColor1;
        _sprite.color = _isColor1 ? Color1 : Color2;
    }
}
