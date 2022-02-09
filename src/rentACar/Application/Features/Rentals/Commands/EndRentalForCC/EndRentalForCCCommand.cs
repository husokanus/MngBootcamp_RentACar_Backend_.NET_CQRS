﻿using Application.Features.Cars.Commands.UpdateCar;
using Application.Features.Rentals.Rules;
using Application.Services.CarService;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Rentals.Commands.EndRentalForCC
{
    public class EndRentalForCCCommand : IRequest<Rental>
    {
        public int Id { get; set; }
        public DateTime ReturnedDate { get; set; }
        public int ReturnedKilometer { get; set; }
        public int ReturnedCityId { get; set; }
        public int CarId { get; set; }

        public class EndRentalForCCCommandHandler : IRequestHandler<EndRentalForCCCommand, Rental>
        {
            IRentalRepository _rentalRepository;
            IMapper _mapper;
            RentalBusinessRules _rentalBusinessRules;
            ICarService carService;
            public EndRentalForCCCommandHandler(IRentalRepository rentalRepository,
                IMapper mapper,
                RentalBusinessRules rentalBusinessRules, ICarService carService)
            {
                _rentalRepository = rentalRepository;
                _mapper = mapper;
                _rentalBusinessRules = rentalBusinessRules;
                this.carService = carService;
            }

            public async Task<Rental> Handle(EndRentalForCCCommand request, CancellationToken cancellationToken)
            {
                var rentalToEnd = await _rentalRepository.GetAsync(r => r.Id == request.Id);
                var mappedRental = _mapper.Map(request, rentalToEnd);                

                var createdRental = await _rentalRepository.UpdateAsync(mappedRental);

                //Update CarState and Kilometer
                UpdateCarAfterRentalEndCommand command = new UpdateCarAfterRentalEndCommand
                {
                    Id = request.CarId,
                    CarState = CarState.Available,
                    Kilometer = request.ReturnedKilometer,
                    CityId = request.ReturnedCityId
                };
                await this.carService.UpdateCarAfterRentalEnd(command);

                return createdRental;
            }
        }
    }
}
