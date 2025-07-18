﻿namespace FarmProject.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            throw new ArgumentException("Invalid error", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result Success()
        => new Result(isSuccess: true, Error.None);
    public static Result<T> Success<T>(T value)
        => new Result<T>(value, isSuccess: true, Error.None);

    public static Result Failure(Error error)
        => new Result(isSuccess: false, error);

    public static Result<T> Failure<T>(Error error)
        => new Result<T>(default!, isSuccess: false, error);
}

public class Result<T> : Result
{
    private readonly T _value;

    protected internal Result(T value, bool isSuccess, Error error)
        : base(isSuccess, error)
        => _value = value;

    public T Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Cannot access the value of a failure result");
}