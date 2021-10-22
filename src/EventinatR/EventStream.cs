namespace EventinatR;

public abstract class EventStream : IEventStreamReader
{
    protected EventStream()
        : this(EventStreamId.None)
    {
    }

    public EventStream(EventStreamId id)
        => Id = id ?? throw new ArgumentNullException(nameof(id));

    public EventStreamId Id { get; }

    public abstract Task<EventStreamVersion> AppendAsync<T>(IEnumerable<T> events, CancellationToken cancellationToken = default)
        where T : class;

    public abstract IAsyncEnumerable<Event> ReadAsync(CancellationToken cancellationToken = default);

    public abstract Task<EventStreamSnapshot<T>> ReadSnapshotAsync<T>(CancellationToken cancellationToken = default);

    public abstract Task<EventStreamSnapshot<T>> WriteSnapshotAsync<T>(T state, EventStreamVersion version, CancellationToken cancellationToken = default);
}
