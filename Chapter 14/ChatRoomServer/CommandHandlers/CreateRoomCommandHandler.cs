using ChatRoomServer.Interfaces;
using MediatR;
using SharedContracts.Commands;

namespace ChatRoomServer.CommandHandlers;

public class CreateRoomCommandHandler(IChatRoomStore chatRoomStore) : IRequestHandler<CreateRoomCommand, Unit>
{
    public Task<Unit> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        chatRoomStore.CreateRoom(request.RoomId, request.RoomName, request.UserIds);
        return Unit.Task;
    }
}
