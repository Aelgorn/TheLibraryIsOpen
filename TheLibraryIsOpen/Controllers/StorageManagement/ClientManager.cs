using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLibraryIsOpen.Models.DBModels;

namespace TheLibraryIsOpen.Controllers.StorageManagement
{

    public class ClientManager : UserManager<Client>
    {
        public ClientManager(ClientStore store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<Client> passwordHasher, IEnumerable<IUserValidator<Client>> userValidators, IEnumerable<IPasswordValidator<Client>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<Client>> logger) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override Task<Client> FindByEmailAsync(string email)
        {
            return FindByNameAsync(email);
        }

        public async Task<Client> FindAsync(string userName, string password)
        {
            Client c = await FindByNameAsync(userName);
            if (c.PasswordVerify(password))
                return c;
            else
                return null;
        }

        public override async Task<IdentityResult> CreateAsync(Client user)
        {
            return await Store.CreateAsync(user, new System.Threading.CancellationToken(false));
        }
    }
}
