namespace UI
{
    using UnityEngine;

    public class ObjectToHashColorCalculator : IObjectToHashColorCalculator
    {
        public Color GetColor(object @object)
        {
            int hash = @object.GetHashCode();
            float red = (hash % 19) / 19f; // prime numbers
            float green = (hash % 5) / 5f;
            float blue = (hash % 11) / 11f;
			
            var color = new Color(red, green, blue);

            return color;
        }
    }
}