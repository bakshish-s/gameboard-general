using System.Collections.Generic;
using UnityEngine;
namespace Hashbyte.GameboardGeneral
{
    public class ArmingLobby : MonoBehaviour, IPlayerUpdates
    {        
        [SerializeField] List<ArmingWidget> armingWidgets;
        private Dictionary<ePlayerDirection, ArmingWidget> playersMap;

        #region Singleton
        public static ArmingLobby Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Initialize();
        }
        #endregion

        #region Initialization
        private void Initialize()
        {
            playersMap = new Dictionary<ePlayerDirection, ArmingWidget>();            
            foreach(ArmingWidget armingWidget in armingWidgets)
            {
                if (!playersMap.ContainsKey(armingWidget.direction))
                {
                    Debug.Log($"Drawer added at position {armingWidget.direction}");
                    playersMap.Add(armingWidget.direction, armingWidget);
                }
                else
                {
                    throw new System.Exception($"Player with direction {armingWidget.direction} already exists");
                }
            }
            HashbyteUserUpdates.Instance.Register(this);
        }
        #endregion
        public void PlayerArmed(ePlayerDirection direction, bool isArmed)
        {

        }
        public void PlayerUpdate(ePlayerDirection direction, PlayerPresence playerPresence)
        {

        }

        public void OnPlayerLoginToDrawer(ePlayerDirection direction, PlayerPresence presence)
        {
            Debug.Log($"Player login to drawer received");
            playersMap[direction].UpdatePresence(presence);
        }

        public void OnPlayerLogout(ePlayerDirection direction, PlayerPresence presence)
        {
            Debug.Log($"Player logout to drawer received");
            playersMap[direction].UpdatePresence(presence);
        }

        public void OnPlayerChangePosition(ePlayerDirection direction, PlayerPresence presence)
        {
            Debug.Log($"Player position change received");
            playersMap[direction].UpdatePresence(presence);
        }

        public void PlayerDisarmedBySystem(ePlayerDirection direction, PlayerPresence presence)
        {
            
        }
    }
}
