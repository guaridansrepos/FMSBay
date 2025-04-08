using FMSBay.BAL.IService;
using FMSBay.DBContext;
using FMSBay.Models.DTOs;
using FMSBay.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace FMSBay.BAL.Service
{
    public class ContactFrmService : IContactFrmService
    {
        private readonly MyDbContext _context;

        public ContactFrmService(MyDbContext context)
        {
            _context = context;
        }

        // -------------------- CONTACT FORM --------------------

        public async Task<IEnumerable<ContactFormDTO>> GetAllAsync()
        {
            return await _context.ContactFormEntity
                .Where(x => !x.IsDeleted)
                .Include(x => x.LoanType)
                .Select(x => new ContactFormDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone,
                    LoanTypeId = x.LoanTypeId,
                    LoanTypeName = x.LoanType != null ? x.LoanType.LoanType : null,
                    Message = x.Message,
                 //   CreatedOn = x.CreatedOn
                })
                .ToListAsync();
        }

        public async Task<ContactFormDTO> GetByIdAsync(int id)
        {
            var entity = await _context.ContactFormEntity
                .Include(x => x.LoanType)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (entity == null) return null;

            return new ContactFormDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Phone = entity.Phone,
                LoanTypeId = entity.LoanTypeId,
                LoanTypeName = entity.LoanType?.LoanType,
                Message = entity.Message,
               // CreatedOn = entity.CreatedOn
            };
        }

        public async Task<ContactFormEntity> CreateAsync(ContactFormDTO contact)
        {
            var entity = new ContactFormEntity
            {
                Name = contact.Name,
                Phone = contact.Phone,
                LoanTypeId = contact.LoanTypeId,
                Message = contact.Message,
                CreatedOn = DateTime.UtcNow,
               // CreatedBy = contact.CreatedBy,
                IsDeleted = false
            };

            _context.ContactFormEntity.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> UpdateAsync(int id, ContactFormDTO contact)
        {
            var entity = await _context.ContactFormEntity.FindAsync(id);
            if (entity == null) return false;

            entity.Name = contact.Name;
            entity.Phone = contact.Phone;
            entity.LoanTypeId = contact.LoanTypeId;
            entity.Message = contact.Message;
            entity.UpdatedOn = DateTime.UtcNow;
          //  entity.UpdatedBy = contact.UpdatedBy;

            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ContactFormEntity.FindAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.UpdatedOn = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        // -------------------- LOAN TYPE --------------------

        public async Task<IEnumerable<LoanTypeDTO>> GetAllLAsync()
        {
            return await _context.LoanTypeEntity
                .Where(x => !x.IsDeleted)
                .Select(x => new LoanTypeDTO
                {
                    Id = x.Id,
                    LoanType = x.LoanType,
                  //  CreatedOn = x.CreatedOn
                })
                .ToListAsync();
        }

        public async Task<LoanTypeEntity> CreateAsync(LoanTypeDTO dto)
        {
            var entity = new LoanTypeEntity
            {
                LoanType = dto.LoanType,
                CreatedOn = DateTime.UtcNow,
               // CreatedBy = dto.CreatedBy,
                IsDeleted = false
            };

            _context.LoanTypeEntity.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> UpdateAsync(int id, LoanTypeDTO dto)
        {
            var entity = await _context.LoanTypeEntity.FindAsync(id);
            if (entity == null) return false;

            entity.LoanType = dto.LoanType;
           // entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedOn = DateTime.UtcNow;

            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<LoanTypeDTO?> GetByIdLAsync(int id)
        {
            var entity = await _context.LoanTypeEntity.FindAsync(id);
            if (entity == null || entity.IsDeleted) return null;

            return new LoanTypeDTO
            {
                Id = entity.Id,
                LoanType = entity.LoanType,
               // CreatedOn = entity.CreatedOn
            };
        }

        public async Task<bool> DeleteLAsync(int id)
        {
            var entity = await _context.LoanTypeEntity.FindAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.UpdatedOn = DateTime.UtcNow;

            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<LoanTypeDTO>> GetDropdownListAsync()
        {
            return await _context.LoanTypeEntity
                .Where(x => !x.IsDeleted)
                .Select(x => new LoanTypeDTO
                {
                    Id = x.Id,
                    LoanType = x.LoanType
                })
                .ToListAsync();
        }
    }
}
