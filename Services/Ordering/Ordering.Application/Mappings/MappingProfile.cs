﻿using AutoMapper;
using Ordering.Application.Features.Orders.Command.CheckoutOrder;
using Ordering.Application.Features.Orders.Command.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Doman.Entities;

namespace Ordering.Application.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrdersVM>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();

        }
    }
}
