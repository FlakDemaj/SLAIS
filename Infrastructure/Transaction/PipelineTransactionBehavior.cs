using Application.Common;
using Application.Utils.Exceptions;
using Application.Utils.Logger;

namespace Application.Utils;

public class PipelineTransactionBehavior<TRequest, TResponse> :
    IPipelineTransactionBehavior<TRequest, TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISlaisLogger<PipelineTransactionBehavior<TRequest, TResponse>> _logger;

    public PipelineTransactionBehavior(IUnitOfWork unitOfWork,
        ISlaisLogger<PipelineTransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var response = await next();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return response;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError($"An error occurred during transaction creation: {e}", e);
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode, e);
        }

    }
}
