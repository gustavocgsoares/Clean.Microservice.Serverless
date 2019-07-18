using Microsoft.AspNetCore.Mvc;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Presenters;

namespace Clean.Microservice.Serverless.SharedKernel.Function.UseCases.Presenters
{
    /// <summary>
    /// Interface to result presenter.
    /// </summary>
    /// <typeparam name="TResult">Type of result data.</typeparam>
    public interface IPresenter<TResult> : IPresenter
    {
        /// <summary>
        /// Gets api response model.
        /// </summary>
        /// <value>
        /// Api response model.
        /// </value>
        IActionResult ResponseModel { get; }

        /// <summary>
        /// Populate response with <typeparamref name="TResult"/> data.
        /// </summary>
        /// <param name="result">Result to be mapped.</param>
        void Populate(TResult result);
    }
}