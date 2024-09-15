using AutoMapper;
using Shop.WebAPI.Infrastructure.Mappings;

namespace Shop.Tests;

public class MappingsTests
{
    [Fact]
    public void Mapping_Configuration_Is_Valid()
    {
        // Создание конфигурации AutoMapper
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>(); // Убедитесь, что вы добавляете все ваши профили
        });

        // Проверка конфигурации на наличие ошибок
        configuration.AssertConfigurationIsValid();
    }

}