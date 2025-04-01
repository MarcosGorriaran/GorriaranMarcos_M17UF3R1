

public enum Group
{
    Player,
    Enemy
}
public interface ITargetable
{
    public Group GroupMember { get; set; }
    public float ThreatLevel { get; set; }
}
