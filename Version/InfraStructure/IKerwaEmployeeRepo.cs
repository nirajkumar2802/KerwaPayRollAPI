using Version.EntityModels;

namespace Version.InfraStructure
{
    public interface IKerwaEmployeeRepo
    {

        public Task<List<KerwaEmployee>> GetAll();
        public Task<long> Save(KerwaEmployee kerwaEmployee);
    }
}
