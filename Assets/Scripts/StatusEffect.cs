public class StatusEffect
{
    public enum EffectType
    {
        IncreaseDamage,
        ReduceDefense,
        Bleeding
    }
    public EffectType type;
    public int duration;
    public int value;
    public int endTurn;
    public bool applyToPlayer;
    public bool applyNextTurn;
    public StatusEffect(StatusEffectData data, int currentTurn)
    {
        this.type = data.type;
        this.duration = data.duration;
        this.value = data.value;
        if (data.applyNextTurn)
        {
            this.endTurn = currentTurn + data.duration;
        }
        else
        {
            this.endTurn = currentTurn + data.duration - 1;
        }
        this.applyNextTurn = data.applyNextTurn;
        this.applyToPlayer = data.applyToPlayer;

    }
}
