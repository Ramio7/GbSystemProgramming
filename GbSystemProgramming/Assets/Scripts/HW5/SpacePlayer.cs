public class SpacePlayer : Player
{
    protected override void SpawnCharacter()
    {
        base.SpawnCharacter();
        playerCharacter.name = HW5EntryPoint.NetworkManager.playerName;
        playerCharacter.GetComponent<ShipController>().PlayerName = HW5EntryPoint.NetworkManager.playerName;
    }
}
