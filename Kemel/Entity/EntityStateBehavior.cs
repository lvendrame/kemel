using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Entity
{
    public class EntityStateBehavior: ProviderContainer<IEntityStateProvider, EntityState>
    {
        public EntityStateBehavior(IEntityStateProvider entityStateProvider)
            : base(entityStateProvider)
        {
        }

        public override void DoBehavior(EntityState enumType)
        {
            switch (enumType)
            {
                case EntityState.Added:
                    this.Provider.AddedBehavior();
                    break;
                case EntityState.Deleted:
                    this.Provider.DeletedBehavior();
                    break;
                case EntityState.Unchanged:
                    this.Provider.UnchangedBehavior();
                    break;
                case EntityState.Modified:
                    this.Provider.ModifiedBehavior();
                    break;
                default:
                    throw new ArgumentException("Invalid EntityState", "enumType");
            }
        }
    }
}
