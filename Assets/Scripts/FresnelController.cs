using Alchemy.Inspector;
using UnityEngine;

public class FresnelController : MonoBehaviour
{
    [SerializeField] private float fresnelPower = 5.0f;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private bool alwaysOn = false;
    [SerializeField] private float sineSpeed = 5.0f;
    [SerializeField] private float strength = 1.0f;
    
    private Material _material;
    
    private int _fresnelPowerID;
    private int _colorID;
    private int _alwaysOnID;
    private int _sineSpeedID;
    private int _strengthID;

    void Awake()
    {
        var renderer = gameObject.GetComponent<Renderer>();
        _material = Instantiate(renderer.material);
        renderer.material = _material;
        
        _fresnelPowerID = Shader.PropertyToID("_FresnelPower");
        _colorID = Shader.PropertyToID("_Color");
        _alwaysOnID = Shader.PropertyToID("_AlwaysOn");
        _sineSpeedID = Shader.PropertyToID("_SineSeed");
        _strengthID = Shader.PropertyToID("_Strength");

        UpdateFresnel();
    }

    [Button]
    public void UpdateFresnel()
    {
        _material.SetFloat(_fresnelPowerID, fresnelPower);
        _material.SetColor(_colorID, color);
        _material.SetFloat(_alwaysOnID, alwaysOn ? 1.0f : 0.0f);
        _material.SetFloat(_sineSpeedID, sineSpeed);
        _material.SetFloat(_strengthID, strength);
    }
}
