using System.Collections;
using UnityEngine;
namespace Hashbyte.GameboardGeneral
{
    public class ArmingWidget : MonoBehaviour
    {
        public ePlayerDirection direction;
        [SerializeField] private GameObject armingObject;
        [SerializeField] private GameObject loginObject;
        [SerializeField] private TMPro.TextMeshProUGUI playerName;
        private bool isArmed;
        private PlayerPresence playerPresence;

        public void Awake()
        {
            //Show Login info    
            armingObject.SetActive(false);
            loginObject.SetActive(true);
        }
        public void UpdatePresence(PlayerPresence playerPresence)
        {
            this.playerPresence = playerPresence;
            playerName.text = playerPresence.userName;
            if (playerPresence.isLoggedIn)
            {
                loginObject.SetActive(false);
                armingObject.SetActive(true);
            }
            else
            {
                loginObject.SetActive(true);
                armingObject.SetActive(false);
            }
            Debug.Log("Presence updated");
        }        

        public void GUI_ArmDisarm()
        {
            isArmed = !isArmed;
            ArmingLobby.Instance.PlayerArmed(direction, isArmed);
        }
    }
}
