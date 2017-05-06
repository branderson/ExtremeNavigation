using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MoneyUIController : MonoBehaviour
    {
        [SerializeField] private Text _moneyText;

        public void SetMoney(int money)
        {
            _moneyText.text = "Money: " + money.ToString();
        }
    }
}