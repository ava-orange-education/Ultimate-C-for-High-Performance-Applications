using ChatRoomServer.Interfaces;
using MediatR;
using SharedContracts.Commands;
using SharedContracts.Events;

namespace ChatRoomServer.CommandHandlers;

public class SendChatMessageCommandHandler(IChatRoomStore chatRoomStore, IMediator mediator) : IRequestHandler<SendChatMessageCommand, Unit>
{
    public async Task<Unit> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
    {
        if (chatRoomStore.StoreMessage(request))
        {
            var chatMessageReceivedEvent = new ChatMessageReceivedEvent(request.User,
                request.RoomId,
                request.Id,
                request.Timestamp,
                request.Message);
            await mediator.Publish(chatMessageReceivedEvent, cancellationToken);
        }
        return Unit.Value;
    }
}
