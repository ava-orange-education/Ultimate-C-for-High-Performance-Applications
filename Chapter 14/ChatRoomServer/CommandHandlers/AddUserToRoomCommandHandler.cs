using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using MediatR;
using SharedContracts.Commands;

namespace ChatRoomServer.CommandHandlers;

public class AddUserToRoomCommandHandler(IChatRoomStore chatRoomStore, IUserStore userStore, IMediator mediator) : IRequestHandler<AddUserToRoomCommand, Unit>
{
    public async Task<Unit> Handle(AddUserToRoomCommand request, CancellationToken cancellationToken)
    {
        foreach (var userId in request.AddedUserIds)
        {
            if (!userStore.CheckUserExists(userId))
            {
                throw new InvalidOperationException($"User {userId} not found.");
            }
        }

        foreach (var userId in request.AddedUserIds)
        {
            //TODO: User Joined Chat Event
            chatRoomStore.AddUser(request.RoomId, userId);

            var userInfo = userStore.GetUser(userId);
            var userAddedToRoomEvent = new UserAddedToRoomEvent(request.RoomId, request.SenderId, userInfo!);
            await mediator.Publish(userAddedToRoomEvent, cancellationToken);
        }

        return Unit.Value;
    }
}
