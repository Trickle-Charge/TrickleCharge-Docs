namespace TrickleCharge.Math.Waves
{
public interface IWave
{
    public bool Enabled { get; set; }
    public float Evaluate(float time, float position = 0);
}
}
