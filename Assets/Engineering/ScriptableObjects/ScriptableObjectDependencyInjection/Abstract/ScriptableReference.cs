namespace ScriptableObjectDependencyInjection
{
    /// <summary>
    /// Extensible base class for ScriptableReferences. Children should also have[Serializable] attribute.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ScriptableReference<T1, T2> where T1 : ScriptableVariable<T2>
    {
        public bool UseConstant = true;
        public T2 ConstantValue;
        public T1 Variable;

        public T2 Value {
            get {
                return UseConstant ? ConstantValue : Variable.Value;
            }
        }
    }
}