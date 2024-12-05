using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class FileRepository : Repository<FileEntity>, IFileRepository
{
    public FileRepository(SupabaseContext context) : base(context)
    {
    }
}