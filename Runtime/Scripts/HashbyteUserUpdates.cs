namespace Hashbyte.GameboardGeneral
{
    public class HashbyteUserUpdates
    {
        #region Singleton
        private static HashbyteUserUpdates instance;
        public static HashbyteUserUpdates Instance { get { instance ??= new HashbyteUserUpdates(); return instance; } }
        private HashbyteUserUpdates()
        {                        
            playerDrawerUpdates = new PlayerDrawerUpdates();            
        }
        #endregion        

        private PlayerDrawerUpdates playerDrawerUpdates;
        public System.Collections.Generic.Dictionary<ePlayerDirection, PlayerPresence> PlayerDatabase => playerDrawerUpdates.PlayerDatabase;
        public Gameboard.RatingController ratingController => playerDrawerUpdates.ratingController;
        public void Register(IPlayerUpdates updates)
        {
            playerDrawerUpdates.RegisterForUpdates(updates);
        }
        public void UpdateDrawerVisibility(bool show)
        {
            if(show) playerDrawerUpdates.drawerController.ShowDrawers();
            else playerDrawerUpdates.drawerController.HideDrawers();
        }

        public void SendGameSessionEvent(bool started, System.Collections.Generic.List<string> userIds)
        {
            if (started) playerDrawerUpdates.engagementController.SendGameSessionStarted(userIds);
            else playerDrawerUpdates.engagementController.SendGameSessionEnded(userIds);
        }        
    }
}
