using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField] private float textureXOffsetSpeed = 1f;
    [SerializeField] private float textureYOffsetSpeed = 1f;
    
    private Material _material;
    private Transform _transform;
    private float _offsetX;
    private float _offsetY;
    void Awake()
    {
        var renderer = gameObject.GetComponent<Renderer>();
        _material = Instantiate(renderer.material);
        renderer.material = _material;
        _transform = transform;
    }

    private void Update()
    {
        _offsetX += Time.deltaTime * textureXOffsetSpeed;
        _offsetY += Time.deltaTime * textureYOffsetSpeed;
        _material.mainTextureOffset = new Vector2(_offsetX, _offsetY);
    }
}
