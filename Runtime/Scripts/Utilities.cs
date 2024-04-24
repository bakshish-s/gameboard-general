namespace Hashbyte.GameboardGeneral
{
    public static class Utilities
    {
        public static ePlayerDirection GetDirection(this global::Gameboard.EventArgs.BoardUserPosition boardPosition)
        {
            if (boardPosition.x == 0.25f)
            {
                if (boardPosition.y == 0) return ePlayerDirection.TOP_LEFT; else if (boardPosition.y == 1) return ePlayerDirection.BOTTOM_LEFT;
            }
            else if (boardPosition.x == 0.75f)
            {
                if (boardPosition.y == 0) return ePlayerDirection.TOP_RIGHT; else if (boardPosition.y == 1) return ePlayerDirection.BOTTOM_RIGHT;
            }
            else if (boardPosition.x == 0)
            {
                if (boardPosition.y == 0.25f) return ePlayerDirection.LEFT_TOP; else if (boardPosition.y == 0.75f) return ePlayerDirection.LEFT_BOTTOM;
            }
            else if (boardPosition.x == 1)
            {
                if (boardPosition.y == 0.25f) return ePlayerDirection.RIGHT_TOP; else if (boardPosition.y == 0.75f) return ePlayerDirection.RIGHT_BOTTOM;
            }
            return ePlayerDirection.NONE;
        }
    }
}
