using FluentAssertions;
using Phoenix.TestTools.Infrastructure;
using ServiceLayer.Services.ColorService.Contracts;
using ServiceLayer.Services.ColorService.Contracts.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestsLayer.ColorsAppServiceTests
{
    public class ColorAppServiceTest : UnitTestSut<IColorService>
    {
        [Fact]
        public async Task Add_Color_Properly()
        {
            var addColorDto = new AddColorDto()
            {
                Title = "dummy_Title",
                ColorHex = "dummy_HexColor"
            };

            await Sut.Add(addColorDto);

            var expected = Context.Colors.ToList();
            expected.Should().HaveCount(1);
        }
    }
}
