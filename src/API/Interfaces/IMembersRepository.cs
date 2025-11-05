using API.Entities;

namespace API.Interfaces;

public interface IMembersRepository
{
    void Update(Member member);
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Member>> GetMembersAsync();
    Task<Member?> GetMemberAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotosAsync(string memberId);
}
