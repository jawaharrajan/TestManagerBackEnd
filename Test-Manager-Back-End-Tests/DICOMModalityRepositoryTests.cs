using Clinic_Manager_Back_End_Tests;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestManager.DataAccess;

namespace Clinic_Manager_Back_End_Tests
{
    public class DICOMModalityRepositoryTests
    {
        private readonly Mock<ISequenceRepository> _mockSequenceRepo;
        private readonly Mock<ICMS_SyncTrackingRepository> _mockICMS_SyncTrackingRepo;
        public DICOMModalityRepositoryTests()
        {
            _mockSequenceRepo = new Mock<ISequenceRepository>();
            _mockICMS_SyncTrackingRepo = new Mock<ICMS_SyncTrackingRepository>();
        }

        [Fact]
        public async Task GetAndrologistsAsync_WithNoFilter_ReturnsAll()
        {
            var context = TestDbContextFactory.Create("GetAllTest");
            var data = new List<DICOMModality>
                {
                    new() { DICOMModalityId = 1, RoomCode = "MCNDXWL1", StudyDescription = "X-Ray Chest", ProcedureCode = "C001", ModalityCode = "DX"},
                    new() { DICOMModalityId = 2, RoomCode = "MCNULWL1", StudyDescription = "US Abdomen", ProcedureCode = "C004", ModalityCode = "US"}
                }.AsQueryable();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllTest")
                .Options;

            //await using var context = new ApplicationDbContext(options);
            await context.AddRangeAsync(data);
            await context.SaveChangesAsync();

            var repository = new DICOMModalityRepository(context, _mockICMS_SyncTrackingRepo.Object, _mockSequenceRepo.Object);

            var modality = await repository.GetDICOMModalityAsync();

            Assert.Equal(2, modality.Count);
        }

        [Fact]
        public async Task AddDICOMModality_ShouldAddAndReturnDto()
        {
            var context = TestDbContextFactory.Create("AddTest");

            var repository = new DICOMModalityRepository(context, _mockICMS_SyncTrackingRepo.Object, _mockSequenceRepo.Object);

            var dto = new DICOMModalityDTO { ModalityId = 1, RoomCode = "MCNDXWL1", StudyDescription = "X-Ray Chest", ProcedureCode = "C001", ModalityCode = "DX" };

            var result = await repository.AddDICOMModality(dto);

            Assert.Equal(dto.RoomCode, result.RoomCode);
            Assert.Equal(dto.StudyDescription, result.StudyDescription);
            Assert.Equal(dto.ProcedureCode, result.ProcedureCode);
            Assert.Equal(dto.ModalityCode, result.ModalityCode);           
        }

        //[Fact]
        //public async Task UpdateDICOMModality_DICOMMOdalityExists_ShouldUpdate()
        //{
        //    var context = TestDbContextFactory.Create("UpdateTest");
        //    var repository = new DICOMModalityRepository(context, _mockICMS_SyncTrackingRepo.Object, _mockSequenceRepo.Object);
        //    var dicomm = new DICOMModality { DICOMModalityId = 1, RoomCode = "MCNDXWL1", StudyDescription = "X-Ray Chest", ProcedureCode = "C001", ModalityCode = "DX" };

        //    await context.DICOMModality.AddAsync(dicomm);
        //    await context.SaveChangesAsync();                       

        //    var dto = new DICOMModalityDTO { ModalityId = 1, RoomCode = "MCNDXWL2", StudyDescription = "X-Ray Chest 2", ProcedureCode = "C001", ModalityCode = "DX" };
        //    _mockICMS_SyncTrackingRepo.Setup(x => x.GetTIPSId(It.IsAny<string>(),It.IsAny<int>())).ReturnsAsync(It.IsAny<int>);
        //    var result = await repository.UpdateDICOMModality(dto);

        //    Assert.NotNull(result);
        //    Assert.Equal("MCNDXWL2", result.RoomCode);
        //    Assert.Equal("X-Ray Chest 2", result.StudyDescription);
        //}
    }
}
