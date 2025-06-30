public class WinAreaItem : MapItem
{
    public override bool Interact(PlayerController player)
    {
        player.ChangeAnim(2);
        player.EndLevel();
        return true;
    }
}