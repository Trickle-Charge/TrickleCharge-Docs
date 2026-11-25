namespace Thrust
{
// TODO Unused? Remove?
public interface IThruster
{
    public float MaxForce { get; }

    public float CurrentForce { get; }

    public void SetForce(float force);
}
}
