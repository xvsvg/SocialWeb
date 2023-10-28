﻿using Domain.Common.Models;
using FluentValidation;

namespace Application.Handlers.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        if (error is null) throw new ArgumentNullException(nameof(error), "The error is required");

        return rule.WithErrorCode(error.Code).WithMessage(error.Message);
    }
}