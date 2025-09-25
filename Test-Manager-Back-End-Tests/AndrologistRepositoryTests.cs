using Clinic_Manager_Back_End_Tests;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManagerBackEnd.Functions;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestManager.DataAccess;


public class AndrologistRepositoryTests
{
    //private readonly Mock<ISequenceRepository> _mockSequenceRepo;
    private readonly Mock<ICMS_SyncTrackingRepository> _mockICMS_SyncTrackingRepo;
    public AndrologistRepositoryTests()
    {
        //_mockSequenceRepo = new Mock<ISequenceRepository>();
        _mockICMS_SyncTrackingRepo = new Mock<ICMS_SyncTrackingRepository>();
    }

    [Fact]
    public async Task GetAndrologistsAsync_WithNoFilter_ReturnsAll()
    {
        var context = TestDbContextFactory.Create("GetAllTest");
        var data = new List<Andrologist>
        {
            new() { AndrologistID = 1, FirstName = "John", LastName = "Doe", Gender = "M", Address = "123 St" },
            new() { AndrologistID = 2, FirstName = "Jane", LastName = "Smith", Gender = "F", Address = "456 Ave" }
        }.AsQueryable();        

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetAllTest")
            .Options;

        //await using var context = new ApplicationDbContext(options);
        await context.AddRangeAsync(data);
        await context.SaveChangesAsync();

        var repository = new AndrologistRepository(context, _mockICMS_SyncTrackingRepo.Object);
       
        var andrologists = await repository.GetAndrologistsAsync();

        Assert.Equal(2, andrologists.Count);
    }

    [Fact]
    public async Task AddAndrologist_ShouldAddAndReturnDto()
    {
        var context = TestDbContextFactory.Create("AddTest");
        var repository = new AndrologistRepository(context, _mockICMS_SyncTrackingRepo.Object);

        var dto = new AndrologistDto { FirstName = "John", LastName = "Doe", Gender = "M", Address = "123 St" };
        var result = await repository.AddAndrologist(dto);

        Assert.Equal(dto.FirstName, result.FirstName);
        Assert.Equal(dto.LastName, result.LastName);
        Assert.Equal(dto.Gender, result.Gender);
        Assert.Equal(dto.Address, result.Address);
        Assert.NotEqual(0, result.AndrologistId);
    }

    //[Fact]
    //public async Task UpdateAndrologist_AndrologistExists_ShouldUpdate()
    //{
    //    var context = TestDbContextFactory.Create("UpdateTest");
    //    var andrologist = new Andrologist { AndrologistID = 1, FirstName = "John", LastName = "Doe", Gender = "Male" };
    //    await context.Andrologist.AddAsync(andrologist);
    //    await context.SaveChangesAsync();

    //    var repository = new AndrologistRepository(context, _mockICMS_SyncTrackingRepo.Object);

    //    var dto = new AndrologistDto { AndrologistId = 1, FirstName = "Updated", LastName = "Doe", Gender = "Male" };
    //    var result = await repository.UpdateAndrologist(dto);

    //    Assert.NotNull(result);
    //    Assert.Equal("Updated", result.FirstName);
    //}

    //[Fact]
    //public async Task DeleteAndrologist_AndrologistExists_ShouldSetIsDeleted()
    //{
    //    var context = TestDbContextFactory.Create("DeleteTest");
    //    var andrologist = new Andrologist { AndrologistID = 1, FirstName = "Updated", LastName = "Doe", Gender = "Male"};
    //    await context.Andrologist.AddAsync(andrologist);
    //    await context.SaveChangesAsync();

    //    var repository = new AndrologistRepository(context, _mockICMS_SyncTrackingRepo.Object);

    //    var result = await repository.DeleteAdnrologist(1);

    //    Assert.True(result);        
    //}
} 
