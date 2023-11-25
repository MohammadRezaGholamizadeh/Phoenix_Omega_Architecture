
using AccessControlLayer.AccessControll.Domains;
using DomainLayer.Entities.Color;
using DomainLayer.Entities.Organizations;
using IdentityLayer.AspDotNetIdentity.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using ServiceLayer.Setups.TokenManagerInterface;
using System.Linq.Expressions;

namespace DataAccessLayer.EFTech.EFDataContexts
{
    public partial class EFDataContext : IdentityDbContext<ApplicationUser,
                                         ApplicationRole,
                                         string,
                                         ApplicationUserClaim,
                                         ApplicationUserRole,
                                         ApplicationUserLogin,
                                         ApplicationRoleClaim,
                                         ApplicationUserToken>
    {
        private readonly string _tenantId;
        private const string TenantName = "TenantId";

        public DbSet<Color> Colors { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserResource> UserResources { get; set; }

        public EFDataContext(
            DbContextOptions<EFDataContext> options,
            UserTokenService userTokenService)
            : base(options)
        {
            _tenantId = userTokenService.TenantId;
        }

        public EFDataContext(
               string connectionString,
               UserTokenService userTokenService)
            : this(new DbContextOptionsBuilder<EFDataContext>()
                      .UseSqlServer(connectionString).Options, userTokenService)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            Expression<Func<ITenant, bool>> tenantFilteringExpression
                = expression => expression.TenantId == _tenantId;

            var entities =
                typeof(ITenant).Assembly
                               .GetTypes()
                               .ToHashSet()
                               .Union(typeof(ApplicationUser).Assembly.GetTypes())
                               .Where(_ => _.IsAssignableTo(typeof(ITenant))
                                        && !_.IsInterface);

            foreach (var entity in entities.AsParallel())
            {
                var parameter = Expression.Variable(entity);
                var body = ReplacingExpressionVisitor
                           .Replace(tenantFilteringExpression.Parameters.First(),
                                    parameter,
                                    tenantFilteringExpression.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);
                modelBuilder.Entity(entity).Metadata
                            .SetQueryFilter(lambdaExpression);
            }
        }

        public override int SaveChanges()
        {
            foreach (var entity in ChangeTracker.Entries()
                                        .Where(entity => entity.State
                                                         == EntityState.Added))
            {
                var tenantproperty = entity.Metadata.FindProperty(TenantName);

                if (tenantproperty != null &&
                    entity.Property(TenantName).CurrentValue == null)
                    entity.Property(TenantName).CurrentValue = _tenantId;
            }

            var result = base.SaveChanges();
            return result;
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries()
                                                .Where(entity => entity.State
                                                                 == EntityState.Added))
            {
                var property = entity.Metadata.FindProperty(TenantName);

                if (property != null &&
                    entity.Property(TenantName).CurrentValue == null)
                    entity.Property(TenantName).CurrentValue = _tenantId;
            }

            var result = base.SaveChangesAsync(cancellationToken);

            return result;
        }

        public override ChangeTracker ChangeTracker
        {
            get
            {
                var tracker = base.ChangeTracker;
                tracker.LazyLoadingEnabled = false;
                tracker.AutoDetectChangesEnabled = true;
                tracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                return tracker;
            }
        }
    }
}
