using Application.Common.Authentication;
using Application.Common.Base;
using Application.Interfaces;
using Application.Utils.Logger;
using Application.Utils.Mediator.Interfaces;

using AutoMapper;

using Domain.Common;

namespace Application.Public.Users.Commands.GetUsers;

public class GetUsersCommandHandler :
    BaseHandler<GetUsersCommandHandler>,
    IRequestHandler<GetUsersCommand, List<GetUserResponseDto>>
{
    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;

    public GetUsersCommandHandler(
        ISlaisLogger<GetUsersCommandHandler> logger,
        IUserRepository userRepository,
        IMapper mapper)
        : base(logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<GetUserResponseDto>> HandleAsync(
        GetUsersCommand request, CancellationToken cancellationToken = default,
        IAuthentication authentication = null)
    {
        var users = await _userRepository.GetAllUsersFromInstitute(
            authentication.InstitutionGuid, authentication.UserRole);

        return _mapper.Map<List<GetUserResponseDto>>(users);

    }
}
