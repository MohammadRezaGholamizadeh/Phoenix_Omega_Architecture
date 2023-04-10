using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts;
using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts.Dtos;
using FluentAssertions;
using Phoenix.TestTools.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestsLayer.ColorsAppServiceTests
{
    public class ColorAppServiceTest : UnitTestSut<IColorAppService>
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
