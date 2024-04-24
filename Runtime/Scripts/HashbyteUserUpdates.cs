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
        public void Register(IPlayerUpdates updates)
        {
            playerDrawerUpdates.RegisterForUpdates(updates);
        }
    }
}
