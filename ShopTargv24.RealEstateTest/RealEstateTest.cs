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
            dto.Location = "Tallinn";
            dto.RoomNumber = 3;
            dto.BuildingType = "Apartment";
            dto.CreatedAt = DateTime.Now;
            dto.ModifiedAt = DateTime.Now;

            // Act
            var result = await Svc<IRealEstateServices>().Create(dto);

            // Assert
            Assert.NotNull(result);
        }

        // Test for getting real estate by ID when IDs do not match
        [Fact]
        public async Task ShouldNot_GetByIdRealestate_WhenReturnsNotEqual()
        {
            // Arrange
            Guid wrongGuid = Guid.Parse(Guid.NewGuid().ToString());
            Guid guid = Guid.Parse("c4ef2d3f-0f95-4082-873c-7f7ea3c54b5f");

            // Act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            // Assert
            Assert.NotEqual(wrongGuid, guid);
        }

        // Test for getting real estate by ID when IDs match
        [Fact]
        public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
        {
            // Arrange
            Guid correctGuid = Guid.Parse("c4ef2d3f-0f95-4082-873c-7f7ea3c54b5f");
            Guid guid = Guid.Parse("c4ef2d3f-0f95-4082-873c-7f7ea3c54b5f");

            // Act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            // Assert
            Assert.Equal(correctGuid, guid);
        }

        // Test for deleting real estate by ID
        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            // Arrange
            RealEstateDto dto = MockRealEstateData();

            // Act
            var createdRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var deletedRealEstate = await Svc<IRealEstateServices>().Delete((Guid)createdRealEstate.Id);

            // Assert
            Assert.Equal(createdRealEstate.Id, deletedRealEstate.Id);
        }

        // Test for deleting real estate by ID when IDs do not match
        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
        {
            // Arrange
            RealEstateDto dto = MockRealEstateData();

            // Act
            var createdRealEstate1 = await Svc<IRealEstateServices>().Create(dto);
            var createdRealEstate2 = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Delete((Guid)createdRealEstate2.Id);

            // Assert
            Assert.NotEqual(createdRealEstate1.Id, result.Id);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            // Arrange
            var guid = Guid.Parse("c4ef2d3f-0f95-4082-873c-7f7ea3c54b5f");
            RealEstateDto dto = MockRealEstateData();

            RealEstateDto domain = new();
            domain.Id = Guid.Parse("c4ef2d3f-0f95-4082-873c-7f7ea3c54b5f");
            domain.Area = 100.0;
            domain.Location = "Tartu";
            domain.RoomNumber = 4;
            domain.BuildingType = "House";
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            // Act
            await Svc<IRealEstateServices>().Update(dto);

            // Assert
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(domain.Area, dto.Area);
            Assert.DoesNotMatch(domain.Location, dto.Location);
            // kui kasutada DoesNotMatch, siis peab teha string'iks (kui on number)
            Assert.DoesNotMatch(domain.RoomNumber.ToString(), dto.RoomNumber.ToString());
            Assert.NotEqual(domain.RoomNumber, dto.RoomNumber);
            Assert.DoesNotMatch(domain.BuildingType, dto.BuildingType);
            Assert.NotEqual(domain.ModifiedAt, dto.ModifiedAt);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData2()
        {
            // Arrange
            RealEstateDto dto = MockRealEstateData();
            RealEstateDto update = MockUpdateRealEstateData();

            // Act
            await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(update);

            // Assert
            Assert.NotEqual(dto.Area, result.Area);
            Assert.DoesNotMatch(dto.Location, result.Location);
            Assert.NotEqual(dto.RoomNumber, result.RoomNumber);
            Assert.DoesNotMatch(dto.BuildingType, result.BuildingType);
            Assert.NotEqual(dto.ModifiedAt, result.ModifiedAt);
        }

        [Fact]
        public async Task ShouldNot_UpdateRealEstate_WhenDidNotUpdateData()
        {
            // Arrange
            RealEstateDto dto = MockRealEstateData();
            RealEstateDto update = MockNullRealEstateData();

            // Act
            var createdRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(update);

            // Assert
            Assert.NotEqual(dto.Id, result.Id);
        }

        [Fact]
        public async Task Should_ReturnNull_When_GetDeletedRealEstateById()
        {
            // Arrange
            RealEstateDto dto = MockRealEstateData();

            // Act
            var created = await Svc<IRealEstateServices>().Create(dto);
            await Svc<IRealEstateServices>().Delete((Guid)created.Id);
            var result = await Svc<IRealEstateServices>().DetailAsync((Guid)created.Id);

            // Assert
            Assert.Null(result);
        }

        // Helper method to mock real estate data
        private RealEstateDto MockRealEstateData()
        {
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Location = "Tallinn",
                RoomNumber = 3,
                BuildingType = "Apartment",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            return dto;
        }

        // Helper method to mock updated real estate data
        private RealEstateDto MockUpdateRealEstateData()
        {
            RealEstateDto dto = new()
            {
                Area = 100.0,
                Location = "Tartu",
                RoomNumber = 4,
                BuildingType = "House",
                CreatedAt = DateTime.Now.AddYears(1),
                ModifiedAt = DateTime.Now.AddYears(1)
            };

            return dto;
        }

        private RealEstateDto MockNullRealEstateData()
        {
            RealEstateDto dto = new()
            {
                Id = null,
                Area = null,
                Location = null,
                RoomNumber = null,
                BuildingType = null,
                CreatedAt = null,
                ModifiedAt = null
            };

            return dto;
        }
    }
}