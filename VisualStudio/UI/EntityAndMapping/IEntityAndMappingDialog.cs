using System;
using VisualStudio.Workflows.EntityAndMapping;

namespace VisualStudio.UI.EntityAndMapping
{
    public interface IEntityAndMappingDialog
    {
        Maybe<EntityDefinition> Display(string defaultEntityName,
                                        Func<EntityDefinition, Maybe<Exception>> createEntity,
                                        Func<EntityDefinition, Maybe<Exception>> createMapping,
                                        Func<EntityDefinition, Tuple<Maybe<Exception>, string>> previewEntityOperation,
                                        Func<EntityDefinition, Tuple<Maybe<Exception>, string>> previewMappingOperation);
    }
}
