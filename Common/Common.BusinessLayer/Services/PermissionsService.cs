using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IPermissionNounsRepository _permissionNounsRepository;

        private readonly IPermissionVerbsRepository _permissionVerbsRepository;

        private readonly IMapper _mapper;

        public PermissionsService(IPermissionNounsRepository permissionNounsRepository,
            IPermissionVerbsRepository permissionVerbsRepository,
            IMapper mapper)
        {
            _permissionNounsRepository = permissionNounsRepository;
            _permissionVerbsRepository = permissionVerbsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> GetAllPermissions()
        {
            var permissions = await _permissionNounsRepository.GetAllAsync();
            return permissions.Select(p => p.Name);
        }

        public async Task<IEnumerable<string>> GetAllPermissionVerbs()
        {
            var permissionVerbs = await _permissionVerbsRepository.GetAllAsync();
            return permissionVerbs.Select(p => p.Name);
        }
    }
}
