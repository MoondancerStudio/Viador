using UnityEngine;
using Viador.Character;

namespace Viador.GameMechanics
{
    public class CombatLogic
    {
        public AttackResult CalculateAttack(int attackValue, int defenseValue, Dice dice)
        {
            AttackResult result;
            var attackRoll = dice.Roll();
            var defenseRoll = dice.Roll();
            Debug.Log($"Attack calculation: {attackValue + attackRoll} ({attackValue} + {attackRoll}) vs {defenseValue + defenseRoll} ({defenseValue} + {defenseRoll})");
            
            int effectiveAttack = (attackValue + attackRoll) - (defenseValue + defenseRoll);

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

        public int HandleDamage(int rawDamage, int armor)
        {
            var calculatedDamage = rawDamage - armor;
            var damage = calculatedDamage < 0 ? 0 : calculatedDamage;
            Debug.Log($"Damage handling: {damage} ({rawDamage} - {armor})");
            return damage;
        }

        public int CalculateDefenseValue(CharacterData characterData)
        {
            return characterData.defense;
        }

        public int CalculateAttackValue(CharacterData characterData)
        {
            return characterData.attack;
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