using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VillaAPIProject.Data;
using VillaAPIProject.Model;
using VillaAPIProject.Repository.IRepository;

namespace VillaAPIProject.Repository
{
    public class VillaNumberRepository :Repository<VillaNumber>, IVillaNumberRepository
    {

        //implement DBContext using dependency injection
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db): base(db) 
        {
            _db = db;
        }
                
        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
