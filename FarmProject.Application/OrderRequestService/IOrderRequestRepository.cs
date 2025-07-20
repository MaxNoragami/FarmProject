using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.OrderRequestService;

public interface IOrderRequestRepository
{
    public Task<OrderRequest> AddAsync(OrderRequest orderRequest);
    public Task<OrderRequest?> GetByIdAsync(int orderRequestId);
    public Task<PaginatedResult<OrderRequest>> GetPaginatedAsync(
        PaginatedRequest<OrderRequestFilterDto> request);
    public Task<List<OrderRequest>> FindAsync(ISpecification<OrderRequest> specification);
    public Task<OrderRequest> UpdateAsync(OrderRequest orderRequest);
}
