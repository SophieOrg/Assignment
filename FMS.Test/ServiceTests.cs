
using Xunit;
using FMS.Data.Models;
using FMS.Data.Services;

namespace FMS.Test
{

    public class ServiceTests
    {
        private readonly IFleetService svc;


        public ServiceTests()
        {
            // general arrangement
            svc = new FleetServiceDb();
          
            // ensure data source is empty before each test
            svc.Initialise();
        }

        // ========================== Fleet Tests =========================
   

        // write suitable tests to verify operation of the fleet service

    }
}