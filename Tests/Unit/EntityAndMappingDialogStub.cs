using System;
using VisualStudio;
using VisualStudio.UI.EntityAndMapping;
using VisualStudio.Workflows.EntityAndMapping;

namespace Tests.Unit
{
    public class EntityAndMappingDialogStub : IEntityAndMappingDialog
    {
        private enum Operation { Create, PreviewOperation }

        private readonly Operation _operation;

        private EntityAndMappingDialogStub(Operation operation)
        {
            _operation = operation;
        }

        public static EntityAndMappingDialogStub Create()
        {
            return new EntityAndMappingDialogStub(Operation.Create);
        }

        public static EntityAndMappingDialogStub PreviewOperation()
        {
            return new EntityAndMappingDialogStub(Operation.PreviewOperation);
        }

        public Maybe<EntityDefinition>  Display(string defaultEntityName, 
                    Func<EntityDefinition,Maybe<Exception>> createEntity, 
                    Func<EntityDefinition,Maybe<Exception>> createMapping, 
                    Func<EntityDefinition,Tuple<Maybe<Exception>,string>> previewEntityOperation, 
                    Func<EntityDefinition,Tuple<Maybe<Exception>,string>> previewMappingOperation)
        {
            var entityDefinition = new EntityDefinition { Database = string.Empty, Server = string.Empty, Table = string.Empty, 
                                                          EntityName = string.Empty, Namespace = string.Empty};
            switch (_operation)
            {
                case Operation.Create: return createEntity(entityDefinition).Bind(x => createMapping(entityDefinition)).HasValue ?
                                                    Maybe.Just(entityDefinition) : Maybe.Nothing<EntityDefinition>();
                case Operation.PreviewOperation: return previewEntityOperation(entityDefinition).ToMaybe().Bind(x => x.Item1).
                                                        Bind(x => previewMappingOperation(entityDefinition).ToMaybe()).Bind(x => x.Item1).HasValue ?
                                                    Maybe.Just(entityDefinition) : Maybe.Nothing<EntityDefinition>();
            }
            return null;
        }
    }
}
