using UnityEngine;

[CreateAssetMenu(fileName ="PlayerType",menuName = "Assets/PlayerType")]
public class PlayerType : ScriptableObject
{
    public Sprite Sprite;
    public Vector2Int[] Moves;
    public bool isInfinite;
}
