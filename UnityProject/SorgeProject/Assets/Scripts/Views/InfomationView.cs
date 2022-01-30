using UnityEngine;
using UnityEngine.UI;

namespace SorgeProject.View
{
    public class InfomationView : MonoBehaviour
    {
        [SerializeField] Image cardImage;
        [SerializeField] Text titleStr;
        [SerializeField] Text powerValue;
        [SerializeField] Text moralValue;
        [SerializeField] Text trustValue;
        [SerializeField] Text profitValue;
        [SerializeField] Text costValue;
        [SerializeField] Text flavor;

        public void SetData(Object.InfomationBehaviour behaviour)
        {
            cardImage.sprite = Controller.InfomationController.GetCardSpriteByType(behaviour);
            titleStr.text = behaviour.Title;
            powerValue.text = behaviour.Power.ToString();
            moralValue.text = behaviour.Moral.ToString();
            trustValue.text = behaviour.Trust.ToString();
            profitValue.text = behaviour.SellCost.ToString();
            costValue.text = "$" + behaviour.Cost.ToString();
            flavor.text = behaviour.flavorString;
        }
    }
}