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

    public readonly struct ExperienceGained
    {
        public readonly int Amount;
        public readonly Entity Source;
        public readonly Player Player;
        
        public ExperienceGained(int amount, Entity source, Player player)
        {
            Amount = amount;
            Source = source;
            Player = player;
        }
    }
}