using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;


namespace ShopTARgv24.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange
            RealEstateDto dto = new();

            dto.Area = 120.5;
            dto.Location = "Downtown";
            dto.RoomNumber = 3;
            dto.BuildingType = "Apartment";
            dto.CreatedAt = DateTime.Now;
            dto.ModifiedAt = DateTime.Now;

            // Act
            var result = await Svc<IRealEstateServices>().Create(dto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_GetByIdRealestate_WhenReturnsNotEqual()
        {
            //Arrange
            Guid wrongGuid = Guid.Parse(Guid.NewGuid().ToString());
            Guid guid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");

            //Act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            //Assert
            Assert.NotEqual(wrongGuid, guid);
        }

        public async Task Should_GetByIdRealestate_WhenReturnsEqual()
        {
            Guid databaseGuid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");
            Guid guid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");
        }
        //Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        //ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
    }
}