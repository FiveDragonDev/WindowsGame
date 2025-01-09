using System.Collections.ObjectModel;

public interface IEffectable
{
    public IHealth Health { get; }
    public ReadOnlyCollection<TemporaryEffect> Effects { get; }

    public void HandleEffects();
    public void AddEffect(TemporaryEffect effect);
    public void ClearEffects();
}
