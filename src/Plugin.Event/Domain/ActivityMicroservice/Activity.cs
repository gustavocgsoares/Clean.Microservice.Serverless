using System;
using System.Collections.Generic;

namespace Clean.Microservice.Serverless.Plugin.Event.Domain.ActivityMicroservice
{
    /// <summary>
    /// Activity class informartion
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// Gets Correlation identification
        /// </summary>
        /// <value>
        /// Correlation identification
        /// </value>
        public virtual string CorrelationId { get; private set; }

        /// <summary>
        /// Gets customer information
        /// </summary>
        /// <value>
        /// Customer information
        /// </value>
        public virtual Customer Customer { get; private set; }

        /// <summary>
        /// Gets microservice name
        /// <example>
        ///     Clean.Microservice.Serverless
        /// </example>
        /// </summary>
        /// <value>
        /// Microservice name
        /// </value>
        public virtual string Microservice { get; private set; }

        /// <summary>
        /// Gets feature description
        /// <example>
        ///    Activate
        /// </example>
        /// </summary>
        /// <value>
        /// Feature description
        /// </value>
        public virtual string Feature { get; private set; }

        /// <summary>
        /// Gets title description
        /// <example>
        ///    Ativação de cartão
        /// </example>
        /// </summary>
        /// <value>
        /// Title description
        /// </value>
        public virtual string Title { get; private set; }

        /// <summary>
        /// Gets activity description
        /// <example>
        /// Ativação de cartão final 2125 realizada com sucesso
        /// </example>
        /// </summary>
        /// <value>
        /// Activity description
        /// </value>
        public virtual string Description { get; private set; }

        /// <summary>
        /// Gets information of who created activity
        /// <remarks>
        /// client or user
        /// </remarks>
        /// </summary>
        /// <value>
        /// Information of who created activity
        /// </value>
        public virtual string CreateBy { get; private set; }

        /// <summary>
        /// Gets activity created date
        /// </summary>
        /// <value>
        /// Activity created date
        /// </value>
        public virtual DateTime CreateAt { get; private set; }

        /// <summary>
        /// Gets a value indicating whether success or failure in performing the activity
        /// </summary>
        /// <value>
        /// A value indicating whether success or failure in performing the activity
        /// </value>
        public virtual bool Successfully { get; private set; }

        /// <summary>
        /// Gets additional information
        /// </summary>
        /// <value>
        /// Additional information
        /// </value>
        public virtual IDictionary<string, string> AdditionalInfo { get; private set; }

        /// <summary>
        /// Create activity.
        /// </summary>
        /// <param name="correlationId">Correlation identification.</param>
        /// <param name="customerId">Customer id.</param>
        /// <param name="customerName">Customer name.</param>
        /// <param name="microservice">Microservice name.</param>
        /// <param name="feature">Feature name.</param>
        /// <param name="title">Activity title.</param>
        /// <param name="description">Activity description.</param>
        /// <param name="createBy">User that create the activity.</param>
        /// <param name="createAt">Date and time of the activity creation.</param>
        /// <param name="successfully">True if the activity was executed successfully. Otherwise, false.</param>
        /// <param name="additionalInfo">Additional info.</param>
        /// <returns>Activity created.</returns>
        public static Activity Builder(
            string correlationId,
            string customerId,
            string customerName,
            string microservice,
            string feature,
            string title,
            string description,
            string createBy,
            DateTime createAt,
            bool successfully,
            IDictionary<string, string> additionalInfo)
        {
            return new Activity
            {
                CorrelationId = correlationId,
                Customer = Customer.Builder(customerId, customerName),
                Microservice = microservice,
                Feature = feature,
                Title = title,
                Description = description,
                CreateAt = createAt,
                CreateBy = createBy,
                Successfully = successfully,
                AdditionalInfo = additionalInfo
            };
        }
    }
}
