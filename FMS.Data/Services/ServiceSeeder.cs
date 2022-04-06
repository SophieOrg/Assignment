using System;
using System.Text;
using System.Collections.Generic;

namespace FMS.Data.Services
{
    public static class FleetServiceSeeder
    {
        // use this class to seed the database with dummy test data using an IFleetService
        public static void Seed(IFleetService svc)
        {
            svc.Initialise();

            // add seed data
            

        }
    }
}
