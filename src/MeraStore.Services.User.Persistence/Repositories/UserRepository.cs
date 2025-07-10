using MeraStore.Services.User.Application.Repositories;
using MeraStore.Shared.Kernel.Persistence.Interfaces;
using MeraStore.Shared.Kernel.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

namespace MeraStore.Services.User.Persistence.Repositories;

public class UserRepository(AppDbContext context, ICommitStrategy commit)
  : Repository<Domain.Entities.User>(context, commit), IUserRepository;