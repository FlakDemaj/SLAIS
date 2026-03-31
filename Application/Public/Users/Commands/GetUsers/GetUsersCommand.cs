using Application.Utils.Interfaces.Mediator;
using Application.Utils.Interfaces.Transaction;

namespace Application.Public.Users.Commands.GetUsers;

public class GetUsersCommand : IRequest<List<GetUserResponseDto>>, INoTransaction;
