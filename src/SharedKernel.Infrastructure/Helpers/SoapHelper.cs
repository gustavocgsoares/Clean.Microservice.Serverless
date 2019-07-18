using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.Helpers
{
    /// <summary>
    /// Soap using ChannelFactory to build instance with Authentication when necessary
    /// </summary>
    public static class SoapHelper
    {
        /// <summary>
        /// Get SoapClient instance based on parameters
        /// </summary>
        /// <typeparam name="TSoapClient">Generic Soap Client type</typeparam>
        /// <param name="endpointAddress"></param>
        /// <param name="behaviors">optional behaviors list to apply on channel factory</param>
        /// <returns>TSoapClient instance</returns>
        public static TSoapClient GetServicesSoap<TSoapClient>(string endpointAddress, List<IEndpointBehavior> behaviors = null)
        {
            return GetChannelFactory<TSoapClient>(endpointAddress, behaviors, false, null, null, null);
        }

        /// <summary>
        /// Get SoapClient instance based on parameters
        /// </summary>
        /// <typeparam name="TSoapClient">Generic Soap Client type</typeparam>
        /// <param name="endpointAddress"></param>
        /// <param name="behaviors"></param>
        /// <param name="auth">indicates if must to authenticate</param>
        /// <param name="username">AD username</param>
        /// <param name="password">AD password</param>
        /// <param name="domain">AD domain</param>
        /// <returns>TSoapClient instance</returns>
        public static TSoapClient GetServicesSoap<TSoapClient>(string endpointAddress, List<IEndpointBehavior> behaviors = null, bool auth = false, string username = null, string password = null, string domain = null)
        {
            return GetChannelFactory<TSoapClient>(endpointAddress, behaviors, auth, username, password, domain);
        }

        private static TSoapClient GetChannelFactory<TSoapClient>(string endpointAddress, List<IEndpointBehavior> behaviors, bool auth = false, string username = null, string password = null, string domain = null)
        {
            var mustUseHttpsBinding = endpointAddress.IndexOf("https", StringComparison.InvariantCultureIgnoreCase) != -1;

            ChannelFactory<TSoapClient> channelFactory = CreateChannelWithEndpointBinding<TSoapClient>(endpointAddress, mustUseHttpsBinding);

            AddAuthentication(auth, username, password, domain, channelFactory);
            AddBehaviors(behaviors, channelFactory);

            return channelFactory.CreateChannel();
        }

        private static ChannelFactory<TSoapClient> CreateChannelWithEndpointBinding<TSoapClient>(string endpointAddress, bool mustUseHttpsBinding)
        {
            ChannelFactory<TSoapClient> channelFactory;
            if (mustUseHttpsBinding)
            {
                channelFactory = new ChannelFactory<TSoapClient>(GetHttpsBinding(), GetEndpoint(endpointAddress));
            }
            else
            {
                channelFactory = new ChannelFactory<TSoapClient>(GetHttpBinding(), GetEndpoint(endpointAddress));
            }

            return channelFactory;
        }

        private static void AddBehaviors<TSoapClient>(List<IEndpointBehavior> behaviors, ChannelFactory<TSoapClient> channelFactory)
        {
            if (behaviors != null && behaviors.Any())
            {
                foreach (var behavior in behaviors)
                {
                    channelFactory.Endpoint.EndpointBehaviors.Add(behavior);
                }
            }
        }

        private static void AddAuthentication<TSoapClient>(bool auth, string username, string password, string domain, ChannelFactory<TSoapClient> channelFactory)
        {
            if (auth)
            {
                channelFactory.Credentials.Windows.ClientCredential = GetCredentials(username, password, domain);
            }
        }

        private static NetworkCredential GetCredentials(string username, string password, string domain)
        {
            return new NetworkCredential
            {
                UserName = username,
                Password = password,
                Domain = domain
            };
        }

        private static BasicHttpsBinding GetHttpsBinding(bool useTransportWithNtlm = true)
        {
            var binding = new BasicHttpsBinding
            {
                MaxBufferSize = int.MaxValue,
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                MaxReceivedMessageSize = int.MaxValue,
                AllowCookies = true
            };

            if (useTransportWithNtlm)
            {
                binding.Security.Mode = BasicHttpsSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
                binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            }

            return binding;
        }

        private static BasicHttpBinding GetHttpBinding()
        {
            var binding = new BasicHttpBinding
            {
                MaxBufferSize = int.MaxValue,
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                MaxReceivedMessageSize = int.MaxValue,
                AllowCookies = true
            };

            return binding;
        }

        private static EndpointAddress GetEndpoint(string endpointAddress)
        {
            return new EndpointAddress(endpointAddress);
        }
    }
}
