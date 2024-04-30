namespace Hashbyte.GameboardGeneral
{
    public static class Utilities
    {
        public static ePlayerDirection GetDirection(this Gameboard.EventArgs.BoardUserPosition boardPosition)
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

        public static bool IsOnSameSide(this ePlayerDirection direction, ePlayerDirection directionsToCheck)
        {
            if ((int)direction % 2 == 1)/*Direction is odd*/return (int)directionsToCheck == (int)direction + 1;
            else /*Direction is even*/return (int)directionsToCheck == (int)direction - 1;            
        }
        public static bool IsOnOppositeSide(this ePlayerDirection direction, ePlayerDirection directionsToCheck)
        {
            if (direction == ePlayerDirection.TOP_LEFT || direction == ePlayerDirection.TOP_RIGHT)
                return (directionsToCheck == ePlayerDirection.BOTTOM_LEFT || directionsToCheck == ePlayerDirection.BOTTOM_RIGHT);
            else if (direction == ePlayerDirection.BOTTOM_LEFT || direction == ePlayerDirection.BOTTOM_RIGHT)
                return (directionsToCheck == ePlayerDirection.TOP_LEFT || directionsToCheck == ePlayerDirection.TOP_RIGHT);
            else if (direction == ePlayerDirection.RIGHT_TOP || direction == ePlayerDirection.RIGHT_BOTTOM)
                return (directionsToCheck == ePlayerDirection.LEFT_TOP || directionsToCheck == ePlayerDirection.LEFT_BOTTOM);
            else if (direction == ePlayerDirection.LEFT_TOP || direction == ePlayerDirection.LEFT_BOTTOM)
                return (directionsToCheck == ePlayerDirection.RIGHT_TOP || directionsToCheck == ePlayerDirection.RIGHT_BOTTOM);
            return false;
        }
    }
}
