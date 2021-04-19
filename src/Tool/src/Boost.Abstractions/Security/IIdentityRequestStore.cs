using System;
using System.Collections.Generic;

namespace Boost.Security
{
    public interface IIdentityRequestStore
    {
        IdentityRequest Save<T>(SaveIdentityRequest<T> request) where T : class;
        IEnumerable<IdentityRequest> SearchRequest(SearchIdentityRequest searchRequest);
    }

    public class SaveIdentityRequest<T>
        where T : class
    {
        public Guid? Id { get; set; }

        public IdentityRequestType Type { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Tags { get; set; } = new List<string>();

        public T Data { get; set; }

        public static SaveIdentityRequest<T> Create(T data)
        {
            return new SaveIdentityRequest<T>
            {
                Data = data
            };
        }
    }


    public record SearchIdentityRequest(IdentityRequestType Type)
    {
        public string? SearchText { get; init; }

        public string? Tag { get; init; }
    }

    public class IdentityRequest
    {
        public Guid Id { get; set; }

        public IdentityRequestType Type { get; set; }

        public string Name { get; set; }

        public IList<string> Tags { get; set; } = new List<string>();

        public DateTime CreatedAt { get; set; }

        public byte[] Data { get; set; }
    }

    public enum IdentityRequestType
    {
        Token,
        Authorize
    }
}
