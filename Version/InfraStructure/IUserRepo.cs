using Version.EntityModels;

namespace Version.InfraStructure
{
    public interface IUserRepo
    {

        public Task <KerwaUsers> Get();
    }
}
