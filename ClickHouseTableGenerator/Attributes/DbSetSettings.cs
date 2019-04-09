using System;
using ExpressionTreeVisitor;

namespace ClickHouseTableGenerator
{
    public class DbSetSettings : Attribute, IHasKey<ICreatingDbSettingsProvider>
    {
        private ICreatingDbSettingsProvider _settings { get; set; }

        public DbSetSettings(Type creatingDbInfoProviderType)
        {
            if (!typeof(ICreatingDbSettingsProvider).IsAssignableFrom(creatingDbInfoProviderType))
                throw new ArgumentException($"Укажите экземпляр реализующий тип {nameof(ICreatingDbSettingsProvider)}");
            _settings = (ICreatingDbSettingsProvider) Activator.CreateInstance(creatingDbInfoProviderType);
        }

        public object ObjectKey
        {
            get => _settings;
            set => _settings = (ICreatingDbSettingsProvider) value;
        }

        public ICreatingDbSettingsProvider Key
        {
            get => _settings;
            set => _settings = value;
        }
    }
}