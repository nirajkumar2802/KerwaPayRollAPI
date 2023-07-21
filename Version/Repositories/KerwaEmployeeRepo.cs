using Microsoft.EntityFrameworkCore;
using Version.DataContext;
using Version.EntityModels;
using Version.InfraStructure;

namespace Version.Repositories
{
    public class KerwaEmployeeRepo : IKerwaEmployeeRepo
    {
        private readonly PayRollKerwaDbContext _dbContext;
        public KerwaEmployeeRepo(PayRollKerwaDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<KerwaEmployee>> GetAll()
        {
            var res = await _dbContext.KerwaEmployee.ToListAsync();
            return res;
        }

        public async Task<long> Save(KerwaEmployee kerwaEmployee)
        {
            var res = await _dbContext.KerwaEmployee.AddAsync(kerwaEmployee);
            await _dbContext.SaveChangesAsync();
            return kerwaEmployee.Id;
        }


    }
}
