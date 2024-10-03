public enum StatModifierType
{
    Buff = 1, Debuff = -1
}
public struct StatModifier
{
    public int modifierValue;

    public StatModifierType StatModifierType;
}
