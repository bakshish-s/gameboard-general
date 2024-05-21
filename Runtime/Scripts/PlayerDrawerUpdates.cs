using System;
using System.Linq;
using System.Collections.Generic;
using Gameboard.EventArgs;

namespace Hashbyte.GameboardGeneral
{
    internal class PlayerDrawerUpdates
    {
        public Dictionary<ePlayerDirection, PlayerPresence> PlayerDatabase;
        public Dictionary<string, ePlayerDirection> DirectionsMap;
        private List<IPlayerUpdates> playerUpdateListeners;
        public readonly Gameboard.DrawerController drawerController;
        public readonly Gameboard.EngagementController engagementController;        
        public PlayerDrawerUpdates()
        {
            playerUpdateListeners = new List<IPlayerUpdates>();
            DirectionsMap = new Dictionary<string, ePlayerDirection>();
            PlayerDatabase =
            Enum.GetValues(typeof(ePlayerDirection)).Cast<ePlayerDirection>().ToDictionary(direction => direction, val => new PlayerPresence());
            var gameboardObject = UnityEngine.GameObject.FindGameObjectWithTag("Gameboard");
            if (gameboardObject == null) throw new EntryPointNotFoundException("Gameboard SDK should be present in scene before using Hashbyte User Updates");
            Gameboard.UserPresenceController userPresenceController = gameboardObject.GetComponent<Gameboard.UserPresenceController>();
            drawerController = gameboardObject.GetComponent<Gameboard.DrawerController>();
            engagementController = gameboardObject.GetComponent<Gameboard.EngagementController>();            
            userPresenceController.OnUserPresence += OnPlayerUpdate;
            if (userPresenceController.IsInitialized) Init(userPresenceController.Users);
            else userPresenceController.UserPresenceControllerInitialized += () => { Init(userPresenceController.Users); };
        }
        public void Init(Dictionary<string, GameboardUserPresenceEventArgs> existingUsers)
        {
            foreach (string userId in existingUsers.Keys)
            {
                UpdateDatabase(existingUsers[userId]);
            }
        }

        public void OnPlayerUpdate(GameboardUserPresenceEventArgs changeInfo)
        {
            UpdateDatabase(changeInfo);
        }
        private void UpdateDatabase(GameboardUserPresenceEventArgs changeInfo)
        {
            ePlayerDirection playerPosition = changeInfo.boardUserPosition.GetDirection();
            switch (changeInfo.changeValue)
            {
                case Gameboard.DataTypes.UserPresenceChangeTypes.REMOVE:
                    RemovePlayer(playerPosition, changeInfo);
                    break;
                case Gameboard.DataTypes.UserPresenceChangeTypes.UNKNOWN:
                case Gameboard.DataTypes.UserPresenceChangeTypes.ADD:
                case Gameboard.DataTypes.UserPresenceChangeTypes.CHANGE:
                case Gameboard.DataTypes.UserPresenceChangeTypes.CHANGE_POSITION:
                    UpdateUserDrawer(playerPosition, changeInfo);
                    break;
                default:
                    break;
            }
        }

        private void RemovePlayer(ePlayerDirection fromPosition, GameboardUserPresenceEventArgs removedPlayer)
        {
            PlayerDatabase[fromPosition].isLoggedIn = false;
            DirectionsMap.Remove(removedPlayer.userId);
            foreach (IPlayerUpdates playerUpdates in playerUpdateListeners) playerUpdates.OnPlayerLogout(fromPosition, PlayerDatabase[fromPosition]);
        }
        private void UpdateUserDrawer(ePlayerDirection newPosition, GameboardUserPresenceEventArgs changeInfo)
        {
            if (newPosition == ePlayerDirection.NONE) return;
            //Is this user already recorded ?
            if (DirectionsMap.ContainsKey(changeInfo.userId))
            {
                ePlayerDirection oldPosition = DirectionsMap[changeInfo.userId];
                //Logout old position if new position to occupy was empty
                if (!PlayerDatabase[newPosition].isLoggedIn)
                {
                    PlayerDatabase[oldPosition].isLoggedIn = false;
                    foreach (IPlayerUpdates playerUpdates in playerUpdateListeners) playerUpdates.OnPlayerLogout(oldPosition, PlayerDatabase[oldPosition]);
                }
                //If received position is a valid new position, call changePosition
                if (newPosition != oldPosition)
                {
                    PlayerDatabase[newPosition].Update(changeInfo);
                    DirectionsMap[changeInfo.userId] = newPosition;
                    foreach (IPlayerUpdates playerUpdates in playerUpdateListeners) playerUpdates.OnPlayerChangePosition(newPosition, PlayerDatabase[newPosition]);
                }
            }
            else
            {
                PlayerDatabase[newPosition].Update(changeInfo);
                foreach (IPlayerUpdates playerUpdates in playerUpdateListeners) playerUpdates.OnPlayerLoginToDrawer(newPosition, PlayerDatabase[newPosition]);
                DirectionsMap.Add(changeInfo.userId, newPosition);
            }
        }
        public void RegisterForUpdates(IPlayerUpdates playerUpdates)
        {
            if (!playerUpdateListeners.Contains(playerUpdates))
                playerUpdateListeners.Add(playerUpdates);
            foreach (ePlayerDirection direction in PlayerDatabase.Keys)
            {
                if (PlayerDatabase[direction].isLoggedIn)
                    playerUpdates.OnPlayerLoginToDrawer(direction, PlayerDatabase[direction]);
            }
        }

        public void UnregisterForUpdates(IPlayerUpdates unregister)
        {
            playerUpdateListeners.Remove(unregister);
        }

        public void ForceUpdate()
        {
            var gameboardObject = UnityEngine.GameObject.FindGameObjectWithTag("Gameboard");
            Gameboard.UserPresenceController userPresenceController = gameboardObject.GetComponent<Gameboard.UserPresenceController>();
            foreach(GameboardUserPresenceEventArgs presence in userPresenceController.Users.Values)
            {                
                UpdateDatabase(presence);
            }
        }
    }

    public class PlayerPresence
    {
        public string id;
        public string userId;
        public string userName;
        public bool isLoggedIn;
        public ePlayerDirection direction;
        public PlayerPresence() { isLoggedIn = false; }
        public PlayerPresence(GameboardUserPresenceEventArgs userInfo)
        {
            Update(userInfo);
        }

        public void Update(GameboardUserPresenceEventArgs userInfo)
        {
            id = userInfo.id;
            userId = userInfo.userId;
            userName = userInfo.userName;
            isLoggedIn = true;
            direction = userInfo.boardUserPosition.GetDirection();
        }
    }
}
