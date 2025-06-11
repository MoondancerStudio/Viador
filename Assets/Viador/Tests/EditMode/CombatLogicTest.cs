using NUnit.Framework;
using UnityEngine;
using Viador.Character;
using Viador.GameMechanics;

namespace Viador.Tests.EditMode
{
    [TestFixture]
    public class CombatLogicTest
    {
        private CombatLogic _underTest = new();
        
        [Test]
        public void CalculateAttackShouldReturnMiss()
        {
            // GIVEN
            CharacterData attacker = ScriptableObject.CreateInstance<CharacterData>();
            attacker.attack = 10;
            CharacterData defender = ScriptableObject.CreateInstance<CharacterData>();
            defender.defense = 20;
            Dice mockDice = new MockDice();
            
            // WHEN
            var actual = _underTest.CalculateAttack(attacker, defender, mockDice);
            
            // THEN
            Assert.IsNotNull(actual);
            Assert.AreEqual(false, actual.Success);
            Assert.AreEqual(0, actual.Damage);
        }
        
        [Test]
        public void CalculateAttackShouldReturnHit()
        {
            // GIVEN
            CharacterData attacker = ScriptableObject.CreateInstance<CharacterData>();
            attacker.attack = 20;
            CharacterData defender = ScriptableObject.CreateInstance<CharacterData>();
            defender.defense = 10;
            Dice mockDice = new MockDice();
            
            // WHEN
            var actual = _underTest.CalculateAttack(attacker, defender, mockDice);
            
            // THEN
            Assert.IsNotNull(actual);
            Assert.AreEqual(true, actual.Success);
            Assert.AreEqual(5, actual.Damage);
        }
        
        [Test]
        public void ValidateMockDice()
        {
            // GIVEN
            Dice mockDice = new MockDice();
            
            // WHEN
            var actual = mockDice.Roll();
            
            // THEN
            Assert.AreEqual(0, actual);
        }
    }

    class MockDice : Dice
    {
        public override int Roll()
        {
            return 0;
        }
    }
}