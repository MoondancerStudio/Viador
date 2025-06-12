namespace Viador.Events
{
    public class GameEvents
    {
        public static readonly string StartGame = "StartGame";
        public static readonly string GameOver = "GameOver";
        public static readonly string NextTurn = "NextTurn";
        public static readonly string TurnStarted = "TurnStarted";
        public static readonly string PlayerUpdated = "PlayerUpdated";
        public static readonly string ActionPointsUpdated = "ActionPointsUpdated";
        public static readonly string RenderMoveOptions = "RenderMoveOptions";
        public static readonly string MoveSelected = "MoveSelected";
        public static readonly string CharacterChoosenToAttack = "CharacterChoosenToAttack";
        public static readonly string CharacterAttacked = "CharacterAttacked";
        public static readonly string CharacterDefensed= "CharacterDefensed";
        public static readonly string AttackResultUpdated = "AttackResultUpdated";
        public static readonly string UIStateUpdated = "UIStateUpdated";
        public static readonly string CharacterMoved = "CharacterMoved";
        public static readonly string StateUpdated = "StateUpdated";
        public static readonly string Player_1_HealthPointUpdated = "Player_1_HealthPointUpdated";
        public static readonly string Player_2_HealthPointUpdated = "Player_2_HealthPointUpdated";
    }
}