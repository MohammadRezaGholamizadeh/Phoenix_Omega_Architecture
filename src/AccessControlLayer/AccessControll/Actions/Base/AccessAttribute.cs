namespace AccessControlLayer.AccessControll.Actions.Base
{
    public abstract class AccessAttribute : Attribute
    {
        protected AccessAttribute(Type resourceType)
        {
            DependentActions = new HashSet<Type>();
            AssignableActions = new HashSet<Type>();
            ResourceType = resourceType;
        }

        public Type ResourceType { get; }
        public HashSet<Type> DependentActions { get; set; }
        public HashSet<Type> AssignableActions { get; set; }
    }
}
