public class FinishAreaItem : MapItem
{
    public override bool Interact(PlayerController player)
    {
        if (player.LevelComplete != null) player.LevelComplete.PlayParticle();
        player.SetTimeToMove(0.1f);
        return true;
    }
}