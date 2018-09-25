
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
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
    public class ClientSignInManager : SignInManager<Client>
    {
        public ClientSignInManager(ClientManager userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<Client> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<ClientSignInManager> logger, IAuthenticationSchemeProvider schemes) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }
    }
}
