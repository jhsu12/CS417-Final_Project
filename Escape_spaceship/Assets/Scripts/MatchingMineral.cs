using UnityEngine;

public enum MineralType
{
    Red,
    Green,
    Blue
}

public class MatchingMineral : MonoBehaviour
{
    [SerializeField] private MineralType mineralType;

    public MineralType Type => mineralType;
}
