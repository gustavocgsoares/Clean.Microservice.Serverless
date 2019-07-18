using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.SharedKernel.Core.Serializers;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Clean.Microservice.Serverless.Plugin.Logger.Entities
{
    /// <summary>
    /// Class to log wcf calls.
    /// </summary>
    public class LogWcf : IClientMessageInspector
    {
        private string url;
        private string method;
        private string requestText;
        private DateTimeOffset startProcess;

        private readonly ITransactionFlow transactionFlow;
        private readonly IServerlessLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWcf"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="transactionFlow"></param>
        public LogWcf(IServerlessLogger logger, ITransactionFlow transactionFlow)
        {
            this.logger = logger;
            this.transactionFlow = transactionFlow;
        }

        /// <inheritdoc/>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (reply != null)
            {
                try
                {
                    var endProcess = DateTimeOffset.UtcNow;
                    var buffer = reply.CreateBufferedCopy(int.MaxValue);
                    var msgCopy = buffer.CreateMessage();

                    reply = buffer.CreateMessage();
                    var isFault = reply.IsFault;

                    CreateLog(msgCopy, endProcess, isFault);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Fail to create a third party wcf log response: {ex.ToString()}");
                }
            }
        }

        /// <inheritdoc/>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (channel != null)
            {
                try
                {
                    url = channel.RemoteAddress.Uri.AbsoluteUri;
                    method = request?.Headers?.Action;

                    var buffer = request?.CreateBufferedCopy(int.MaxValue);
                    var msgCopy = buffer.CreateMessage();

                    request = buffer.CreateMessage();

                    var xrdr = msgCopy.GetReaderAtBodyContents();

                    requestText = xrdr.ReadOuterXml();
                    startProcess = DateTimeOffset.UtcNow;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Fail to create a third party wcf log request: {ex.ToString()}");
                }
            }

            return null;
        }

        internal static string Serialize(object log)
        {
            var settings = new JsonSerializerSettings().GetJsonSerializerSettingsWithPrivateCamelCaseSerializer();

            return JsonConvert.SerializeObject(log, settings);
        }

        private void CreateLog(Message response, DateTimeOffset endProcess, bool isFault)
        {
            try
            {
                var xrdr = response.GetReaderAtBodyContents();
                var responseText = xrdr.ReadOuterXml();

                var log = new
                {
                    type = "Third Party",
                    date = DateTimeOffset.UtcNow,
                    correlationId = transactionFlow.CorrelationId.ToString(),
                    startTimestamp = startProcess,
                    endTimestamp = endProcess,
                    duration = Convert.ToDecimal(Math.Round((endProcess - startProcess).TotalMilliseconds, 2, MidpointRounding.AwayFromZero)),
                    authToken = transactionFlow.Token,
                    culture = transactionFlow.Culture,
                    url,
                    machine = transactionFlow.Machine,
                    hostIp = transactionFlow.HostIp,
                    localIp = transactionFlow.LocalIp,
                    serviceType = "wcf",
                    request = requestText,
                    response = responseText,
                    method,
                    state = response.State.ToString(),
                    hasError = isFault,
                    customerDocument = transactionFlow.CustomerDocument
                };

                Task.Factory.StartNew(() =>
                {
                    var logJson = Serialize(log);
                    logger.LogInformation(
                        logJson,
                        url,
                        requestText,
                        responseText,
                        transactionFlow);
                });
            }
            catch (Exception ex)
            {
                logger.LogError($"Fail to log third party wcf: {ex.ToString()}");
            }
        }
    }
}
