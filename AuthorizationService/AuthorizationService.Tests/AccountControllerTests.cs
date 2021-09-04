using AuthorizationService.Controllers;
using AuthorizationService.Dto;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizationService.Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private List<AccountDto> GetAccountsList()
        {
            var list = new List<AccountDto>
            {
                new AccountDto(new Account() { NickName = "acc1",IsDeleted = false}),
                new AccountDto(new Account() {NickName = "acc2", IsDeleted = false})
             };

            return list;
        }

        private List<AccountDto> GetDeletedAccountsList()
        {
            var list = new List<AccountDto>
            {
                new AccountDto(new Account() { NickName = "acc1",IsDeleted = true}),
                new AccountDto(new Account() {NickName = "acc2", IsDeleted = true})
             };

            return list;
        }

        [Test]
        public async Task GetAllAccounts_AccountsReceived()
        {
            //Arrange
            var mockAccounts = new Mock<IAccounts>();
            var mockAuthorization = new Mock<AuthorizationDbContext>();
            var accountController = new AccountController(mockAccounts.Object, mockAuthorization.Object);
            var expected = GetAccountsList();
            mockAccounts.Setup(c=>c.GetAllAccounts()).ReturnsAsync(GetAccountsList());

            //Act
            var actual = await accountController.GetAllAccounts();
            
            //Assert
            mockAccounts.Verify(c => c.GetAllAccounts(), Times.Once);

            Assert.AreEqual(expected.Count, actual.Value.Count);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].NickName, actual.Value[i].NickName);
                    Assert.AreEqual(expected[i].Role, actual.Value[i].Role);
                }
            });
        }

        [Test]
        public async Task GetAllDeletedAccounts_DeletedAccountsReceived()
        {
            //Arrange
            var mockAccounts = new Mock<IAccounts>();
            var mockAuthorization = new Mock<AuthorizationDbContext>();
            var accountController = new AccountController(mockAccounts.Object, mockAuthorization.Object);
            var expected = GetDeletedAccountsList();
            mockAccounts.Setup(c => c.GetAllDeletedAccounts()).ReturnsAsync(GetDeletedAccountsList());

            //Act
            mockAccounts.Verify(c => c.GetAllDeletedAccounts(), Times.Once);

            var actual = await accountController.GetAllDeletedAccounts();

            Assert.AreEqual(expected.Count, actual.Value.Count);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].NickName, actual.Value[i].NickName);
                    Assert.AreEqual(expected[i].Role, actual.Value[i].Role);
                }
            });
        }
    }
}

