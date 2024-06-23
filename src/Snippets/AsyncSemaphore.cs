public class AsyncSemaphore
{
    SemaphoreSlim _semaphore;

    public AsyncSemaphore(int maxConcurrent = 1) =>
      _semaphore = new (maxConcurrency, maxConcurrency);
    
    public async Task<IDisposable> EnterAsync()
    {
        await _semaphore.WaitAsync().ConfigureAwait(false);
        return Disposable.Create(() => _semaphore.Release());
    }
}
