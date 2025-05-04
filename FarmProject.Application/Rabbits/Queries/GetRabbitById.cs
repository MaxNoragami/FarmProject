using FarmProject.Application.Rabbits.Responses;
using MediatR;

namespace FarmProject.Application.Rabbits.Queries;

public record GetRabbitById(int RabbitId) : IRequest<RabbitDto>;

public class GetRabbitByIdHandler(IRabbitRepository rabbitRepository) : IRequestHandler<GetRabbitById, RabbitDto>
{
    private readonly IRabbitRepository _rabbitRepository = rabbitRepository;
    public Task<RabbitDto> Handle(GetRabbitById request, CancellationToken cancellationToken)
    {
        var requestRabbit = _rabbitRepository.GetById(request.RabbitId) 
            ?? throw new ArgumentNullException("Rabbit not found.");

        return Task.FromResult(RabbitDto.FromRabbit(requestRabbit));
    }
}