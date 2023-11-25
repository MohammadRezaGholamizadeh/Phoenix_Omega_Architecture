using AccessControlLayer.AccessControll.Domains;
using AccessControlLayer.Infrastructure.SeedDataInfra.Contracts;

namespace AccessControlLayer.Infrastructure.SeedDataInfra
{
    public class RoleActionSeedDataHandler
    {
        private readonly AccessControlUnitOfWork _unitOfWork;
        private readonly RoleActionSeedDataRepository
            _roleAccessMakerRepository;

        public RoleActionSeedDataHandler(
            AccessControlUnitOfWork unitOfWork,
            RoleActionSeedDataRepository roleAccessMakerRepository)
        {
            _unitOfWork = unitOfWork;
            _roleAccessMakerRepository = roleAccessMakerRepository;
        }

        public async Task<string> RunUp(long? targetVersion = null)
        {
            var logResult = string.Empty;
            var lastVersionInfo = _roleAccessMakerRepository
                                  .GetLastSeedDataVersion();
            var seedDataTypes =
                typeof(RoleActionSeedDataGeneration).Assembly
                .GetTypes()
                .Where(_ => _.BaseType == typeof(RoleActionSeedDataGeneration)
                         && (targetVersion != null ?
                             OnTargetVersionAndLastVersionCondition(
                                 _, targetVersion, lastVersionInfo)
                             : OnLastVersionCondition(_, lastVersionInfo)));
            try
            {
                foreach (var type in seedDataTypes)
                {
                    _unitOfWork.Begin();
                    var seedData = Activator.CreateInstance(
                                              type,
                                              _roleAccessMakerRepository,
                                              _unitOfWork);
                    type.GetMethod("Up")!.Invoke(seedData, null);

                    logResult +=
                        "*********************************" +
                        "*******************************"
                        + Environment.NewLine + Environment.NewLine +
                        $"{type.Name} RoleAction SeedData" +
                        $" Inserted Successfully !!!"
                        + Environment.NewLine + Environment.NewLine;

                    var version = new RoleActionSeedDataVersionInfo()
                    {
                        Version = GetSeedDataVersion(type)
                    };
                    _roleAccessMakerRepository.SetVersionInfo(version);
                    _unitOfWork.CommitPartial();
                    await _unitOfWork.Commit();
                }
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }

            return logResult;
        }

        private static bool OnTargetVersionAndLastVersionCondition(
            Type _,
            long? targetVersion,
            long? lastVersionInfo)
        {
            return _.CustomAttributes
                    .Any(_ => _.ConstructorArguments
                               .Any(_ =>
                                 (long)_.Value! <= targetVersion))
                                && lastVersionInfo != null ?
                                   _.CustomAttributes
                                    .Any(_ => _.ConstructorArguments
                                         .Any(_ =>
                                              (long)_.Value! <= targetVersion
                                           && (long)_.Value > lastVersionInfo))
                                             : true;
        }

        public async Task<string> RunDown(long? version = 0)
        {
            var logResult = string.Empty;
            var types =
                 typeof(RoleActionSeedDataGeneration).Assembly
                 .GetTypes()
                 .Where(_ => _.BaseType
                             == typeof(RoleActionSeedDataGeneration)
                          && (version != 0 ?
                              _.CustomAttributes
                              .Any(_ => _.ConstructorArguments
                                         .Any(_ =>
                                           (long)_.Value! > version))
                             : true));
            try
            {
                foreach (var type in types)
                {
                    _unitOfWork.Begin();
                    var seedData = Activator.CreateInstance(
                                             type,
                                             _roleAccessMakerRepository,
                                             _unitOfWork);
                    type.GetMethod("Down")!.Invoke(seedData, null);

                    logResult +=
                        "*********************************" +
                        "*******************************"
                        + Environment.NewLine + Environment.NewLine +
                        $"{type.Name} RoleAction SeedData" +
                        $" Removed Successfully !!!" + Environment.NewLine
                        + Environment.NewLine;

                    var seedDataVersion = GetSeedDataVersion(type);
                    var versionInfo = _roleAccessMakerRepository
                                      .FindSeedDataVersion(seedDataVersion);
                    GuardAgainstInvalidSeedDataVersion(type.Name, versionInfo);
                    _roleAccessMakerRepository.DeleteVersionInfo(versionInfo!);
                    _unitOfWork.CommitPartial();
                    await _unitOfWork.Commit();
                }
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }

            return logResult;
        }

        private static bool OnLastVersionCondition(
            Type _,
            long? lastVersionInfo)
        {
            return lastVersionInfo != null ?
                   _.CustomAttributes
                    .Any(_ => _.ConstructorArguments
                           .Any(_ =>
                             (long)_.Value! > lastVersionInfo))
                   : true;
        }

        private static long GetSeedDataVersion(Type type)
        {
            return (long)type.CustomAttributes
                   .First(_ => _.AttributeType
                          == typeof(SeedDataVersionAttribute))
                   .ConstructorArguments.First().Value!;
        }

        private static void GuardAgainstInvalidSeedDataVersion(
            string typeName,
            RoleActionSeedDataVersionInfo? versionInfo)
        {
            if (versionInfo == null)
                throw new Exception($"Invalid Version :" +
                    $" {typeName} SeedData Version Not Found !!! ");
        }
    }
}
