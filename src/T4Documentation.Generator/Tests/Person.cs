using System;
using System.Collections.Generic;


namespace T4Documentation.Generator.Tests
{
    /// <summary>
    /// A programming structure that represents a person.
    /// </summary>
    /// <code>
    /// var p = new Person();
    /// </code>
    public class Person
    {
        /// <summary>
        /// This is the name of a person.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This is the height of a person.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Makes a person think
        /// </summary>
        /// <code>
        /// var p = new Person();
        /// p.Think();
        /// </code>
        public void Think()
        {
            //Do something
        }

        /// <summary>
        /// Determine if a person is willing to dance
        /// </summary>
        /// <code>
        /// var p = new Person();
        /// var willingnessToDance = p.GetWillingnessToDance();
        /// </code>
        /// <returns></returns>
        public bool GetWillingnessToDance()
        {
            return true;
        }

    }
}
