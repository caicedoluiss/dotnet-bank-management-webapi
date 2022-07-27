using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class CreateTransactionRequestCommandHandlerTests
{
  private readonly CreateTransactionRequestCommandHandler handler;

  private NewTransactionDTO newTransactionDTO;
  private CreateTransactionRequestCommand request;

  public CreateTransactionRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<CreateTransactionRequestCommandHandler>();

    newTransactionDTO = new()
    {
      AccountId = 1,
      Value = 1000M
    };

    request = new()
    {
      TransactionInfo = newTransactionDTO
    };
  }

  [Fact]
  public void Handle_InvalidNullTransactionInfo_ThrowsArgumentException()
  {
    request.TransactionInfo = null;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    newTransactionDTO.AccountId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_ValidAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    newTransactionDTO.AccountId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1000)]
  [InlineData(0)]
  [InlineData(1000)]
  [InlineData(55.09)]
  [InlineData(-895.14)]
  public async Task Handle_ValidValue_ReturnsValidId(decimal value)
  {
    newTransactionDTO.Value = value;
    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}


