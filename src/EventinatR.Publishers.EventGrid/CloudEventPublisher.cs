using System.Globalization;
using System.Net.Mime;
using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace EventinatR.Publishers.EventGrid;

public class CloudEventPublisher
{
    private readonly EventGridPublisherClient _client;
    private readonly string _source;

    public CloudEventPublisher(EventGridPublisherClient client, string source)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _source = source ?? throw new ArgumentNullException(nameof(source));
    }

    public Task PublishAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default)
    {
        var batch = new List<CloudEvent>();

        foreach (var e in events)
        {
            if (ShouldPublish(e))
            {
                var @event = Convert(e);
                batch.Add(@event);
            }
        }

        return _client.SendEventsAsync(batch, cancellationToken);
    }

    protected virtual bool ShouldPublish(Event @event)
        => true;

    protected virtual CloudEvent Convert(Event @event)
    {
        var cloudEvent = new CloudEvent(_source, @event.Data.Type.Name, @event.Data.Value, MediaTypeNames.Application.Json, CloudEventDataFormat.Json)
        {
            Subject = @event.StreamId.Value,
            Time = @event.Timestamp
        };

        cloudEvent.ExtensionAttributes.Add("partitionkey", @event.StreamId.Value);
        cloudEvent.ExtensionAttributes.Add("sequence", @event.Version.Value.ToString(CultureInfo.InvariantCulture));
        cloudEvent.ExtensionAttributes.Add("sequencetype", "Integer");
        cloudEvent.ExtensionAttributes.Add("transaction", @event.Transaction.Version.Value.ToString(CultureInfo.InvariantCulture));

        return cloudEvent;
    }
}
