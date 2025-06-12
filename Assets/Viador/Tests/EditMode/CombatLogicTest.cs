using NUnit.Framework;
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
            int attackValue = 10;
            int defenseValue = 20;
            Dice mockDice = new MockDice();
            
            // WHEN
            var actual = _underTest.CalculateAttack(attackValue, defenseValue, mockDice);
            
            // THEN
            Assert.IsNotNull(actual);
            Assert.AreEqual(false, actual.Success);
            Assert.AreEqual(0, actual.Damage);
        }
        
        [Test]
        public void CalculateAttackShouldReturnHit()
        {
            // GIVEN
            int attackValue = 20;
            int defenseValue = 10;
            Dice mockDice = new MockDice();
            
            // WHEN
            var actual = _underTest.CalculateAttack(attackValue, defenseValue, mockDice);
            
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