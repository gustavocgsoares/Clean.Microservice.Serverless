namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models
{
    /// <summary>
    /// Base response model with message fields.
    /// </summary>
    public class MessageResponseModel : IResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageResponseModel"/> class.
        /// </summary>
        /// <param name="code">Response code.</param>
        /// <param name="message">Resposne message.</param>
        public MessageResponseModel(string code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Gets response code.
        /// </summary>
        /// <value>
        /// Response code.
        /// </value>
        public virtual string Code { get; private set; }

        /// <summary>
        /// Gets response message.
        /// </summary>
        /// <value>
        /// Response message.
        /// </value>
        public virtual string Message { get; private set; }
    }
}
