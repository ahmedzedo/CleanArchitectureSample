﻿using CleanArchitecture.Application.Common.Messaging;
using MediatR;
using System.Diagnostics;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class LoggingPreprocessorBehaviour<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : notnull, IBaseRequest
    {
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Preprocessor");
            return Task.CompletedTask;
        }
    }
}
