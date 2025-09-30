using Microsoft.EntityFrameworkCore;
using ShopTARgv24.Core.Domain;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;

namespace ShopTARgv24.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly ShopTARgv24Context _context;

        public KindergartenServices
            (
                ShopTARgv24Context context
            )
        {
            _context = context;
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten kindergarten = new Kindergarten();

            kindergarten.Id = Guid.NewGuid();
            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.CreatedAt = DateTime.Now;
            kindergarten.UpdatedAt = DateTime.Now;

            await _context.Kindergartens.AddAsync(kindergarten);
            await _context.SaveChangesAsync();

            return kindergarten;
        }

        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            var result = await _context.Kindergartens
            .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            Kindergarten domain = new();

            domain.Id = dto.Id;
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.CreatedAt = dto.CreatedAt;
            domain.UpdatedAt = DateTime.Now;

            _context.Kindergartens.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Kindergartens.Remove(kindergarten);
            await _context.SaveChangesAsync();

            return kindergarten;
        }
    }
}