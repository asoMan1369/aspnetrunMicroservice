using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Models;
using Ordering.Doman.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Command.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder =  await  _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {newOrder.Id} is succesfully created.");
            await SendEmail(newOrder);
            return newOrder.Id;
        }

        private async Task SendEmail(Order order)
        {
            var email = new Email()
            {
                To = "aso.mansoury@gmail.com",
                Body = $"Order was created",
                Subject = $"Order was created"
            };
            try
            {
                await _emailService.SendEmail(email);
            }
            catch (System.Exception)
            {

                _logger.LogError($"Order {order.Id} failed due to an error with the mail");
            }
        }
    }
}
