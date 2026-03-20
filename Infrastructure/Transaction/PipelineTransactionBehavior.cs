using Application.Common;
using Application.Utils.Exceptions;
using Application.Utils.Interfaces.Transaction;
using Application.Utils.Logger;

namespace Infrastructure.Transaction;

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

    public async Task<TResponse> HandleAsync(
        TRequest request,
        Func<Task<TResponse>> next,
        CancellationToken cancellationToken)
    {
        if (request is not INoTransaction)
        {
            return await StartWithTransactionAsync(
                next,
                cancellationToken);
        }

        return await StartWithoutTransactionAsync(
            next,
            cancellationToken);
    }

    private async Task<TResponse> StartWithoutTransactionAsync(
        Func<Task<TResponse>> next,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await next();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return response;
        }
        catch (SlaisException)
        {
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError($"An error occurred: {e}", e);
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode, e);
        }
    }

    private async Task<TResponse> StartWithTransactionAsync(
        Func<Task<TResponse>> next,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var response = await next();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return response;
        }
        catch (SlaisException)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError($"An error occurred during transaction: {e}", e);
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode, e);
        }
    }
}
