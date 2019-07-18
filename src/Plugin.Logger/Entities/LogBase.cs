using System;
using Clean.Microservice.Serverless.SharedKernel.Core.Serializers;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;
using Newtonsoft.Json;

namespace Clean.Microservice.Serverless.Plugin.Logger.Entities
{
    internal abstract class LogBase
    {
        public LogBase(ITransactionFlow transactionFlow)
        {
            Id = transactionFlow?.Id;
            Name = transactionFlow?.Name;
            NickName = transactionFlow?.Nickname;
            Email = transactionFlow?.Email;
            ClientId = transactionFlow?.ClientId;
            Scope = transactionFlow?.Scope;
            Role = transactionFlow?.Role;
            CustomerId = transactionFlow?.CustomerId;
            AppType = transactionFlow?.AppType;
            Machine = transactionFlow?.Machine;
            Culture = transactionFlow?.Culture;
            HostIp = transactionFlow?.HostIp;
            LocalIp = transactionFlow?.LocalIp;
            CorrelationId = transactionFlow?.CorrelationId;
            CustomerDocument = transactionFlow?.CustomerDocument;
        }

        public abstract string Type { get; }

        public virtual DateTimeOffset Date => DateTimeOffset.UtcNow;

        public virtual string Culture { get; set; }

        public virtual string Url { get; set; }

        public virtual string Machine { get; set; }

        public virtual string HostIp { get; set; }

        public virtual string LocalIp { get; set; }

        public virtual string RequestIp { get; set; }

        public virtual string Id { get; }

        public virtual string Name { get; }

        public virtual string NickName { get; }

        public virtual string Email { get; }

        public virtual string ClientId { get; }

        public virtual string Scope { get; }

        public virtual string Role { get; }

        public string CorrelationId { get; }

        public string CustomerDocument { get; }

        public string CustomerId { get; }

        public string Account { get; }

        public string AppType { get; }

        internal string Serialize()
        {
            var settings = new JsonSerializerSettings().GetJsonSerializerSettingsWithPrivateCamelCaseSerializer();

            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
