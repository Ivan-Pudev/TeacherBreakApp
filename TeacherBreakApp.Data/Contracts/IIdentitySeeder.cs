using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherBreakApp.Data.Contracts
{
    public interface IIdentitySeeder
    {
        Task SeedRolesAsync();

        Task SeedAdminUserAsync();
    }
}
