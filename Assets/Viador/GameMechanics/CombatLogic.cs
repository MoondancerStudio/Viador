using Viador.Character;

namespace Viador.GameMechanics
{
    public class CombatLogic
    {
        public AttackResult CalculateAttack(CharacterData attacker, CharacterData defender, Dice dice)
        {
            AttackResult result;
            int effectiveAttack = (attacker.attack + dice.Roll()) - (defender.defense + dice.Roll());

            if (effectiveAttack > 1)
            {
                result = new AttackResult(true, effectiveAttack/2);
            }
            else
            {
                result = new AttackResult(false, 0);
            }

            return result;
        }
    }

    public class AttackResult
    {
        public AttackResult(bool success, int damage)
        {
            Success = success;
            Damage = damage;
        }
        
        public bool Success { get; }

        public int Damage { get; }
    }
}