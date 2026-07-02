using Microsoft.EntityFrameworkCore;

namespace Maxsys.Data.Extensions;

public static class ConventionsExtensions
{
    /// <summary>
    /// Use:
    /// <code>
    /// protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    /// {
    ///     configurationBuilder.StringToVarcharConvention(100);
    ///
    ///     base.ConfigureConventions(configurationBuilder);
    /// }
    /// </code>
    /// </summary>
    public static ModelConfigurationBuilder StringToVarcharConvention(this ModelConfigurationBuilder configurationBuilder, int maxLength = -1)
    {
        configurationBuilder.Properties<string>(x => x.AreUnicode(false).HaveMaxLength(maxLength));

        return configurationBuilder;
    }
}