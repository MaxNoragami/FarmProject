using FarmProject.Application.Rabbits.Responses;
using MediatR;

namespace FarmProject.Application.Rabbits.Queries;

public record GetAllRabbits : IRequest<List<RabbitDto>>;

public class GetAllRabbitsHandler(IRabbitRepository rabbitRepository) : IRequestHandler<GetAllRabbits, List<RabbitDto>>
{
    private readonly IRabbitRepository _rabbitRepository = rabbitRepository;
    public Task<List<RabbitDto>> Handle(GetAllRabbits request, CancellationToken cancellationToken)
    {
        var rabbits = _rabbitRepository.GetAll();
        return Task.FromResult(rabbits.Select(RabbitDto.FromRabbit).ToList());
    }
}