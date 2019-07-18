using System;
using AutoMapper;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models.V1
{
    /// <summary>
    /// Convert <see cref="DateTimeOffset"/> to <see cref="DateModel"/>.
    /// </summary>
    public class DateModelConverter : IValueConverter<DateTimeOffset, DateModel>
    {
        /// <inheritdoc/>
        public DateModel Convert(DateTimeOffset sourceMember, ResolutionContext context)
        {
            if (sourceMember == DateTimeOffset.MinValue)
            {
                return null;
            }

            return new DateModel
            {
                FullDate = sourceMember,
                Day = sourceMember.Day,
                Month = DateMonthModel.Builder(sourceMember.Month),
                Year = DateYearModel.Builder(sourceMember.Year)
            };
        }
    }
}
