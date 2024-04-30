using UnityEngine;
using TMPro;
namespace Hashbyte.GameboardGeneral
{
    public class ArmedPlayer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI playerName;

        public void OnArmed(string playerName)
        {
            this.playerName.text = playerName;
            gameObject.SetActive(true);
        }
    }
}
