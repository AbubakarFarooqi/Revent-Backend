using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Revent.DataAccess.Implementation.DbContexts;
using Revent.DataAccess.Implementation.IRepositories;
using Revent.DataAccess.Implementation.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.DataAccess.Implementation.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private IDbContextTransaction? _currentTransaction;
        private readonly ReventDbContext _applicationDbContext;
        private UserManager<IdentityUser> _userManager;

        private IUserRepository _userRepository;
        private ILookupRepository _lookupRepository;
        private IEventRepository _eventRepository;
        private IGroupChatRepository _groupChatRepository;
        private IGroupMessageRepository _groupMessageRepository;

        public UnitOfWork(
          ReventDbContext context,
          UserManager<IdentityUser> userManager
            )
        {
            _applicationDbContext = context;
            _userManager = userManager;
        }
        //public IUserRepository UserRepository => _userRepository ??= new UserRepository( _userManager);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_userManager,_applicationDbContext);

        public ILookupRepository LookupRepository => _lookupRepository ??= new LookupRepository(_applicationDbContext);
        public IEventRepository EventRepository => _eventRepository ??= new EventRepository(_applicationDbContext);
        public IGroupChatRepository GroupChatRepository => _groupChatRepository ??= new GroupChatRepository(_applicationDbContext);
        public IGroupMessageRepository GroupMessageRepository => _groupMessageRepository ??= new GroupMessageRepository(_applicationDbContext);

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }
            _currentTransaction = await _applicationDbContext.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _applicationDbContext.SaveChangesAsync();
                await _currentTransaction?.CommitAsync();
            }
            catch
            {
                throw;
            }
            
        }
        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _currentTransaction?.RollbackAsync();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }
        public async Task<int> CompleteAsync()
        {
            return await _applicationDbContext.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
