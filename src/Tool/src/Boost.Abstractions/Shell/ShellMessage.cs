using System;
using System.Collections.Generic;

namespace Boost
{
    public record ShellMessage(MessageSession Session, string Type, string? Message)
    {
        public IEnumerable<string> Tags { get; init; } = new List<string>();
    }

    public class MessageSession
    {
        public MessageSession()
        {
            Id = Guid.NewGuid();
        }

        private MessageSession(Guid id, int number)
        {
            Id = id;
            Number = number;
        }

        public Guid Id { get; }

        public int Number { get; private set; }

        public MessageSession Next()
        {
            Number++;

            return new MessageSession(Id, Number);
        }
    }
}
