using ASPNETCoreIdentitySample.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPNETCoreIdentitySample.Services.Services
{
    public interface ICategoryServices : IService<Category> { }


    public class CategoryServices : Service<Category>, ICategoryServices
    {
        public CategoryServices(IRepository<Category> repository) : base(repository) { }
    }
}
