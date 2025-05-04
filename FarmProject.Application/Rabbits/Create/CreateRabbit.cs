using MediatR;
using FarmProject.Application.Rabbits.Responses;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Rabbits.Create;

public record CreateRabbit(string Name, Gender Gender, bool Breedable) : IRequest<RabbitDto>;

public class CreateRabbitHandler(IRabbitRepository rabbitRepository) : IRequestHandler<CreateRabbit, RabbitDto>
{
    private readonly IRabbitRepository _rabbitRepository = rabbitRepository;

    public Task<RabbitDto> Handle(CreateRabbit request, CancellationToken cancellationToken)
    {
        var requestRabbit = new Rabbit()
            { 
                Id = GetNextId(), 
                Name = request.Name, 
                Gender = request.Gender, 
                Breedable = request.Breedable 
            };

        var createdRabbit = _rabbitRepository.Create(requestRabbit);

        return Task.FromResult(RabbitDto.FromRabbit(createdRabbit));
    }

    private int GetNextId()
        => _rabbitRepository.GetLastId();
}
