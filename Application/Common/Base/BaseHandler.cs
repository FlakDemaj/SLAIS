using Application.Utils.Logger;
using AutoMapper;

namespace Application.Common;

public abstract class BaseHandler<T> where T : class
{
    protected ISAISLogger<T> _logger;

    protected IMapper _mapper;
    
    protected BaseHandler(
        ISAISLogger<T> logger,
        IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }
}