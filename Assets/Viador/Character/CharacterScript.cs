using UnityEngine;


namespace Viador.Character
{
    public class CharacterScript : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;

        private void Awake()
        {
            name = characterData.name;
            GetComponent<SpriteRenderer>().sprite = characterData.icon;
            GetComponent<CharacterAttackScript>().characterData = characterData;
        }
    }
}
