namespace Hashbyte.GameboardGeneral
{
    public interface IPlayerUpdates
    {
        public void OnPlayerLoginToDrawer(ePlayerDirection direction, PlayerPresence presence);
        public void OnPlayerLogout(ePlayerDirection direction, PlayerPresence presence);
        public void OnPlayerChangePosition(ePlayerDirection newPosition, PlayerPresence presence);
        public void PlayerDisarmedBySystem(ePlayerDirection direction, PlayerPresence presence);
    }
}
