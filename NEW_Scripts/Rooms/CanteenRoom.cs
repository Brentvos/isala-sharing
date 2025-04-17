namespace New
{
    public class CanteenRoom : RoomRoot
    {
        public override void OnPlayerEnter()
        {
            base.OnPlayerEnter();
            Player.Instance.IsInCanteenRoom = true;
            // Stress go down
        }

        public override void OnPlayerExit()
        {
            Player.Instance.IsInCanteenRoom = false;
            base.OnPlayerExit();
            // Stress default (up)
        }
    }
}