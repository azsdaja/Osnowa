namespace Osnowa.Osnowa.Unity
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class AssetLoader
    {
        /// <summary>
        /// Returns all assets of given type. Sometimes a corrupted asset may be omitted even if it looks fine in Unity inspector
        /// (for example when it has wrong field name, but this field value is null). 
        /// </summary>
        public static List<TAsset> LoadAll<TAsset>() where TAsset : UnityEngine.Object
        {
            return Resources.LoadAll<TAsset>("").ToList();
        }
    }
}