using Microsoft.AspNetCore.Identity;

namespace OurBeautyReferralNetwork.Repositories
{
    public class UserRepo
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepo(UserManager<IdentityUser> userManager) 
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser> AddNewUser(string id, string email, string password, string role)
        {
            var user = new IdentityUser
            {
                UserName = id,
                Email = email,
            };

            var addUserResult = await _userManager.CreateAsync(user, password);

            if (addUserResult.Succeeded)
            {
                Console.WriteLine("New user added");
                await _userManager.AddToRoleAsync(user, role);
            }

            return user;
        }
    }
}
