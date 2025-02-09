using UnityEngine;
using UnityEngine.Assertions;

// Classes inheriting from Singleton should be sealed.
public abstract class Singleton<T> where T : Singleton<T>
{
    private static T instance;

    /// <summary>
    /// The singleton instance of this class.
    /// </summary>
    public static T Instance
    {
        get { return instance; }
        internal set
        {
            // Setting the singleton after it has already been set should assert since the singleton pattern has been violated somewhere.
            Assert.IsNull(instance, "Tried to set the singleton instance of " + typeof(T).Name + " after it was already initialized.");
            Assert.IsNotNull(value, "Cannot set the value of " + typeof(T).Name + "'s singleton instance to null.");
            instance = value;
            Debug.Log("Created instance for singleton " + typeof(T).Name);
        }
    }

    /// <summary>
    /// OnSingletonInit is called once before awake and start.
    /// </summary>
    protected virtual void OnSingletonInit() { }
}