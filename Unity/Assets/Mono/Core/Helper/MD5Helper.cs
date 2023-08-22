using System;
using System.IO;
using System.Security.Cryptography;

namespace ET
{
	public static class MD5Helper
	{
		public static string FileMD5(string filePath)
		{
			byte[] retVal;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
	            MD5 md5 = MD5.Create();
				retVal = md5.ComputeHash(file);
			}
			return retVal.ToHex("x2");
		}
		public static string StringMD5(string content)
		{
			MD5 md5 = MD5.Create();
			byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(content));
			return BitConverter.ToString(data).Replace("-", "");
		}
	}
}
