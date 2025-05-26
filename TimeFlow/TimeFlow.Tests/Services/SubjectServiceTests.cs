using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using System;
using System.Threading.Tasks;
using Services.Abstractions.DTO;
using Domain.Repository;
using Services;
using Domain.Entities;

namespace TimeFlow.Tests.Services
{
    public class SubjectServiceTests
    {
        [Fact]
        public async Task CreateAsync_ShouldMapAndAddSubject_ThenReturnMappedDto()
        {
            
            var subjectDtoForCreate = new SubjectDtoForCreate("Информатика");

            var createdSubject = new Subject
            {
                Id = Guid.NewGuid(),
                Name = "Информатика"
            };

            var subjectDto = new SubjectDto
            (
                createdSubject.Id,
                createdSubject.Name
            );

            var mockRepo = new Mock<ISubjectRepository>();
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<Subject>(subjectDtoForCreate))
                      .Returns(createdSubject);

            mockMapper.Setup(m => m.Map<SubjectDto>(createdSubject))
                      .Returns(subjectDto);

            var subjectService = new SubjectService(mockRepo.Object, mockMapper.Object);

            var result = await subjectService.CreateAsync(subjectDtoForCreate);

            mockRepo.Verify(r => r.AddAsync(It.Is<Subject>(s => s == createdSubject)), Times.Once);
            result.Should().BeEquivalentTo(subjectDto);
        }
    }
}
