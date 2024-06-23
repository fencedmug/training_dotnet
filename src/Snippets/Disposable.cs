public Disposable : IDisposable
{
    public static IDisposable Create(Action onDispose) => new Disposable(onDispose);

    Action _onDispose;

    Disposable(Action onDispose) => _onDispose = onDispose;

    public void Dispose()
    {
        Action todo;
        lock (this)
        {
            todo = _onDispose;
            _onDispose = null;
        }

        if (todo != null) todo(); // todo?.Invoke();
    }
}
