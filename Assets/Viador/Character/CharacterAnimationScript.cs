using UnityEngine;
using System.Collections;
using Viador.Game;

namespace Viador.Character
{
    public class CharacterAnimationScript : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private bool IsCurrentPlayer() => TurnManager._currentPlayer != name;

        public void OnCharacterMoved(Component sender, object animatorParam)
        {
            if (IsCurrentPlayer())
                return;

            StartCoroutine(DelayedAnimationProcess("walk"));
        }

        public void OnCharacterAttacked(Component sender, object animatorParam)
        {
            if (IsCurrentPlayer())
                return;

            StartCoroutine(DelayedAnimationProcess("attack"));
        }

        public void OnCharacterDamaged(Component sender, object animatorParam)
        {
            if (!IsCurrentPlayer())
                return;

            StartCoroutine(DelayedAnimationProcess("hurt"));
        }

        public void OnCharacterDefensed(Component sender, object animatorParam)
        {
            if (!IsCurrentPlayer())
                return;

            StartCoroutine(DelayedAnimationProcess("defense"));
        }

        public void OnCharacterDead(Component sender, object animatorParam)
        {
            if (!IsCurrentPlayer())
                return;

            StartCoroutine(DelayedAnimationProcess("die"));
        }

        private float getCurrentAnimationLength() => _animator.GetCurrentAnimatorStateInfo(0).length;
        
        // Enable animation state until it reaches the length of the animation, then disable it.
        private IEnumerator DelayedAnimationProcess(string animationState)
        {
            GameLogger.Log(LoggerType.ANIMATION, animationState, "animation starts");

            _animator.SetBool(animationState, true);
        
            yield return new WaitForSeconds(getCurrentAnimationLength());

            _animator.SetBool(animationState, false);

            GameLogger.Log(LoggerType.ANIMATION, $"{animationState} Animation stops");

            if(animationState.Equals("die"))
                 Destroy(gameObject);
        }
    }
}
