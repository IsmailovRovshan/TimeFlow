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
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public LessonService(
            ILessonRepository lessonRepository,
            ITeacherRepository teacherRepository,
            IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }

        public async Task<LessonDto> CreateAsync(LessonDtoForCreate lessonDto)
        {
            // Проверки
            var teacher = await _teacherRepository.GetByIdAsync(lessonDto.TeacherId);

            var lesson = _mapper.Map<Lesson>(lessonDto);
            await _lessonRepository.AddAsync(lesson);
            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task DeleteAsync(Guid TeacherId, Guid ClientId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(TeacherId, ClientId);
            if (lesson == null)
            {
                throw new KeyNotFoundException("Урок не найден.");
            }
            await _lessonRepository.DeleteAsync(lesson);
        }

        public async Task<List<LessonDto>> GetAllAsync()
        {
            var lessons = await _lessonRepository.GetAllAsync();
            return _mapper.Map<List<LessonDto>>(lessons);
        }

        public async Task<LessonDto> GetByIdAsync(Guid TeacherId, Guid ClientId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(TeacherId, ClientId);
            if (lesson == null)
            {
                throw new KeyNotFoundException("Урок не найден.");
            }
            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task UpdateAsync(Guid TeacherId, Guid ClientId, LessonDtoForUpdate lessonDto)
        {
            var lesson = await _lessonRepository.GetByIdAsync(TeacherId, ClientId);

            if (lesson == null)
            {
                throw new KeyNotFoundException("Урок не найден.");
            }

            _mapper.Map(lessonDto, lesson);

            await _lessonRepository.UpdateAsync(lesson);
        }

        public async Task<List<LessonDto>> GetLessonsByDateAsync(Guid teacherId, DateTime date)
        {
            var lessons = await _lessonRepository.GetLessonsByDateAsync(teacherId, date);
            return _mapper.Map<List<LessonDto>>(lessons);
        }
    }
}
