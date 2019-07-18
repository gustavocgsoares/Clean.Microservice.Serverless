using System;
using AutoMapper;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models.V1
{
    /// <summary>
    /// Convert <see cref="DateTimeOffset"/> to <see cref="DateTimeModel"/>.
    /// </summary>
    public class DateTimeModelConverter : IValueConverter<DateTimeOffset, DateTimeModel>
    {
        /// <inheritdoc/>
        public DateTimeModel Convert(DateTimeOffset sourceMember, ResolutionContext context)
        {
            if (sourceMember == DateTimeOffset.MinValue)
            {
                return null;
            }

            return new DateTimeModel
            {
                FullDate = sourceMember,
                Day = sourceMember.Day,
                Month = DateMonthModel.Builder(sourceMember.Month),
                Year = DateYearModel.Builder(sourceMember.Year),
                FormattedTime = sourceMember.ToString("HH:mm:ss"),
                Hour = sourceMember.Hour,
                Minute = sourceMember.Minute,
                Second = sourceMember.Second
            };
        }
    }
}
