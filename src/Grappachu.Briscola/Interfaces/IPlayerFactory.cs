namespace Grappachu.Briscola.Interfaces
{
    public interface IPlayerFactory
    {
        IPlayer CreatePlayer(string playerName, IStrategy strategy);
    }
}