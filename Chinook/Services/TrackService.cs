using Chinook.Data;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services;

public class TrackService
{
    private readonly IDbContextFactory<ChinookContext> _dbContextFactory;

    public TrackService(IDbContextFactory<ChinookContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }


}
