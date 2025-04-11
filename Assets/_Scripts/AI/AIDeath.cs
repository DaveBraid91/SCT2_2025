
public class AIDeath : AIBase
{
    protected override void Start()
    {
        base.Start();
        agent.speed = 0;
        agent.ResetPath();
    }

    
}
