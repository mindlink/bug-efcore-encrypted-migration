using System;

namespace EncryptionMigrationBug.Data
{
    public sealed class Message
    {
        public string MessageId { get; set; }

        public string GroupId { get; set; }

        public string SenderId { get; set; }

        public string Content { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string Context { get; set; }
    }
}
