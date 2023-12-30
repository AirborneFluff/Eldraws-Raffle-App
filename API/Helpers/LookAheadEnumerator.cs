using System.Collections;

namespace RaffleApi.Helpers;

public class LookAheadEnumerator<T> : IEnumerator<T>
{
    private readonly IEnumerator<T> _inner;
    private bool _hasNext;
    private T _next;
    private T _current;

    public LookAheadEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
        
        _hasNext = _inner.MoveNext();
        if (_hasNext) _next = _inner.Current;
    }

    public T Current => _current;

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        _inner.Dispose();
    }

    public bool MoveNext()
    {
        if (!_hasNext) return false;
        
        _current = _next;
        _hasNext = _inner.MoveNext();
        if (_hasNext) _next = _inner.Current;
        return true;
    }

    public void Reset()
    {
        _inner.Reset();
        _hasNext = _inner.MoveNext();
        if (_hasNext) _next = _inner.Current;
    }

    public bool HasNext => _hasNext;
}