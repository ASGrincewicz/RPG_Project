using UnityEngine;

namespace RPG.Control
{
    
    [System.Serializable]
    public struct CursorMapping
    {
        public CursorType cursorType;
        public Texture2D texture;
        public Vector2 hotspot;
    }
    
}