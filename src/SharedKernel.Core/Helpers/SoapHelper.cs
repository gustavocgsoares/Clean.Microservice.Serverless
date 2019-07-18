using System;
using System.Net;
using System.ServiceModel;
using Modal.SharedKernel.Core.Helpers.Soap.Behavior;

namespace Modal.SharedKernel.Core.Helpers
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
        /// <param name="auth">indicates if must to authenticate</param>
        /// <param name="username">AD username</param>
        /// <param name="password">AD password</param>
        /// <param name="domain">AD domain</param>
        /// <returns>TSoapClient instance</returns>
        public static TSoapClient GetServicesSoap<TSoapClient>(string endpointAddress, bool auth = false, string username = null, string password = null, string domain = null)
        {
            return GetChannelFactory<TSoapClient>(endpointAddress, auth, username, password, domain);
        }

        private static TSoapClient GetChannelFactory<TSoapClient>(string endpointAddress, bool auth = false, string username = null, string password = null, string domain = null)
        {
            var mustUseHttpsBinding = endpointAddress.IndexOf("https", StringComparison.InvariantCultureIgnoreCase) != -1;

            ChannelFactory<TSoapClient> channelFactory = null;

            if (mustUseHttpsBinding)
            {
                channelFactory = new ChannelFactory<TSoapClient>(GetHttpsBinding(), GetEndpoint(endpointAddress));
            }
            else
            {
                channelFactory = new ChannelFactory<TSoapClient>(GetHttpBinding(), GetEndpoint(endpointAddress));
            }

            if (auth)
            {
                channelFactory.Credentials.Windows.ClientCredential = GetCredentials(username, password, domain);
            }

            var handlerFactoryBehavior = new HttpMessageHandlerBehavior
            {
                OnSending = (message, token) =>
                {
                    message.Headers.ExpectContinue = false;
                    return null;
                }
            };

            channelFactory.Endpoint.EndpointBehaviors.Add(handlerFactoryBehavior);

            return channelFactory.CreateChannel();
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

        private static BasicHttpsBinding GetHttpsBinding()
        {
            var binding = new BasicHttpsBinding
            {
                MaxBufferSize = int.MaxValue,
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                MaxReceivedMessageSize = int.MaxValue,
                AllowCookies = true
            };

            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;

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
