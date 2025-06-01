using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public class ConsumerErrors
{
    public static readonly Error RabbitNotFound = new(
        "Consumer.RabbitNotFound", "Rabbit referenced in the event could not be found");

    public static readonly Error ProcessingFailed = new(
        "Consumer.ProcessingFailed", "Event processing failed due to an internal error");
}
