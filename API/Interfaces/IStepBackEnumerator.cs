namespace RaffleApi.Interfaces;

public interface IStepBackEnumerator<T> : IEnumerator<T>
{
    bool MovePrevious();
}