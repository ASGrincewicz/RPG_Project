namespace RPG.Control
{
    public interface IRaycastable
    {
        public bool HandleRaycast(PlayerController controller);

        public CursorType GetCursorType();
    }
}
