using FMSBay.Models.DTOs;
using FMSBay.Models.Entitys;
namespace FMSBay.BAL.IService
{
    public interface IContactFrmService
    {
        // CONTACT FORM CRUD
        Task<IEnumerable<ContactFormDTO>> GetAllAsync();
        Task<ContactFormDTO> GetByIdAsync(int id);
        Task<ContactFormEntity> CreateAsync(ContactFormDTO contact);
        Task<bool> UpdateAsync(int id, ContactFormDTO contact);
        Task<bool> DeleteAsync(int id);

        // LOAN TYPE CRUD
        Task<IEnumerable<LoanTypeDTO>> GetAllLAsync();
        Task<LoanTypeEntity> CreateAsync(LoanTypeDTO loanType);
        Task<bool> UpdateAsync(int id, LoanTypeDTO loanType);
        Task<LoanTypeDTO?> GetByIdLAsync(int id);
        Task<bool> DeleteLAsync(int id);

        // Dropdown (ID + Name)
        Task<IEnumerable<LoanTypeDTO>> GetDropdownListAsync();
    }
}
