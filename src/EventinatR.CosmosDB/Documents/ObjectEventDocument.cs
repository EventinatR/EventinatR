﻿using System;

namespace EventinatR.CosmosDB.Documents
{
    internal record ObjectEventDocument(string StreamId, string Id, long Version, DateTimeOffset Timestamp, string DataType, object Data) : Document(StreamId, Id, Version, DocumentTypes.Event);
}
