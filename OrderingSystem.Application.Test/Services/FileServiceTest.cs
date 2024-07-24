using Bogus;
using Moq;
using OrderingSystem.Application.Repositories;
using OrderingSystem.Application.Services;
using OrderingSystem.Infrastructure;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using OrderingSystem.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Test.Services
{
    public class FileServiceTest
    {
        private Mock<IBaseRepository> baseRepository = new();
        private Mock<IFileRepository> fileRepository = new();
        private FileService fileService;
        public FileServiceTest()
        {
            fileService = new(baseRepository.Object, fileRepository.Object);
        }
        [Fact]
        public async Task AddFile_ShouldThrowBadRequestException()
        {
            //arrange
            var faker = new Faker();
            var addFile = new AddFileDto(faker.Name.Prefix(), faker.Random.String(), faker.Random.String(), faker.Random.Bytes(FileSize.MaxSize + 1), false);
            //act
            //assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await fileService.AddFile(addFile));
        }
        [Theory]
        [InlineData("file name 1")]
        [InlineData("file name 2")]
        public async Task AddFile_ShouldSuccess(string fileName)
        {
            //arrange
            var faker = new Faker();
            var addFile = new AddFileDto(fileName, faker.Random.String(), faker.Random.String(), faker.Random.Bytes(faker.Random.Number(1, FileSize.MaxSize)), false);
            //act
            var result = await fileService.AddFile(addFile);
            //assert
            Assert.NotNull(result);
            Assert.Equal(fileName, result.Name);
            baseRepository.Verify(x => x.AddData(It.IsAny<TblFile>()), Times.Once);
            baseRepository.Verify(x => x.SaveChanges(It.IsAny<Guid?>()), Times.Once);
        }
    }
}
