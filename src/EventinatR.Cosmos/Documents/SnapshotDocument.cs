using System.Text.Json.Serialization;

namespace EventinatR.Cosmos.Documents;

internal record SnapshotDocument(
    string StreamId,
    string Id,
    long Version,
    string StateType,
    [property: JsonConverter(typeof(BinaryDataConverter))] BinaryData State)
    : Document(StreamId, Id, Version, DocumentTypes.Snapshot);
