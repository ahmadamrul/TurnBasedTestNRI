using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewStatusEffect", menuName = "StatusEffect")]
public class StatusEffectData : ScriptableObject
{
    public string skillName;
    public StatusEffect.EffectType type;
    public int duration;
    public int value;
    public bool applyToPlayer;
    public bool applyNextTurn;
}
