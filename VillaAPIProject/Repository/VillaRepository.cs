using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VillaAPIProject.Data;
using VillaAPIProject.Model;
using VillaAPIProject.Repository.IRepository;

namespace VillaAPIProject.Repository
{
    public class VillaRepository :Repository<Villa>, IVillaRepository
    {

        //implement DBContext using dependency injection
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db): base(db) 
        {
            _db = db;
        }
                
        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
