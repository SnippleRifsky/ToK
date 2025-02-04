public static class PlayerEvents
{
    public readonly struct LevelingChanged
    {
        public readonly int CurrentLevel;
        public readonly int CurrentXp;
        public readonly int XpToNextLevel;
        public readonly bool AtLevelCap;
        public readonly Player Player;

        public LevelingChanged(int currentLevel, int currentXp, int xpToNextLevel, bool atLevelCap, Player player)
        {
            CurrentLevel = currentLevel;
            CurrentXp = currentXp;
            XpToNextLevel = xpToNextLevel;
            AtLevelCap = atLevelCap;
            Player = player;
        }
    }
}