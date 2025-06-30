public class BridgeItem : MapItem
{
    protected override void OnInitialize()
    {
        if (MeshRenderer != null) MeshRenderer.enabled = false;
    }

    public override bool Interact(PlayerController player)
    {
        if (!IsPush)
        {
            player.BrickPlayer.Count -= 2;
        }
        SetPushState(true);
        if (MeshRenderer != null) MeshRenderer.enabled = true;
        return true;
    }
}