public class StatSet
{
    public CharacterStat Health;
    public CharacterStat Resource;

    public CharacterStat Strength;
    public CharacterStat Intelligence;
    public CharacterStat Agility;
    public CharacterStat Constitution;

    public StatSet()
    {
        Health = new CharacterStat(100f, 100f,"Health", "Health");
        Resource = new CharacterStat(100f, 100f,"Resource", "Resource");
        Strength = new CharacterStat(10f, "Strength", "Strength");
        Intelligence = new CharacterStat(10f, "Intelligence", "Intelligence");
        Agility = new CharacterStat(10f, "Agility", "Agility");
        Constitution = new CharacterStat(10f, "Constitution", "Constitution");
    }
}
