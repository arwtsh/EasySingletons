using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace EasySingletons
{
    public static class SingletonManager
    {
        private static GameObject singletonGameObject;
        /// <summary>
        /// A persistant GameObject.
        /// Components can be attached to this GameObject to make them persist between levels.
        /// </summary>
        public static GameObject SingletonGameObject
        {
            get { return singletonGameObject; }
            set
            {
                Assert.IsNull(singletonGameObject, "Tried to set the singleton GameObject for the SingletonManager.");
                Assert.IsNotNull(value, "Cannot set the value of SingletonGameObject to null.");
                singletonGameObject = value;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void LoadSingletons()
        {
            List<System.Type> singletonTypes = FindSingletonTypes();
            List<(MethodInfo, System.Object)> methodRefs = CreateSingletons(singletonTypes);
            CreateSingletonGameObject();
            InitializeSingletons(methodRefs);
        }

        /// <summary>
        /// Find all types that inherit from Singleton<> in this assembly. 
        /// </summary>
        private static List<System.Type> FindSingletonTypes()
        {
            Debug.Log("Searching for singleton types...");

            List<System.Type> singletonTypes = (
                from assembly in System.AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(Singleton<>)
                select type
                ).ToList();

            Debug.Log("Found " + singletonTypes.Count + " singleton types.");

            return singletonTypes;
        }

        /// <summary>
        /// Create each Singleton instance
        /// </summary>
        private static List<(MethodInfo, System.Object)> CreateSingletons(List<System.Type> singletonTypes)
        {
            List<(MethodInfo, System.Object)> methodRefs = new List<(MethodInfo, object)>();

            Debug.Log("Started creating singleton instances.");

            foreach (System.Type type in singletonTypes)
            {
                System.Object singleton = System.Activator.CreateInstance(type);
                Assert.IsNotNull(singleton, "Did not successfuly create instance for singleton " + type.Name);

                PropertyInfo instanceRef = type.BaseType.GetProperty("Instance");
                Assert.IsNotNull(instanceRef, "Did not successfuly find Instance variable for singleton " + type.Name);

                try
                {
                    instanceRef.SetValue(null, singleton, BindingFlags.Static, null, null, null);
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                    Assert.IsTrue(false, "SingletonManager was unable to set Instance property for singleton " + type.Name);
                }

                MethodInfo methodRef = type.BaseType.GetMethod("OnSingletonInit", BindingFlags.NonPublic | BindingFlags.Instance);
                if (methodRef != null)
                {
                    methodRefs.Add((methodRef, singleton));
                }
            }

            Debug.Log("Finished creating singleton instances.");

            return methodRefs;
        }

        /// <summary>
        /// Create the singleton GameObject. Other scripts can access this singleton GameObject to adjust components made to it.
        /// </summary>
        private static void CreateSingletonGameObject()
        {
            SingletonGameObject = new GameObject("Singleton");
            Object.DontDestroyOnLoad(SingletonGameObject); //Make singleton persistant between levels.
            Debug.Log("Created GameObject singleton.");
        }

        private static void InitializeSingletons(List<(MethodInfo, System.Object)> methodRefs)
        {
            Debug.Log("Started initialization of singletons.");

            foreach ((MethodInfo, System.Object) methodRef in methodRefs)
            {
                //type.GetMethod("OnSingletonInit")

                try
                {
                    methodRef.Item1.Invoke(methodRef.Item2, null);
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                    Assert.IsTrue(false, "Unable to initialize singleton " + methodRef.Item2.GetType().Name);
                }
            }

            Debug.Log("Finished initialization of singletons.");
        }
    }
}