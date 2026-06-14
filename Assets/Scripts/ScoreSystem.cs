public class ScoreSystem
{
    public int Score { get; private set; }

    public void AddPoint() => Score++;

    public void Reset() => Score = 0;
}