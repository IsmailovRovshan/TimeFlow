using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Services.Abstractions;
using Services.Abstractions.DTO;


namespace Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;

        public SubjectService(
            ISubjectRepository subjectRepository,
            IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        public async Task DeleteAllAsync()
        {
            await _subjectRepository.DeleteAllAsync();
        }
        public async Task<SubjectDto> CreateAsync(SubjectDtoForCreate subjectDto)
        {
            var subject = _mapper.Map<Subject>(subjectDto);
            await _subjectRepository.AddAsync(subject);
            return _mapper.Map<SubjectDto>(subject);

            Console.WriteLine('1111');
        }

        public async Task DeleteAsync(Guid subjectId)
        {
            var subject = await _subjectRepository.GetByIdAsync(subjectId) 
                ?? throw new ArgumentException("Предмет не найден."); 

            await _subjectRepository.DeleteAsync(subject);

            Console.WriteLine('111122');
        }

        public async Task<List<SubjectDto>> GetAllAsync()
        {
            var subjects = await _subjectRepository.GetAllAsync();
            return _mapper.Map<List<SubjectDto>>(subjects);
        }

        public async Task<SubjectDto> GetByIdAsync(Guid id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("Менеджер не найден."); ;

            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task UpdateAsync(Guid subjectId, SubjectDtoForUpdate subject)
        {
            var existingSubject = await _subjectRepository.GetByIdAsync(subjectId)
                ?? throw new ArgumentException("Менеджер не найден."); ;

            _mapper.Map(subject, existingSubject);

            await _subjectRepository.UpdateAsync(existingSubject);
        }
    }
}
