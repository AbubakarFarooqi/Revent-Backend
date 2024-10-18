using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Revent.DataAccess.Implementation.IRepositories;
using Revent.DataAccess.Implementation.UnitOfWork;
using Revent.EFCore.DataModel.Models;
using Revent.Services.IServices;

namespace Revent.Services.Services
{
    public class LookupService : ILookupService
    {
        private readonly IDistributedCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public LookupService(IDistributedCache cache, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
            _config = config;
        }
        public async Task<Lookups?> GetLookupById(int Id)
        {
            string cacheKey = $"lookup_{Id}";
            var cachedLookup = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedLookup))
            {
                return JsonConvert.DeserializeObject<Lookups>(cachedLookup);
            }

            // Otherwise, get the lookup from the database
            var lookup = await _unitOfWork.LookupRepository.GetAsync(Id);
            if (lookup == null)
                return null;

            // Store the result in Redis with an expiration time
            var cacheOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(int.Parse(_config["RedisCacheSettings:ExpirationTimeInHour"].ToString())));  // Cache expires after 1 hour

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(lookup), cacheOptions);

            return lookup;
        }
    }
}
