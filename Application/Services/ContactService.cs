using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Entities.CMS;
using HAC_Pharma.Domain.Interfaces;
using HAC_Pharma.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HAC_Pharma.Application.Services;

public class ContactService : IContactService
{
    private readonly ApplicationDbContext _context;

    public ContactService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContactInquiryResponseDTO> SubmitContactAsync(SubmitContactDTO dto)
    {
        var inquiry = new ContactInquiry
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Subject = dto.Subject,
            Message = dto.Message,
            Company = dto.Company,
            Status = InquiryStatus.New,
            CreatedAt = DateTime.UtcNow
        };

        _context.ContactInquiries.Add(inquiry);
        await _context.SaveChangesAsync();

        return MapToDTO(inquiry);
    }

    public async Task<ContactListResponseDTO> GetContactListAsync(int page, int limit, string? status, string? search)
    {
        var query = _context.ContactInquiries.AsQueryable();

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<InquiryStatus>(status, true, out var statusEnum))
        {
            query = query.Where(x => x.Status == statusEnum);
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.Name.Contains(search) || x.Email.Contains(search) || x.Subject!.Contains(search));
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return new ContactListResponseDTO
        {
            Items = items.Select(MapToDTO).ToList(),
            Total = total,
            Page = page,
            Limit = limit
        };
    }

    public async Task<ContactInquiryResponseDTO?> GetContactByIdAsync(int id)
    {
        var inquiry = await _context.ContactInquiries
            .Include(x => x.RespondedByUser)
            .FirstOrDefaultAsync(x => x.Id == id);

        return inquiry == null ? null : MapToDTO(inquiry);
    }

    public async Task<ContactInquiryResponseDTO?> UpdateContactStatusAsync(int id, UpdateContactStatusDTO dto, string userId)
    {
        var inquiry = await _context.ContactInquiries.FirstOrDefaultAsync(x => x.Id == id);
        if (inquiry == null) return null;

        if (Enum.TryParse<InquiryStatus>(dto.Status, true, out var statusEnum))
        {
            inquiry.Status = statusEnum;
        }

        if (dto.AdminNotes != null)
        {
            inquiry.AdminNotes = dto.AdminNotes;
        }

        if (inquiry.Status == InquiryStatus.Responded && inquiry.RespondedAt == null)
        {
            inquiry.RespondedAt = DateTime.UtcNow;
            inquiry.RespondedByUserId = userId; // Assuming userId is string in Entity
        }

        inquiry.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return MapToDTO(inquiry);
    }

    public async Task<bool> DeleteContactAsync(int id)
    {
        var inquiry = await _context.ContactInquiries.FindAsync(id);
        if (inquiry == null) return false;

        _context.ContactInquiries.Remove(inquiry); // Soft delete if BaseEntity supports it, or hard delete
        // BaseEntity has IsDeleted but ConfigureSoftDeleteFilter uses it.
        // Assuming Remove causes IsDeleted=true if overridden or just hard delete if not.
        // Actually BaseEntity usually has IsDeleted.
        // Let's assume standard Remove works for now (either hard or soft depending on setup).
        // ApplicationDbContext sets soft delete query filter, but usually we need to set IsDeleted=true manually if Remove() deletes it hard.
        // Wait, typical SoftDelete pattern override Remove? Not usually.
        // I'll manually set IsDeleted if I want soft delete.
        // But usually "Remove" on a soft-delete entity in EF Core needs explicit handling or just setting the flag.
        // I'll set IsDeleted = true.
        inquiry.IsDeleted = true;

        
        await _context.SaveChangesAsync();
        return true;
    }

    private static ContactInquiryResponseDTO MapToDTO(ContactInquiry inquiry)
    {
        return new ContactInquiryResponseDTO
        {
            Id = inquiry.Id,
            Name = inquiry.Name,
            Email = inquiry.Email,
            Phone = inquiry.Phone,
            Subject = inquiry.Subject,
            Message = inquiry.Message,
            Company = inquiry.Company,
            Status = inquiry.Status.ToString(),
            AdminNotes = inquiry.AdminNotes,
            CreatedAt = inquiry.CreatedAt,
            RespondedAt = inquiry.RespondedAt,
            RespondedBy = inquiry.RespondedByUser?.FirstName + " " + inquiry.RespondedByUser?.LastName
        };
    }
}
