using MassTransit;
using Application.Services;
using Domain.Messages;
using Application.Interfaces;

namespace WebApi.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedMessage>
    {
        private readonly IUserService _userService;

        public UserCreatedConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<UserCreatedMessage> context)
        {
            var userId = context.Message.Id;
            await _userService.AddUserReferenceAsync(userId);
        }
    }
}