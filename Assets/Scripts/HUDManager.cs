using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Image fuelFillBar;
    [SerializeField] private Image boostSymbol;
    
    public void SetFuelFillBar(float fillAmount)
    {
        fuelFillBar.fillAmount = fillAmount;
    }
    
    public void SetBoostSymbol(bool exhausted)
    {
        boostSymbol.DOColor(exhausted ? Color.red : Color.white, .2f);
    }
}
