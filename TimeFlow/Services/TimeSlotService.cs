using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Services.Abstractions;
using Services.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IMapper _mapper;

        public TimeSlotService(
            ITimeSlotRepository timeSlotRepository,
            IMapper mapper)
        {
            _timeSlotRepository = timeSlotRepository;
            _mapper = mapper;
        }
        public async Task DeleteAllAsync()
        {
            await _timeSlotRepository.DeleteAllAsync();
        }
        public async Task<TimeSlotDto> CreateAsync(TimeSlotDtoForCreate timeSlotDto)
        {
            var existingSlot = await _timeSlotRepository.GetByUserDayTimeAsync(timeSlotDto.UserId, timeSlotDto.DayOfWeek, timeSlotDto.Time);

            if (existingSlot != null) {
                throw new ArgumentException("Такой слот уже существует");
            }

            var timeSlot = _mapper.Map<TimeSlot>(timeSlotDto);
            await _timeSlotRepository.AddAsync(timeSlot);
            return _mapper.Map<TimeSlotDto>(timeSlot);
        }

        public async Task DeleteAsync(Guid timeSlotId)
        {
            var timeSlot = await _timeSlotRepository.GetByIdAsync(timeSlotId);

            if (timeSlot == null)
            {
                throw new ArgumentException("Слот не найден.");
            }

            await _timeSlotRepository.DeleteAsync(timeSlot);
        }

        public async Task<List<TimeSlotDto>> GetAllAsync()
        {
            var timeSlots = await _timeSlotRepository.GetAllAsync();

            return _mapper.Map<List<TimeSlotDto>>(timeSlots);
        }

        public async Task<TimeSlotDto> GetByIdAsync(Guid id)
        {
            var timeSlot = await _timeSlotRepository.GetByIdAsync(id);

            if (timeSlot == null)
            {
                throw new ArgumentException("Слот не найден.");
            }

            return _mapper.Map<TimeSlotDto>(timeSlot);
        }

        public async Task UpdateAsync(Guid timeSlotId, TimeSlotDtoForUpdate timeSlot)
        {
            var existingTimeSlot = await _timeSlotRepository.GetByIdAsync(timeSlotId);

            if (existingTimeSlot == null)
            {
                throw new ArgumentException("Слот не найден.");
            }

            _mapper.Map(timeSlot, existingTimeSlot);

            await _timeSlotRepository.UpdateAsync(existingTimeSlot);
        }

        public async Task<List<TimeSlotDto>> GetFreeTimeSlotsByUser(Guid userId)
        {
            var timeSlots = await _timeSlotRepository.GetFreeTimeSlotsByUser(userId);
            return _mapper.Map<List<TimeSlotDto>>(timeSlots);

        }
    }
}
