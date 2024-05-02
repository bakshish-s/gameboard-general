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
        [SerializeField] private TMPro.TextMeshProUGUI armText;
        private bool isArmed;
        private ArmedPlayer armedPlayerObject;
        private PlayerPresence playerPresence;
        private bool isShowing;

        public void Awake()
        {
            //Show Login info    
            armingObject.SetActive(false);
            loginObject.SetActive(true);
        }
        public void UpdatePresence(PlayerPresence playerPresence)
        {            
            playerName.text = playerPresence.userName;
            this.playerPresence = playerPresence;
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
            if (isArmed)
            {
                armedPlayerObject.gameObject.SetActive(false);
                armText.text = "Arm Player";
                isArmed = false;
                ArmingLobby.Instance.CanBeArmed(direction, isArmed);
            }
        }

        public void GUI_ArmDisarm()
        {
            if (!ArmingLobby.Instance.CanBeArmed(direction, !isArmed)) return;
            isArmed = !isArmed;
            if (isArmed)
            {
                if(armedPlayerObject == null)armedPlayerObject = Instantiate(ArmingLobby.Instance.armedPlayerObj, ArmingLobby.Instance.armedPlayerObj.transform.parent).GetComponent<ArmedPlayer>();
                armedPlayerObject.transform.SetAsLastSibling();
                armedPlayerObject.OnArmed(playerPresence.userName);
                armText.text = "Dis-Arm";
            }
            else
            {
                armedPlayerObject.gameObject.SetActive(false);
                armText.text = "Arm Player";
            }
        }

        public void HideArming(bool hide)
        {
            if (hide)
            {
                isShowing = false;
                loginObject.SetActive(false);
                armingObject.SetActive(false);
            }else if (!isShowing)
            {
                if (playerPresence != null && playerPresence.isLoggedIn)
                {
                    armingObject.SetActive(true);
                }
                else
                {
                    loginObject.SetActive(true);
                }
                isShowing = true;
            }
        }
        
    }
}
