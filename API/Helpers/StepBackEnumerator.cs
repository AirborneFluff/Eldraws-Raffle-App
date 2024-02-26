using System.Collections;
using RaffleApi.Interfaces;

namespace RaffleApi.Helpers;

public class StepBackEnumerator<T>: IStepBackEnumerator<T>
{
    private readonly IEnumerator<T> _enumerator;
    private T _previous;
    private bool _isPrevious = false;

    public StepBackEnumerator(IEnumerator<T> enumerator)
    {
        _enumerator = enumerator;
    }

    public bool MovePrevious()
    {
        if (_isPrevious) return false;
        _isPrevious = true;
        return true;
    }

    public bool MoveNext()
    {
        if (_isPrevious)
        {
            _isPrevious = false;
            return true;
        }

        _previous = _enumerator.Current;
        return _enumerator.MoveNext();
    }

    public T Current
    {
        get
        {
            if (_isPrevious) return _previous;
            return _enumerator.Current;
        }
    }

    public void Reset()
    {
        _enumerator.Reset();
        _isPrevious = false;
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        _enumerator.Dispose();
    }
}