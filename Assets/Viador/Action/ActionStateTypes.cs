namespace Viador.Action
{
    // Action name = Defense or attack points
    public enum ActionState
    {
        EXTRA_HIT = 3,
        EXTRA_DEFENSE = 2,
        ESCAPE = 1
    }

    public enum ActionCost
    {
        EXTRA_HIT = 2,
        EXTRA_DEFENSE = 1,
        ESCAPE = 2
    }
}