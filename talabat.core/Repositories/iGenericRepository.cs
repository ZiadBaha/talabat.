using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talabat.core.Entites;
using talabat.core.Specifications;

namespace talabat.core.Repositories
{
    public interface iGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> Getallasync();
        Task<T> GetbyidAsync(int id);

        Task<IReadOnlyList<T>> Getallwithspecasync(ISpecification<T> spec);

        Task<T> GetEntitywithspecasync(ISpecification<T> spec);
        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);

        Task add(T item);

        void Update(T item);

        void Delete(T item);



    }
}
