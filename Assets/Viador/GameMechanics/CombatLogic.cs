using UnityEngine;
using Viador.Action;
using Viador.Character;
using Viador.Game;

namespace Viador.GameMechanics
{
    public class CombatLogic
    {
        public static bool IsAttackBegin = false;

        public AttackResult CalculateAttack(int attackValue, int defenseValue, Dice dice, IActionStateProvider actionStateProvider)
        {
            AttackResult result;
            var attackRoll = dice.Roll();
            var defenseRoll = dice.Roll();
            GameLogger.Log(LoggerType.COMBAT, $"Attack calculation: {attackValue + attackRoll} ({attackValue} + {attackRoll}) vs {defenseValue + defenseRoll} ({defenseValue} + {defenseRoll})");

            // Calculate if there is any purchased feat
            attackValue += actionStateProvider.getActionStateAttack();

            defenseValue += actionStateProvider.getActionStateDefense();

            GameLogger.Log(LoggerType.COMBAT, $"Attack value after Action state {attackValue}");

            int effectiveAttack = (attackValue + attackRoll) - (defenseValue + defenseRoll);

            if (effectiveAttack > 1)
            {
                result = new AttackResult(true, effectiveAttack/2);
            }
            else
            {
                result = new AttackResult(false, 0);
            }

            //Clear the content of the list, where payment is occured
           // actionStateProvider.removeAllActivatedActionStates();

            return result;
        }

        public int HandleDamage(int rawDamage, int armor)
        {
            var calculatedDamage = rawDamage - armor;
            var damage = calculatedDamage < 0 ? 0 : calculatedDamage;
            GameLogger.Log(LoggerType.COMBAT, $"Damage handling: {damage} ({rawDamage} - {armor})");
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