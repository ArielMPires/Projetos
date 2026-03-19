using Microsoft.Extensions.Options;
namespace Domus.Services
{
    public sealed class TenantProvider
    {
        private const string TenanIdHeaderName = "X-tenantId";

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly TenantConnectionStrings _ConnectionStrings;

        public TenantProvider(IHttpContextAccessor httpContextAccessor, IOptions<TenantConnectionStrings> connectionStrings)
        {
            _httpContextAccessor = httpContextAccessor;
            _ConnectionStrings = connectionStrings.Value;
        }

        public int GetTenantId()
        {
            var tenantIdHeader = _httpContextAccessor.HttpContext?
                .Request
                .Headers[TenanIdHeaderName];

            if (!tenantIdHeader.HasValue || !int.TryParse(tenantIdHeader.Value, out int tenantId))
            {
                throw new ApplicationException("Tenant ID Não está Presente");
            }

            return tenantId;
        }

        public string GetConnectionStrings()
        {
            return _ConnectionStrings.Values[GetTenantId()];
        }

        public Dictionary<int,string> GetAllConnectionStrings()
        {
            return _ConnectionStrings.Values;
        }
    }
}
