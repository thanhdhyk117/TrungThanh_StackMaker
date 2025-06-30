public class BrickItem : MapItem
{
    public override bool Interact(PlayerController player)
    {
        if (!IsPush)
        {
            player.Score++;
            player.BrickPlayer.Count++;
        }
        SetPushState(true);
        if (MeshRenderer != null) MeshRenderer.enabled = false;
        return true;
    }
}