using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Rayvarz.invHServ.Library.Tools.Security
{
    public class MD5Builder
    {
        public static string GenerateKey(string InputStr)
        {
            using (MD5 md5 = MD5.Create())
            {
                return GetMd5Hash(md5, InputStr);
            }
        }
        private static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }





        private static readonly object locker = new object();

        /// <summary>
        /// Generates a hashed - key for an instance of a class.
        /// The hash is a classic MD5 hash (e.g. BF20EB8D2C4901112179BF5D242D996B). So you can distinguish different 
        /// instances of a class. Because the object is hashed on the internal state, you can also hash it, then send it to
        /// someone in a serialized way. Your client can then deserialize it and check if it is in
        /// the same state.
        /// The method just just estimates that the object implements the ISerializable interface. What's
        /// needed to save the state or so, is up to the implementer of the interface.
        /// <b>The method is thread-safe!</b>
        /// </summary>
        /// <param name="sourceObject">The object you'd like to have a key out of it.</param>
        /// <returns>An string representing a MD5 Hashkey corresponding to the object or null if the object couldn't be serialized.</returns>
        /// <exception cref="ApplicationException">Will be thrown if the key cannot be generated.</exception>
        public static string GenerateKey(object sourceObject)
        {
            string hashString;

            //Catch unuseful parameter values
            if (sourceObject == null)
            {
                throw new ArgumentNullException("Null as parameter is not allowed");
            }
            else
            {
                //We determine if the passed object is really serializable.
                try
                {
                    //Now we begin to do the real work.
                    hashString = ComputeHash(ObjectToByteArray(sourceObject));
                    return hashString;
                }
                catch (AmbiguousMatchException ame)
                {
                    throw new ApplicationException("Could not definitely decide  if object is serializable.Message:"+ame.Message);
                }
            }
        }

        /// <summary>
        /// Converts an object to an array of bytes. This array is used to hash the object.
        /// </summary>
        /// <param name="objectToSerialize">Just an object</param>
        /// <returns>A byte - array representation of the object.</returns>
        /// <exception cref="SerializationException">Is thrown if something went wrong during serialization.</exception>
        private static byte[] ObjectToByteArray(object objectToSerialize)
        {
            MemoryStream fs = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                //Here's the core functionality! One Line!
                //To be thread-safe we lock the object
                lock (locker)
                {
                    formatter.Serialize(fs, objectToSerialize);
                }
                return fs.ToArray();
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Error occurred during serialization. Message: " +
                se.Message);
                return null;
            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>
        /// Generates the hashcode of an given byte-array. The byte-array can be an object. Then the
        /// method "hashes" this object. The hash can then be used e.g. to identify the object.
        /// </summary>
        /// <param name="objectAsBytes">bytearray representation of an object.</param>
        /// <returns>The MD5 hash of the object as a string or null if it couldn't be generated.</returns>
        private static string ComputeHash(byte[] objectAsBytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            try
            {
                byte[] result = md5.ComputeHash(objectAsBytes);

                // Build the final string by converting each byte
                // into hex and appending it to a StringBuilder
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("X2"));
                }

                // And return it
                return sb.ToString();
            }
            catch (ArgumentNullException ane)
            {
                //If something occured during serialization, this method is called with an null argument. 
                Console.WriteLine("Hash has not been generated.");
                return null;
            }
        }
    }

}
