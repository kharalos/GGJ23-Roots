using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] private RectTransform tracker;
    [SerializeField] private UnityEngine.UI.Image trackerImage;
    [SerializeField] private TMPro.TMP_Text trackerTmp;
    [SerializeField] private float trackerLerpMultiplier = 3f;
    [SerializeField] private float trackerTextAlpha = .2f;
    
    public void SetWeaponTrackingInfo(Vector2 sizeDelta, Vector2 position, TrackedType type, string targetName = "", float targetDistance = 0f)
    {
        var targetPosition = new Vector2(position.x * sizeDelta.x - sizeDelta.x * 0.5f,
            position.y * sizeDelta.y - sizeDelta.y * 0.5f);

        tracker.anchoredPosition = Vector2.Lerp(
            tracker.anchoredPosition, targetPosition, Time.deltaTime * trackerLerpMultiplier);


        trackerTmp.text = type == TrackedType.None ?
            "" : $"{targetName} <color=#81FF88>{targetDistance:0.0}</color>";
        
        var targetColor = Color.white;
        switch (type)
        {
            case TrackedType.Neutral:
                targetColor = new Color(1f, 1f, 0.24f);
                break;
            case TrackedType.Friendly:
                targetColor = new Color(0.3f, 0.39f, 1f);
                break;
            case TrackedType.Enemy:
                targetColor = new Color(1f, 0.35f, 0.24f);
                break;
        }
        var targetSize = 1f;
        if (type != TrackedType.None)
        {
            targetSize = 1f - targetDistance / 100f;
            targetSize = Mathf.Clamp(targetSize, 0.3f, 1f);
        }
        
        trackerImage.transform.localScale = Vector3.Lerp(trackerImage.transform.localScale,
            Vector3.one * targetSize, Time.deltaTime * trackerLerpMultiplier);
        trackerImage.color = Color.Lerp(trackerImage.color, targetColor, Time.deltaTime * trackerLerpMultiplier);
        targetColor.a = trackerTextAlpha;
        trackerTmp.color = Color.Lerp(trackerTmp.color, targetColor, Time.deltaTime * trackerLerpMultiplier);
    }

    public void PlanetTracking(Vector2 sizeDelta, Vector2 position, EnemyManager.PlanetType planet, int enemyCount)
    {
        var colorCode = planet switch
        {
            EnemyManager.PlanetType.Mercury => "#800000ff",
            EnemyManager.PlanetType.Venus => "orange",
            EnemyManager.PlanetType.Earth => "blue",
            EnemyManager.PlanetType.Mars => "red",
            EnemyManager.PlanetType.Jupiter => "#c0c0c0ff",
            EnemyManager.PlanetType.Saturn => "yellow",
            EnemyManager.PlanetType.Uranus => "#008080ff",
            EnemyManager.PlanetType.Neptune => "#00ffffff",
            _ => "white"
        };

        var targetPosition = new Vector2(position.x * sizeDelta.x - sizeDelta.x * 0.5f,
            position.y * sizeDelta.y - sizeDelta.y * 0.5f);

        tracker.anchoredPosition = targetPosition;

        trackerTmp.text = $"<color={colorCode}>{planet}</color>\n<color=red>{enemyCount} Threats</color>";

        // get from colorcode
        var targetColor = planet switch
        {
            EnemyManager.PlanetType.Mercury => new Color(0.5f, 0f, 0f),
            EnemyManager.PlanetType.Venus => new Color(1f, 0.65f, 0f),
            EnemyManager.PlanetType.Earth => new Color(0f, 0f, 1f),
            EnemyManager.PlanetType.Mars => new Color(1f, 0f, 0f),
            EnemyManager.PlanetType.Jupiter => new Color(0.75f, 0.75f, 0.75f),
            EnemyManager.PlanetType.Saturn => new Color(1f, 1f, 0f),
            EnemyManager.PlanetType.Uranus => new Color(0f, 0.5f, 0.5f),
            EnemyManager.PlanetType.Neptune => new Color(0f, 1f, 1f),
            _ => Color.white
        };
        var targetSize = 1f - enemyCount / 100f;
        targetSize = Mathf.Clamp(targetSize, 0.3f, 1f);

        trackerImage.transform.localScale = Vector3.Lerp(trackerImage.transform.localScale,
            Vector3.one * targetSize, Time.deltaTime * trackerLerpMultiplier);
        trackerImage.color = targetColor;
        targetColor.a = trackerTextAlpha;
        trackerTmp.color = targetColor;
    }
}