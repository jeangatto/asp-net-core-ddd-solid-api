﻿using SGP.Shared.Notifications;
using System.Collections.Generic;

namespace SGP.Shared.Results
{
    public partial class Result : IResult
    {
        internal Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        internal Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Errors = new[] { new Notification(default, error) };
        }

        internal Result(bool isSuccess, IEnumerable<Notification> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public bool IsSuccess { get; }
        public IEnumerable<Notification> Errors { get; }
    }
}
