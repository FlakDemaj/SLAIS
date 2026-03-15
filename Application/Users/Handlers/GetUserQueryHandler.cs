using Application.Common;
using Application.Interfaces;
using Application.Users.DTOs;
using Application.Utils;
using Application.Utils.Logger;
using Application.Utils.MediatR.Interfaces;

namespace Application.Users.Handlers;

public class GetUserQueryHandler : BaseHandler<GetUserQueryHandler>, IRequestHandler<GetUserQuery, GetUserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository, ISAISLogger<GetUserQueryHandler> logger) :
        base(logger)
    {
        _userRepository = userRepository;
    }
    
    public async Task<GetUserDto> HandleAsync(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByGuidAsync(request.userGuid);

        if (user != null)
            return new GetUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
            };
        _logger.LogWarning("Würde mir stinken");
        throw new SAISException(UserErrorCodes.UserNotFound);

    }
}