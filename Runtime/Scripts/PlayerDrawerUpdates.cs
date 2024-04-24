using System;
using System.Linq;

namespace Hashbyte.GameboardGeneral
{
    internal class PlayerDrawerUpdates
    {
        public System.Collections.Generic.Dictionary<ePlayerDirection, PlayerPresence> PlayerDatabase;

        private System.Collections.Generic.List<IPlayerUpdates> playerUpdateListeners;
        public PlayerDrawerUpdates() {
            playerUpdateListeners = new System.Collections.Generic.List<IPlayerUpdates>();
            var gameboardObject = UnityEngine.GameObject.FindGameObjectWithTag("Gameboard");
            if (gameboardObject == null) throw new System.EntryPointNotFoundException("Gameboard SDK should be present in scene before using Hashbyte User Updates");
            Gameboard.UserPresenceController userPresenceController = gameboardObject.GetComponent<Gameboard.UserPresenceController>();
            userPresenceController.OnUserPresence += OnPlayerUpdate;
            if (userPresenceController.IsInitialized) Init(userPresenceController.Users);
            else userPresenceController.UserPresenceControllerInitialized += () => { Init(userPresenceController.Users); };
        }
        public void Init(System.Collections.Generic.Dictionary<string, Gameboard.EventArgs.GameboardUserPresenceEventArgs> existingUsers)
        {
            UnityEngine.Debug.Log($"Presence initialized {existingUsers.Count}");
            PlayerDatabase = 
            Enum.GetValues(typeof(ePlayerDirection)).Cast<ePlayerDirection>().ToDictionary(direction => direction, val => new PlayerPresence());
            foreach (string userId in existingUsers.Keys) {
                UpdateDatabase(existingUsers[userId]);
            }
        }        

        public void OnPlayerUpdate(Gameboard.EventArgs.GameboardUserPresenceEventArgs changeInfo)
        {
            UnityEngine.Debug.Log($"Presence update {changeInfo}");
            UpdateDatabase(changeInfo);
        }
        private void UpdateDatabase(Gameboard.EventArgs.GameboardUserPresenceEventArgs changeInfo)
        {
            ePlayerDirection playerPosition = changeInfo.boardUserPosition.GetDirection();
            PlayerDatabase[playerPosition].Update(changeInfo);
            foreach (IPlayerUpdates playerUpdates in playerUpdateListeners)
            {
                playerUpdates.OnPlayerLoginToDrawer(playerPosition, PlayerDatabase[playerPosition]);
            }
        }
        public void RegisterForUpdates(IPlayerUpdates playerUpdates)
        {
            if (!playerUpdateListeners.Contains(playerUpdates))
                playerUpdateListeners.Add(playerUpdates);
        }

        public void UnregisterForUpdates(IPlayerUpdates unregister)
        {
            playerUpdateListeners.Remove(unregister);
        }
    }

    public class PlayerPresence
    {
        public string id;
        public string userId;
        public string userName;
        public bool isLoggedIn;
        public PlayerPresence() { isLoggedIn = false; }
        public PlayerPresence(Gameboard.EventArgs.GameboardUserPresenceEventArgs userInfo)
        {
            Update(userInfo);
        }

        public void Update(Gameboard.EventArgs.GameboardUserPresenceEventArgs userInfo)
        {
            id = userInfo.id;
            userId = userInfo.userId;
            userName = userInfo.userName;
            isLoggedIn = true;
        }
    }
}
