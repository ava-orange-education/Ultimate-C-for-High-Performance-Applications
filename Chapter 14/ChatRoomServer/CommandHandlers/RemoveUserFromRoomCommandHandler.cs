using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using MediatR;
using SharedContracts.Commands;

namespace ChatRoomServer.CommandHandlers;

public class RemoveUserFromRoomCommandHandler(IChatRoomStore chatRoomStore, IMediator mediator) : IRequestHandler<RemoveUserFromRoomCommand, Unit>
{
    public async Task<Unit> Handle(RemoveUserFromRoomCommand request, CancellationToken cancellationToken)
    {
        if (chatRoomStore.RemoveUser(request.RoomId, request.RemovedUserId, request.SenderId))
        {
            await mediator.Publish(new UserRemovedFromRoomEvent(request.RoomId, request.RemovedUserId, request.SenderId), cancellationToken);
        }
        return Unit.Value;
    }
}
